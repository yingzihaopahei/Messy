using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Test
{
    /// <summary>
    /// Data 的摘要说明
    /// </summary>
    public class Data : IHttpHandler
    { 
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            JObject j = new JObject(); 
            JArray ja = new JArray() { 5, 20, 36, 10, 10, 20 };
            if (context.Request["type"]!=null)
            {
                ja = new JArray() { 5, 5, 5, 5, 5, 5 };
            }

            j.Add("d", ja);
            context.Response.Write(j.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}