using System;
using System.Text.Json;
using System.Dynamic;
using System.Collections.Generic;

class Converter
{
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
		//dynamic fileLookup = new JArray();
		dynamic renderMetadata = new Object();
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

			dynamic file = new ExpandoObject();
			file.name = name;
			file.offset = offset;
			file.type = type;
			file.size = size;

			if (name.IndexOf(".js") != -1) 
			{ // render_metadata.js
				renderMetadata = JsonSerializer.Deserialize<RenderMetadata>(TGXMUtils.String(fileData,0,0));
				file.data = renderMetadata;
			} 
			else
			{
				file.data = fileData;
			}

			files.Add(name, file);
			//fileLookup.Add(name);
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

	public static void Convert(byte[] data, string fileOut, string game) 
	{	
		dynamic tgxBin = loadTGXBin(data);
		List<dynamic> renderMeshes = Parsers.parseTGXAsset(tgxBin);
		dynamic renderModel = new ExpandoObject();
		renderModel.meshes = renderMeshes;
		renderModel.name = "Model";
		List<dynamic> renderModels = new List<dynamic>();
		renderModels.Add(renderModel);

		WriteCollada.WriteFile(renderModels, fileOut, game);
	}

	public static void Convert(APIItemData[] binItems, string fileOut, string game) 
	{	
		List<dynamic> renderModels = new List<dynamic>();
		foreach (APIItemData itemContainers in binItems)
		{
			byte[][] geometry = itemContainers.geometry;
			byte[][] textures = itemContainers.texture;
			string name = itemContainers.name;
			
			dynamic renderModel = new ExpandoObject();
			List<dynamic> renderMeshes = new List<dynamic>();
			Dictionary<string,dynamic> renderTextures = new Dictionary<string,dynamic>();
			//JArray textureLookup = new JArray();
			List<dynamic> plates = new List<dynamic>();
			
			foreach (byte[] data in geometry)
			{
				dynamic tgxBin = loadTGXBin(data);
				if (tgxBin == null) continue;
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
					//textureLookup.Add(texture.name.Value);
				}
			}

			//renderTextures.names = textureLookup;
			
			renderModel.meshes = renderMeshes;
			renderModel.textures = renderTextures;
			renderModel.name = name;
			
			renderModels.Add(renderModel);
		}
		
		WriteCollada.WriteFile(renderModels, fileOut, game);
	}
}