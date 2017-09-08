using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aliyun.Acs.Core;
using Aliyun.Acs.Sms.Model.V20160927;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;

namespace Test
{
    public partial class testMessage2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SendMessage();
        }


        public void SendMessage()
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-beijing", "LTAIwCYgtEpVlVFI", "EnmrPYlfryHSxBgMXFOVJigZeSPUOl");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest request = new SingleSendSmsRequest();
            try
            {
                request.SignName = "范国伟";
                request.TemplateCode = "SMS_24950286";
                request.RecNum = "18901083337,18612260498,18601341099";
                request.ParamString = "{'name':'李总','time':'2016-11-04'}";
                SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
               // e.printStackTrace();
            }
            catch (ClientException e)
            {
                //e.printStackTrace();
            }
        }
    }
}