using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using NLPToken;


namespace BNLP2008
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            NLPToken.GlobalVariable.ResourceLocation = Server.MapPath("~/Data/");
            Common.Dummy();

            Thread thread = new Thread(new ThreadStart(ThreadFunc));
            thread.IsBackground = true;
            thread.Name = "ThreadFunc";
            thread.Start();
        }

        protected void ThreadFunc()
        {
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new System.Timers.ElapsedEventHandler(TimerWorker);
            t.Interval = 1000*60*30; // 1 Hour
            t.Enabled = true;
            t.AutoReset = true;
            t.Start();
        }

        protected void TimerWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            string _DictLocation = String.Format("{0}//bdictuser.db", NLPToken.GlobalVariable.DictionaryLocation);
            //NLPToken.NLPGoogle.TranslateLog(Server.MapPath("~/log/NLPLogger.log"), _DictLocation);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}