using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CSharpTGXMConverter
{
    class Program
    {   
        static void Main(string[] args)
        {
            bool runMain = true;
            while (runMain)
            {
                Console.Write("Select an action:\n"+
                            "[1] Convert local files\n"+
                            "[2] Convert item from API\n"+
                            "[3] Search manifest by item name\n"+
                            //"[4] Update local manifest\n"+
                            "[4] Quit\n"+
                            " > ");
                
                switch(Console.ReadLine())
                {
                    case ("1"):
                        ReadLocal.Read();
                        break;
                    case ("2"):
                        apiSupport.convertByHash();
                        break;
                    case ("3"):
                        break;
                    //case ("4"):
                    //    apiSupport.updateLocalManifest();
                    //    break;
                    case ("4"):
                        runMain = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
