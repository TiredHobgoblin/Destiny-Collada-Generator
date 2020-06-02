using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace DestinyColladaGenerator
{
    public enum Channels : uint
    {
        ArmorPlate = 662199250,
        ArmorSuit = 218592586,
        ArmorCloth = 1367384683,
        Weapon1 = 1667433279,
        Weapon2 = 1667433278,
        Weapon3 = 1667433277,
        ShipUpper = 3073305669,
        ShipDecals = 3073305668,
        ShipLower = 3073305671,
        SparrowUpper = 1971582085,
        SparrowEngine = 1971582084,
        SparrowLower = 1971582087,
        GhostMain = 373026848,
        GhostHighlights = 373026849,
        GhostDecals = 373026850
    }



    public class D1Shader {
        public D1SlotContainer[] default_dyes { get; set; }
        public D1SlotContainer[] locked_dyes { get; set; }
        public D1SlotContainer[] custom_dyes { get; set; }
        public string reference_id { get; set; }
        public dynamic art_content { get; set; }
        public dynamic art_content_sets { get; set; }
        
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            if (default_dyes.Length > 0)
            {
                text.Append("Default dyes:\n");
                foreach (D1SlotContainer slot in default_dyes)
                    text.Append(slot.ToString());
            }
            if (locked_dyes.Length > 0)
            {
                text.Append("Locked dyes:\n\n");
                foreach (D1SlotContainer slot in locked_dyes)
                    text.Append(slot.ToString());
            }
            if (custom_dyes.Length > 0)
            {
                text.Append("Custom dyes:\n");
                foreach (D1SlotContainer slot in custom_dyes)
                    text.Append(slot.ToString());
            }
            return text.ToString();
        }
    }

    public class D2Shader {
        public D2SlotContainer[] default_dyes { get; set; }
        public D2SlotContainer[] locked_dyes { get; set; }
        public D2SlotContainer[] custom_dyes { get; set; }
        public string reference_id { get; set; }
        public dynamic art_content_sets { get; set; }
        
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            if (default_dyes.Length > 0)
            {
                text.Append("Default dyes:\n");
                foreach (D2SlotContainer slot in default_dyes)
                    text.Append(slot.ToString());
            }
            if (locked_dyes.Length > 0)
            {
                text.Append("Locked dyes:\n");
                foreach (D2SlotContainer slot in locked_dyes)
                    text.Append(slot.ToString());
            }
            if (custom_dyes.Length > 0)
            {
                text.Append("Custom dyes:\n");
                foreach (D2SlotContainer slot in custom_dyes)
                    text.Append(slot.ToString());
            }
            return text.ToString();
        }
    }

    public class D1SlotContainer
    {
        public uint hash { get; set; }
        public uint investment_hash { get; set; }
        public int slot_type_index { get; set; }
        public int variant { get; set; }
        public int blend_mode { get; set; }
        public bool cloth { get; set; }
        public D1MatProps material_properties { get; set; }
        public D1TexturesContainer textures { get; set; }
        
        public override string ToString()
        {
            StringBuilder text = new StringBuilder($"\t{ShaderPresets.propertyChannels[investment_hash]}:\n");
            text.Append($"\t\tBlend mode: {blend_mode}\n");
            text.Append($"\t\tIs cloth: {cloth}\n");
            text.Append($"\t\tProperties: \n{material_properties.ToString()}");
            text.Append($"{textures.ToString()}\n");
            return text.ToString();
        }
    }

    public class D2SlotContainer
    {
        public uint hash { get; set; }
        public uint investment_hash { get; set; }
        public int slot_type_index { get; set; }
        public bool cloth { get; set; }
        public D2MatProps material_properties { get; set; }
        public D2TexturesContainer textures { get; set; }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder($"\t{ShaderPresets.propertyChannels[investment_hash]}:\n");
            text.Append($"\t\tIs cloth: {cloth}\n");
            text.Append($"\t\tProperties: {material_properties.ToString()}");
            text.Append($"{textures.ToString()}\n");
            return text.ToString();
        }
    }

    public class D1TexturesContainer
    {
        public TextureRef diffuse { get; set; }
        public TextureRef normal { get; set; }
        public NameOnlyTextureRef decal { get; set; }
        public TextureRef primary_diffuse { get; set; }
        public TextureRef secondary_diffuse { get; set; }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append($"\t\tDiffuse: {diffuse.name}\n");
            text.Append($"\t\tNormal: {normal.name}\n");
            text.Append($"\t\tDecal: {decal.name}\n");
            text.Append($"\t\tPrimary Diffuse: {primary_diffuse.name}\n");
            text.Append($"\t\tSecondary Diffuse: {secondary_diffuse}\n");
            return text.ToString();
        }
    }

    public class D2TexturesContainer
    {
        public TextureRef diffuse { get; set; }
        public TextureRef normal { get; set; }
        public TextureRef primary_diffuse { get; set; }
        public TextureRef secondary_diffuse { get; set; }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append($"\t\tDiffuse: {diffuse.name}\n");
            text.Append($"\t\tNormal: {normal.name}\n");
            text.Append($"\t\tPrimary Diffuse: {primary_diffuse.name}\n");
            text.Append($"\t\tSecondary Diffuse: {secondary_diffuse.name}\n");
            return text.ToString();
        }
    }

    public class NameOnlyTextureRef
    {
        public string name { get; set; }
    }

    public class TextureRef
    {
        public string name { get; set; }
        public string reference_id { get; set; }
    }

    public class D1MatProps
    {
        public float[] primary_color { get; set; } // [r,g,b,a]
        public float[] secondary_color { get; set; } // [r,g,b,a]
        public float[] detail_transform { get; set; } // [sx,sy,tx,ty]
        public float[] detail_normal_contribution_strength { get; set; }
        public float[] decal_alpha_map_transform { get; set; }
        public int decal_blend_option { get; set; }
        public float[] specular_properties { get; set; } // [specular, shininess, unk, unk]
        public float[] subsurface_scattering_strength { get; set; }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.Append($"\t\t\tPrimary Color: ({primary_color[0]}, {primary_color[1]}, {primary_color[2]}, {primary_color[3]})\n");
            text.Append($"\t\t\tSecondary Color: ({secondary_color[0]}, {secondary_color[1]}, {secondary_color[2]}, {secondary_color[3]})\n");
            text.Append($"\t\t\tDetail Map Transform: ({detail_transform[0]}, {detail_transform[1]}, {detail_transform[2]}, {detail_transform[3]})\n");
            text.Append($"\t\t\tDetail Normal Contribution: ({detail_normal_contribution_strength[0]}, {detail_normal_contribution_strength[1]}, {detail_normal_contribution_strength[2]}, {detail_normal_contribution_strength[3]})\n");
            text.Append($"\t\t\tDecal Alpha Map Transform: ({decal_alpha_map_transform[0]}, {decal_alpha_map_transform[1]}, {decal_alpha_map_transform[2]}, {decal_alpha_map_transform[3]})\n");
            text.Append($"\t\t\tSpecular(?): {specular_properties[0]}\n");
            text.Append($"\t\t\tShininess(?): {specular_properties[1]}\n");
            return text.ToString();
        }
    }

    public class D2MatProps
    {
        public float[] detail_diffuse_transform { get; set; } // [sx,sy,tx,ty]
        public float[] detail_normal_transform { get; set; } // [sx,sy,tx,ty]
        public float[] spec_aa_xform { get; set; } // Values for geometric specular antialiasing settings
        
        public float[] primary_emissive_tint_color_and_intensity_bias { get; set; } // [r,g,b,i]
        public float[] secondary_emissive_tint_color_and_intensity_bias { get; set; } // [r,g,b,i]

        public float[] specular_properties { get; set; } // [specular, shininess, unk, unk]
        public float[] lobe_pbr_params { get; set; } // IBL sky reflection? Bungie seems to refer to the skybox as "lobe"
        public float[] tint_pbr_params { get; set; } // IBL environment reflection?
        public float[] emissive_pbr_params { get; set; } // IBL world/character emission? i.e. bloom on reflected emissive shaders.

        public float[] primary_albedo_tint { get; set; } // [r,g,b,a]
        public float[] primary_material_params { get; set; } // [?,?,?,metalness]
        public float[] primary_advanced_material_params { get; set; }
        public float[] primary_roughness_remap { get; set; } // [1s,1e,2s,2e]
        
        public float[] secondary_albedo_tint { get; set; } // [r,g,b,a]
        public float[] secondary_material_params { get; set; } 
        public float[] secondary_advanced_material_params { get; set; } 
        public float[] secondary_roughness_remap { get; set; } // [1s,1e,2s,2e]
        
        // Unsure why primary and secondary worn parameters, standard shader only seems to use one set of worn params.
        // Maybe for clarity or easier separation in community renderers?
        public float[] primary_worn_albedo_tint { get; set; } // [r,g,b,a]
        public float[] primary_wear_remap { get; set; } // [1s,1e,2s,2e]
        public float[] primary_worn_roughness_remap { get; set; } // [1s,1e,2s,2e]
        public float[] primary_worn_material_parameters { get; set; } 
        
        public float[] secondary_worn_albedo_tint { get; set; } // [r,g,b,a]
        public float[] secondary_wear_remap { get; set; } // [1s,1e,2s,2e]
        public float[] secondary_worn_roughness_remap { get; set; } // [1s,1e,2s,2e]
        public float[] secondary_worn_material_parameters { get; set; } 
        
        public float[] primary_subsurface_scattering_strength_and_emissive { get; set; } // [str,r,g,b]? [r,g,b,str]?
        public float[] secondary_subsurface_scattering_strength_and_emissive { get; set; } // [str,r,g,b]? [r,g,b,str]?

        public override string ToString()
        {
            return "";
        }
    }

    public static class ShaderPresets
    {
        public static Dictionary<uint, Channels> propertyChannels { get; set; }
        public static Dictionary<string, string> presets { get; set; }
        public static void generatePresets(string game, dynamic itemDef, string name)
        {
        	if (game == "")
            {
                dynamic equippingBlock = itemDef.definition.GetProperty("equippingBlock");
                for (int c=0; c<equippingBlock.GetProperty("defaultDyes").GetArrayLength(); c++)
                {
                    dynamic channel = equippingBlock.GetProperty("defaultDyes")[c];
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
                }
                for (int c=0; c<equippingBlock.GetProperty("lockedDyes").GetArrayLength(); c++)
                {
                    dynamic channel = equippingBlock.GetProperty("lockedDyes")[c];
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
                }
                for (int c=0; c<equippingBlock.GetProperty("customDyes").GetArrayLength(); c++)
                {
                    dynamic channel = equippingBlock.GetProperty("customDyes")[c];
                    propertyChannels.Add(channel.GetProperty("dyeHash").GetUInt32(), (Channels)(channel.GetProperty("channelHash").GetUInt32()));
                }
                
                string gearJs = itemDef.gearAsset.GetProperty("gear")[0].GetString();
                presets.Add(name, apiSupport.makeCallGear($"https://www.bungie.net/common/destiny{game}_content/geometry/gear/{gearJs}",game).ToString());
            }
        }
    }
}