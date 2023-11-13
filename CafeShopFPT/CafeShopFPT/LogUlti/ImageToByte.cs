using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.LogUlti
{
   public class ImageToByte
    {
      public  byte[] ReadFile(string sPath) {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath,FileMode.Open,FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes
            //to read from file.
            //In this case we want to read entire file.
            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);

            return data;
        }
    }
}
