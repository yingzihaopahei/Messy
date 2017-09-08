using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Speech.Synthesis;

namespace Test
{
    public partial class TestSpech : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SpeechSynthesizer hello = new SpeechSynthesizer();
            string str = "请输入您的名字";
            hello.Speak(str);  //Speak(string),Speak加上字符串类型的参数
            Console.ReadKey();
        }
    }
}