using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CSharpTGXMConverter
{
    // The main code of the program
    class Program
    {   
        private static JObject loadTGXBin(byte[] data) 
        {
            Console.WriteLine("Loading model data...");
            
            Console.Write("Reading TGXM header... ");
            string magic = TGXMUtils.String(data, 0x0, 0x4); // TGXM
            int version = TGXMUtils.Uint(data, 0x4);
            int fileOffset = TGXMUtils.Uint(data, 0x8);
            int fileCount = TGXMUtils.Uint(data, 0xC);
            string fileIdentifier = TGXMUtils.String(data, 0x10, 0x100);
            //if (magic != "TGXM") {
            //    console.error('Invalid TGX File', url);
            //    return;
            //}
            Console.WriteLine("Done.");

            dynamic files = new JArray();
            dynamic fileLookup = new JArray();
            dynamic renderMetadata = new JObject();
            for (var f=0; f<fileCount; f++) 
            {
                int headerOffset = fileOffset+(0x110*f);
                string name = TGXMUtils.String(data, headerOffset, 0x100);
                int offset = TGXMUtils.Uint(data, headerOffset+0x100);
                int type = TGXMUtils.Uint(data, headerOffset+0x104);
                int size = TGXMUtils.Uint(data, headerOffset+0x108);
                Console.WriteLine("Loading file \""+name+".\" File size: "+size+" bytes.");
                //byte[] fileData = Arrays.copyOfRange(data, offset, offset+size);
                byte[] fileData = new byte[size];
                Array.ConstrainedCopy(data, offset, fileData, 0, size);

                dynamic file = new JObject();
                file.name = name;
                file.offset = offset;
                file.type = type;
                file.size = size;

                //JSONParser jsonParser = new JSONParser();
                if (name.IndexOf(".js") != -1) 
                { // render_metadata.js
                    //try 
                    //{
                        //renderMetadata = (JSONObject) jsonParser.parse(TGXMUtils.string(fileData,0,0));
                        renderMetadata = JObject.Parse(TGXMUtils.String(fileData,0,0));
                        //file.put("data", renderMetadata);
                        file.data = renderMetadata;
                    //} catch (ParseException e)
                    //{}
                } 
                else
                {
                    //file.put("data", fileData);
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

        private static void converter() 
    	{
    		Console.Write("Input file location > ");
    		string fileIn = Console.ReadLine();

    		if (File.Exists(fileIn)) 
    		{
    			Console.Write("Output directory > ");
    			string fileOut = Console.ReadLine();

    			if (!Directory.Exists(fileOut)) 
    			{
    				Directory.CreateDirectory(fileOut);
    			}
    
    			byte[] data;

                Console.Write("Reading file... ");
    			using (BinaryReader inputStream = new BinaryReader(File.Open(fileIn, FileMode.Open)))
    			{
    				int fileSize = (int) (new FileInfo(fileIn).Length);
    
    				data = new byte[fileSize];
    
    				inputStream.Read(data,0,fileSize);

                    inputStream.Close();
    			}
                Console.WriteLine("Done.");
                
                JObject tgxBin = loadTGXBin(data);//new JSONObject();
    			JArray renderMeshes = Parsers.parseTGXAsset(tgxBin);

    			WriteFBX.Write(renderMeshes, fileOut);
    		}
        }
            
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            bool runConverter = true;
            while (runConverter) 
            {
                converter();

                while (true) 
                {
                    Console.Write("Convert another file? (Y/N) ");
                    string runAgain = "";
                    runAgain = Console.ReadLine();

                    if (runAgain == "Y" || runAgain == "y") 
                    {
                        break;
                    }
                    else if (runAgain == "N" || runAgain == "n") 
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
}
