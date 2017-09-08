using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using CCPGW;
using GatewayRouting.Routing;

namespace Test
{
    public partial class TestRefer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequestUtil httpUtil = null;
            Hashtable headers = new Hashtable();
            Logger m_oLogger = new Logger(@"C:\Users\Fan\Desktop\ceshi\test.log",0);
            headers.Add("merNo", 1);//商户编号
            headers.Add("terNo", 1);//终端号
            headers.Add("transType", "sales");//交易类型
            headers.Add("apiType", 1);//接口类型 默认传递1；(1普通接口、2 app sdk、3快捷支付、4 虚拟 
            headers.Add("transModel", "M");//交易模式
            headers.Add("EncryptionMode", "SHA256");//加密方式类型
            headers.Add("CharacterSet", "UTF8");//字符编码
            headers.Add("amount", 1);//保留两位小数 Math.Round( Convert.ToDecimal(m_PostAmt) / 100,2)
            headers.Add("currencyCode", "CNY");//交易币种
            headers.Add("orderNo", 1);//订单号改为流水号  订单编号 非空 txnGetModel.OrderID
            headers.Add("merremark", "商户预留字段");//商户预留字段
            headers.Add("returnURL", "http://www.yourshop.com/returnpage.php");//returnURL
            headers.Add("webInfo", @"userAgent:Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0");//客户端浏览器信息
            headers.Add("language", "En");//语言  
            using (httpUtil = new HttpRequestUtil("https://h7226.dkhdz.com:4430/ACCPGW/Payment/PaymentInterface.aspx", 60))
            {
             string   szdata = httpUtil.SendMessage(headers);
            }
        }
    }
}