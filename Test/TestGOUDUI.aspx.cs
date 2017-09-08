using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using CCPGW;
using System.Xml;

namespace Test
{
    public partial class TestGOUDUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            HttpRequestUtil httpUtil = null;
            string md5Str = "21349" + "21349001" + "rdL68v6n";
            // 获取签名
            string SHAInfo = GetSHA256_HEBCB(md5Str);

            Hashtable headers = new Hashtable();
            headers.Add("merNo", "21349");//商户编号
            headers.Add("gatewayNo", "21349001");//网关接入号
            headers.Add("orderNo", "201608170013764468");//订单号改为流水号  订单编号 非空 txnGetModel.OrderID
            headers.Add("signInfo", SHAInfo);//交易币种

            string szdata = "";
            using (httpUtil = new HttpRequestUtil("https://check.payforasia.com/servlet/NormalCustomerCheck", 60))
            {
                szdata = httpUtil.SendMessage(headers);
            }

            if (szdata == "1003" || szdata == "1004" || szdata == "")
            {
                string n = "1";

            }
            else
            {


                XmlDocument XMLReasult = new XmlDocument();
                XMLReasult.LoadXml(szdata);


            }


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
            return sha256Str.ToLower();
        }
    }
}