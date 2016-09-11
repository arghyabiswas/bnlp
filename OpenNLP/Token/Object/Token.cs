using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace NLPToken
{
    public class Token
    {
        public Guid ID { get { return Guid.NewGuid(); } }
        public String Type { get; set; }
        public String English { get; set; }
        public String Lemma { get; set; }
        public string TaggesEnglish { get; set; }

        private string _Bengali = String.Empty;
        public String Bengali { get { return _Bengali; } set { _Bengali = value; } }
        public SVOO SVOO { get; set; }
        public List<Token> _Tokens = new List<Token>();
        public List<Token> Tokens { get { return _Tokens; } set { _Tokens = value; } }
        //public string NextWord = String.Empty;
        //public string NextWordType = String.Empty;
        public int WordPosition { get; set; }

        public GlobalTokenProperty GlobalTokenProperty;

        public bool IsBeginSentense
        {
            get // Arghya What
            {
                bool IsBegin = (
                  this.Type == TagsType.S
                  || this.Type == TagsType.SQ
                  || this.Type == TagsType.SBARQ
                  || this.Type == TagsType.SBAR
                  || this.Type == TagsType.FRAG
                  || this.Type == TagsType.SINV
                  || this.Type == TagsType.TOP
                  );
                return IsBegin;
            }
        }

        public bool IsIntermediateBegin
        {
            get // Arghya What
            {
                bool IsBegin = (
                  this.Type == TagsType.S
                  );
                return IsBegin;
            }
        }

        public void GetFormattedType()
        {
            this.Lemma = (Constant.Lemma[this.English] != null) ? Constant.Lemma[this.English].ToString() : this.English;

            #region Tag Mapping
            switch (this.Type)
            {
                case TagsType.VB:
                case TagsType.VBD:
                case TagsType.VBG:
                case TagsType.VBN:
                case TagsType.VBP:
                case TagsType.VBZ:
                    if (Constant.IsHaveVerbPresent(this.English))
                    {
                        this.Type = this.Type.Replace("VB", "VH");
                        this.Lemma = "have";
                    }
                    else if (Constant.IsBeVerbPresent(this.English))
                    {
                        this.Lemma = "be";
                    }
                    else
                    {
                        this.Type = this.Type.Replace("VB", "VV");
                    }
                    break;
                case TagsType.PRP:
                    this.Type = TagsType.PP;
                    break;
                case TagsType.PRPP:
                    this.Type = TagsType.PPP;
                    break;
                case TagsType.NNP:
                    this.Type = TagsType.NP;
                    break;
                case TagsType.NNPS:
                    this.Type = TagsType.NPS;
                    break;
            }
            #endregion
        }

        public void Translate()
        {
            int State = 0;
            this.ProcessToken(new GlobalTokenProperty()); // y
            List<Token> _WordList = new List<Token>();

            bool IsInterrogative = (this.Tokens[0].GlobalTokenProperty != null)? this.Tokens[0].GlobalTokenProperty.Purpose == TypesByPurpose.Interrogative : false;
            this.ParseWord(this, IsInterrogative);// my

            Token _WhToken = null;
            this.Format(ref State, ref _WhToken, this.GlobalTokenProperty);
            this.Translate(this.GlobalTokenProperty);
        }


        public void ParseWord(Token _Token, bool IsInterrogative)
        {
            if (IsInterrogative)
            {
                switch (_Token.Type)
                {
                    case "SBAR":
                        _Token.Type = "SBARQ";
                        break;
                    case "S":
                        _Token.Type = "SQ";
                        _Token.GlobalTokenProperty.Purpose = TypesByPurpose.Interrogative;
                        _Token.GlobalTokenProperty.ConfusionPurpose = true;
                        break;
                    case "FRAG":
                        _Token.Type = "SBARQ";
                        _Token.GlobalTokenProperty.Purpose = TypesByPurpose.Interrogative;
                        _Token.GlobalTokenProperty.ConfusionPurpose = true;
                        break;
                    default:
                        break;
                }
            }

            foreach (Token _SubToken in _Token.Tokens)
            {
                ParseWord(_SubToken, IsInterrogative);
            }
        }

        public void Format(ref int State, ref Token _WhToken, GlobalTokenProperty GlobalProperty)
        {
            if (this.GlobalTokenProperty != null)
            {
                GlobalProperty = this.GlobalTokenProperty;
            }


            for (int i = 0; i < Tokens.Count; i++)
            {
                Token _SubToken = Tokens[i];

                switch (_SubToken.Type)
                {
                    case "SBARQ":
                        State = 1; // SBARQ Found
                        break;
                    case "WHADVP": // Wh-adverb
                    case "WHNP": // Wh-noun
                    case "WHPP": // Wh-prepositional Phrase.
                        if (State == 1)
                        {
                            _WhToken = _SubToken;
                            this.Tokens.Remove(_SubToken);
                            i--;
                            State = 12; // Wh word found need to replace the location
                        }
                        break;
                    case "WHADJP": //Wh-adjective
                        if (State == 1)
                        {
                            _WhToken = _SubToken;
                            this.Tokens.Remove(_SubToken);
                            i--;
                            State = 22; // Wh word found need to replace the location
                        }
                        break;
                    case "SQ":
                        _SubToken.Format(ref State, ref _WhToken, GlobalProperty);
                        if (State % 10 == 2) // If till Wh exists replace it in SQ 
                        {
                            //_SubToken.Tokens.Insert(0, _WhToken);
                            _SubToken.Tokens.Add(_WhToken);
                            State = 3; // Location of wh wor replaced.
                        }
                        break;
                    case "VP":

                        if (GlobalProperty.ConfusionPurpose)
                        {
                            if (State % 10 == 2)
                            {
                                _SubToken.Format(ref State, ref _WhToken, GlobalProperty);
                                if (State == 12)
                                {
                                    _SubToken.Tokens.Insert(0, _WhToken);
                                    State = 3; // Location of wh wor replaced.
                                }
                            }
                        }
                        if (State == 12)
                        {
                            _SubToken.Tokens.Insert(1, _WhToken);
                            State = 3; // Location of wh wor replaced.
                        }

                        break;
                    case "NP":
                        if (State == 22)
                        {
                            this.Tokens.Insert(0, _WhToken);
                            State = 3; // Location of wh wor replaced.
                        }
                        break;
                    default:
                        break;
                }
                if (State % 10 < 2)
                {
                    _SubToken.Format(ref State, ref _WhToken, GlobalProperty);
                }

                switch (this.Type)
                {
                    case "SQ":
                        if (State == 3)
                        {
                            GlobalProperty.IsWhWordPresent = true;
                            //State = 0; // Set Globalproperty as Wh Word
                        }
                        break;
                    default:

                        break;
                }
            }
        }

        public void Translate(GlobalTokenProperty GlobalProperty)
        {
            if (this.GlobalTokenProperty != null)
            {
                GlobalProperty = this.GlobalTokenProperty;
                GlobalProperty.IsVerbDetected = false;
            }

            foreach (Token _SubToken in Tokens)
            {
                _SubToken.Translate(GlobalProperty);

                if (this.Tokens.Count > 0)
                {
                    switch (this.Type)
                    {
                        case "VP":
                            switch (_SubToken.Type)
                            {
                                case "S":
                                case "SBAR":
                                    //case "ADVP":
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                                case "VP":
                                    if (GlobalProperty.IsNegetive)
                                    {
                                        if (GlobalProperty.IsNegetiveApplied)
                                        {
                                            this.Bengali = _SubToken.Bengali;
                                        }
                                        else
                                        {
                                            this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                        }
                                    }
                                    else
                                    {
                                        this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    }
                                    break;
                                case "CC":
                                    if (GlobalProperty.Purpose == TypesByPurpose.Imperative
                                        || GlobalProperty.Purpose == TypesByPurpose.Declarative)
                                    {
                                        this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    }
                                    else
                                    {
                                        this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    }
                                    break;
                                case "NP":
                                    if (GlobalProperty.IsVerbDetected)
                                    {
                                        if (GlobalProperty != null && !String.IsNullOrEmpty(GlobalProperty.BengaliDeterminerSuffix))
                                        {
                                            _SubToken.Bengali = Sondhi.SondhiPreposition(_SubToken.Bengali, GlobalProperty.BengaliDeterminerSuffix);
                                            GlobalProperty.BengaliDeterminerSuffix = String.Empty;
                                        }
                                    }
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                                case ",":
                                case ";":
                                case "!":
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                            }

                            break;
                        case "PP":
                            this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                            break;
                        case "NP":
                            switch (_SubToken.Type)
                            {
                                case "PP":
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                            }


                            break;
                        case "SBARQ":
                            switch (_SubToken.Type)
                            {
                                case ".":
                                    if (String.IsNullOrEmpty(_SubToken.Bengali))
                                    {
                                        _SubToken.Bengali = "?";
                                    }
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                                case "SQ":
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                            }

                            break;
                        case "SQ":
                            switch (_SubToken.Type)
                            {
                                case "NP":
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                            }

                            break;
                        case "SBAR":
                            switch (_SubToken.Type)
                            {
                                case ".":
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                            }
                            break;
                        case "ADVP":
                            switch (_SubToken.Type)
                            {
                                case "PP":
                                    this.Bengali = String.Format("{0} {1}", _SubToken.Bengali, this.Bengali);
                                    break;
                                default:
                                    this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                                    break;
                            }
                            break;
                        default:
                            this.Bengali = String.Format("{0} {1}", this.Bengali, _SubToken.Bengali);
                            break;
                    }
                }

            }

            if (GlobalProperty != null)
            {
                if (this.Tokens.Count == 0)
                {
                    Translater.Translate(this, ref GlobalProperty);
                }
                else
                {
                    string _Temp;
                    switch (this.Type)
                    {
                        case "WHADVP":
                            _Temp = Translater.TranslateConjugteWord(this.English, this.Type);
                            if (!String.IsNullOrEmpty(_Temp))
                            {
                                this.Bengali = _Temp;
                            }
                            break;
                        case "NP":
                            _Temp = Translater.TranslateConjugteWord(this.English, this.Type);
                            if (!String.IsNullOrEmpty(_Temp))
                            {
                                this.Bengali = _Temp;
                            }

                            // Before the verb
                            //if (!GlobalProperty.IsVerbDetected)
                            {
                                if (GlobalProperty != null && !String.IsNullOrEmpty(GlobalProperty.BengaliDeterminerSuffix))
                                {
                                    this.Bengali = Sondhi.SondhiPreposition(this.Bengali, GlobalProperty.BengaliDeterminerSuffix);
                                    GlobalProperty.BengaliDeterminerSuffix = String.Empty;
                                }
                            }
                            break;
                    }
                }
            }
        }

        public static string TranslateSingle(string Text)
        {
            return Translater.LookupWord(Text);
        }

        public void ProcessToken(GlobalTokenProperty GlobalTokenProperty)
        {
            if (this.Tokens.Count == 0)
            {
                this.WordPosition = ++GlobalTokenProperty.TempWordPosition;
            }

            if ((!GlobalTokenProperty.IsBeginDetected && this.IsBeginSentense) || this.IsIntermediateBegin)
            {
                GlobalTokenProperty = new GlobalTokenProperty();
                GlobalTokenProperty.IsBeginDetected = true;
            }

            this.English = !String.IsNullOrEmpty(this.English) ? this.English : "";

            foreach (Token childToken in this.Tokens)
            {
                childToken.ProcessToken(GlobalTokenProperty);

                if (!String.IsNullOrEmpty(childToken.Type) && childToken.Tokens.Count == 0)
                {
                    childToken.GetFormattedType();
                    GlobalTokenProperty.DetectSentenceType(childToken);
                    GlobalTokenProperty.DetectTenseVoiceTenseType(childToken);
                    GlobalTokenProperty.DetectPerson(childToken);
                    GlobalTokenProperty.DetectMainVerb(childToken);
                }
            }

            if (this.IsBeginSentense)
            {
                GlobalTokenProperty.DetectSentenceType(this, true);
                GlobalTokenProperty.DetectTenseVoiceTenseType(this, true);
                GlobalTokenProperty.DetectPerson(this, true);
                GlobalTokenProperty.DetectMainVerb(this, true);
                this.GlobalTokenProperty = GlobalTokenProperty;
                //GlobalTokenProperty = new GlobalTokenProperty();
                //Parent.WordPosition = 0;
            }
        }

        public static Token ToToken(string ParsedSentence)
        {
            List<TranslationFormatText> replaceExpression = new List<TranslationFormatText>();
            replaceExpression.Add(new TranslationFormatText() { Expression = "{([A-Z.,$-`]*):([^{}]*)}", Replace = "{\"Type\":\"$1\",\"English\":\"$2\",\"Tokens\":[}" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "{([A-Z.,$-`]*):", Replace = "{\"Type\":\"$1\",\"Tokens\":[" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "([A-Z.,$-`]*):{", Replace = "{\"Type\":\"$1\",\"Tokens\":[" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "}", Replace = "]}" });
            replaceExpression.Add(new TranslationFormatText() { Expression = "} {", Replace = "},{" });

            foreach (TranslationFormatText f in replaceExpression)
            {
                ParsedSentence = Regex.Replace(ParsedSentence, f.Expression, f.Replace);
            }

            JavaScriptSerializer json = new JavaScriptSerializer();


            Token oToken = json.Deserialize<Token>(ParsedSentence);

            return oToken;
        }


        public override string ToString()
        {
            string RawToken = "";
            RawToken = "{" + this.Type + ":" + this.English;
            foreach (Token childToken in this.Tokens)
            {
                RawToken = RawToken + childToken.ToString() + " ";
            }
            RawToken = RawToken.Trim();
            RawToken = RawToken + "}";
            return RawToken;
        }
    }

    public class TekenXml
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Bengali { get; set; }
    }
}
