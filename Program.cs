using System;
using System.Threading;
using System.Reflection;

namespace DestinyColladaGenerator
{
    class Program
    {   
        public static bool useTor = false;
        public static bool disableLodCulling;
        public static bool multipleFolderOutput;
        static int Main(string[] args)
        {
            bool runMain = true;

            if (args.Length > 0)
            {
                runMain = false;
                if (args[0].ToLower()=="--help"||args[0].ToLower()=="-h")
                {
                    Console.WriteLine("Usage: DestinyColladaGenerator.exe [<GAME>] [-o|--output <OUTPUTPATH>] [<HASHES>]");
                }
                else if (args[0].ToLower()=="--version"||args[0].ToLower()=="-v")
                {
                    
                    //Assembly[] assemblies = Thread.GetDomain().GetAssemblies();
                    //for (int i = 0; i < assemblies.Length; i++)
                    //{
                    //    if (string.Compare(assemblies[i].GetName().Name, "DestinyColladaGenerator") == 0)
                    //    {
                    //        assemblies[i].GetName().Version;
                    //    }
                    //}
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                    return 0;
                }
                else
                {
                    int firstHash = 1;
                    string output = "";
                    if (Array.IndexOf(args,"-o")!=-1) 
                    {
                        output = args[Array.IndexOf(args,"-o")+1];
                        firstHash += 2;
                    }
                    else if (Array.IndexOf(args,"--output")!=-1) 
                    {
                        output = args[Array.IndexOf(args,"--output")+1];
                        firstHash += 2;
                    }

                    if (Array.IndexOf(args,"-m")!=-1) 
                    {
                        multipleFolderOutput = args[Array.IndexOf(args,"-m")+1]=="true";
                        firstHash += 2;
                    }
                    else if (Array.IndexOf(args,"--multiplefolders")!=-1) 
                    {
                        multipleFolderOutput = args[Array.IndexOf(args,"-m")+1]=="true";
                        firstHash += 2;
                    }

                    string game = args[0]!="2"?"":"2";
                    string[] hashes = new string[args.Length-firstHash];
                    Array.ConstrainedCopy(args, firstHash, hashes, 0, args.Length-firstHash);
                    apiSupport.convertByHash(game, hashes, output);
                }
            }

            while (runMain)
            {
                Console.Write("Select an action:\n"+
                            "[1] Convert local files\n"+
                            "[2] Convert item from API\n"+
                            "[3] Convert item from D1 API\n" +
                            "[4] Quit\n"+
                            "[5] Enable multiple output folders\n" +
                            " > ");
                
                switch(Console.ReadLine().ToLower())
                {
                    case ("1"):
                        ReadLocal.Read();
                        break;
                    case ("2"):
                        apiSupport.convertByHash("2");
                        break;
                    case ("3"):
                        apiSupport.convertByHash("");
                        break;
                    case ("4"):
                        runMain = false;
                        break;
                    case ("5"):
                        multipleFolderOutput = !multipleFolderOutput;
                        Console.WriteLine($"Multiple folder output is now {multipleFolderOutput}");
                        break;
                    case ("tor"):
                        useTor = !useTor;
                        Console.WriteLine("TOR proxying enabled.");
                        break;
                    case ("debug"):
                        apiSupport.customCall();
                        break;
                    case ("cm1"):
                        apiSupport.convertContentManifest("");
                        break;
                    case ("cm2"):
                        apiSupport.convertContentManifest("2");
                        break;
                    case ("lod"):
                        disableLodCulling = !disableLodCulling;
                        Console.WriteLine($"LOD culling is now {disableLodCulling}.");
                        break;
                    case ("beep"):
                        Boop(Tone.Eflat4, Dura.EIGHTH3);
                        Boop(Tone.F4,     Dura.EIGHTH);
                        Boop(Tone.Gflat4, Dura.EIGHTH);
                        Boop(Tone.Aflat3, Dura.EIGHTH);
                        Boop(Tone.Bflat4, Dura.HALF);

                        Sloop(Dura.SIXTEENTH5);

                        Boop(Tone.Aflat4, Dura.SIXTEENTH);
                        Boop(Tone.Gflat4, Dura.SIXTEENTH);
                        Boop(Tone.F4,     Dura.EIGHTH);
                        Boop(Tone.Gflat4, Dura.EIGHTH);
                        Boop(Tone.Eflat4, Dura.HALF);
                        Boop(Tone.Gflat4, Dura.QUARTER);
                        Boop(Tone.Aflat4, Dura.WHOLE);

                        Sloop(Dura.HALF);

                        Boop(Tone.Eflat4, Dura.EIGHTH3);
                        Boop(Tone.F4,     Dura.EIGHTH);
                        Boop(Tone.Gflat4, Dura.EIGHTH);
                        Boop(Tone.Aflat3, Dura.EIGHTH);
                        Boop(Tone.C5, Dura.HALF);

                        Sloop(Dura.SIXTEENTH5);

                        Boop(Tone.Gflat4, Dura.SIXTEENTH);
                        Boop(Tone.Aflat4, Dura.SIXTEENTH);
                        Boop(Tone.Bflat4, Dura.EIGHTH);
                        Boop(Tone.Gflat4, Dura.EIGHTH);
                        Boop(Tone.Dflat5, Dura.HALF);
                        Boop(Tone.Bflat4, Dura.QUARTER);
                        Boop(Tone.F5,     Dura.HALF);
                        Boop(Tone.Aflat5, Dura.HALF);

                        Sloop(Dura.HALF);

                        Boop(Tone.C5,     Dura.HALF);
                        Boop(Tone.Aflat4, Dura.QUARTER);
                        Boop(Tone.C5,     Dura.QUARTER);
                        Boop(Tone.Dflat5, Dura.HALF);

                        Sloop(Dura.HALF);

                        Boop(Tone.Gflat4, Dura.EIGHTH);
                        Boop(Tone.Aflat4, Dura.EIGHTH);
                        Boop(Tone.Bflat4, Dura.HALF);
                        Boop(Tone.Dflat5, Dura.HALF);
                        Boop(Tone.Eflat5, Dura.QUARTER3);
                        Boop(Tone.Eflat5, Dura.QUARTER);
                        Boop(Tone.Eflat5, Dura.QUARTER3);
                        Boop(Tone.Dflat5, Dura.EIGHTH);
                        Boop(Tone.C5,     Dura.EIGHTH);
                        Boop(Tone.Dflat5, Dura.QUARTER3);
                        Boop(Tone.Dflat5, Dura.EIGHTH);
                        Boop(Tone.C5,     Dura.EIGHTH);
                        Boop(Tone.Bflat4, Dura.HALF);
                        Boop(Tone.Bflat4, Dura.QUARTER);
                        Boop(Tone.Aflat4, Dura.QUARTER);
                        Boop(Tone.Gflat4, Dura.WHOLE);
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
            return 0;
        }
        protected enum Tone
        {
        Aflat3 = 207,
        Eflat4 = 311,
        F4     = 349,
        Gflat4 = 370,
        Aflat4 = 415,
        Bflat4 = 466,
        C5     = 523,
        Dflat5 = 554,
        Eflat5 = 622,
        F5     = 698,
        Aflat5 = 831
        }
        protected enum Dura
        {
        WHOLE      = 2667, // 90 bpm
        HALF       = WHOLE/2,
        QUARTER    = HALF/2,
        QUARTER3   = QUARTER*3,
        EIGHTH     = QUARTER/2,
        EIGHTH3    = EIGHTH*3,
        SIXTEENTH  = EIGHTH/2,
        SIXTEENTH5 = SIXTEENTH*5
        }
        protected static void Boop(Tone toneVal, Dura duraVal)
        {
            Console.Beep((int) toneVal, (int) duraVal);
        }
        protected static void Sloop(Dura duraVal)
        {
            Thread.Sleep((int) duraVal);
        }
    }
}
