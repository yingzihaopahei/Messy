using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace Test
{
    public partial class testIP : System.Web.UI.Page
    {

        public static bool EnableFileWatch = true;
        private static int offset;
        private static uint[] index = new uint[256];
        private static byte[] dataBuffer;
        private static byte[] indexBuffer;
        private static long lastModifyTime = 0L;
        private static string ipFile;
        private static readonly object @lock = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
             


            //uint n = BytesToLong(byte.Parse("1"), byte.Parse("2"), byte.Parse("3"), byte.Parse("4"));
            //string m = string.Empty;




            string aa = @"D:\17monipdb\17monipdb.dat";
            IPCountryHelper.Load(aa);



            string N = IPCountryHelper.Find("14.23.92.186")[0].ToString();

            string n = string.Empty;



            //string filename = @"D:\17monipdb\17monipdb.dat";
            //ipFile = new FileInfo(filename).FullName;
            //lock (@lock)
            //{
            //    var file = new FileInfo(ipFile);
            //    lastModifyTime = file.LastWriteTime.Ticks;
            //    try
            //    {
            //        dataBuffer = new byte[file.Length];
            //        using (var fin = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            //        {
            //            fin.Read(dataBuffer, 0, dataBuffer.Length);
            //        }

            //        var indexLength = BytesToLong(dataBuffer[0], dataBuffer[1], dataBuffer[2], dataBuffer[3]);
            //        indexBuffer = new byte[indexLength];
            //        Array.Copy(dataBuffer, 4, indexBuffer, 0, indexLength);
            //        offset = (int)indexLength;

            //        for (var loop = 0; loop < 256; loop++)
            //        {
            //            index[loop] = BytesToLong(indexBuffer[loop * 4 + 3], indexBuffer[loop * 4 + 2],
            //                indexBuffer[loop * 4 + 1],
            //                indexBuffer[loop * 4]);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}





            //var start = 0;
            //var max_comp_len = offset - 1028;
            //long ip2long_value;
            //long index_offset = -1;
            //var index_length = -1;
            //byte b = 0;
            //List<string> l = new List<string>();
            //for (start = start * 8 + 1024; start < max_comp_len; start += 8)
            //{
            //    ip2long_value = BytesToLong(indexBuffer[start + 0], indexBuffer[start + 1], indexBuffer[start + 2],
            //                 indexBuffer[start + 3]);
            //    index_offset = BytesToLong(b, indexBuffer[start + 6], indexBuffer[start + 5],
            //        indexBuffer[start + 4]);
            //    index_length = 0xFF & indexBuffer[start + 7];

            //    var areaBytes = new byte[index_length];
            //    Array.Copy(dataBuffer, offset + (int)index_offset - 1024, areaBytes, 0, index_length);
            //    l.Add(ip2long_value + ":" + Encoding.UTF8.GetString(areaBytes).ToString());

            //}


            //File.AppendAllLines(@"C:\Users\Fan\Desktop\IP\ip.txt",  l.ToArray());

            ////60.16.27.222
            ////uint n = BytesToLong(byte.Parse("60"), byte.Parse("16"), byte.Parse("27"), byte.Parse("0"));
            ////string h = string.Empty;
        }

 
        private static uint BytesToLong(byte a, byte b, byte c, byte d)
        {
            return ((uint)a << 24) | ((uint)b << 16) | ((uint)c << 8) | d;
        }



    }
}