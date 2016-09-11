using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RavSoft.GoogleTranslator;


namespace NLPToken
{
    public class NLPGoogle
    {
        public static string TranslateGoogle(string EnglishText)
        {
            string BengaliWord = "";

            // Initialize the translator
            Translator t = new Translator();
            t.SourceLanguage = "English";
            t.TargetLanguage = "Bengali";
            t.SourceText = EnglishText;

            // Translate the text
            try
            {
                t.Translate();
                BengaliWord = t.Translation;
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

            return BengaliWord;
        }

        public static void TranslateLog(string SourceLogFile, string DestinationFile)
        {
            //List<string> _Logs = NLPLogger.ReadLog(Server.MapPath("~/log/NLPLogger.log"));
            List<string> _Logs = NLPLogger.ReadLog(SourceLogFile);
            foreach (string _Log in _Logs)
            {
                string[] oDictWord = _Log.Split(':');
                string Word = oDictWord[0].ToLower();
                string Tag = string.Empty;
                if (oDictWord.Length > 1)
                    Tag = oDictWord[1];

                if (Constant.Dict[_Log] == null)
                {
                    string _Bengali = TranslateGoogle(Word);
                    if (_Bengali.Length > 0 && !_Bengali.Equals(Word, StringComparison.OrdinalIgnoreCase))
                    {
                        Constant.Dict.AddWord(_Log, _Bengali);
                    }
                    else
                    {
                        Constant.Dict.AddJunkWord(_Log, _Log);
                    }
                }
            }
        }
    }
}
