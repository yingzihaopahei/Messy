using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Text;

namespace GatewayRouting.Routing
{
    public class Logger
    {
         /// <summary>
        /// Log level information.
        /// </summary>
        public enum LogLevel : int
        {
            /// <summary>
            /// Value: 0, Log everything.
            /// </summary>
            LogVerbose = (int)0,

            /// <summary>
            /// Value 1: Log Information and above.
            /// </summary>
            LogInfo = (int)1,

            /// <summary>
            /// Value: 2, Log Normal and above.
            /// </summary>
            LogNormal = (int)2,

            /// <summary>
            /// Value: 3, Log Warning and above.
            /// </summary>
            LogWarning = (int)3,

            /// <summary>
            /// Value: 4, Log Critical only.
            /// </summary>
            LogCritical = (int)4
        };

        private FileStream m_oFSLog = null;
        private StreamWriter m_oSWLog = null;
        private int m_iLogLevel = (int)LogLevel.LogCritical;

        ~Logger()
        {
            /*if (null != m_oSWLog)
            {
                m_oSWLog.Close ();
                //m_oSWLog = null;
            }
            if (null != m_oFSLog)
            {
                m_oFSLog.Close ();
                //m_oFSLog = null;
            }*/
        }

        /// <summary>
        /// <p>Constructor.</p>
        /// </summary>
        public Logger()
        {
        }


        /// <summary>
        /// <p>Constructor.</p>
        /// </summary>
        /// <param name="p_sFullFileName"><p>Log file path, usually stored in Web.config file.</p></param>
        /// <param name="p_iLogLevel"><p>Events level to log, logger will only log events that have the same or higher level with the specified Log Level supplied.</p></param>
        public Logger(string p_sFullFileName, int p_iLogLevel)
        {
            m_iLogLevel = (int)p_iLogLevel;
            m_oFSLog = new FileStream(FormatLogFileName(p_sFullFileName), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            m_oSWLog = new StreamWriter(m_oFSLog);
        }


        /// <summary>
        /// <p>Write log information to stream buffer. This function log stored proc call.</p>
        /// </summary>
        /// <param name="p_iLogLevel"><p>Events level to log, logger will only log events that have the same or higher level with the specified Log Level.</p></param>
        /// <param name="p_iFlag"><p>Flag to change the format of stored procedure log.</p></param>
        /// <param name="p_sName"><p>Stored Proc name.</p></param>
        /// <param name="p_aoArg"><p>Stored proc parameters.</p></param>
        public void Log(int p_iLogLevel, int p_iFlag, string p_sName, params object[] p_aoArg)
        {
            if (m_iLogLevel <= p_iLogLevel && null != m_oSWLog)
            {
                bool bPadComma = false;
                StringBuilder oSBData = new StringBuilder();
                oSBData.Append(p_sName + " ");

                foreach (SqlParameter oSqlParam in p_aoArg)
                {
                    if (bPadComma) oSBData.Append(", ");
                    switch (oSqlParam.SqlDbType)
                    {
                        case SqlDbType.BigInt:
                        case SqlDbType.Bit:
                        case SqlDbType.Decimal:
                        case SqlDbType.Float:
                        case SqlDbType.Int:
                        case SqlDbType.Money:
                        case SqlDbType.Real:
                        case SqlDbType.SmallInt:
                        case SqlDbType.SmallMoney:
                        case SqlDbType.TinyInt:
                            if (1 == p_iFlag && oSqlParam.Value == DBNull.Value) // fix part to print null to log file if the param value is null
                                oSBData.AppendFormat("{0}={1}", oSqlParam.ToString(), "NULL");
                            else if (1 == p_iFlag)
                                oSBData.AppendFormat("{0}={1}", oSqlParam.ToString(), oSqlParam.Value);
                            else
                                oSBData.AppendFormat("{0}", oSqlParam.Value);
                            break;
                        default:
                            if (1 == p_iFlag && oSqlParam.Value == DBNull.Value)
                            {
                                oSBData.AppendFormat("{0}={1}", oSqlParam.ToString(), "NULL");
                            }
                            else if (1 == p_iFlag)
                                oSBData.AppendFormat("{0}='{1}'", oSqlParam.ToString(), oSqlParam.Value.ToString().Replace("'", "''"));
                            else
                                oSBData.AppendFormat("'{0}'", oSqlParam.Value.ToString().Replace("'", "''"));
                            break;
                    }
                    bPadComma = true;
                }
                m_oSWLog.BaseStream.Seek(0, SeekOrigin.End);
                //m_oSWLog.WriteLine ("{0} [{1}]\t{2}", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff"), AppDomain.GetCurrentThreadId (), oSBData.ToString ());
                //Modified by PH 14 Nov 2011
                m_oSWLog.WriteLine("{0} [{1}]\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId, oSBData.ToString());
                m_oSWLog.Flush();
            }
        }


        /// <summary>
        /// <p><font color="#ff0000">This function was Deprecated.</font></p>
        /// <p>Write log information to stream buffer. This function used to log stored proc error.</p>
        /// </summary>
        /// <param name="p_iLogLevel"><p>Events level to log, logger will only log events that have the same or higher level with the specified Log Level.</p></param>
        /// <param name="p_aoArg"><p>Stored Proc parameter.</p></param>
        public void Log(int p_iLogLevel, params object[] p_aoArg)
        {
            if (m_iLogLevel <= p_iLogLevel && null != m_oSWLog)
            {
                if (p_aoArg.GetType().ToString().Equals("SqlParameter[]"))
                {
                    StringBuilder oSBData = new StringBuilder();
                    foreach (SqlParameter oSqlParam in p_aoArg)
                    {
                        switch (oSqlParam.SqlDbType)
                        {
                            case SqlDbType.BigInt:
                            case SqlDbType.Bit:
                            case SqlDbType.Decimal:
                            case SqlDbType.Float:
                            case SqlDbType.Int:
                            case SqlDbType.Money:
                            case SqlDbType.Real:
                            case SqlDbType.SmallInt:
                            case SqlDbType.SmallMoney:
                            case SqlDbType.TinyInt:
                                if (oSqlParam.Value == DBNull.Value)
                                    oSBData.AppendFormat("{0}={1}, ", oSqlParam.ToString(), oSqlParam.Value);
                                else
                                    oSBData.AppendFormat("{0}={1}, ", oSqlParam.ToString(), "NULL");
                                break;
                            default:
                                oSBData.AppendFormat("{0}='{1}', ", oSqlParam.ToString(), oSqlParam.Value.ToString().Replace("'", "''"));
                                break;
                        }
                    }
                    m_oSWLog.BaseStream.Seek(0, SeekOrigin.End);
                    //Modified by PH 14 Nov 2011
                    m_oSWLog.WriteLine("{0} [{1}]\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId, oSBData.ToString());
                    //m_oSWLog.WriteLine ("{0} [{1}]\t{2}", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff"), AppDomain.GetCurrentThreadId (), oSBData.ToString ());
                    m_oSWLog.Flush();
                }
                else
                {
                    m_oSWLog.BaseStream.Seek(0, SeekOrigin.End);
                    //m_oSWLog.Write ("{0} [{1}]\t", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff"), AppDomain.GetCurrentThreadId ());
                    //Modified by PH 14 Nov 2011
                    m_oSWLog.Write("{0} [{1}]\t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId);
                    foreach (object oData in p_aoArg)
                        m_oSWLog.Write(oData);

                    m_oSWLog.Write("\r\n");
                    m_oSWLog.Flush();
                }
            }
        }


        /// <summary>
        /// <p>Write a string to stream buffer. need Session["SerialNumber"]</p>
        /// </summary>
        /// <param name="p_iLogLevel"><p>Events level to log, logger will only log events that have same or higher level with the specified Log Level.</p></param>
        /// <param name="p_sData"><p>A string to log.</p></param>
        public void Log(int p_iLogLevel, string p_sData)
        {
            if (m_iLogLevel <= p_iLogLevel && null != m_oSWLog)
            {
                m_oSWLog.BaseStream.Seek(0, SeekOrigin.End);
                //			m_oSWLog.WriteLine ("{0} [{1}]\t{2}", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff"), AppDomain.GetCurrentThreadId (), p_sData);
                //Modified by PH 14 Nov 2011
                m_oSWLog.WriteLine("{0} [{1}]\t{2}\t{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId, "{" +  HttpContext.Current.Session["SerialNumber"] + "}", p_sData);
                m_oSWLog.Flush();
            }
        }


        /// <summary>
        /// <p>Print a line break to log information, primarily used to seperate between each page load.</p>
        /// </summary>
        public void PrintLineBreak()
        {
            m_oSWLog.WriteLine();
        }


        /// <summary>
        /// <p>Format log file name, append current date information to the end of file name.</p>
        /// </summary>
        /// <param name="p_sFullFileName"><p>Full path for log file.</p></param>
        /// <returns><p>Formatted log file name.</p></returns>
        private string FormatLogFileName(string p_sFullFileName)
        {
            StringBuilder oSBLogFile = new StringBuilder();
            int iPos = p_sFullFileName.LastIndexOf(".");
            string sExtName = p_sFullFileName.Substring(iPos);

            if (iPos <= 0)
                iPos = p_sFullFileName.Length;

            oSBLogFile.Append(p_sFullFileName, 0, iPos);
            oSBLogFile.Append("_");
            oSBLogFile.Append(DateTime.Now.ToString("yyyyMMdd"));
            oSBLogFile.Append(sExtName);

            return oSBLogFile.ToString();
        }
    }
}