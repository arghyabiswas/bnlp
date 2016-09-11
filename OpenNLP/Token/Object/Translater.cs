using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NLPToken
{
    public class Translater
    {
        public static void Translate(Token Token, ref GlobalTokenProperty GlobalProperty)
        {
            //if (Token.Type != "TO" && GlobalProperty.WordUsed.Exists(p => p.Equals(Token.English)))
            //{
            //    Token.Bengali = "";
            //    return;
            //}

            switch (Token.Type)
            {
                #region Verb
                case TagsType.RB:
                case TagsType.RBR:
                case TagsType.RBS:
                    if (Constant.AdverbNegation.Table(Token.Lemma).Length > 0)
                    {
                        GlobalProperty.BengaliNegationWord = Constant.AdverbNegation.Table(Token.Lemma);
                        Token.Bengali = Constant.AdverbNegation.Table(Token.Lemma);
                    }
                    else
                    {
                        Token.Bengali = LookupAdverbWord(Token, ref GlobalProperty);
                        
                    }
                    //GlobalProperty.BengaliAdverb += String.Format(" {0}", Token.Bengali);

                    break;
                case TagsType.VVP:
                case TagsType.VV:
                case TagsType.VVD:
                case TagsType.VVG:
                case TagsType.VVN:
                case TagsType.VVZ:
                    if (Token.Lemma.ToLower().Equals("do")
                        && (GlobalProperty.MainVerb == MainVerb.Other
                            || (GlobalProperty.Purpose != TypesByPurpose.Imperative && GlobalProperty.MainVerb != MainVerb.Do)
                            || (GlobalProperty.Purpose == TypesByPurpose.Interrogative && !GlobalProperty.IsWhWordPresent && GlobalProperty.MainVerb != MainVerb.Do)))
                    {
                        Token.Bengali = "";
                    }
                    else
                    {
                        Token.Bengali = FinalRootVerb(Token.Lemma + ":VV", GlobalProperty); // Arghya
                        GlobalProperty.IsVerbDetected = true;
                    }
                    GlobalProperty.IsObject = true;
                    break;
                case TagsType.VB:
                case TagsType.VBD:
                case TagsType.VBG:
                case TagsType.VBP:
                case TagsType.VBN:
                case TagsType.VBZ:
                    if (GlobalProperty.MainVerb == MainVerb.Do || GlobalProperty.MainVerb == MainVerb.Other)
                    {
                        Token.Bengali = "";
                    }
                    else if (GlobalProperty.MainVerb == MainVerb.Be)
                    {
                        if (Token.English.ToLower().Equals("be")
                            || Token.English.ToLower().Equals("been")
                            || Token.English.ToLower().Equals("was")
                            || Token.English.ToLower().Equals("were"))
                        {
                            Token.Bengali = ProcessBeAsMainverb(Token.English, GlobalProperty);
                        }
                        else
                        {
                            Token.Bengali = "";
                        }
                    }
                    else
                    {
                        Token.Bengali = ProcessBeAsNonMainverb(Token.English, GlobalProperty);
                    }
                    GlobalProperty.IsObject = true;
                    //GlobalProperty.BengaliMainverb = String.Format("{0} {1}", GlobalProperty.BengaliMainverb, Token.Bengali);
                    /*
                    if (!String.IsNullOrEmpty(Token.English))
                    {
                        Token.Bengali = FinalRootVerbAfterPreposition(Token.English + ":VV", GlobalProperty);
                    }
                    else
                    {
                        Token.Bengali = LookupWord(Token.English + ":VV");
                    }
                    */
                    break;
                case TagsType.VH:
                case TagsType.VHD:
                case TagsType.VHG:
                case TagsType.VHN:
                case TagsType.VHP:
                case TagsType.VHZ:
                    if (GlobalProperty.MainVerb != MainVerb.Other && GlobalProperty.MainVerb != MainVerb.Do && GlobalProperty.MainVerb != MainVerb.Be)
                    {
                        Token.Bengali = ProcessHhAsMainverb(Token.Lemma, GlobalProperty);
                    }
                    else
                    {
                        Token.Bengali = "";
                    }
                    GlobalProperty.IsObject = true;
                    //GlobalProperty.BengaliMainverb = String.Format("{0} {1}", GlobalProperty.BengaliMainverb, Token.Bengali);
                    break;
                case TagsType.MD:
                    //Token.Bengali = FinalRootVerb(Token.English, GlobalProperty);
                    /*
                    if (!String.IsNullOrEmpty(GlobalProperty.BengaliNegationWord))
                    {
                        if (Token.WordPosition == 2)
                        {
                            Token.Bengali = FinalRootVerb(Token.English, GlobalProperty);
                            //GlobalProperty.BengaliMainverb = String.Format("{0} {1}", GlobalProperty.BengaliMainverb, Token.Bengali);
                        }
                    }
                    else if (Token.WordPosition  == 1)
                    {
                        Token.Bengali = FinalRootVerb(Token.English, GlobalProperty);
                        //GlobalProperty.BengaliMainverb = String.Format("{0} {1}", GlobalProperty.BengaliMainverb, Token.Bengali);
                    }
                    */

                    break;
                #endregion

                #region Subject and Object
                #region PunchuationSymbol
                case "":
                    Token.Bengali = Token.English;
                    break;
                #endregion
                #region UH
                case TagsType.UH:
                    Token.Bengali = LookupWord(Token.English);
                    GlobalProperty.Purpose = TypesByPurpose.Interrogative;
                    break;
                #endregion
                #region FW
                case TagsType.FW:
                    Token.Bengali = LookupWord(Token.English + ":NP");
                    break;
                #endregion
                #region LS
                case TagsType.LS:
                    Token.Bengali = LookupWord(Token.English);
                    break;
                #endregion
                #region SYM
                case TagsType.SYM:
                    Token.Bengali = LookupWord(Token.English);
                    break;
                #endregion
                #region CD
                case TagsType.CD:
                    Token.Bengali = CardinalNumber(Token.English);
                    break;
                #endregion
                #region POS
                case TagsType.POS:
                    Token.Bengali = Sondhi.BanglaSondhiPossessive(LookupWord(Token.English), BengaliWord.er);
                    break;
                #endregion
                #region PDT
                case TagsType.PDT:
                    //GlobalProperty.BengaliPreDeterminer = String.Format("{0} {1}", GlobalProperty.BengaliAdverb, LookupWord(GlobalProperty.English));
                    break;
                #endregion
                #region DT
                case TagsType.DT:
                    string _Determiner = ProcessDeterminer(Token.English, GlobalProperty);
                    if (_Determiner.Length > 0)
                    {
                        Token.Bengali = _Determiner;
                        //Token.BengaliPreDeterminer = "";
                    }
                    else
                    {
                        Token.Bengali = LookupWord(Token.English);
                    }
                    break;
                #endregion
                #region IN TO
                case TagsType.IN:
                    Token.Bengali = LookupWord(Token.English + ":IN");
                    if (Token.Bengali == Token.English) // Patch arghya
                    {
                        Token.Bengali = "";
                    }
                    break;
                case TagsType.TO:
                    GlobalProperty.BengaliAdverb = LookupWord(Token.English + ":TO");
                    if (GlobalProperty.IsObject && Token.English.IndexOf('.') > 0)
                    {
                        Token.Bengali = GlobalProperty.BengaliAdverb;
                    }
                    break;
                #endregion

                #region JJ JJR JJS
                case TagsType.JJ:
                case TagsType.JJR:
                case TagsType.JJS:
                    Token.Bengali = LookupWord(Token.English + ":JJ");
                    //Token.Bengali = String.Format("{1} {0}", LookupWord(Token.English + ":JJ"), GlobalProperty.BengaliAdverb);
                    break;
                #endregion
                #region NNS
                case TagsType.NNS:
                    Token.Bengali = LookupWord(Token.English);
                    break;
                #endregion
                #region WDT WRB WPP WP
                case TagsType.WDT:
                case TagsType.WRB:
                case TagsType.WPP:
                case TagsType.WP:
                    //Token.NextWord = GlobalProperty.MainVerb.ToString().ToLower();
                    Token.Bengali = LookupWhWord(Token, ref GlobalProperty);
                    break;
                #endregion
                #region PRP PRPP
                case TagsType.PRP:
                case TagsType.PRPP:
                case TagsType.PP:
                case TagsType.PPP:
                    Token.Bengali = LookupPronoun(Token.English, GlobalProperty);

                    if (GlobalProperty.Purpose == TypesByPurpose.Interrogative
                        && !GlobalProperty.IsWhWordPresent
                        && !GlobalProperty.IsBengaliKiAdded)
                    {
                        Token.Bengali += BengaliWord.ki;
                        GlobalProperty.IsBengaliKiAdded = true;
                    }
                    break;
                #endregion
                #region NN
                case TagsType.NN:
                    Token.Bengali = LookupWord(Token.English + ":NN");
                    break;
                #endregion
                #region NNP NNPS
                case TagsType.NNP:
                case TagsType.NNPS:
                case TagsType.NP:
                case TagsType.NPS:
                    Token.Bengali = LookupWord(Token.English + ":NP");
                    break;
                #endregion
                #region CC
                case TagsType.CC:
                    if (!String.IsNullOrEmpty(Constant.Conjunction.Table(Token.English)))
                    {
                        Token.Bengali = Constant.Conjunction.Table(Token.English);
                    }
                    else
                    {
                        Token.Bengali = LookupWord(Token.English);
                    }
                    break;
                #endregion
                #endregion
                case ".":
                    if (Token.English.Length == 1)
                    {
                        Token.Bengali = Constant.Punctuation.Table(char.Parse(Token.English)).ToString();
                    }
                    else
                    {
                        Token.Bengali = LookupWord(Token.English);
                    }
                    break;
                default:
                    if (!Token.IsBeginSentense)
                    {
                        Token.Bengali = LookupWord(Token.English);
                        //GlobalProperty.BengaliMainverb = String.Format("{0} {1}", GlobalProperty.BengaliMainverb, Token.Bengali);
                    }
                    break;
            }
        }

        private static string FinalRootVerbAfterPreposition(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string BengaliRootVerb = LookupWord(EnglishWord);
            if (GlobalProperty.IsNegetive)
            {
                BengaliRootVerb = GlobalProperty.BengaliNegationWord + BengaliRootVerb;
            }
            return BengaliRootVerb;
        }

        private static string LookupPronoun(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string BengaliWord = "";

            if (GlobalProperty.IsVerbDetected)
            {
                if (EnglishWord.ToLower().Equals("you")) { EnglishWord = "you_obj"; }
                if (EnglishWord.ToLower().Equals("her")) { EnglishWord = "him"; }
            }
            if (GlobalProperty.Voice == Voice.Passive)
            {
                BengaliWord = LookupPassivePronoun(EnglishWord, GlobalProperty.IsVerbDetected);
            }
            else
            {
                BengaliWord = LookupActivePronoun(EnglishWord, GlobalProperty.IsVerbDetected, GlobalProperty);
            }

            return BengaliWord;
        }

        private static string LookupActivePronoun(string EnglishWord, bool IsObject, GlobalTokenProperty GlobalProperty)
        {
            if (!IsObject && (GlobalProperty.MainVerb == MainVerb.Have || GlobalProperty.ModalType == ModalType.Ought || GlobalProperty.ModalType == ModalType.Should))
            {
                EnglishWord = Constant.Pronoun.Modal(EnglishWord.ToLower());
            }
            if (GlobalProperty.IsWhWordPresent && GlobalProperty.Purpose == TypesByPurpose.Interrogative && EnglishWord.Equals("her") && IsObject)
            {
                EnglishWord = "him";
            }

            return LookupBasicPronounWord(EnglishWord);
        }

        private static string LookupPassivePronoun(string EnglishWord, bool IsObject)
        {
            if (!String.IsNullOrEmpty(Constant.Pronoun.Passive(EnglishWord)))
            {
                EnglishWord = Constant.Pronoun.Passive(EnglishWord);
            }
            return LookupBasicPronounWord(EnglishWord);
        }

        private static string LookupBasicPronounWord(string EnglishWord)
        {
            if (!String.IsNullOrEmpty(Constant.Pronoun.Table(EnglishWord)))
            {
                return Constant.Pronoun.Table(EnglishWord);
            }
            else
            {
                return EnglishWord;
            }
        }

        private static string LookupWhWord(Token Token, ref GlobalTokenProperty GlobalProperty)
        {
            string BengaliWord = "";


            if (GlobalProperty.Purpose == TypesByPurpose.Interrogative)
            {
                //if (!String.IsNullOrEmpty(Token.NextWord))
                //{
                //    BengaliWord = Constant.Wh.Question(String.Format("{0}.{1}", Token.English, Token.NextWord));
                //}

                BengaliWord = Constant.Wh.Question(Token.English);

                if (BengaliWord.Length == 0)
                {
                    BengaliWord = LookupExactWord(Token, ref GlobalProperty);
                }
                //else
                //{
                //    GlobalProperty.WordUsed.Add(Token.NextWord);
                //}
                if (BengaliWord.Length == 0)
                {
                    BengaliWord = Constant.Wh.Question(Token.English);
                }
            }
            else if (!String.IsNullOrEmpty(Constant.Wh.Relation(Token.English)))
            {
                //if (!String.IsNullOrEmpty(Token.NextWord))
                //{
                //    BengaliWord = Constant.Wh.Relation(String.Format("{0}.{1}", Token.English, Token.NextWord));
                //}
                BengaliWord = Constant.Wh.Relation(Token.English);
                if (BengaliWord.Length == 0)
                {
                    BengaliWord = LookupExactWord(Token, ref GlobalProperty);
                }
                //else
                //{
                //    GlobalProperty.WordUsed.Add(Token.NextWord);
                //}
                if (BengaliWord.Length == 0)
                {
                    BengaliWord = Constant.Wh.Relation(Token.English);
                }
            }
            else
            {
                BengaliWord = LookupWord(Token.English);
            }
            return BengaliWord;
        }

        private static string ProcessDeterminer(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string BengaliWord = Constant.Determiner.Table(EnglishWord);
            if (BengaliWord.Trim().Length > 0)
            {
                BengaliWord = String.Format("{0} {1}", BengaliWord, GlobalProperty.BengaliPreDeterminer);
                GlobalProperty.BengaliDeterminerSuffix = Constant.Determiner.Suffix(EnglishWord, GlobalProperty.IsObject);
            }

            return BengaliWord;
        }

        private static string ProcessHhAsMainverb(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string _BengaliWord = "";
            if (GlobalProperty.ModalType == ModalType.Should
                || GlobalProperty.ModalType == ModalType.Ought
                || GlobalProperty.ModalType == ModalType.Can
                || GlobalProperty.ModalType == ModalType.May)
            {
                if (GlobalProperty.IsNegetive)
                {
                    _BengaliWord = Constant.ModalHaveHas.Negation(String.Format("{0}.{1}", (int)GlobalProperty.Tense, GlobalProperty.ModalWord));
                    GlobalProperty.IsNegetiveApplied = _BengaliWord.Length > 0;

                    if (_BengaliWord.Length == 0)
                    {
                        _BengaliWord = Constant.ModalHaveHas.Table(String.Format("{0}.{1}", (int)GlobalProperty.Tense, GlobalProperty.ModalWord));
                    }
                }
                else
                {
                    _BengaliWord = Constant.ModalHaveHas.Table(String.Format("{0}.{1}", (int)GlobalProperty.Tense, GlobalProperty.ModalWord));
                }
            }
            else
            {
                if (GlobalProperty.IsNegetive)
                {
                    _BengaliWord = Constant.HaveHas.Negation(String.Format("{1}.{0}", EnglishWord, (int)GlobalProperty.Tense));
                    GlobalProperty.IsNegetiveApplied = _BengaliWord.Length > 0;
                    if (_BengaliWord.Length == 0)
                    {
                        _BengaliWord = Constant.HaveHas.Table(String.Format("{1}.{0}", EnglishWord, (int)GlobalProperty.Tense));
                    }
                }
                else
                {
                    _BengaliWord = Constant.HaveHas.Table(String.Format("{1}.{0}", EnglishWord, (int)GlobalProperty.Tense));
                }
            }
            if (_BengaliWord == EnglishWord)
                _BengaliWord = LookupWord(EnglishWord);
            return _BengaliWord;
        }

        private static string LookupExactWord(Token Token, ref GlobalTokenProperty GlobalProperty)
        {
            string BengaliWord = LookupDictWord(Token.English.ToLower());
            //string _Word = String.Format("{0}.{1}", Token.English.ToLower(), Token.NextWord.ToLower());
            //string BengaliWord = LookupDictWord(_Word);
            //if (BengaliWord.Length == 0)
            //{
            //    _Word = String.Format("{0}:{1}", Token.English.ToLower(), Token.NextWordType);
            //    BengaliWord = LookupDictWord(_Word);
            //}
            //else
            //{
            //    GlobalProperty.WordUsed.Add(Token.NextWord);
            //}
            return BengaliWord;
        }

        private static string LookupAdverbWord(Token Token, ref GlobalTokenProperty GlobalProperty)
        {
            string _Bengali = LookupExactWord(Token, ref GlobalProperty);
            if (_Bengali.Length == 0)
            {
                _Bengali = LookupWord(Token.English + ":RB");
            }
            return _Bengali;
        }

        public static string LookupWord(string EnglishWord)
        {
            string[] oDictWord = EnglishWord.Split(':');
            string Word = oDictWord[0].ToLower();
            string Tag = string.Empty;
            if (oDictWord.Length > 1)
                Tag = oDictWord[1];
            if (Word.Trim().Length == 0)
                return "";

            if (Tag.Length > 0)
                EnglishWord = String.Format("{0}:{1}", Word, Tag);
            else
                EnglishWord = Word.ToLower();

            string BengaliWord = LookupDictWord(EnglishWord);
            if (BengaliWord.Length > 0)
                return BengaliWord;

            BengaliWord = LookupDictWord(Word);
            if (BengaliWord.Length > 0)
                return BengaliWord;
            /*
            else if (Tag == TagsType.CD || Formatter.WordType(Word, "^[0-9]"))
            {
                return CardinalNumber(Word);
            }
            */
            if (Word.IndexOf('.') > 0) //  Not for Numeric value for dotted word.
            {
                string[] DottedWord = Word.Split('.');
                foreach (string s in DottedWord)
                {
                    EnglishWord = String.Format("{0}:{1}", s.ToLower(), Tag);
                    string TempBengaliWord = (Constant.Dict[EnglishWord] != null) ? Constant.Dict[EnglishWord].ToString() : ((Constant.Dict[s.ToLower()] != null) ? Constant.Dict[s.ToLower()].ToString() : s);
                    BengaliWord += (TempBengaliWord.Length > 0) ? String.Format("{0} ", TempBengaliWord) : String.Format("{0} ", s);
                }
                return BengaliWord;
            }
            else
            {
                NLPLogger.Error(EnglishWord);
                return Word;
            }

        }

        private static string LookupDictWord(string EnglishWord)
        {
            string BengaliWord = (Constant.Dict[EnglishWord] != null) ? Constant.Dict[EnglishWord].ToString() : string.Empty;
            if (BengaliWord.Length == 0)
            {
                if (EnglishWord.EndsWith("s"))
                {
                    if (EnglishWord.Length > 2)
                    {
                        EnglishWord = EnglishWord.TrimEnd('s');
                        BengaliWord = (Constant.Dict[EnglishWord] != null) ? Constant.Dict[EnglishWord].ToString() : string.Empty;
                    }
                }
                else
                {
                    EnglishWord = String.Format("{0}s",EnglishWord);
                    BengaliWord = (Constant.Dict[EnglishWord] != null) ? Constant.Dict[EnglishWord].ToString() : string.Empty;
                }
            }
            return BengaliWord;
        }

        private static string CardinalNumber(string Word)
        {
            string BengaliNumber = "";

            foreach (Char c in Word.ToCharArray())
            {
                BengaliNumber += Constant.Numbers.Table(c).ToString();
            }
            if (BengaliNumber.Length == 0 || BengaliNumber.Equals(Word))
            {
                BengaliNumber = LookupWord(Word + ":CD");
            }
            return BengaliNumber;
        }

        private static string FinalRootVerb(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            if (GlobalProperty.Voice == Voice.Passive)
            {
                return FinalRootVerbPassive(EnglishWord, GlobalProperty);
            }
            else
            {
                return FinalRootVerbActive(EnglishWord, GlobalProperty);
            }
        }

        public static string FinalRootVerbPassive(string EnglishRootVerb, GlobalTokenProperty GlobalProperty)
        {
            string _BengaliRootverb = LookupWord(EnglishRootVerb);
            string VerbSuffix = String.Empty; ;
            if (GlobalProperty.ModalType == ModalType.Should || GlobalProperty.ModalType == ModalType.Ought)
            {
                if (GlobalProperty.IsNegetive)
                {
                    VerbSuffix = Constant.Modal.Passive.Negation(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                    GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                    if (VerbSuffix.Length == 0)
                    {
                        VerbSuffix = Constant.Modal.Passive.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord)).ToString();
                    }
                }
                else
                {
                    VerbSuffix = Constant.Modal.Passive.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord)).ToString();
                }
                _BengaliRootverb = Sondhi.VerbSondhiPassive(_BengaliRootverb, VerbSuffix);
            }
            else if (GlobalProperty.ModalType == ModalType.Can || GlobalProperty.ModalType == ModalType.May)
            {
                if (GlobalProperty.IsNegetive)
                {
                    VerbSuffix = Constant.Modal.Passive.Negation(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                    GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                    if (VerbSuffix.Length == 0)
                    {
                        VerbSuffix = Constant.Modal.Passive.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                    }
                }
                else
                {
                    VerbSuffix = Constant.Modal.Passive.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                }
                _BengaliRootverb = Sondhi.VerbSondhiPreposition(_BengaliRootverb, VerbSuffix);
            }
            else if (GlobalProperty.IsNegetive)
            {
                VerbSuffix = Constant.Verb.VerbSuffixPassive.Negation(String.Format("{0}.{1}", (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                if (VerbSuffix.Length == 0)
                {
                    VerbSuffix = Constant.Verb.VerbSuffixPassive.Table(String.Format("{0}.{1}", (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                }
                _BengaliRootverb = Sondhi.VerbSondhiPassive(_BengaliRootverb, VerbSuffix);
            }
            else
            {
                VerbSuffix = Constant.Verb.VerbSuffixPassive.Table(String.Format("{0}.{1}", (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                _BengaliRootverb = Sondhi.VerbSondhiPassive(_BengaliRootverb, VerbSuffix);
            }
            return _BengaliRootverb;
        }

        private static string FinalRootVerbActive(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string _BengaliRootverb = LookupWord(EnglishWord);
            string VerbSuffix = String.Empty;
            if (GlobalProperty.ModalType == ModalType.Should || GlobalProperty.ModalType == ModalType.Ought)
            {
                if (GlobalProperty.IsNegetive)
                {
                    VerbSuffix = Constant.Modal.Active.Negation(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                    GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                    if (VerbSuffix.Length == 0)
                    {
                        VerbSuffix = Constant.Modal.Active.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                    }
                }
                else
                {
                    VerbSuffix = Constant.Modal.Active.Table(String.Format("{0}.{1}", (int)GlobalProperty.TenseType, GlobalProperty.ModalWord));
                }
                _BengaliRootverb = Sondhi.VerbSondhiPassive(_BengaliRootverb, VerbSuffix);
            }
            else if (GlobalProperty.ModalType == ModalType.Can || GlobalProperty.ModalType == ModalType.May)
            {
                if (GlobalProperty.IsNegetive)
                {
                    VerbSuffix = Constant.Modal.Active.Negation(String.Format("0.{0}", GlobalProperty.ModalWord));
                    GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                    if (VerbSuffix.Length == 0)
                    {
                        VerbSuffix = Constant.Modal.Active.Table(String.Format("0.{0}", GlobalProperty.ModalWord));
                    }
                }
                else
                {
                    VerbSuffix = Constant.Modal.Active.Table(String.Format("0.{0}", GlobalProperty.ModalWord));
                }
                VerbSuffix = Sondhi.VerbSondhiActive(VerbSuffix, Constant.Verb.VerbMod.Table(String.Format("{0}.{1}.1", (int)GlobalProperty.Person, (int)GlobalProperty.Tense)), GlobalProperty.Purpose == TypesByPurpose.Imperative);
                _BengaliRootverb = Sondhi.VerbSondhiPreposition(_BengaliRootverb, VerbSuffix);
            }
            else if (GlobalProperty.IsNegetive)
            {
                //..................................................person,tence,tencetype,mode,nagetive,word
                VerbSuffix = Constant.Verb.VerbMod.Negation(String.Format("{0}.{1}.{2}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                GlobalProperty.IsNegetiveApplied = VerbSuffix.Length > 0;
                if (!String.IsNullOrEmpty(VerbSuffix))
                    _BengaliRootverb = Sondhi.VerbSondhiActive(_BengaliRootverb, VerbSuffix, GlobalProperty.Purpose == TypesByPurpose.Imperative);
                else
                {
                    VerbSuffix = Constant.Verb.VerbMod.Table(String.Format("{0}.{1}.{2}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                    if (!String.IsNullOrEmpty(GlobalProperty.BengaliAdverb))
                    {
                        _BengaliRootverb = String.Format("{0}{1}", _BengaliRootverb, GlobalProperty.BengaliAdverb);
                    }
                    else
                    {
                        _BengaliRootverb = Sondhi.VerbSondhiActive(_BengaliRootverb, VerbSuffix, GlobalProperty.Purpose == TypesByPurpose.Imperative);
                    }
                }

            }
            else
            {
                VerbSuffix = Constant.Verb.VerbMod.Table(String.Format("{0}.{1}.{2}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense, (int)GlobalProperty.TenseType));
                if (!String.IsNullOrEmpty(GlobalProperty.BengaliAdverb))
                {
                    _BengaliRootverb = String.Format("{0}{1}", _BengaliRootverb, GlobalProperty.BengaliAdverb);
                }
                else
                {
                    _BengaliRootverb = Sondhi.VerbSondhiActive(_BengaliRootverb, VerbSuffix, GlobalProperty.Purpose == TypesByPurpose.Imperative);
                }
            }
            return _BengaliRootverb;
        }

        private static string ProcessBeAsMainverb(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string _BengaliWord = "";
            if (GlobalProperty.IsNegetive)
            {
                _BengaliWord = Constant.Verb.BeVerb.MainVerb.Negation(String.Format("{0}.{1}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense));
                GlobalProperty.IsNegetiveApplied = _BengaliWord.Length > 0;
                if (_BengaliWord.Length == 0)
                {
                    _BengaliWord = Constant.Verb.BeVerb.MainVerb.Table(String.Format("{0}.{1}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense));
                }
            }
            else
            {
                _BengaliWord = Constant.Verb.BeVerb.MainVerb.Table(String.Format("{0}.{1}", (int)GlobalProperty.Person, (int)GlobalProperty.Tense));
            }
            return _BengaliWord;
        }

        private static string ProcessBeAsNonMainverb(string EnglishWord, GlobalTokenProperty GlobalProperty)
        {
            string _BengaliWord = ""; ;
            if (GlobalProperty.IsNegetive)
            {
                if (GlobalProperty.IsExistentialThere)
                {
                    _BengaliWord = Constant.Verb.BeVerb.ExtendedAuxilearyVerb.Negation(String.Format("{0}.{1}", (int)GlobalProperty.Tense, EnglishWord));
                }
                if (_BengaliWord.Length == 0)
                {
                    _BengaliWord = Constant.Verb.BeVerb.AuxilearyVerb.Negation(String.Format("{0}.{1}", (int)GlobalProperty.Person, EnglishWord));
                }
                if (_BengaliWord.Length == 0)
                {
                    _BengaliWord = Constant.Verb.BeVerb.AuxilearyVerb.Table(String.Format("{0}.{1}", (int)GlobalProperty.Person, EnglishWord));
                    if (_BengaliWord.Length == 0)
                    {
                        _BengaliWord = LookupWord(EnglishWord);
                    }
                    _BengaliWord = String.Format("{0} {1}", _BengaliWord, GlobalProperty.BengaliNegationWord);
                }
            }
            else
            {
                if (GlobalProperty.IsExistentialThere)
                {
                    _BengaliWord = Constant.Verb.BeVerb.ExtendedAuxilearyVerb.Table(String.Format("{0}.{1}", (int)GlobalProperty.Tense, EnglishWord));
                }
                if (_BengaliWord.Length == 0)
                {
                    _BengaliWord = Constant.Verb.BeVerb.AuxilearyVerb.Table(String.Format("{0}.{1}", (int)(int)GlobalProperty.Person, EnglishWord));
                }
                if (_BengaliWord.Length == 0)
                {
                    _BengaliWord = LookupWord(EnglishWord);
                }
            }

            return _BengaliWord;
        }

        public static string TranslateConjugteWord(string English, string Type)
        {
            string BengaliWord = String.Empty;
            English = Regex.Replace(English.ToLower(), "\\s+", " ");
            if (English.IndexOf(" ") > 0)
            {
                English = English.Replace(" ", ".");
                BengaliWord = (Constant.Dict[English + ":" + Type] != null) ?
                    Constant.Dict[English + ":" + Type].ToString() :
                    (
                        (Constant.Dict[English] != null) ?
                        Constant.Dict[English].ToString() :
                        String.Empty
                    );
            }
            return BengaliWord;
        }
    }
}
