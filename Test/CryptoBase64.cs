using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace CryptoBase64
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class Crypto
    {
        public TripleDES m_des = new TripleDESCryptoServiceProvider();

        public string EncTripleDes(string skey, string sIV, string p_sData, int p_iHexFlag, int p_iBase64Flag)
        {
            string sData;
            string sRetStr;
            string szIV = sIV;  //"0000000000000000";
            string szKey = skey;  //"201007241630CAC1234567890ABCDEF201007241630CAC10";
            byte[] sz_Key = null;
            byte[] sz_IV = null;
            byte[] aucOutput = null;

            if (szKey == "")
                szKey = "2A1FC7241630CAC1234567890ABCDEF201007241630CAC10";
            if (szIV == "")
                szIV = "0000000000000000";

            try
            {
                if ((szKey.Length > 0 | szKey.Length <= 0))
                {
                    sz_Key = HexToByte(szKey.ToCharArray());
                    m_des.Key = sz_Key;
                }
                m_des.Mode = CipherMode.CBC;
                m_des.Padding = PaddingMode.ANSIX923;
                if ((szIV.Length > 0 | szIV.Length <= 0))
                {
                    sz_IV = HexToByte(szIV.ToCharArray());
                }
                m_des.IV = sz_IV;

                if (1 == p_iHexFlag)
                {
                    // Padding only required when convert from Hex to Byte with odd length
                    if (0 != p_sData.Length % 2)
                        sData = "0" + p_sData;
                    else
                        sData = p_sData;

                    aucOutput = Encrypt(HexToByte(sData.ToCharArray()));
                }
                else
                    aucOutput = Encrypt(CharToByte(p_sData.ToCharArray()));

                if (1 == p_iBase64Flag)
                    sRetStr = Convert.ToBase64String(aucOutput);
                else
                    sRetStr = sWriteHex(aucOutput);
            }
            catch (Exception ex)
            {
                sRetStr = ex.ToString();
            }
            return sRetStr;
        }

        public string DecTripleDes(string skey, string sIV, string p_sData, int p_iHexFlag, int p_iBase64Flag)
        {
            string sRetStr;
            byte[] aucInput;
            byte[] aucOutput;
            string szIV = sIV;  //"0000000000000000";
            string szKey = skey;  //"201007241630CAC1234567890ABCDEF201007241630CAC10";
            byte[] sz_Key = null;
            byte[] sz_IV = null;

            if (szKey == "")
                szKey = "2A1FC7241630CAC1234567890ABCDEF201007241630CAC10";
            if (szIV == "")
                szIV = "0000000000000000";

            if (1 == p_iBase64Flag)
            {
                aucInput = Convert.FromBase64String(p_sData);
            }
            else
            {
                aucInput = HexToByte(p_sData.ToCharArray());
            }

            try
            {
                if ((szKey.Length > 0 | szKey.Length <= 0))
                {
                    sz_Key = HexToByte(szKey.ToCharArray());
                    m_des.Key = sz_Key;
                }
                m_des.Mode = CipherMode.CBC;
                m_des.Padding = PaddingMode.ANSIX923;

                if ((szIV.Length > 0 | szIV.Length <= 0))
                {
                    sz_IV = HexToByte(szIV.ToCharArray());
                }
                m_des.IV = sz_IV;

                aucOutput = Decrypt(aucInput);

                if (1 == p_iHexFlag)
                    sRetStr = sWriteHex(aucOutput);
                else
                    sRetStr = ASCIIEncoding.ASCII.GetString(aucOutput);

                if (1 == p_iHexFlag && "0" == sRetStr.Substring(0, 1))
                    sRetStr = sRetStr.Substring(1);
            }
            catch (Exception ex)
            {
                sRetStr = ex.ToString();
            }
            return sRetStr;
        }


        public byte[] HexToByte(char[] hex)
        {
            byte[] output = new byte[hex.Length / 2];
            int i = 0;
            int k = 0;
            while (k < hex.Length)
            {
                string s = Convert.ToString(hex[k]) + Convert.ToString(hex[k + 1]);
                output[i] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                i += 1;
                k = k + 2;
            }
            return output;
        }

        public byte[] CharToByte(char[] p_acInput)
        {
            //          byte[] aucOutput = new byte[p_acInput.Length / 2];
            byte[] aucOutput = new byte[p_acInput.Length];
            int i = 0;
            while (i < p_acInput.Length)
            {
                aucOutput[i] = (byte)p_acInput[i];
                i += 1;
            }
            return aucOutput;
        }

        public byte[] Decrypt(byte[] p_aucInput)
        {
            byte[] aucOutput = Transform(p_aucInput, m_des.CreateDecryptor());
            return aucOutput;
        }

        public byte[] Encrypt(byte[] p_aucInput)
        {
            //			byte[] input = HexToByte(text.ToCharArray());
            byte[] aucOutput = Transform(p_aucInput, m_des.CreateEncryptor());
            return aucOutput;
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            byte[] result = null;
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and convert it back into byte array
            memStream.Position = 0;
            result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return result;
        }

        public string sWriteHex(byte[] bt_Array)
        {
            string szHex = "";
            int iCounter = 0;

            for (iCounter = 0; iCounter <= (bt_Array.Length - 1); iCounter++)
            {
                szHex += bt_Array[iCounter].ToString("X2");
            }

            return szHex;
        }

        public string ConvertToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }
    }
}
