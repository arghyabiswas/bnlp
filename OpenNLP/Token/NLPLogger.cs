using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.IO;

namespace NLPToken
{
    public class NLPLogger
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(NLPLogger));

        static NLPLogger()
        {
            XmlConfigurator.Configure();
            
        }

        public static void Debug(string Message)
        {
            logger.Debug(Message);
        }

        public static void DebugFormat(string Format,params object[] args)
        {
            logger.DebugFormat(Format, args);
        }

        public static void Info(string Message)
        {
            logger.Info(Message);
        }

        public static void InfoFormat(string Format, params object[] args)
        {
            logger.InfoFormat(Format, args);
        }

        public static void Warn(string Message)
        {
            logger.Warn(Message);
        }

        public static void WarnFormat(string Format, params object[] args)
        {
            logger.WarnFormat(Format, args);
        }

        public static void Error(string Message)
        {
            logger.Error(Message);
        }

        public static void ErrorFormat(string Format, params object[] args)
        {
            logger.ErrorFormat(Format, args);
        }

        public static void Fatal(string Message)
        {
            logger.Fatal(Message);
        }

        public static void FatalFormat(string Format, params object[] args)
        {
            logger.FatalFormat(Format, args);
        }

        public static List<string> ReadLog(string LogPath)
        {
            List<string> _Logs = new List<string>();
            using (FileStream fileStream = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        _Logs.Add(streamReader.ReadLine());
                    }
                }
            }

            return _Logs;
        }
        
    }
}
