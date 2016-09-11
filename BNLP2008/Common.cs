using System;
using System.Collections.Generic;
using System.Web;
using OpenNLP;
using NLPToken;

namespace BNLP2008
{
    public static class Common
    {
        public static OpenNLP.Tools.Parser.EnglishTreebankParser Parser;
        public static OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector SentenceDetector;

        static Common()
        {
            SentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(GlobalVariable.ResourceLocation + "EnglishSD.nbin");
            Parser = new OpenNLP.Tools.Parser.EnglishTreebankParser(GlobalVariable.ResourceLocation, true, false);
        }
        public static void Dummy()
        {
        }
    }
}
