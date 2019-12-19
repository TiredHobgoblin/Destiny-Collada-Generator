using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

class WriteFBX
{
	public static void Write(JArray meshes, string writeLocation)
	{
		int fileNum = 0;
		while( File.Exists(writeLocation+@"\DestinyModel"+fileNum+@".fbx") ) 
		{
			fileNum++;
		}

		using (StreamWriter sw = new StreamWriter(writeLocation+@"\DestinyModel"+fileNum+@".fbx"))
		{
			
			
			sw.Close();
		}
	}
}