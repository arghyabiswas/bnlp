using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Xml;
using NLPToken;

namespace BNLP.API.Controllers
{
    public class BNLPApiController : ApiController
    {
        


		public string TranslateSite(string Text)
		{
			string _Reuslt = "";
			//try
			{
				string[] _RArray = Text.Split(new string[] { "--NT--" }, StringSplitOptions.None);
				XmlDocument _TempXml = new XmlDocument();
				foreach (string _s in _RArray)
				{
					string[] _Temp = _s.Split(new string[] { "--@--" }, StringSplitOptions.None);
					if (_Temp.Length > 1)
					{
						string _Eng = _Temp[1];
						if (_Eng.Trim().Length > 0)
						{
							string[] _TempString = _Eng.Split(' ');
							if (_TempString.Length > 1)
							{
								if (_TempString.Length == 2 && _TempString[1] == "|")
								{
									_Reuslt += String.Format("{0}--@--{1} {2}--NT--", _Temp[0], TranslateSingle(_TempString[0]), _TempString[1]);
								}
								else
								{

									string _TempText = String.Format("<Entity id=\"{0}\">{1}</Entity>", _Temp[0], _Temp[1]); ;
									_TempXml.LoadXml(_TempText);

									_Reuslt += String.Format("{0}--@--{1}--NT--", _Temp[0], TranslateXml(_TempXml));
								}
							}
							else
							{
								_Reuslt += String.Format("{0}--@--{1}--NT--", _Temp[0], TranslateSingle(_Eng));
							}
						}
						else
						{
							_Reuslt += String.Format("{0}--@--{1}--NT--", _Temp[0], _Eng);
						}
					}
				}
			}
			// catch (Exception ex)
			{
			}
			return _Reuslt;
		}

		private string TranslateXml(XmlDocument _TempXml)
		{
			List<TekenXml> _TekenXml = new List<TekenXml>();
			string _Text = "";
			if (_TempXml.DocumentElement.ChildNodes.Count == 1)
			{
				_Text = _TempXml.DocumentElement.InnerXml;
				_TekenXml.Add(new TekenXml() { Id = _TempXml.DocumentElement.Attributes["id"].Value, Text = _Text });
			}
			else
			{
				foreach (XmlNode _Node in _TempXml.DocumentElement.ChildNodes)
				{
					if (_Node.Attributes != null && _Node.Attributes["id"] != null)
					{
						_TekenXml.Add(new TekenXml() { Id = _Node.Attributes["id"].Value, Text = _Node.InnerXml });
					}
					_Text = String.Format("{0} {1}", _Text, _Node.InnerText);
				}
			}

			StringBuilder output = new StringBuilder();
			string[] sentences = SplitSentences(_Text);
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
				ParseToken(oTran.Token, _TekenXml);
				foreach (TekenXml _X in _TekenXml)
				{
					if (!String.IsNullOrEmpty(_X.Bengali))
					{
						oTran.Bengali = oTran.Bengali.Replace(_X.Bengali, String.Format("<Entity id=\"{0}\">{1}</Entity>", _X.Id, _X.Bengali));
					}
				}
				oTran.Bengali = Regex.Replace(oTran.Bengali, "\\s+", " ");
				oTran.Bengali = oTran.Bengali.Replace("-lrb-", "(").Replace("-rrb-", ")");
				output.Append(oTran.Bengali);
			}

			return output.ToString();
		}

		private void ParseToken(Token _Token, List<TekenXml> _TekenXmls)
		{
			TekenXml _TekenXml = _TekenXmls.Find(o => o.Text.Equals(_Token.English));
			if (_TekenXml != null)
			{
				_TekenXml.Bengali = _Token.Bengali;
			}
			foreach (Token _SubToken in _Token.Tokens)
			{
				ParseToken(_SubToken, _TekenXmls);
			}
		}


		private string TranslateSingle(string _Eng)
		{
			return Token.TranslateSingle(_Eng + ":NN");
		}



		//private ParseToken ParseSentence(string sentence)
		//{
		//    sentence = PreFormat.ProcessPhase(sentence);
		//    Parse _Parse = Common.Parser.DoParse(sentence);
		//    return NLPToken.ParseToken.ToParseToken(_Parse);
		//}

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
