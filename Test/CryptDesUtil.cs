using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace demoJsonHandle
{
	public class CryptDesUtil
	{
        //private static Logger logger = new Logger(ConfigurationKeys.LogPath, 0);
        private string sKey = string.Empty;
        private string a = string.Empty;
        public CryptDesUtil( string serverUrl )
        {
            this.a = serverUrl;
        }
        /// <summary>
        /// 设置加密KEY
        /// </summary>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns></returns>
        public bool SetKey( string encryptKey )
        {
            if ( encryptKey.Length == 8 )
            {
                sKey = encryptKey;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public string EncryptString( string encryptString )
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider( );
                byte[ ] inputByteArray;
                inputByteArray = Encoding.UTF8.GetBytes( encryptString );
                des.Key = Encoding.UTF8.GetBytes( sKey );
                des.IV = Encoding.UTF8.GetBytes( sKey );
                System.IO.MemoryStream ms = new System.IO.MemoryStream( );
                CryptoStream cs = new CryptoStream( ms, des.CreateEncryptor( ), CryptoStreamMode.Write );
                cs.Write( inputByteArray, 0, inputByteArray.Length );
                cs.FlushFinalBlock( );
                StringBuilder ret = new StringBuilder( );
                foreach ( byte b in ms.ToArray( ) )
                {
                    ret.AppendFormat( "{0:X2}", b );
                }
                return ret.ToString( );
            }
            catch ( Exception ex )
            {
                //logger.Log (0,"EncryptString：" + ex.Message);
            }
            return encryptString;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public string DecryptString( string decryptString )
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider( );
                int len;
                len = decryptString.Length / 2;
                byte[ ] inputByteArray = new byte[ len ];
                int x, i;
                for ( x = 0; x < len; x++ )
                {
                    i = Convert.ToInt32( decryptString.Substring( x * 2, 2 ), 16 );
                    inputByteArray[ x ] = ( byte ) i;
                }
                des.Key = Encoding.UTF8.GetBytes( sKey );
                des.IV = Encoding.UTF8.GetBytes( sKey );
                System.IO.MemoryStream ms = new System.IO.MemoryStream( );
                CryptoStream cs = new CryptoStream( ms, des.CreateDecryptor( ), CryptoStreamMode.Write );
                cs.Write( inputByteArray, 0, inputByteArray.Length );
                cs.FlushFinalBlock( );
                return Encoding.UTF8.GetString( ms.ToArray( ), 0, ms.ToArray( ).Length );
            }
            catch ( Exception ex )
            {
                //logger.Log(0,"DecryptString：" + ex.Message);
            }
            return decryptString;
        }

        /// <summary>
        /// 加密数组
        /// </summary>
        /// <param name="buffer">待加密的数组</param>
        /// <returns>加密成功返回加密后的数组，失败返源数组</returns>
        public Byte[ ] EncryptBytes( Byte[ ] buffer )
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider( );
                byte[ ] inputByteArray = buffer;
                des.Key = Encoding.UTF8.GetBytes( sKey );
                des.IV = Encoding.UTF8.GetBytes( sKey );
                System.IO.MemoryStream ms = new System.IO.MemoryStream( );
                CryptoStream cs = new CryptoStream( ms, des.CreateEncryptor( ), CryptoStreamMode.Write );
                cs.Write( inputByteArray, 0, inputByteArray.Length );
                cs.FlushFinalBlock( );
                return ms.ToArray( );
            }
            catch ( Exception ex )
            {
                //logger.Log(0,"EncryptBytes：" + ex.Message);
            }
            return buffer;
        }

        /// <summary>
        /// 解密数组
        /// </summary>
        /// <param name="buffer">待解密的数组</param>
        /// <returns>解密成功返回加密后的数组，失败返源数组</returns>
        public Byte[ ] DecryptBytes( Byte[ ] buffer )
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider( );
                byte[ ] inputByteArray = buffer;
                des.Key = Encoding.UTF8.GetBytes( sKey );
                des.IV = Encoding.UTF8.GetBytes( sKey );
                System.IO.MemoryStream ms = new System.IO.MemoryStream( );
                CryptoStream cs = new CryptoStream( ms, des.CreateDecryptor( ), CryptoStreamMode.Write );
                cs.Write( inputByteArray, 0, inputByteArray.Length );
                cs.FlushFinalBlock( );
                return ms.ToArray( );
            }
            catch ( Exception ex )
            {
                //logger.Log(0,"DecryptBytes：" + ex.Message);
            }
            return buffer;
        }


        #region CBC模式**

        /// <summary>
        /// DES3 CBC模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public byte[ ] Des3EncodeCBC( byte[ ] key, byte[ ] iv, byte[ ] data )
        {
            //复制于MSDN

            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream( );

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider( );
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream( mStream,
                    tdsp.CreateEncryptor( key, iv ),
                    CryptoStreamMode.Write );

                // Write the byte array to the crypto stream and flush it.
                cStream.Write( data, 0, data.Length );
                cStream.FlushFinalBlock( );

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[ ] ret = mStream.ToArray( );

                // Close the streams.
                cStream.Close( );
                mStream.Close( );

                // Return the encrypted buffer.
                return ret;
            }
            catch ( CryptographicException e )
            {
                Console.WriteLine( "A Cryptographic error occurred: {0}", e.Message );
                return null;
            }
        }

        /// <summary>
        /// DES3 CBC模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public byte[ ] Des3DecodeCBC( byte[ ] key, byte[ ] iv, byte[ ] data )
        {
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream( data );

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider( );
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream( msDecrypt,
                    tdsp.CreateDecryptor( key, iv ),
                    CryptoStreamMode.Read );

                // Create buffer to hold the decrypted data.
                byte[ ] fromEncrypt = new byte[ data.Length ];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read( fromEncrypt, 0, fromEncrypt.Length );

                //Convert the buffer into a string and return it.
                return fromEncrypt;
            }
            catch ( CryptographicException e )
            {
                Console.WriteLine( "A Cryptographic error occurred: {0}", e.Message );
                return null;
            }
        }

        #endregion
	}
}