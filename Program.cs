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
                            "[3] Convert item from D1 API\n" +
                            "[4] Quit\n"+
                            " > ");
                
                switch(Console.ReadLine().ToLower())
                {
                    case ("1"):
                        ReadLocal.Read();
                        break;
                    case ("2"):
                        apiSupport.convertByHash();
                        break;
                    case ("3"):
                        apiSupport.convertD1ByHash();
                        break;
                    case ("4"):
                        runMain = false;
                        break;
                    case ("debug"):
                        apiSupport.customCall();
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
