using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using NLPToken;

namespace BNLP.API.Controllers
{
    public class TranslationController : ApiController
    {
        public string Get(string id)
		{
			StringBuilder output = new StringBuilder();

			string[] sentences = SplitSentences(id);
			foreach (string sentence in sentences)
			{
				string _TempSentence = sentence.Trim();
				Regex _AlphaNumeric = new Regex("[.||!||?]");
				if (!_AlphaNumeric.IsMatch(_TempSentence, _TempSentence.Length - 1))
				{
					_TempSentence = _TempSentence + " .";
				}

				_TempSentence = ParseSentence(_TempSentence);
				Translate oTran = new Translate(_TempSentence);
				output.Append(oTran.Bengali);
			}

			return output.ToString();
		}

		private string ParseSentence(string sentence)
		{
			sentence = PreFormat.ProcessPhase(sentence);
			string ParsedToken = Common.Parser.DoParse(sentence).Show();

			return ParsedToken;
		}

		private string[] SplitSentences(string paragraph)
		{
			return Common.SentenceDetector.SentenceDetect(paragraph);
		}
    }
}
