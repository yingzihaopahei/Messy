using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aliyun_api_gateway_sdk.Constant;
using System.Net;
using aliyun_api_gateway_sdk.Util;
using System.IO;
using System.Text;

namespace Test.Message
{
    public partial class TestMessage : System.Web.UI.Page
    {
        private const String appKey = "************";
        private const String appSecret = "****************************";
        private const String host = "http://sms.market.alicloudapi.com";
        private const String path = "/singleSendSms";
        protected void Page_Load(object sender, EventArgs e)
        {


            var headers = new Dictionary<string, string>();
            var querys = new Dictionary<string, string>();
            var signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, aliyun_api_gateway_sdk.Constant.ContentType.CONTENT_TYPE_TEXT);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, aliyun_api_gateway_sdk.Constant.ContentType.CONTENT_TYPE_TEXT);

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("ParamString", "{'name':'XXXX'}");  // 模板变量
            querys.Add("RecNum", "18600000000,18600000000");  // 接收手机号，多个手机号以英文逗号分隔
            querys.Add("SignName", "签名名称"); // 短信签名名称
            querys.Add("TemplateCode", "模板CODE"); // 短信模板CODE

            using (HttpWebResponse response = HttpUtil.HttpGet(host, path, appKey, appSecret, 30000, headers, querys, signHeader))
            {

               
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                var reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);
            }
            Console.Read();

        }
    }
}