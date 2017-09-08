using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPress.Web.Models.MyRedis;

namespace Test
{
    public partial class TestReids : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
              for (int i = 0; i < 500000; i++)
            {
                Many m = new Many();
                m.Name = "你哭着对我手啊是的减肥啦啥地方阿里那是来得及伐<embed src=\"/File/KindEditor/flash/20160504/20160504152346_3073.swf\" type=\"application/x-shockwave-flash\" width=\"200\" height=\"100\" quality=\"high\" />啊发生的是打发打发是打发打发ss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506153158_5436.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdf";
                m.Email = "你哭着对我手啊是的减肥啦啥地方阿里那是来得及伐<embed src=\"/File/KindEditor/flash/20160504/20160504152346_3073.swf\" type=\"application/x-shockwave-flash\" width=\"200\" height=\"100\" quality=\"high\" />啊发生的是打发打发是打发打发ss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506153158_5436.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdf";
                m.ID = 12345678;
                m.Adress = "你哭着对我手啊是的减肥啦啥地方阿里那是来得及伐<embed src=\"/File/KindEditor/flash/20160504/20160504152346_3073.swf\" type=\"application/x-shockwave-flash\" width=\"200\" height=\"100\" quality=\"high\" />啊发生的是打发打发是打发打发ss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506153158_5436.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdfss<embed src=\"/File/KindEditor/media/20160506/20160506164834_9174.mp4\" type=\"video/x-ms-asf-plugin\" width=\"550\" height=\"400\" autostart=\"false\" loop=\"true\" />sdf";
                m.TrueName = "你哭着对我手啊是的减肥啦啥地方阿里那是来得及伐" + i.ToString();
                MyRedisHelper.Hash_Set<Many>("sss", i.ToString(), m);
            }
            
            // 移除
            MyRedisHelper.Hash_Remove("sss");
            // 获取
            Many xx = MyRedisHelper.Hash_Get<Many>("sss", "22");
            
            Many one = new Many();
            one.ID = 1;
            one.Name = "1st";
            one.Email = "1email";
            MyRedisHelper.Hash_Set<Many>("manytest", "1", one);
            MyRedisHelper.Hash_Remove("manytest");
            Many two = new Many();
            two.ID = 2;
            two.Name = "2st";
            two.Email = "2email";
            MyRedisHelper.Hash_Set<Many>("manytest", "2", two);            
            
            Many m1 = MyRedisHelper.Hash_Get<Many>("manytest", "2");
            List<Many> list = MyRedisHelper.Hash_GetAll<Many>("manytest");
            MyRedisHelper.Hash_Remove("sss");
            Many m2 = MyRedisHelper.Hash_Get<Many>("manytest", "1");
            List<string> mm = new List<string>() { "11","22" };
            List<int> sas= mm.ConvertAll<int>(p => Convert.ToInt32(p));
           // List<Many> list1 = new List<Many>() {new Many(){ ID=1, Name="1", One="1" } }; 

           
        }
    }
}