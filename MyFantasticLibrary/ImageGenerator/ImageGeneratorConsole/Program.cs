using ImageGeneratorCore;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace ImageGeneratorConsole
{
    class Program
    {
        /// <summary>
        /// Print to standard output Usage Information.
        /// </summary>
        private static void Usage()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string name = Path.GetFileName(codeBase);
            Console.WriteLine(name + " InputImagePath OutputImagePath");
        }
        /// <summary>
        /// Entry point of application.
        /// </summary>
        /// <param name="args">Arguments passed to program.</param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && (args[0] == "h" || args[0] == "H" ||
               args[0] == "/h" || args[0] == "/H" || args[0] == "-h" || args[0] == "-H"))
                {
                    Usage();
                    return;
                }
                else if (args.Length != 2)
                {
                    Console.WriteLine("Invalid params count!");
                    Console.WriteLine("Usage: ");
                    Usage();
                    return;
                }
                else
                {
                    string input = args[0];
                    string output = args[1];
                    Generator generator = new Generator();
                    generator.Generate(input, output, ImageFormat.Png);
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           

        }
    }
}
