using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services; //引入命名空间
namespace Test
{
    public partial class TestEchart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static int[] getData()
        {

            return new int[] { 5, 20, 36, 10, 10, 20 };

        }
    }
}