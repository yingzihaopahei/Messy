using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;

namespace Test
{
     
 
    public partial class testBlack : System.Web.UI.Page
    {
        //int[] sa = new int[] { 1, 2, 3, 4 };
        //int[] sa3 = new int[] { 1, 2, 3, 4, 5 };
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    string n = "20161214115643";

        //    string str = DateTime.ParseExact(n, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd hh:mm:ss");


        //}
        delegate void weituoyi();
        protected void Page_Load(object sender, EventArgs e)
        {


            sendMail();

            //try
            //{

            //    WebClient MyWebClient = new WebClient();


            //    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            //    Byte[] pageData = MyWebClient.DownloadData("http://www.paovw.us"); //从指定网站下载数据

            //    string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

            //    //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句

            //    Console.WriteLine(pageHtml);//在控制台输入获取的内容

            //    using (StreamWriter sw = new StreamWriter(@"C:\Users\Fan\Desktop\新建文件夹\test.txt"))//将获取的内容写入文本
            //    {

            //        sw.Write(pageHtml);

            //    }

            //    Console.ReadLine(); //让控制台暂停,否则一闪而过了             

            //}

            //catch (WebException webEx)
            //{

            //    Console.WriteLine(webEx.Message.ToString());

            //}

        }

        public int sendMail()
        {
            int status = -1;
            try
            {
                string strFrom = string.Empty;
                string strPwd = string.Empty;
                MailMessage mail = new MailMessage();
                strFrom = "payworks@webmicroshop.com";
                strPwd = "asd123$%";
                mail.Subject = "主题";
                //m_szBody = m_szBody.Replace("[DeliverID]", m_DeliverID);
                //m_szBody = m_szBody.Replace("[CheckUrl]", m_CheckUrl);
                //m_szBody = m_szBody.Replace("[OrderID]", m_OrderID);
                //m_szBody = m_szBody.Replace("[ProductName]", m_ProductName);
                //m_szBody = m_szBody.Replace("[m_MerchEmail]", m_MerchEmail);

                mail.To.Add("fanguowei@hrtpayment.com"); //收件人

                mail.Body = "test";
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.Default;  //编码
                SmtpClient client = new SmtpClient("mail.webmicroshop.com", 25);
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(strFrom, strPwd); //验证身份
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;
                client.Send(mail);
            }
            catch (Exception ex)
            {


            }
            return status;
        }
        private string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }
            return strMsg;
        }

    }




    //string n=   Regex.Replace("IssCountry=&UserData3=&BrowserSystemLanguage=&BrowserDate=&BCity=96&MIPAddress=123.56.4.2&CMSName=&ExpDate=2211&Issuer=90&Resolution=&CardType=VISA&BAddress=477835564&ShipCity=59&BrowserSystem=&AcctNo=botong011&CName=621&CardPAN=4283428305041009&Agent_AcctNo=botong&Amount=732&HashValue=RKpglOp1K54X2GIoir2WRg==&IVersion=5.0&IPAddress=0:0:0:0:0:0:0:1&UserData2=&TxnID=&ShipPostCode=858874&RetURL=www.abc.com&BrowserLanguage=&CurrCode=156&AMAID=2889&MTAID=278706&Template=&Email=359@600.com&CommodityBrand=正品&PostCode=307037&MAID=2890&ShipAddress=848945791&Telephone=899166&BrowserUserAgent=unknown&Shipstate=4&ShipName=179&TxnType=01&UserData1=8055240&ShipCountry=13&OrderID=12161219164546956697&BrowserName=&Shipphone=858874&BrowserDateTimezone=&CVV2=123", @"[^\x21-\x7E]", "有特殊字符");
    //  Response.Write(n);

    public class fu
    {
        protected string name = "fu";
    }
    public class zi : fu
    {
        new string name = "zi";

        public string GetName()
        {
            return name;
        }
        public string GetFatherName()
        {
            return base.name;
        }
    }
}