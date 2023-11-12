using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.LogUlti
{
    public class FileUtil
    {
        public static string GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = Path.GetDirectoryName(Environment.ProcessPath);
            var folderpath = $"{appStartPath}\\{foldername}\\";

            if (filename.Contains(foldername))
            {
                return filename;
            }

            if(!File.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            var result = folderpath + filename;
            return result;
        }
    }
}
