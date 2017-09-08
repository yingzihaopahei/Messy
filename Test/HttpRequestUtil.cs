using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections;
using System.IO;
using System.Configuration;


namespace CCPGW
{

    /// <summary>
    /// Http请求辅助类
    /// </summary>
    public class HttpRequestUtil : IDisposable
    {
        private int bufferLength = 1024;
        /// <summary>
        /// Content-Type 值定义
        /// </summary>
        public const string Content_Type = "application/x-www-form-urlencoded";   // Zip 类型的
        public const string Content_Type_Xml = "text/xml";          // XML 类型的 
        public const string Content_Type_Octet_Stream = "application/octet-stream"; // 八进制流
        /// <summary>
        /// Http 头信息 Action
        /// </summary>
        public const string Http_Header_Action = "Action";

        /// <summary>
        /// Http 头信息，数据格式
        /// </summary>
        public const string Http_Header_Data_Format = "Data-Format";

        /// <summary>
        /// Http 头信息 Content-Type
        /// </summary>
        public const string Http_Header_Content_Type = "Content-Type";

        /// <summary>
        /// 内容长度
        /// </summary>
        public const string Http_Header_Content_Length = "Content-Length";

        /// <summary>
        /// 客户端类型 Client-Type
        /// </summary>
        public const string Http_Header_Client_Type = "Client-Type";
        private HttpWebRequest httpRequest;
        private HttpWebResponse httpResponse;
        private string serverUrl;

       
        private string m_szLogPath;
        private string m_szLogName;
        private int m_szLogLevel;
        private int timeOut= 60 * 1000;

        private string referer = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUrl">发送网址</param>
        /// <param name="A_oLogger">日志</param>
        /// <param name="TimeOut">超时时间/秒（小于等于0时用默认值60秒）</param>
        public HttpRequestUtil(string serverUrl,  int TimeOut)
        {
            //m_szLogPath = ConfigurationManager.AppSettings["PaymentLogPath"].ToString();
            //m_szLogName = ConfigurationManager.AppSettings["PaymentLogName"].ToString();
            //m_szLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings["LogLevel"].ToString());
            if (ConfigurationManager.AppSettings["HEB_YHFreferer"] != null && ConfigurationManager.AppSettings["HEB_YHFreferer"].ToString() != string.Empty)
            {
                referer = ConfigurationManager.AppSettings["HEB_YHFreferer"].ToString();
            } 
           
            this.serverUrl = serverUrl;
            if (TimeOut > 0 )
            {
                this.timeOut = timeOut * 1000;
            }  
        }

        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }

        private void SetHttpRequest(Hashtable headers)
        {
            foreach (DictionaryEntry objDE in headers)
            {
                try
                {
                    string strKey = objDE.Key.ToString();
                    string strValue = objDE.Value.ToString();

                    // 已设置过了 httpRequest.ContentType = GeneralConstants.Content_Type_Zip
                    // 如果用 httpRequest.Headers[Content-Type] = XXX 会报错
                    if (strKey == "Content-Type")
                    {
                        continue;
                    }

                    httpRequest.Headers[strKey] = strValue;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private string SetHttpRequests(Hashtable headers)
        {
            string s = "";
            foreach (DictionaryEntry objDE in headers)
            {
                try
                {
                    string strKey = objDE.Key.ToString();
                    string strValue = objDE.Value.ToString();
                    //"p1=x&p2=y&p3=测试的中文";
                    s += strKey + "=" + strValue + "&";
                }
                catch (Exception ex)
                {
                    
                }
            }
            s = s.Substring(0, s.Length - 1);
          
            return s;
        }
        public string SendMessage(Hashtable headers)
        {
            return SendMessageA(headers);
        }

        /// <summary>
        /// 业务数据传输
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string SendMessageA(Hashtable headers)
        {
            // 接收返回结果变量
            string result = string.Empty;
            try
            {
                httpRequest = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
                httpRequest.Timeout = timeOut; // 设置超时时间
                httpRequest.ContentType = Content_Type;
                httpRequest.Method = "POST"; // 设置为POST方式上传数据
                if (referer != string.Empty)
                {
                    httpRequest.Referer = referer;
                } 
                // 设置请求头
                //  SetHttpRequest(headers);
                string paraUrlCoded = SetHttpRequests(headers);

                byte[] originalBuffer = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);

                httpRequest.ContentLength = originalBuffer.Length;

                DateTime startTime = DateTime.Now;
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    int writeIndex = 0;
                    int writeLength = 0;
                    while (writeIndex < originalBuffer.Length)
                    {
                        if (writeIndex + bufferLength < originalBuffer.Length)
                        {
                            writeLength = bufferLength;
                        }
                        else
                        {
                            writeLength = originalBuffer.Length - writeIndex;
                        }
                        requestStream.Write(originalBuffer, writeIndex, writeLength);
                        requestStream.Flush();
                        writeIndex += writeLength;
                    }
                }
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                string interval = DateTime.Now.Subtract(startTime).Duration().ToString();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    MemoryStream memStream = null;
                    string responseContentType = httpResponse.Headers[Http_Header_Content_Type]; 
                    int responseContentLength = 0;
                    try
                    {
                        responseContentLength = Convert.ToInt32(httpResponse.Headers[Http_Header_Content_Length]);
                       
                    }
                    catch (Exception ex)
                    {
                        //logger.Error("contentlength转化: " + httpResponse.Headers[Http_Header_Content_Length]);
                        //logger.Error(ex.ToString());
                    }
                    using (Stream outputStream = httpResponse.GetResponseStream())
                    {
                      
                        memStream = new MemoryStream();
                        byte[] buffer = new byte[bufferLength];
                        int readLength, readIndex = 0;
                        if (responseContentLength!=0)
                        {
                            while (readIndex < responseContentLength)
                            {

                                readLength = outputStream.Read(buffer, 0, buffer.Length);
                                memStream.Write(buffer, readIndex, readLength);
                                memStream.Flush();
                                readIndex += readLength;

                            }
                        }
                        else
                        {
                            do
                            {
                                readLength = outputStream.Read(buffer, 0, buffer.Length);
                                memStream.Write(buffer, readIndex, readLength);
                                memStream.Flush();
                                readIndex += readLength;
                            } while (readLength ==buffer.Length);
                        }
                            
                       
                    }

                    if (memStream.Length > 0)
                    {
                        byte[] resultBuffer = memStream.ToArray();
                        byte[] temp;

                        temp = resultBuffer;
                        result = System.Text.Encoding.UTF8.GetString(temp, 0, temp.Length);
                    }
                    memStream.Close();
                }
            }
            catch (WebException webEx)
            {
             
            }
            catch (Exception ex)
            {
                 
            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Close();
                }
            }
       
            return result;
        }


        public string SendMessageB(string str)
        {
            // 接收返回结果变量
            string result = string.Empty;
            try
            {
                httpRequest = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
                httpRequest.Timeout = timeOut; // 设置超时时间
                httpRequest.ContentType = Content_Type;
                httpRequest.Method = "POST"; // 设置为POST方式上传数据
                if (referer != string.Empty)
                {
                    httpRequest.Referer = referer;
                }
                // 设置请求头
                //  SetHttpRequest(headers);
                string paraUrlCoded = str;

                byte[] originalBuffer = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);

                httpRequest.ContentLength = originalBuffer.Length;

                DateTime startTime = DateTime.Now;
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    int writeIndex = 0;
                    int writeLength = 0;
                    while (writeIndex < originalBuffer.Length)
                    {
                        if (writeIndex + bufferLength < originalBuffer.Length)
                        {
                            writeLength = bufferLength;
                        }
                        else
                        {
                            writeLength = originalBuffer.Length - writeIndex;
                        }
                        requestStream.Write(originalBuffer, writeIndex, writeLength);
                        requestStream.Flush();
                        writeIndex += writeLength;
                    }
                }
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                string interval = DateTime.Now.Subtract(startTime).Duration().ToString();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    MemoryStream memStream = null;
                    string responseContentType = httpResponse.Headers[Http_Header_Content_Type];
                    int responseContentLength = 0;
                    try
                    {
                        responseContentLength = Convert.ToInt32(httpResponse.Headers[Http_Header_Content_Length]);

                    }
                    catch (Exception ex)
                    {
                        //logger.Error("contentlength转化: " + httpResponse.Headers[Http_Header_Content_Length]);
                        //logger.Error(ex.ToString());
                    }
                    using (Stream outputStream = httpResponse.GetResponseStream())
                    {

                        memStream = new MemoryStream();
                        byte[] buffer = new byte[bufferLength];
                        int readLength, readIndex = 0;
                        if (responseContentLength != 0)
                        {
                            while (readIndex < responseContentLength)
                            {

                                readLength = outputStream.Read(buffer, 0, buffer.Length);
                                memStream.Write(buffer, readIndex, readLength);
                                memStream.Flush();
                                readIndex += readLength;

                            }
                        }
                        else
                        {
                            do
                            {
                                readLength = outputStream.Read(buffer, 0, buffer.Length);
                                memStream.Write(buffer, readIndex, readLength);
                                memStream.Flush();
                                readIndex += readLength;
                            } while (readLength == buffer.Length);
                        }


                    }

                    if (memStream.Length > 0)
                    {
                        byte[] resultBuffer = memStream.ToArray();
                        byte[] temp;

                        temp = resultBuffer;
                        result = System.Text.Encoding.UTF8.GetString(temp, 0, temp.Length);
                    }
                    memStream.Close();
                }
            }
            catch (WebException webEx)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Close();
                }
            }

            return result;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}