using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

class Converter
{
	public static JObject loadTGXBin(byte[] data) 
	{
		Console.WriteLine("Loading model data...");
		
		Console.Write("Reading TGXM header... ");
		string magic = TGXMUtils.String(data, 0x0, 0x4); // TGXM
		int version = (int)BitConverter.ToUInt32(data, 0x4);
		int fileOffset = (int)BitConverter.ToUInt32(data, 0x8);
		int fileCount = (int)BitConverter.ToUInt32(data, 0xC);
		string fileIdentifier = TGXMUtils.String(data, 0x10, 0x100);
		if (magic != "TGXM") {
		    console.error("Invalid TGX File, skipping");
		    return null;
		}
		Console.WriteLine("Done.");

		dynamic files = new JArray();
		dynamic fileLookup = new JArray();
		dynamic renderMetadata = new JObject();
		for (var f=0; f<fileCount; f++) 
		{
			int headerOffset = fileOffset+(0x110*f);
			string name = TGXMUtils.String(data, headerOffset, 0x100);
			int offset = (int)BitConverter.ToUInt32(data, headerOffset+0x100);
			int type = (int)BitConverter.ToUInt32(data, headerOffset+0x104);
			int size = (int)BitConverter.ToUInt32(data, headerOffset+0x108);
			Console.WriteLine("Loading file \""+name+".\" File size: "+size+" bytes.");
			byte[] fileData = new byte[size];
			Array.ConstrainedCopy(data, offset, fileData, 0, size);

			dynamic file = new JObject();
			file.name = name;
			file.offset = offset;
			file.type = type;
			file.size = size;

			if (name.IndexOf(".js") != -1) 
			{ // render_metadata.js
				renderMetadata = JObject.Parse(TGXMUtils.String(fileData,0,0));
				file.data = renderMetadata;
			} 
			else
			{
				file.data = fileData;
			}

			files.Add(file);
			fileLookup.Add(name);
			Console.WriteLine("File \""+name+"\" loaded.");
		}

		dynamic tgxBin = new JObject();
		tgxBin.fileIdentifier = fileIdentifier;
		tgxBin.files = files;
		tgxBin.lookup = fileLookup;
		tgxBin.metadata = renderMetadata;

		Console.WriteLine("Done loading model data.");
		return tgxBin;
	}

	public static void Convert(byte[] data, string fileOut) 
	{	
		JObject tgxBin = loadTGXBin(data);
		JArray renderMeshes = Parsers.parseTGXAsset(tgxBin);
		dynamic renderModel = new JObject();
		renderModel.meshes = renderMeshes;
		renderModel.name = "Model";
		JArray renderModels = new JArray();
		renderModels.Add(renderModel);

		WriteCollada.WriteFile(renderModels, fileOut);
	}

	public static void Convert(APIItemData[] binItems, string fileOut) 
	{	
		JArray renderModels = new JArray();
		foreach (APIItemData itemContainers in binItems)
		{
			byte[][] geometry = itemContainers.geometry;
			byte[][] textures = itemContainers.texture;
			string name = itemContainers.name;
			
			dynamic renderModel = new JObject();
			JArray renderMeshes = new JArray();
			dynamic renderTextures = new JObject();
			JArray textureLookup = new JArray();
			JArray plates = new JArray();
			
			foreach (byte[] data in geometry)
			{
				dynamic tgxBin = loadTGXBin(data);
				if (tgxBin == null) continue;
				JArray meshes = Parsers.parseTGXAsset(tgxBin);
				foreach (JObject mesh in meshes)
				{
					renderMeshes.Add(mesh);
				}
				plates.Add(tgxBin.metadata.texture_plates);
			}
			
			renderTextures.texturePlates = plates;
			
			foreach (byte[] data in textures)
			{
				dynamic tgxBin = loadTGXBin(data);
				if (tgxBin == null) continue;
				foreach (dynamic texture in tgxBin.files)
				{
					renderTextures.Add(new JProperty(texture.name.Value, texture.data));
					textureLookup.Add(texture.name.Value);
				}
			}

			renderTextures.names = textureLookup;
			
			renderModel.meshes = renderMeshes;
			renderModel.textures = renderTextures;
			renderModel.name = name;
			
			renderModels.Add(renderModel);
		}
		
		WriteCollada.WriteFile(renderModels, fileOut);
	}
}