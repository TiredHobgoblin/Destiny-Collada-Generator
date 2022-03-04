using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace DestinyColladaGenerator
{
    public static class GamePresets
    {
        /*public static Dictionary<uint, Channels> propertyChannels { get; set; }
        public static Dictionary<Channels, D2MatProps> channelData { get; set; }
        public static Dictionary<Channels, D2TexturesContainer> channelTextures { get; set; }
        public static Dictionary<string, string> presets { get; set; }
        public static Dictionary<string, string> scripts { get; set; }*/
        public static void generatePresets(string game, string shaderData, string name)
        {
            /*dynamic translationBlock = JsonSerializer.Deserialize<translationBlock>(File.ReadAllBytes(itemDef));
            for (int c=0; c<translationBlock.GetProperty("defaultDyes").GetArrayLength(); c++)
            {
                dynamic channel = translationBlock.GetProperty("defaultDyes")[c];
                if (!propertyChannels.ContainsKey(channel.GetProperty("dyeHash").GetUInt32()))
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
            }
            for (int c=0; c<translationBlock.GetProperty("lockedDyes").GetArrayLength(); c++)
            {
                dynamic channel = translationBlock.GetProperty("lockedDyes")[c];
                if (!propertyChannels.ContainsKey(channel.GetProperty("dyeHash").GetUInt32()))
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
            }
            for (int c=0; c<translationBlock.GetProperty("customDyes").GetArrayLength(); c++)
            {
                dynamic channel = translationBlock.GetProperty("customDyes")[c];
                if (!propertyChannels.ContainsKey(channel.GetProperty("dyeHash").GetUInt32()))
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
            }*/
            ShaderPresets.propertyChannels = new Dictionary<uint, Channels>();
            ShaderPresets.channelData = new Dictionary<Channels, D2MatProps>();
            ShaderPresets.channelTextures = new Dictionary<Channels, D2TexturesContainer>();
            ShaderPresets.presets = new Dictionary<string, string>();
            ShaderPresets.scripts = new Dictionary<string, string>();

            ShaderPresets.propertyChannels.Add((uint)Channels.ArmorCloth, Channels.ArmorCloth);
            ShaderPresets.propertyChannels.Add((uint)Channels.ArmorPlate, Channels.ArmorPlate);
            ShaderPresets.propertyChannels.Add((uint)Channels.ArmorSuit, Channels.ArmorSuit);
            ShaderPresets.propertyChannels.Add((uint)Channels.GhostDecals, Channels.GhostDecals);
            ShaderPresets.propertyChannels.Add((uint)Channels.GhostHighlights, Channels.GhostHighlights);
            ShaderPresets.propertyChannels.Add((uint)Channels.GhostMain, Channels.GhostMain);
            ShaderPresets.propertyChannels.Add((uint)Channels.ShipDecals, Channels.ShipDecals);
            ShaderPresets.propertyChannels.Add((uint)Channels.ShipLower, Channels.ShipLower);
            ShaderPresets.propertyChannels.Add((uint)Channels.ShipUpper, Channels.ShipUpper);
            ShaderPresets.propertyChannels.Add((uint)Channels.SparrowEngine, Channels.SparrowEngine);
            ShaderPresets.propertyChannels.Add((uint)Channels.SparrowLower, Channels.SparrowLower);
            ShaderPresets.propertyChannels.Add((uint)Channels.SparrowUpper, Channels.SparrowUpper);
            ShaderPresets.propertyChannels.Add((uint)Channels.Weapon1, Channels.Weapon1);
            ShaderPresets.propertyChannels.Add((uint)Channels.Weapon2, Channels.Weapon2);
            ShaderPresets.propertyChannels.Add((uint)Channels.Weapon3, Channels.Weapon3);
            
            D2Shader dyeDef = JsonSerializer.Deserialize<D2Shader>(File.ReadAllBytes(shaderData));
            ShaderPresets.presets.Add(name, dyeDef.ToString());
            if (ShaderPresets.channelData.Count<1)
                ConsoleEx.Warn("Item has no dyes. This may be intended, or more likely it is an error on Bungie's part.");
            else if (ShaderPresets.propertyChannels.Count == 3)
            {
                // Generate one script
                Channels[] channels = new Channels[3];
                foreach (KeyValuePair<uint, Channels> kvp in ShaderPresets.propertyChannels)
                {
                    Channels c = kvp.Value;
                    if (c == Channels.ArmorPlate || c == Channels.GhostMain || c == Channels.ShipUpper || c == Channels.SparrowUpper || c == Channels.Weapon1)
                        channels[0] = c;
                    if (c == Channels.ArmorCloth || c == Channels.GhostHighlights || c == Channels.ShipDecals || c == Channels.SparrowEngine || c == Channels.Weapon2)
                        channels[1] = c;
                    if (c == Channels.ArmorSuit || c == Channels.GhostDecals || c == Channels.ShipLower || c == Channels.SparrowLower || c == Channels.Weapon3)
                        channels[2] = c;
                    if (!ShaderPresets.channelData.ContainsKey(c))
                    {
                        ShaderPresets.channelData.Add(c, new D2MatProps());
                        ShaderPresets.channelTextures.Add(c, new D2TexturesContainer());
                    }

                }
                generateScript(dyeDef, name, channels);
            }
            else if (ShaderPresets.propertyChannels.Count == 15)
            {
                // Generate five scripts
                generateScript(dyeDef, name+"_armor", new Channels[3] {Channels.ArmorPlate, Channels.ArmorCloth, Channels.ArmorSuit});
                generateScript(dyeDef, name+"_weapon", new Channels[3] {Channels.Weapon1, Channels.Weapon2, Channels.Weapon3});
                generateScript(dyeDef, name+"_ship", new Channels[3] {Channels.ShipUpper, Channels.ShipDecals, Channels.ShipLower});
                generateScript(dyeDef, name+"_sparrow", new Channels[3] {Channels.SparrowUpper, Channels.SparrowEngine, Channels.SparrowLower});
                generateScript(dyeDef, name+"_ghost", new Channels[3] {Channels.GhostMain, Channels.GhostHighlights, Channels.GhostDecals});
            }
            else
            {
                // Something is wrong
                Console.WriteLine($"Invalid dye channel count, please report to BIOS. Dyes: {ShaderPresets.propertyChannels.Count}");
            }
        }
        public static void generateScript(D2Shader dyeDef, string name, Channels[] channels)
        {
            if (!ShaderPresets.scripts.ContainsKey(name))
            {
                CultureInfo ci = CultureInfo.InvariantCulture;
			    Thread.CurrentThread.CurrentCulture = ci;
			    Thread.CurrentThread.CurrentUICulture = ci;

                Dictionary<string,float[]> enums = new Dictionary<string,float[]>();

                enums.Add("DiffTrans1", ShaderPresets.channelData[channels[0]].detail_diffuse_transform);
                enums.Add("DiffTrans2", ShaderPresets.channelData[channels[1]].detail_diffuse_transform);
                enums.Add("DiffTrans3", ShaderPresets.channelData[channels[2]].detail_diffuse_transform); // trans rights are human rights

                enums.Add("NormTrans1", ShaderPresets.channelData[channels[0]].detail_normal_transform);
                enums.Add("NormTrans2", ShaderPresets.channelData[channels[1]].detail_normal_transform);
                enums.Add("NormTrans3", ShaderPresets.channelData[channels[2]].detail_normal_transform);

                enums.Add("CPrime1", ShaderPresets.channelData[channels[0]].primary_albedo_tint);
                enums.Add("CSecon1", ShaderPresets.channelData[channels[0]].secondary_albedo_tint);
                enums.Add("CPrime2", ShaderPresets.channelData[channels[1]].primary_albedo_tint);
                enums.Add("CSecon2", ShaderPresets.channelData[channels[1]].secondary_albedo_tint);
                enums.Add("CPrime3", ShaderPresets.channelData[channels[2]].primary_albedo_tint);
                enums.Add("CSecon3", ShaderPresets.channelData[channels[2]].secondary_albedo_tint);

                enums.Add("PrimeRoughMap1", ShaderPresets.channelData[channels[0]].primary_roughness_remap);
                enums.Add("SeconRoughMap1", ShaderPresets.channelData[channels[0]].secondary_roughness_remap);
                enums.Add("PrimeRoughMap2", ShaderPresets.channelData[channels[1]].primary_roughness_remap);
                enums.Add("SeconRoughMap2", ShaderPresets.channelData[channels[1]].secondary_roughness_remap);
                enums.Add("PrimeRoughMap3", ShaderPresets.channelData[channels[2]].primary_roughness_remap);
                enums.Add("SeconRoughMap3", ShaderPresets.channelData[channels[2]].secondary_roughness_remap);

                enums.Add("PrimeWearMap1", ShaderPresets.channelData[channels[0]].primary_wear_remap);
                enums.Add("SeconWearMap1", ShaderPresets.channelData[channels[0]].secondary_wear_remap);
                enums.Add("PrimeWearMap2", ShaderPresets.channelData[channels[1]].primary_wear_remap);
                enums.Add("SeconWearMap2", ShaderPresets.channelData[channels[1]].secondary_wear_remap);
                enums.Add("PrimeWearMap3", ShaderPresets.channelData[channels[2]].primary_wear_remap);
                enums.Add("SeconWearMap3", ShaderPresets.channelData[channels[2]].secondary_wear_remap);

                enums.Add("PrimeMatParams1", ShaderPresets.channelData[channels[0]].primary_material_params);
                enums.Add("SeconMatParams1", ShaderPresets.channelData[channels[0]].secondary_material_params);
                enums.Add("PrimeMatParams2", ShaderPresets.channelData[channels[1]].primary_material_params);
                enums.Add("SeconMatParams2", ShaderPresets.channelData[channels[1]].secondary_material_params);
                enums.Add("PrimeMatParams3", ShaderPresets.channelData[channels[2]].primary_material_params);
                enums.Add("SeconMatParams3", ShaderPresets.channelData[channels[2]].secondary_material_params);

                enums.Add("PrimeAdvMatParams1", ShaderPresets.channelData[channels[0]].primary_material_advanced_params);
                enums.Add("SeconAdvMatParams1", ShaderPresets.channelData[channels[0]].secondary_material_advanced_params);
                enums.Add("PrimeAdvMatParams2", ShaderPresets.channelData[channels[1]].primary_material_advanced_params);
                enums.Add("SeconAdvMatParams2", ShaderPresets.channelData[channels[1]].secondary_material_advanced_params);
                enums.Add("PrimeAdvMatParams3", ShaderPresets.channelData[channels[2]].primary_material_advanced_params);
                enums.Add("SeconAdvMatParams3", ShaderPresets.channelData[channels[2]].secondary_material_advanced_params);

                enums.Add("CPrimeWear1", ShaderPresets.channelData[channels[0]].primary_worn_albedo_tint);
                enums.Add("CSeconWear1", ShaderPresets.channelData[channels[0]].secondary_worn_albedo_tint);
                enums.Add("CPrimeWear2", ShaderPresets.channelData[channels[1]].primary_worn_albedo_tint);
                enums.Add("CSeconWear2", ShaderPresets.channelData[channels[1]].secondary_worn_albedo_tint);
                enums.Add("CPrimeWear3", ShaderPresets.channelData[channels[2]].primary_worn_albedo_tint);
                enums.Add("CSeconWear3", ShaderPresets.channelData[channels[2]].secondary_worn_albedo_tint);

                enums.Add("PrimeWornRoughMap1", ShaderPresets.channelData[channels[0]].primary_worn_roughness_remap);
                enums.Add("SeconWornRoughMap1", ShaderPresets.channelData[channels[0]].secondary_worn_roughness_remap);
                enums.Add("PrimeWornRoughMap2", ShaderPresets.channelData[channels[1]].primary_worn_roughness_remap);
                enums.Add("SeconWornRoughMap2", ShaderPresets.channelData[channels[1]].secondary_worn_roughness_remap);
                enums.Add("PrimeWornRoughMap3", ShaderPresets.channelData[channels[2]].primary_worn_roughness_remap);
                enums.Add("SeconWornRoughMap3", ShaderPresets.channelData[channels[2]].secondary_worn_roughness_remap);

                enums.Add("PrimeWornMatParams1", ShaderPresets.channelData[channels[0]].primary_worn_material_parameters);
                enums.Add("SeconWornMatParams1", ShaderPresets.channelData[channels[0]].secondary_worn_material_parameters);
                enums.Add("PrimeWornMatParams2", ShaderPresets.channelData[channels[1]].primary_worn_material_parameters);
                enums.Add("SeconWornMatParams2", ShaderPresets.channelData[channels[1]].secondary_worn_material_parameters);
                enums.Add("PrimeWornMatParams3", ShaderPresets.channelData[channels[2]].primary_worn_material_parameters);
                enums.Add("SeconWornMatParams3", ShaderPresets.channelData[channels[2]].secondary_worn_material_parameters);

                enums.Add("CPrimeEmit1", ShaderPresets.channelData[channels[0]].primary_emissive_tint_color_and_intensity_bias);
                enums.Add("CSeconEmit1", ShaderPresets.channelData[channels[0]].secondary_emissive_tint_color_and_intensity_bias);
                enums.Add("CPrimeEmit2", ShaderPresets.channelData[channels[1]].primary_emissive_tint_color_and_intensity_bias);
                enums.Add("CSeconEmit2", ShaderPresets.channelData[channels[1]].secondary_emissive_tint_color_and_intensity_bias);
                enums.Add("CPrimeEmit3", ShaderPresets.channelData[channels[2]].primary_emissive_tint_color_and_intensity_bias);
                enums.Add("CSeconEmit3", ShaderPresets.channelData[channels[2]].secondary_emissive_tint_color_and_intensity_bias);
                
                Dictionary<string,string> templates = new Dictionary<string, string>();
                templates.Add("template.py", "_BLENDER");
                templates.Add("template.shader", "_UNITY");
                templates.Add("template.vmat", "_SOURCE2");
                //templates.Add("template.ue4.py", "_UNREAL");

                foreach (KeyValuePair<string,string> templateName in templates)
                {
                    string template = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", templateName.Key));

                    template = template.Replace("SHADERNAMEENUM", name);

                    foreach (KeyValuePair<string,float[]> evp in enums)
                    {
                        template = replaceEnum(template, evp.Key, evp.Value);
                    }

                    for (int tex=0; tex<3; tex++)
                    {
                        string diffExt = "png";
                        string normExt = "png";
                        if (File.Exists(Path.Combine("Tilemaps",$"{ShaderPresets.channelTextures[channels[tex]].diffuse.name}.dds"))) diffExt = "dds";
                        if (File.Exists(Path.Combine("Tilemaps",$"{ShaderPresets.channelTextures[channels[tex]].normal.name}.dds"))) normExt = "dds";
                        template = template.Replace($"DiffMap{tex+1}", $"{ShaderPresets.channelTextures[channels[tex]].diffuse.name}.{diffExt}");
                        template = template.Replace($"NormMap{tex+1}", $"{ShaderPresets.channelTextures[channels[tex]].normal.name}.{normExt}");
                    }

                    ShaderPresets.scripts.Add(name+templateName.Value, template);
                }
            }
        }
        public static string replaceEnum(string template, string enumName, float[] values)
        {
            string[] suffs = new string[4] {".X", ".Y", ".Z", ".W"};
            for (int e=0; e<4; e++)
            {
                template = template.Replace($"{enumName}{suffs[e]}", values[e].ToString());
            }
            return template;
        }
    }
}