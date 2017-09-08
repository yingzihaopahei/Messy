using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Globalization;

namespace Test
{
    public partial class testAES : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
 
            string en = AesEncrypt("210712", "25325634613534543229039428375923");

            string un = AesDecrypt(en, "25325634613534543229039428375923");


            string s = "";

        }


        private string GetMd5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes(str);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }


        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static string AesEncrypt(string str, string key)
        //{
        //    if (string.IsNullOrEmpty(str)) return null;
        //    Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);


        //    System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
        //    {
        //        //new byte[] { 37, 50, 86, 52, 97, 53, 52, 84, 50, 41, 3, -108, 40, 55, 89, 35 },
        //        //ConvertFrom(key);//
        //        //ConvertFrom(key, key.Length % 2 == 0 ? key.Length / 2 : (key.Length / 2)+1 ),//
        //        Key = ConvertFrom(key, key.Length % 2 == 0 ? key.Length / 2 : (key.Length / 2) + 1),//Encoding.UTF8.GetBytes(key),
        //        Mode = System.Security.Cryptography.CipherMode.ECB,
        //        Padding = System.Security.Cryptography.PaddingMode.PKCS7
        //    };

        //    System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
        //    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //}

        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);


            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            { 
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }




        public static byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数  
                {
                    strTemp = "0" + strTemp;//数位为奇数时前面补0  
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//高位在前  
            }
            catch
            { return null; }
        }
        /// <summary>  
        /// BCD码转换16进制(压缩BCD)  
        /// </summary>  
        /// <param name="strTemp"></param>  
        /// <returns></returns>  
        public static Byte[] ConvertFrom(string strTemp, int IntLen)
        {
            try
            {
                Byte[] Temp = ConvertFrom(strTemp.Trim());
                Byte[] return_Byte = new Byte[IntLen];
                if (IntLen != 0)
                {
                    if (Temp.Length < IntLen)
                    {
                        for (int i = 0; i < IntLen - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, IntLen - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            { return null; }
        }
        /// <summary>  
        /// 16进制转换BCD（解压BCD）  
        /// </summary>  
        /// <param name="AData"></param>  
        /// <returns></returns>  
        public static string ConvertTo(Byte[] AData)
        {
            try
            {
                StringBuilder sb = new StringBuilder(AData.Length * 2);
                foreach (Byte b in AData)
                {
                    sb.Append(b >> 4);
                    sb.Append(b & 0x0f);
                }
                return sb.ToString();
            }
            catch { return null; }
        }
    }
}