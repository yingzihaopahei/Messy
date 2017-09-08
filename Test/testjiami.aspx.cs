using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using CryptoBase64;

namespace Test
{
    public partial class testjiami1 : System.Web.UI.Page
    {
        private static readonly string keyVersion = "";
        private static readonly string version = "1.0";
        private static readonly string aesKey = "abc";
        private static readonly string apiUrl = "http://124.172.237.75:8021/GateWay.aspx";
        private static readonly string serviceHost = "124.172.237.75:8021";
        protected void Page_Load(object sender, EventArgs e)
        {
            Crypto o = new Crypto();


            Response.Write(o.EncTripleDes("", "", "123456789101234", 1, 1));
            Response.Write("<br/>");
            Response.Write(o.DecTripleDes("", "", "g9NHEukBbaeJr/Enl5E/PQ==", 1, 1));
            //Response.Write("<br/>");
            //Response.Write(o.EncTripleDes("", "", "123456", 0, 0));
            //Response.Write("<br/>");
            //Response.Write(o.EncTripleDes("", "", "123456", 0, 1));
            //Response.Write("<br/>");
            //Response.Write(o.EncTripleDes("", "", "123456", 1, 0));


           byte by = byte.Parse("2A", System.Globalization.NumberStyles.HexNumber);


        }
        public static byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            //复制于MSDN  
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值  
                tdsp.Padding = PaddingMode.PKCS7;       //默认值  
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        private static string Md5Encrypt(string md5Str)
        {
            var md5Obj = System.Security.Cryptography.MD5.Create();
            var sBuilder = new StringBuilder();
            byte[] data = md5Obj.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static string AesEncrypt(string toEncrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = SHA256(key);

            rDel.Mode = System.Security.Cryptography.CipherMode.CBC;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rDel.BlockSize = 128;
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
            rDel.IV = iv;
            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string toDecrypt, string key)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = SHA256(key);
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
                rDel.IV = iv;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public static byte[] SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.SHA256Managed Sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return by;
        }
        public static string Post(string apiName, string keyVersion, string version, string postData)
        {
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            byte[] bytes = encode.GetBytes(postData);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
            webRequest.ProtocolVersion = new Version("1.1");
            webRequest.SendChunked = true;
            webRequest.Host = serviceHost;
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            webRequest.ContentLength = bytes.Length;

            webRequest.Headers.Add("apiName", apiName);
            webRequest.Headers.Add("keyVersion", keyVersion);
            webRequest.Headers.Add("version", version);


            Stream outStream = webRequest.GetRequestStream();
            outStream.Write(bytes, 0, bytes.Length);
            outStream.Close();

            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            StreamReader readStream = newStreamReader(stream, encode);
            Char[] read = new Char[256];
            var count = readStream.Read(read, 0, 256);
            var result = string.Empty;
            while (count > 0)
            {
                result += new String(read, 0, count);
                count = readStream.Read(read, 0, 256);
            }
            readStream.Close();
            webResponse.Close();

            return result;
        }

        private static StreamReader newStreamReader(Stream stream, Encoding encode)
        {
            throw new NotImplementedException();
        }


        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Default.GetBytes(str));
            s256.Clear();
            return BitConverter.ToString(byte1).Replace("-", "").ToLower(); //64
        }

    }
}