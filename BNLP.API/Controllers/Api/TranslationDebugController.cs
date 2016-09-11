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
    public class TranslationDebugController : ApiController
    {
        public List<Token>  Get(string id)
		{
			List<Token> tokens = new List<Token>();

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
				tokens.Add(oTran.Token);
			}
			return tokens;
			//return CreateXML(_Tokens);
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

		private string CreateXML(List<Token> Tokens)
		{
			string _XMLText = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Root>";
			foreach (Token _Token in Tokens)
			{
				_XMLText += CreateXML(_Token);
			}
			_XMLText += "</Root>";

			return _XMLText;
		}

		private string CreateXML(Token Token)
		{
			if (Token.Type == ".")
			{
				Token.Type = "EoS";
			}
			Token.Type = Token.Type.Replace("$", "P");

			string _XMLText = String.Empty;
			if (Token.Type == "TOP")
			{
				_XMLText = String.Format("<{0}  Text=\"{1}\" >",
					Token.Type, Token.Bengali.Replace("\"", "--dcot--"));
			}
			else
			{
				if (Token.Tokens.Count == 0)
				{
					_XMLText = String.Format("<{0} English=\"{1}\" Lemma=\"{2}\" Bengali=\"{3}\" WordPosition=\"{4}\" >",
						Token.Type, Token.English.Replace("\"", "--dcot--"), Token.Lemma, Token.Bengali.Replace("\"", "--dcot--").Trim(), Token.WordPosition);
				}
				else
				{
					_XMLText = String.Format("<{0} English=\"{1}\" Bengali=\"{2}\" >",
						Token.Type, Token.English.Replace("\"", "--dcot--"), Token.Bengali.Replace("\"", "--dcot--").Trim());
				}
			}

			if (Token.GlobalTokenProperty != null)
			{
				_XMLText += String.Format("<Property {0} />", Token.GlobalTokenProperty.Text);
			}
			if (Token.Tokens.Count > 0)
			{
				foreach (Token _Token in Token.Tokens)
				{
					_XMLText += CreateXML(_Token);
				}
			}
			_XMLText += String.Format("</{0}>", Token.Type);
			return _XMLText;
		}

	}
}
