using System;
using System.IO;
using System.Text.Json;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DestinyColladaGenerator
{
	class Converter
	{
        /// <summary>
        /// Loads a TGX binary file into a dynamic object
        /// </summary>
        /// <param name="data">The TGX binary file data</param>
        /// <returns>A dynamic object containing the TGX binary file data</returns>
        public static dynamic loadTGXBin(byte[] data) 
		{
			Console.WriteLine("Loading model data...");

            Console.Write("Reading TGXM header... ");
			string magic = TGXMUtils.String(data, 0x0, 0x4); // TGXM
			int version = (int)BitConverter.ToUInt32(data, 0x4);
			int fileOffset = (int)BitConverter.ToUInt32(data, 0x8);
			int fileCount = (int)BitConverter.ToUInt32(data, 0xC);
			string fileIdentifier = TGXMUtils.String(data, 0x10, 0x100);
			if (magic != "TGXM") {
				Console.WriteLine("Invalid TGX File, skipping");
				return null;
			}
			Console.WriteLine("Done.");

			Dictionary<string,dynamic> files = new Dictionary<string,dynamic>();

			dynamic renderMetadata = new object();
			for (var f=0; f<fileCount; f++) 
			{
				int headerOffset = fileOffset+(0x110*f);
				string name = TGXMUtils.String(data, headerOffset, 0x100);
				int offset = (int)BitConverter.ToUInt32(data, headerOffset+0x100);
				int type = (int)BitConverter.ToUInt32(data, headerOffset+0x104);
				int size = (int)BitConverter.ToUInt32(data, headerOffset+0x108);
				Console.WriteLine("Loading file \""+name+".\" File size: "+size+" bytes.");
				byte[] fileData = new byte[size];
				if(!File.Exists(Path.Combine("Tilemaps",$"{name}.png")))
					Array.ConstrainedCopy(data, offset, fileData, 0, size);
				else
					fileData = File.ReadAllBytes(Path.Combine("Tilemaps",$"{name}.png"));

				dynamic file = new ExpandoObject();
				file.name = name;
				file.offset = offset;
				file.type = type;
				file.size = size;

				if (name.IndexOf(".js") != -1) 
				{ // render_metadata.js
					renderMetadata = JsonSerializer.Deserialize<RenderMetadata>(TGXMUtils.String(fileData,0,0));
					files.Add("render_metadata_js", fileData);
					file.data = renderMetadata;
				} 
				else
				{
					file.data = fileData;
				}
				if (files.ContainsKey(name)) continue;
				files.Add(name, file);
				Console.WriteLine("File \""+name+"\" loaded.");
			}

			dynamic tgxBin = new ExpandoObject();
			tgxBin.fileIdentifier = fileIdentifier;
			tgxBin.files = files;
			//tgxBin.lookup = fileLookup;
			tgxBin.metadata = renderMetadata;

			Console.WriteLine("Done loading model data.");
			return tgxBin;
		}

        /// <summary>
        /// Converts a TGX binary file into a Collada file
        /// </summary>
        /// <param name="data">The TGX binary file data</param>
        /// <param name="fileOut">The folder to write the Collada file to</param>
        /// <param name="game">Defines which game the model is from. Empty string for D1, "2" for D2</param>
        public static void Convert(byte[] data, string fileOut, string game) 
		{	
			dynamic tgxBin = loadTGXBin(data);
			List<dynamic> renderMeshes = Parsers.parseTGXAsset(tgxBin);
            List<dynamic> renderRaws = new List<dynamic>
            {
                tgxBin
            };
            dynamic renderModel = new ExpandoObject();
			renderModel.meshes = renderMeshes;
			renderModel.textures = null;
			renderModel.raws = renderRaws;
			renderModel.name = "Model";
			renderModel.type = "";
			renderModel.bucket = 0;
            List<dynamic> renderModels = new List<dynamic>
            {
                renderModel
            };

            WriteCollada.WriteFile(renderModels, fileOut, game);
		}

        /// <summary>
        /// Converts an array of API geometry data to Collada files
        /// </summary>
        /// <param name="binItems">The API geometry</param>
        /// <param name="fileOut">The folder to write the Collada files to</param>
        /// <param name="game">Defines which game the model is from. Empty string for D1, "2" for D2</param>
        public static void Convert(APIItemData[] binItems, string fileOut, string game) 
		{	
			List<dynamic> renderModels = new List<dynamic>();
			foreach (APIItemData itemContainers in binItems)
			{
				byte[][] geometry = itemContainers.geometry;
				byte[][] textures = itemContainers.texture;
				string name = itemContainers.name;
				string type = itemContainers.type;
				uint bucket = itemContainers.bucket;
				
				dynamic renderModel = new ExpandoObject();
				List<dynamic> renderMeshes = new List<dynamic>();
				Dictionary<string,dynamic> renderTextures = new Dictionary<string,dynamic>();

				List<dynamic> plates = new List<dynamic>();
				List<dynamic> renderRaws = new List<dynamic>();
				
				foreach (byte[] data in geometry)
				{
					dynamic tgxBin = loadTGXBin(data);
					if (tgxBin == null) continue;
					renderRaws.Add(tgxBin);
					List<dynamic> meshes = Parsers.parseTGXAsset(tgxBin);
					foreach (ExpandoObject mesh in meshes)
					{
						renderMeshes.Add(mesh);
					}
					plates.Add(tgxBin.metadata.texture_plates);
				}
				
				renderTextures.Add("texturePlates", plates);
				
				foreach (byte[] data in textures)
				{
					dynamic tgxBin = loadTGXBin(data);
					if (tgxBin == null) continue;
					foreach (dynamic texture in tgxBin.files)
					{
						renderTextures.Add(texture.Key, texture.Value.data);
					}
				}

				renderModel.meshes = renderMeshes;
				renderModel.textures = renderTextures;
				renderModel.name = name;
				renderModel.type = type;
				renderModel.bucket = bucket;
				renderModel.raws = renderRaws;
				
				renderModels.Add(renderModel);
			}
			
			WriteCollada.WriteFile(renderModels, fileOut, game);
		}
	}
}
