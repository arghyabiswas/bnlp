using System;
using System.Text.RegularExpressions;

namespace NLPToken
{
    /// <summary>
    /// Summary description for Formatter.
    /// </summary>
    public class Formatter
    {
        public Formatter()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string FormatText(string TheText)
        {

            // This is not quite the orthodox Penn TreeBank conventions
            // but it will work and looks (to me) rather nicer, for example I don't like their "``" quotes
            TheText = TheText.Trim();
            TheText = String.Format("{0}{1}", TheText.Substring(0, 1).ToUpper(), TheText.Substring(1, TheText.Length - 1));
            TheText = (TheText + " ");

            foreach (TranslationFormatText f in Constant.PreTranslationFormat)
            {
                TheText = Regex.Replace(TheText, "\\s+", " "); // Replece Multiple Space by Single Space
                TheText = Regex.Replace(TheText, f.Expression, f.Replace);
            }
            TheText = Regex.Replace(TheText, " [\'\"] ", " ");
            TheText = Regex.Replace(TheText, "\\s+", " "); // Replece Multiple Space by Single Space
            return TheText.Trim();
        }

        public static bool WordType(string Word, string ExpressionType)
        {
            Regex oRegex = new Regex(ExpressionType);
            return oRegex.IsMatch(Word);
        }
    }
}
