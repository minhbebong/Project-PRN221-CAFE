using System;
using System.IO;

namespace CafeShopFPT.LogUlti {
    public class FileUlti {

        public static string GetDestinationPath(string filename,string foldername) {

                string appStartPath = Path.GetDirectoryName(Environment.ProcessPath);
                var folderpath = $"{appStartPath}\\{foldername}\\";

                if (filename.Contains(foldername)) {
                    return filename;

                }
   
              
                if (!File.Exists(folderpath)) {
                    Directory.CreateDirectory(folderpath);
                }

            
            var result = folderpath + filename;
            return result;

        }

    }
}
