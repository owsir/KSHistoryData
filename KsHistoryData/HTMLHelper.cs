using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KsHistoryData
{
    public class HTMLHelper
    {
        public HTMLHelper()
        {

        }
        public static string GetHttpData(string Url)
        {
            string sException = null;
            string sRslt = null;
            WebResponse oWebRps = null;
            WebRequest oWebRqst = WebRequest.Create(Url);
            oWebRqst.Timeout = 50000;
            try
            {
                oWebRps = oWebRqst.GetResponse();
            }
            catch (WebException e)
            {
                sException = e.Message.ToString();
                throw e;
            }
            catch (Exception e)
            {
                sException = e.ToString();
                throw e;
            }
            finally
            {
                if (oWebRps != null)
                {
                    StreamReader oStreamRd = new StreamReader(oWebRps.GetResponseStream(), Encoding.UTF8);
                    sRslt = oStreamRd.ReadToEnd();
                    oStreamRd.Close();
                    oWebRps.Close();
                }
            }
            return sRslt;
        }

    }
}
