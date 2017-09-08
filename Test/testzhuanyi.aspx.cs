using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{
    public partial class testzhuanyi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string s = Request["bb"].ToString();
            string s2 = HttpUtility.UrlDecode (Request["bb"].ToString());
            string s3 = string.Empty;
        }
    }
}