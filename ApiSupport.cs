using System;
using System.IO;
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

			List<byte[]> TGXMs = new List<byte[]>();
			foreach (string itemHash in itemHashes)
			{
				Console.Write("Calling item definition from manifest... ");
				dynamic itemDef = makeCallJson($@"https://lowlidev.com.au/destiny/api/gearasset/{itemHash}?destiny2");
				Console.WriteLine("Done.");

				JArray geometries = itemDef.gearAsset.content[0].geometry;

				for (int g=0; g<geometries.Count; g++)
				{
					byte[] TGXM = makeCall($@"https://www.bungie.net/common/destiny2_content/geometry/platform/mobile/geometry/{geometries[g]}");
					TGXMs.Add(TGXM);
				}
			}
			Converter.Convert(TGXMs.ToArray(), fileOut);

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
}