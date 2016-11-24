using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

/**
 * This tool reads the _variables.scss file that is included in the fontAwesome download.
 * Based on this file a IconType.cs file is generated.
 */
namespace IconTypeGenerator
{
    public class IconTypeGenerator
    {
        public static void Main(string[] args)
        {
            String usage = @"Usage : IconTypeGenerator {path to dir containing _variable.css}\n 
                Example : IconTypeGenerator C:\Users\frank\Downloads\font-awesome-4.7.0\font-awesome-4.7.0\scss";

            if (args.Length == 0)
            {
                Console.WriteLine(usage);
            }
            else
            {
                if (!File.Exists(args[0] + @"\_variables.scss"))
                {
                    Console.WriteLine(args[0] + " file could not be found.");
                    Console.WriteLine(usage);
                }
                else
                {
                    string output = "namespace FontAwesomeIcons\n";
                    output += "{\n";
                    output += "    public enum IconType\n";
                    output += "    {\n";


                    var content = File.ReadAllLines(args[0] + @"\_variables.scss");
                    foreach (var line in content)
                    {
                        if (line.Contains(@"\f"))
                        {
                            var splitLine = line.Split(':');
                            var name = splitLine[0];
                            name = name.Replace("$fa-var-", "");

                            name = name.Replace('-', ' ');
                            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name); //capitalizes every word in the string
                            name = name.Replace(" ", String.Empty);
                            if (char.IsDigit(name[0])) name = "_" + name;

                            var value = splitLine[1];
                            value = value.Replace("\"\\", "");
                            value = value.Replace("\";", "");
                            value = "0x" + value.Trim();

                            var result = name + " = " + value + ",";

                            output += "        " + result + "\n";
                        }
                    }

                    output += "    }\n";
                    output += "}\n";

                    File.WriteAllText(args[0] + @"\IconType.cs", output);

                    Console.WriteLine("\n\nExport completed...");
                    Console.WriteLine(@"\n\nGenerated file is located at: " + args[0] + @"\IconType.cs");
                }
            }
        }
    }
}
