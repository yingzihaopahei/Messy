using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Test
{
    public partial class TestDataToFile : System.Web.UI.Page
    {
        protected CAArchitecture.CADataAccess m_oCADA = null;
        private void OpenConn()
        {
            m_oCADA = new CAArchitecture.CADataAccess(Application["SqlConnStr"].ToString(), Application["LogPath"].ToString(), Convert.ToInt32(Session["SqlTimeout"]));
        }

        private void CloseConn()
        {
            if (m_oCADA != null)
            {
                m_oCADA.Dispose();
                m_oCADA = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            DataTable dt = GetUrl();
            string indexlegth = string.Empty; // 8位
            StringBuilder file = new StringBuilder();//总数据
            StringBuilder data = new StringBuilder();//数据
            StringBuilder index = new StringBuilder();// 8位

            indexlegth = string.Format("{0:00000000}",dt.Rows.Count * 2); 
            file.Append(indexlegth); 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                index.Append(string.Format("{0:00}", dt.Rows[i]["BURL"].ToString().Length));
                data.Append(dt.Rows[i]["BURL"].ToString());
            }
            file.Append(index.ToString());
            file.Append(data.ToString());
            File.WriteAllText(@"C:\Users\Fan\Desktop\IP\BURL.txt",file.ToString());
        }
       
        public DataTable GetUrl()
        {
            DataTable DT = new DataTable();
            DataSet ds = new DataSet();

            SqlParameter[] oSQLParams = { 
								}; 
            try
            {
                OpenConn();
                ds = m_oCADA.RunStoredProc_DataSet(Application["PG-DB"].ToString() + "[PG_GetData]", oSQLParams);
                CloseConn();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DT = ds.Tables[0];
                } 
            }
            catch (Exception oErr)
            {
                 
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            } 
            return DT; 
        }


    }
}