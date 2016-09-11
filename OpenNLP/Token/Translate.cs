using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NLPToken
{
    public class Translate
    {
        public string Bengali { get; set; }
        private Token _Token;
        public Token Token { get {return _Token; } }
        public string TaggedSentence { get; set; }

        public Translate(string Sentence)
        {
            Sentence = this.PreFormat(Sentence);

            Sentence = this.PostTaggedFormat(Sentence);
            Token _Token = Token.ToToken(Sentence);
            _Token.Translate();

            this.Bengali = PostProcessFormat(_Token.Bengali);
            PostFormat();

            this._Token = _Token;
            this.TaggedSentence = Sentence;
        }

        private string PostProcessFormat(string Sentence)
        {
            foreach (TranslationFormatText _Ps in Constant.PostProcessFormat)
            {
                Regex oReg = new Regex(_Ps.Expression);
                if (oReg.IsMatch(Sentence))
                {
                    Sentence = oReg.Replace(Sentence, _Ps.Replace);
                }
            }

            return Sentence;
        }

        private string PostTaggedFormat(string Sentence)
        {
            foreach (TranslationFormatText _Ps in Constant.PostTaggedFormat)
            {
                Regex oReg = new Regex(_Ps.Expression);
                if (oReg.IsMatch(Sentence))
                {
                    Sentence = oReg.Replace(Sentence, _Ps.Replace);
                }
            }

            return Sentence;
        }

        private string PreFormat(string ParsedSentence)
        {
            List<TranslationFormatText> replaceExpression = new List<TranslationFormatText>();
            replaceExpression.Add(new TranslationFormatText() { Expression = "{", Replace = "-lcb-" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "}", Replace = "-rcb-" });

            replaceExpression.Add(new TranslationFormatText() { Expression = "[\"]", Replace = "-QUOT-" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "[']", Replace = "-SQUOT-" });

            replaceExpression.Add(new TranslationFormatText() { Expression = "[(]", Replace = "{" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "[)]", Replace = "}" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "} {", Replace = "},{" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "[ ]", Replace = ":" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "},{", Replace = "} {" });

            foreach (TranslationFormatText f in replaceExpression)
            {
                ParsedSentence = Regex.Replace(ParsedSentence, f.Expression, f.Replace);
            }

            return ParsedSentence;
        }

        private void PostFormat()
        {
            List<TranslationFormatText> replaceExpression = new List<TranslationFormatText>();
            replaceExpression.Add(new TranslationFormatText() { Expression = "\\s+", Replace = " " });
            replaceExpression.Add(new TranslationFormatText() { Expression = "-lcb-", Replace = "{" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "-rcb-", Replace = "}" });

            replaceExpression.Add(new TranslationFormatText() { Expression = "-lrb-", Replace = "(" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "-rrb-", Replace = ")" });

            replaceExpression.Add(new TranslationFormatText() { Expression = "-quot-", Replace = "\"" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "-squot-", Replace = "'" });

            foreach (TranslationFormatText f in replaceExpression)
            {
                this.Bengali = Regex.Replace(this.Bengali, f.Expression, f.Replace);
            }
        }
    }
}
