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
        public byte[] ReadFile(string sPath)
        {
            //Tao mang byte voi gia tri null ban dau
            byte[] data = null;
            //            Sử dụng đối tượng FileInfo để lấy kích thước tệp.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            //            Mở FileStream để đọc tệp
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            //            Sử dụng BinaryReader để đọc luồng tệp vào mảng byte.
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int)numBytes);
            return data;
        }
    }
}
