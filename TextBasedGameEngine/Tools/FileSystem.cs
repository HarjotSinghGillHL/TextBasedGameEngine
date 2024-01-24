using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class HL_FileSystem
    {
        void ReadFile(string FilePath)
        {
            string filePath = "@"+FilePath;
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] The file [" + FilePath+ "] could not be read");
            }
        }
    }
}
