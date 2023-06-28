using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace DestinyColladaGenerator
{
    public static class GamePresets
    {
        public static void generatePresets(string game, string shaderData, string name)
        {
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

                Dictionary<string, float[]> enums = new Dictionary<string, float[]>
                {
                    { "DiffTrans1", ShaderPresets.channelData[channels[0]].detail_diffuse_transform },
                    { "DiffTrans2", ShaderPresets.channelData[channels[1]].detail_diffuse_transform },
                    { "DiffTrans3", ShaderPresets.channelData[channels[2]].detail_diffuse_transform }, // trans rights are human rights

                    { "NormTrans1", ShaderPresets.channelData[channels[0]].detail_normal_transform },
                    { "NormTrans2", ShaderPresets.channelData[channels[1]].detail_normal_transform },
                    { "NormTrans3", ShaderPresets.channelData[channels[2]].detail_normal_transform },

                    { "CPrime1", ShaderPresets.channelData[channels[0]].primary_albedo_tint },
                    { "CSecon1", ShaderPresets.channelData[channels[0]].secondary_albedo_tint },
                    { "CPrime2", ShaderPresets.channelData[channels[1]].primary_albedo_tint },
                    { "CSecon2", ShaderPresets.channelData[channels[1]].secondary_albedo_tint },
                    { "CPrime3", ShaderPresets.channelData[channels[2]].primary_albedo_tint },
                    { "CSecon3", ShaderPresets.channelData[channels[2]].secondary_albedo_tint },

                    { "PrimeRoughMap1", ShaderPresets.channelData[channels[0]].primary_roughness_remap },
                    { "SeconRoughMap1", ShaderPresets.channelData[channels[0]].secondary_roughness_remap },
                    { "PrimeRoughMap2", ShaderPresets.channelData[channels[1]].primary_roughness_remap },
                    { "SeconRoughMap2", ShaderPresets.channelData[channels[1]].secondary_roughness_remap },
                    { "PrimeRoughMap3", ShaderPresets.channelData[channels[2]].primary_roughness_remap },
                    { "SeconRoughMap3", ShaderPresets.channelData[channels[2]].secondary_roughness_remap },

                    { "PrimeWearMap1", ShaderPresets.channelData[channels[0]].primary_wear_remap },
                    { "SeconWearMap1", ShaderPresets.channelData[channels[0]].secondary_wear_remap },
                    { "PrimeWearMap2", ShaderPresets.channelData[channels[1]].primary_wear_remap },
                    { "SeconWearMap2", ShaderPresets.channelData[channels[1]].secondary_wear_remap },
                    { "PrimeWearMap3", ShaderPresets.channelData[channels[2]].primary_wear_remap },
                    { "SeconWearMap3", ShaderPresets.channelData[channels[2]].secondary_wear_remap },

                    { "PrimeMatParams1", ShaderPresets.channelData[channels[0]].primary_material_params },
                    { "SeconMatParams1", ShaderPresets.channelData[channels[0]].secondary_material_params },
                    { "PrimeMatParams2", ShaderPresets.channelData[channels[1]].primary_material_params },
                    { "SeconMatParams2", ShaderPresets.channelData[channels[1]].secondary_material_params },
                    { "PrimeMatParams3", ShaderPresets.channelData[channels[2]].primary_material_params },
                    { "SeconMatParams3", ShaderPresets.channelData[channels[2]].secondary_material_params },

                    { "PrimeAdvMatParams1", ShaderPresets.channelData[channels[0]].primary_material_advanced_params },
                    { "SeconAdvMatParams1", ShaderPresets.channelData[channels[0]].secondary_material_advanced_params },
                    { "PrimeAdvMatParams2", ShaderPresets.channelData[channels[1]].primary_material_advanced_params },
                    { "SeconAdvMatParams2", ShaderPresets.channelData[channels[1]].secondary_material_advanced_params },
                    { "PrimeAdvMatParams3", ShaderPresets.channelData[channels[2]].primary_material_advanced_params },
                    { "SeconAdvMatParams3", ShaderPresets.channelData[channels[2]].secondary_material_advanced_params },

                    { "CPrimeWear1", ShaderPresets.channelData[channels[0]].primary_worn_albedo_tint },
                    { "CSeconWear1", ShaderPresets.channelData[channels[0]].secondary_worn_albedo_tint },
                    { "CPrimeWear2", ShaderPresets.channelData[channels[1]].primary_worn_albedo_tint },
                    { "CSeconWear2", ShaderPresets.channelData[channels[1]].secondary_worn_albedo_tint },
                    { "CPrimeWear3", ShaderPresets.channelData[channels[2]].primary_worn_albedo_tint },
                    { "CSeconWear3", ShaderPresets.channelData[channels[2]].secondary_worn_albedo_tint },

                    { "PrimeWornRoughMap1", ShaderPresets.channelData[channels[0]].primary_worn_roughness_remap },
                    { "SeconWornRoughMap1", ShaderPresets.channelData[channels[0]].secondary_worn_roughness_remap },
                    { "PrimeWornRoughMap2", ShaderPresets.channelData[channels[1]].primary_worn_roughness_remap },
                    { "SeconWornRoughMap2", ShaderPresets.channelData[channels[1]].secondary_worn_roughness_remap },
                    { "PrimeWornRoughMap3", ShaderPresets.channelData[channels[2]].primary_worn_roughness_remap },
                    { "SeconWornRoughMap3", ShaderPresets.channelData[channels[2]].secondary_worn_roughness_remap },

                    { "PrimeWornMatParams1", ShaderPresets.channelData[channels[0]].primary_worn_material_parameters },
                    { "SeconWornMatParams1", ShaderPresets.channelData[channels[0]].secondary_worn_material_parameters },
                    { "PrimeWornMatParams2", ShaderPresets.channelData[channels[1]].primary_worn_material_parameters },
                    { "SeconWornMatParams2", ShaderPresets.channelData[channels[1]].secondary_worn_material_parameters },
                    { "PrimeWornMatParams3", ShaderPresets.channelData[channels[2]].primary_worn_material_parameters },
                    { "SeconWornMatParams3", ShaderPresets.channelData[channels[2]].secondary_worn_material_parameters },

                    { "CPrimeEmit1", ShaderPresets.channelData[channels[0]].primary_emissive_tint_color_and_intensity_bias },
                    { "CSeconEmit1", ShaderPresets.channelData[channels[0]].secondary_emissive_tint_color_and_intensity_bias },
                    { "CPrimeEmit2", ShaderPresets.channelData[channels[1]].primary_emissive_tint_color_and_intensity_bias },
                    { "CSeconEmit2", ShaderPresets.channelData[channels[1]].secondary_emissive_tint_color_and_intensity_bias },
                    { "CPrimeEmit3", ShaderPresets.channelData[channels[2]].primary_emissive_tint_color_and_intensity_bias },
                    { "CSeconEmit3", ShaderPresets.channelData[channels[2]].secondary_emissive_tint_color_and_intensity_bias }
                };

                Dictionary<string, string> templates = new Dictionary<string, string>
                {
                    { "template.py", "_BLENDER" },
                    { "template.shader", "_UNITY" },
                    { "template.vmat", "_SOURCE2" }
                };
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
                        if (File.Exists(Path.Combine("Tilemaps",$"{ShaderPresets.channelTextures[channels[tex]].diffuse.name}.png"))) diffExt = "png";
                        if (File.Exists(Path.Combine("Tilemaps",$"{ShaderPresets.channelTextures[channels[tex]].normal.name}.png"))) normExt = "png";
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