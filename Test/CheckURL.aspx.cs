using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Test
{
    public partial class CheckURL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ls_url = File.ReadAllLines(@"C:\Users\Fan\Desktop\网址\URL.txt");
            string[] ls_MTurl = File.ReadAllLines(@"C:\Users\Fan\Desktop\网址\MTURL.txt");
            List<string> LS = new List<string>();
            for (int i = 0; i < ls_url.Length; i++)
            {
                string url = ls_url[i].Split(',')[1];
                for (int j = 0; j < ls_MTurl.Length; j++)
                {
                    if (ls_MTurl[j].Contains(url))
                    {
                        LS.Add(ls_MTurl[j].Split(',')[0] + "," + ls_url[i].Split(',')[0]);
                    }
                }
            }
            File.WriteAllLines(@"C:\Users\Fan\Desktop\网址\Relation.txt", LS.ToArray());

        
        }
    }
}