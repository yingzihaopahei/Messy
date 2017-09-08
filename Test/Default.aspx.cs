using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;

namespace Test
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          //float f1=  levenshtein("123","123");
          //float f2 = levenshtein("123456", "123");
          //float f3 = levenshtein("123", "456");
          //float f4 = levenshtein("1223", "1233");
            CryptoBase64.Crypto oCrypto = new CryptoBase64.Crypto();
            string cardpan_A = oCrypto.DecTripleDes("", "", "vEH0q0JZmE25Pupe0ERlxA==", 1, 1);


            Response.Write(cardpan_A);

        }

        public static float levenshtein(String str1, String str2)
        {
            //计算两个字符串的长度。 
            int len1 = str1.Length;
            int len2 = str2.Length;
            //建立上面说的数组，比字符长度大一个空间 
            int[,] dif = new int[len1 + 1, len2 + 1];
            //赋初值，步骤B。 
            for (int a = 0; a <= len1; a++)
            {
                dif[a, 0] = a;
            }
            for (int a = 0; a <= len2; a++)
            {
                dif[0, a] = a;
            }
            //计算两个字符是否一样，计算左上的值 
            int temp;
            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    //取三个值中最小的 
                    dif[i, j] = min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1,
                    dif[i - 1, j] + 1);
                }
            }

            //计算相似度 
            float similarity = 1 - (float)dif[len1, len2] / Math.Max(str1.Length, str2.Length);
            return similarity;
        }

        //得到最小值 
        private static int min(params int[] arr)
        {
            int min = int.MaxValue;
            foreach (int i in arr)
            {
                if (min > i)
                {
                    min = i;
                }
            }
            return min;
        }
 

        public static int GetIntValue(char schar)
        {
            int strNum;
            switch (schar)
            {
                case 'A':
                    strNum = 10;
                    break;
                case 'B':
                    strNum = 11;
                    break;
                case 'C':
                    strNum = 12;
                    break;
                case 'D':
                    strNum = 13;
                    break;
                case 'E':
                    strNum = 14;
                    break;
                case 'F':
                    strNum = 15;
                    break;
                default:
                    strNum = schar;
                    break;
            }
            return strNum;
        }
        private static void ceshixor()
        {
            string a = "123abc";
            string b = "567bca";
            char[] ca = a.ToCharArray();
            char[] cb = b.ToCharArray();
            char[] cc = new char[a.Length];

            for (int i = 0; i < ca.Length; i++)
            {
                cc[i] = (char)(ca[i] ^ cb[i]);
            }
            string c = new string(cc);

            // 解密

            char[] ccjiemi = c.ToCharArray();
            char[] cccjiemi = new char[ccjiemi.Length];
            for (int i = 0; i < ccjiemi.Length; i++)
            {
                cccjiemi[i] = (char)(ccjiemi[i] ^ cb[i]);
            }

            string jiemi = new string(cccjiemi);
        }
     

        protected void Button1_Click(object sender, EventArgs e)
        {
            Hashtable headers = new Hashtable();
            headers.Add("MIPAddress", 1);
            headers.Add("AcctNo", 2);
            headers.Add("OrderID", 3);
            headers.Add("CardPAN", 4);
            headers.Add("Amount", 5);
            headers.Add("CurrCode", 6);
            headers.Add("IPAddress", 7);
            headers.Add("TxnType", 8);
            headers.Add("HashValue", 9);
            headers.Add("CardType", 0);
            headers.Add("CName", 11);
            StringBuilder sb = new StringBuilder();

            foreach (DictionaryEntry de in headers)
            {
                sb.Append(de.Key.ToString());
                sb.Append("=|=");
                sb.Append(de.Value.ToString());
                sb.Append("&&");
            }

            string infoStr = sb.ToString().Trim('&');
            infoStr = sb.ToString().Trim('&');

            L1.Text = infoStr;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            lb.Text = SHA256Encrypt("EncryptionMode=SHA256&CharacterSet=UTF8&merNo=100003&terNo=88816&orderNo=160601164054397&currencyCode=CNY&amount=0.10&payIP=192.99.144.89&transType=sales&transModel=M&9b9dc4fab5cc48e1b252a5b0904359b5");
        }

        private string GetMd5(string str)
        {
            string str2 = System.Web.HttpUtility.UrlEncode(str);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes(str2);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x");//.PadLeft(2, '0')
            }
            return sTemp;
        }


        public static string SHA256Encrypt(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.UTF8.GetBytes(str));
            s256.Clear();
            return BitConverter.ToString(byte1).Replace("-", "").ToLower(); //64
        }

        public string SHA256Encryp(string str)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.UTF8.GetBytes(str));
            s256.Clear();
            return BitConverter.ToString(byte1).Replace("-", "").ToLower(); //64
        }

        public string GetSHA256_HEBCB(string strData)
        {
            //使用SHA256加密算法：
            System.Security.Cryptography.SHA256 sha256 = new
            System.Security.Cryptography.SHA256Managed();
            byte[] sha256Bytes = System.Text.Encoding.Default.GetBytes(strData);
            byte[] cryString = sha256.ComputeHash(sha256Bytes);
            string sha256Str = string.Empty;
            for (int i = 0; i < cryString.Length; i++)
            {
                sha256Str += cryString[i].ToString("X2");
            }
            return sha256Str;
        }
    }
}
