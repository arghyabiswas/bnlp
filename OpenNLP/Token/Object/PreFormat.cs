using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NLPToken
{
    public class PreFormat
    {

        public static string ProcessPhase(string TheText)
        {
            foreach (TranslationFormatText f in Constant.Phase)
            {
                TheText = Regex.Replace(TheText, f.Expression, f.Replace,RegexOptions.IgnoreCase);
            }
            TheText = TheText.Replace("\\t", "\t");
            return TheText.Trim();
        }
    }
}
