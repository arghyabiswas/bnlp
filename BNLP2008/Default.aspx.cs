using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Text.RegularExpressions;
using OpenNLP;
using BNLP2008;
using OpenNLP.Tools.Parser;
using System.Collections.Generic;
using NLPToken;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;


namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GlobalVariable.ResourceLocation = Server.MapPath("~/Data/Dict");
            //NLPLogger.Debug("Here is a debug log.");
            //NLPLogger.Info("... and an Info log.");
            //NLPLogger.Warn("... and a warning.");
            //NLPLogger.Error("... and an error.");
            //NLPLogger.Fatal("... and a fatal error.");
            //Response.Write(NLPGoogle.TranslateGoogle("cat"));

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            litOutput.Text = "";
            treeAnalysis.Nodes.Clear();
            litBengali.Text = Translate(txtInput.Text);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            litOutput.Text = "";
            treeAnalysis.Nodes.Clear();
            litBengali.Text = TranslateTest(txtInput.Text);
        }

        private string TranslateTest(string Text)
        {

            StringBuilder output = new StringBuilder();
            string[] sentences = SplitSentences(Text);
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
                oTran.Bengali += "<br />";
                output.AppendFormat("{0} {1}", oTran.TaggedSentence, oTran.Bengali);
            }

            return output.ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            litOutput.Text = "";
            treeAnalysis.Nodes.Clear();
            litBengali.Text = TranslateAnalysis(txtInput.Text);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            //litBengali.Text = NLPGoogle.TranslateGoogle(txtInput.Text);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string _DictLocation = String.Format("{0}//bdictuser.db", NLPToken.GlobalVariable.DictionaryLocation);
            //NLPToken.NLPGoogle.TranslateLog(Server.MapPath("~/log/NLPLogger.log"), _DictLocation);
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            StringBuilder output = new StringBuilder();

            List<Token> _Tokens = new List<Token>();

            string[] sentences = SplitSentences(txtInput.Text);

            foreach (string sentence in sentences)
            {
                output.Append(ParseSentence(sentence)).Append("\r\n");
            }

            litBengali.Text = output.ToString();


        }

        private string Translate(string Text)
        {
            StringBuilder output = new StringBuilder();
            string[] sentences = SplitSentences(Text);
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
                oTran.Bengali += "<br />";
                output.Append(oTran.Bengali);
            }

            return output.ToString();
        }

        private string TranslateAnalysis(string Text)
        {
            //GlobalVariable.ResourceLocation = Server.MapPath("~/Data/");
            StringBuilder output = new StringBuilder();
            List<Token> _Tokens = new List<Token>();

            string[] sentences = SplitSentences(Text);
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
                oTran.Bengali += "<br />";
                output.Append(oTran.Bengali);

                _Tokens.Add(oTran.Token);
            }
            CreateTree(_Tokens);
            return output.ToString();

        }

        private void CreateTree(List<Token> _Tokens)
        {
            treeAnalysis.Nodes.Clear();
            TreeNode _Root = new TreeNode("Root");
            _Root.Expand();
            treeAnalysis.Nodes.Add(_Root);
            foreach (Token _Token in _Tokens)
            {
                CreateTree(_Token, _Root);
            }
        }

        private void CreateTree(Token _Token, TreeNode _Node)
        {
            _Node.Expand();
            TreeNode _ChildNode = new TreeNode(String.Format("<b>{0}:{1} ({2})</b> {3}", _Token.Type, _Token.English, _Token.Bengali, (_Token.GlobalTokenProperty != null) ? _Token.GlobalTokenProperty.Text : ""));
            _Node.ChildNodes.Add(_ChildNode);
            foreach (Token _Child in _Token.Tokens)
            {
                CreateTree(_Child, _ChildNode);
            }
        }

        private string[] SplitSentences(string paragraph)
        {
            return Common.SentenceDetector.SentenceDetect(paragraph);
        }

        private string ParseSentence(string sentence)
        {
            sentence = PreFormat.ProcessPhase(sentence);
            string ParsedToken = Common.Parser.DoParse(sentence).Show();

            return ParsedToken;
        }
    }
}
