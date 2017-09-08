using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCPGW;
using GatewayRouting.Routing;

namespace Test
{
    public partial class TestRePost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                byte[] byts = new byte[Request.InputStream.Length];
                Request.InputStream.Read(byts, 0, byts.Length);
                string req = System.Text.Encoding.UTF8.GetString(byts);


                HttpRequestUtil httpUtil = new HttpRequestUtil("http://localhost:9352/Routing/Routing.aspx", 60);
                string szdata = httpUtil.SendMessageB(req);



                Response.Write(szdata);
            }
           
        }
    }
}