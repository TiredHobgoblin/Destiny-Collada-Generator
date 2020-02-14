using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Methods for accessing the bungie.net web api
class apiSupport
{
	private static string apiKey = null;
	private static string apiRoot = @"https://www.bungie.net/Platform";
	
	public static JObject makeCallJson(string url)
	{
		using (var client = new HttpClient())
		{	
			client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

			var response = client.GetAsync(url).Result;
			var content = response.Content.ReadAsStringAsync().Result;
			//dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
			dynamic item = JObject.Parse(content);

			return item;
		}
	}

	public static string makeCallString(string url)
	{
		using (var client = new HttpClient())
		{	
			client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

			var response = client.GetAsync(url).Result;
			return response.Content.ReadAsStringAsync().Result;
		}
	}

	public static byte[] makeCall(string url)
	{
		using (var client = new HttpClient())
		{	
			client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

			var response = client.GetAsync(url).Result;
			var content = response.Content.ReadAsByteArrayAsync().Result;

			return content;
		}
	}
	
	public static void updateLocalManifest()
	{
		Console.Write("Requesting latest api manifest...");
		JObject manifestJson = makeCallJson(apiRoot+"/Destiny2/Manifest/");
		Console.WriteLine("Received.");
		
		Console.Write("Updating local copy...");
		using (StreamWriter manifestWriter = new StreamWriter(@"Resources\localManifest.json"))
		{
			manifestWriter.Write(manifestJson.ToString());
		}
		Console.WriteLine("Done.");
	}

	public static void convertByHash()
	{
		bool runConverter = true;
		while (runConverter) 
		{
			Console.Write("Input item hash(es) > ");
			string[] itemHashes = Console.ReadLine().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

			Console.Write("Output directory > ");
			string fileOut = Console.ReadLine();
			if (fileOut == "") fileOut = "Output";

			if (!Directory.Exists(fileOut)) 
			{
				Directory.CreateDirectory(fileOut);
			}
			
			List<APIItemData> items = new List<APIItemData>();

			List<string> names = new List<string>();
			List<int> counts = new List<int>();

			foreach (string itemHash in itemHashes)
			{
				Console.Write("Calling item definition from manifest... ");
				dynamic itemDef = makeCallJson($@"https://lowlidev.com.au/destiny/api/gearasset/{itemHash}?destiny2");
				Console.WriteLine("Done.");
				
				APIItemData itemContainers = new APIItemData();
				
				List<byte[]> geometryContainers = new List<byte[]>();
				List<byte[]> textureContainers = new List<byte[]>();
				string itemName = itemDef.definition.displayProperties.name.Value;

				int nameIndex = names.IndexOf(itemName);
				if (nameIndex == -1)
				{
					names.Add(itemName);
					counts.Add(0);
				}
				else
				{
					counts[nameIndex]++;
					itemName += "-"+counts[nameIndex];
				}

				JArray geometries = itemDef.gearAsset.content[0].geometry;
				JArray textures = itemDef.gearAsset.content[0].textures;
				
				if (itemDef.gearAsset.content[0].region_index_sets != null)
				{
					for (int g=0; g<geometries.Count; g++)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainers.Add(geometryContainer);
					}
					
					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = itemName;
					items.Add(itemContainers);
				}
				else if ((itemDef.gearAsset.content[0].female_index_set != null) && (itemDef.gearAsset.content[0].male_index_set != null))
				{
					dynamic mSet = itemDef.gearAsset.content[0].male_index_set;
					dynamic fSet = itemDef.gearAsset.content[0].female_index_set;
					
					foreach (int g in mSet.geometry)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainers.Add(geometryContainer);
					}
					
					//foreach (int t in mSet.textures)
					//{
					//	byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
					//	textureContainers.Add(textureContainer);
					//}

					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = "Male_"+itemName;
					items.Add(itemContainers);
					
					
					
					APIItemData itemContainersFemale = new APIItemData();
					List<byte[]> geometryContainersFemale = new List<byte[]>();
					List<byte[]> textureContainersFemale = new List<byte[]>();
					
					foreach (int g in fSet.geometry)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainersFemale.Add(geometryContainer);
					}
					
					//foreach (int t in fSet.textures)
					//{
					//	byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
					//	textureContainersFemale.Add(textureContainer);
					//}

					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainersFemale.Add(textureContainer);
					}
					
					itemContainersFemale.geometry = geometryContainersFemale.ToArray();
					itemContainersFemale.texture = textureContainersFemale.ToArray();
					itemContainersFemale.name = "Female_"+itemName;
					items.Add(itemContainersFemale);
				}
				else if (geometries == null && textures.Count != 0)
				{
					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = itemName;
					items.Add(itemContainers);
				}
				else
				{
					Console.WriteLine(itemName + " has no geometry or textures, or is missing a gendered index set.");
				}
			}
			Converter.Convert(items.ToArray(), fileOut);

			//using (StreamWriter output = new StreamWriter(@"Output\format.json"))
			//{
			//    output.Write(TGXM);
			//}
			
			while (true) 
			{
				Console.Write("Convert another file? (Y/N) ");
				string runAgain = "";
				runAgain = Console.ReadLine();

				if (runAgain.ToUpper() == "Y") 
				{
					break;
				}
				else if (runAgain.ToUpper() == "N") 
				{
					runConverter = false;
					break;
				}
				else 
				{
					Console.WriteLine("Invalid input");
				}
			}
		}
	}

	public static void convertD1ByHash()
	{
		bool runConverter = true;
		while (runConverter) 
		{
			Console.Write("Input item hash(es) > ");
			string[] itemHashes = Console.ReadLine().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

			Console.Write("Output directory > ");
			string fileOut = Console.ReadLine();
			if (fileOut == "") fileOut = "Output";

			if (!Directory.Exists(fileOut)) 
			{
				Directory.CreateDirectory(fileOut);
			}
			
			List<APIItemData> items = new List<APIItemData>();

			foreach (string itemHash in itemHashes)
			{
				Console.Write("Calling item definition from manifest... ");
				dynamic itemDef = makeCallJson($@"https://lowlidev.com.au/destiny/api/gearasset/{itemHash}?destiny");
				Console.WriteLine("Done.");
				
				APIItemData itemContainers = new APIItemData();
				
				List<byte[]> geometryContainers = new List<byte[]>();
				List<byte[]> textureContainers = new List<byte[]>();
				string itemName = itemDef.definition.itemName;

				JArray geometries = itemDef.gearAsset.content[0].geometry;
				JArray textures = itemDef.gearAsset.content[0].textures;
				
				if (itemDef.gearAsset.content[0].region_index_sets != null)
				{
					for (int g=0; g<geometries.Count; g++)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainers.Add(geometryContainer);
					}
					
					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = itemName;
					items.Add(itemContainers);
				}
				else if ((itemDef.gearAsset.content[0].female_index_set != null) && (itemDef.gearAsset.content[0].male_index_set != null))
				{
					dynamic mSet = itemDef.gearAsset.content[0].male_index_set;
					dynamic fSet = itemDef.gearAsset.content[0].female_index_set;
					
					foreach (int g in mSet.geometry)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainers.Add(geometryContainer);
					}
					
					//foreach (int t in mSet.textures)
					//{
					//	byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
					//	textureContainers.Add(textureContainer);
					//}

					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = "Male_"+itemName;
					items.Add(itemContainers);
					
					
					
					APIItemData itemContainersFemale = new APIItemData();
					List<byte[]> geometryContainersFemale = new List<byte[]>();
					List<byte[]> textureContainersFemale = new List<byte[]>();
					
					foreach (int g in fSet.geometry)
					{
						byte[] geometryContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/geometry/{geometries[g]}");
						geometryContainersFemale.Add(geometryContainer);
					}
					
					//foreach (int t in fSet.textures)
					//{
					//	byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
					//	textureContainersFemale.Add(textureContainer);
					//}

					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainersFemale.Add(textureContainer);
					}
					
					itemContainersFemale.geometry = geometryContainersFemale.ToArray();
					itemContainersFemale.texture = textureContainersFemale.ToArray();
					itemContainersFemale.name = "Female_"+itemName;
					items.Add(itemContainersFemale);
				}
				else if (geometries == null && textures.Count != 0)
				{
					for (int t=0; t<textures.Count; t++)
					{
						byte[] textureContainer = makeCall($@"https://www.bungie.net/common/destiny_content/geometry/platform/mobile/textures/{textures[t]}");
						textureContainers.Add(textureContainer);
					}
					
					itemContainers.geometry = geometryContainers.ToArray();
					itemContainers.texture = textureContainers.ToArray();
					itemContainers.name = itemName;
					items.Add(itemContainers);
				}
				else
				{
					Console.WriteLine(itemName + " has no geometry or textures, or is missing a gendered index set.");
				}
			}
			Converter.Convert(items.ToArray(), fileOut);

			//using (StreamWriter output = new StreamWriter(@"Output\format.json"))
			//{
			//    output.Write(TGXM);
			//}
			
			while (true) 
			{
				Console.Write("Convert another file? (Y/N) ");
				string runAgain = "";
				runAgain = Console.ReadLine();

				if (runAgain.ToUpper() == "Y") 
				{
					break;
				}
				else if (runAgain.ToUpper() == "N") 
				{
					runConverter = false;
					break;
				}
				else 
				{
					Console.WriteLine("Invalid input");
				}
			}
		}
	}

	public static void customCall()
	{
		Console.Write("Key > ");
		string userKey = Console.ReadLine();
		if (userKey != "") apiKey = userKey;

		Console.Write("Call > ");
		string callContent = Console.ReadLine();

		Console.Write("Output name > ");
		string fileOut = Console.ReadLine();
		if (fileOut == "") fileOut = "Response.txt";

		byte[] callResponse = makeCall(callContent);

		using (BinaryWriter output = new BinaryWriter(File.Open(fileOut, FileMode.Create)))
		{
		    output.Write(callResponse);
		}

		Console.WriteLine("Done.");
	}
}