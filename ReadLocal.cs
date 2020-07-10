using System;
using System.IO;

namespace DestinyColladaGenerator
{
	public static class ReadLocal
	{
		public static void Read()
		{
			bool runConverter = true;
			while (runConverter) 
			{
				Console.Write("Game > ");
				string game = Console.ReadLine();
				
				Console.Write("Input file location > ");
				string fileIn = Console.ReadLine();

				Console.Write("Output directory > ");
				string fileOut = Console.ReadLine();
				if (fileOut == "") fileOut = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Output");

				if (!Directory.Exists(fileOut)) 
				{
					Directory.CreateDirectory(fileOut);
				}

				if (File.Exists(fileIn)) 
				{
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

					Converter.Convert(data, fileOut, game);
				}
				
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
}