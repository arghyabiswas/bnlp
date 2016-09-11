using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace NLPToken
{
    public class Sondhi
    {
        const string Consonant = "ক|খ|গ|ঘ|ঙ|চ|ছ|জ|ঝ|ঞ|ট|ঠ|ড|ঢ|ণ|ত|থ|দ|ধ|ন|প|ফ|ব|ভ|ম|য|র|ল|শ|ষ|স|হ|ড়|ঢ়|য়";
        const string Vowel = "অ|আ|ই|ঈ|উ|ঊ|ঋ|এ|ঐ|ও|ঔ";
        const string Matra = "া|ি|ী|ু|ূ|ৃ|ে|ৈ|ো|ৌ";
        const string Hasant = "ঁ|ং|:|্";

        static string Any = String.Format("{0}|{1}|{2}|{3}", Consonant, Vowel, Matra, Hasant);

        static public string VerbSondhiPreposition(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

            #region "XX_khol+te_YY =>  XX_khulte_YY
            ExpressionRight = String.Format("{0}{1}([{2}]*)", (char)BengaliLetter.TA, (char)BengaliLetter.ekar, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {

                ExpressionLeft = String.Format("([{2}]*)({0})({1})({0})$", Consonant, (char)BengaliLetter.okar, Any);
                OutPutExpression = String.Format("$1$2{0}$4{1}", (char)BengaliLetter.ukar, WordRight);

                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                }
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion
            #region "XX_ne+ar_YY =>  XX_neoar_YY
            ExpressionRight = String.Format("({0}|{1})({2})([{3}]*)", (char)BengaliLetter.akar, (char)BengaliLetter.ekar, (char)BengaliLetter.RA, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                WordRight = String.Format("{0}{1}", (char)BengaliLetter.akar, WordRight.Substring(1, WordRight.Length - 1));

                ExpressionLeft = String.Format("([{3}]*)({0})({1}|{2})$", Consonant, (char)BengaliLetter.ekar, (char)BengaliLetter.ikar, Any);
                OutPutExpression = String.Format("$1$2{0}{1}{2}{3}", (char)BengaliLetter.ekar, (char)BengaliLetter.O, (char)BengaliLetter.YYA, WordRight);

                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion

            #region XX_de+te_YY =>  XX_dite_YY || XX_pa+te_YY =>  XX_pete_YY
            ExpressionRight = String.Format("({0})({1})([{2}]*)", (char)BengaliLetter.TA, (char)BengaliLetter.ekar, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1}|{3})({2})$", Any, Consonant, (char)BengaliLetter.ekar, (char)BengaliLetter.akar);
                OutPutExpression = String.Format("$1$2{0}{1}", (char)BengaliLetter.ikar, WordRight);

                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion

            #region XX_KAKA+er_YY =>  XX_KAKAar_YY
            ExpressionRight = String.Format("({0})({1})([{2}]*)", (char)BengaliLetter.ekar, (char)BengaliLetter.RA, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})({1})$", Any, Consonant);

                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}{1}", (char)BengaliLetter.akar, WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;

                }
            }
            #endregion


            #region XX_KAKA+er_YY =>  XX_KAKAar_YY
            ExpressionRight = String.Format("({0})([{1}]*)", Matra, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, Matra);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
            }
            #endregion

            return OutPutWord;
        }

        static public string SondhiPreposition(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;
            if (WordLeft.Length == 0 || WordRight.Length == 0)
                return String.Format("{0}{1}", WordLeft, WordRight);

            #region lekh =>  likh and General
            ExpressionLeft = String.Format("([{0}]*)({1})({2})({2})$", Any, (char)BengaliLetter.LA, (char)BengaliLetter.ekar, Consonant);
            OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ikar);
            WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);

            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
            #endregion

            #region Ei+ti =>  Eti
            ExpressionRight = String.Format("({0})({1})([{2}]*)", (char)BengaliLetter.TA, (char)BengaliLetter.ikar, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, (char)BengaliLetter.E, (char)BengaliLetter.I);
                OutPutExpression = String.Format("$1$2{0}", WordRight);

                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }

            #endregion

            #region XX_ke+er_YY =>  XX_ar_YY ( ex. amake+er jonyo = amar + jonyo
            ExpressionRight = String.Format("({0})({1})([{2}]*)", (char)BengaliLetter.ekar, (char)BengaliLetter.RA, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})({1})$", Any, Consonant);
                WordRight = String.Format("{0}{1}", (char)BengaliLetter.akar, WordRight.Substring(1, WordRight.Length - 1));
                OutPutExpression = String.Format("$1$2$2{0}", WordRight);
                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                if (OutPutWord.Length > 0)
                    return OutPutWord;

                ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, (char)BengaliLetter.KA, (char)BengaliLetter.ekar);
                WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                OutPutExpression = String.Format("$1{0}", WordRight);

                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion

            ExpressionRight = String.Format("({3})({0})({1})([{2}]*)", (char)BengaliLetter.TA, (char)BengaliLetter.ekar, Any, Consonant);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }

            ExpressionRight = String.Format("^({0})", Matra);
            if (Regex.IsMatch(WordRight, ExpressionRight)) // Problem i have no home
            {
                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, Matra);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
            }

            ExpressionRight = String.Format("({0})({1})([{2}]*)", (char)BengaliLetter.ekar, (char)BengaliLetter.RA, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, (char)BengaliLetter.anusvar);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}{1}", (char)BengaliLetter.E, WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
            }

            return OutPutWord;
        }

        static public string BanglaSandhi(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

            ExpressionRight = String.Format("{0}{1}([{2}]*)", (char)BengaliLetter.TA, (char)BengaliLetter.ekar, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {

                ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, Matra, Consonant);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }


                ExpressionRight = String.Format("({0})([{1}]*)", Matra, Any);
                if (Regex.IsMatch(WordRight, ExpressionRight))
                {
                    ExpressionLeft = String.Format("([{0}]*)({1})$", Any, Matra);
                    if (Regex.IsMatch(WordLeft, ExpressionLeft))
                    {
                        WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                        OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                        if (OutPutWord.Length > 0)
                            return OutPutWord;
                    }
                }
            }

            return OutPutWord;
        }

        static public string BanglaSondhiPossessive(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

            ExpressionRight = String.Format("({0})([{1}]*)", Matra, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, Matra);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordRight = String.Format("{0}", WordRight.Substring(1, WordRight.Length - 1));
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
            }

            return OutPutWord;
        }

        static public string VerbSondhi(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

            ExpressionLeft = String.Format("([{0}]*)({1})$", Any, (char)BengaliLetter.ikar);
            OutPutExpression = String.Format("$1{0}{1}{2}{3}", (char)BengaliLetter.ekar, WordRight, (char)BengaliLetter.O, (char)BengaliLetter.YYA);
            OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);


            return OutPutWord;
        }

        static public string VerbSondhiPassive(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Empty;

            ExpressionLeft = String.Format("([{0}]*)({1})({2}|{1})$", Any, Consonant, Matra);
            if (Regex.IsMatch(WordLeft, ExpressionLeft))
            {
                OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
            }
            else
            {
                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, (char)BengaliLetter.ikar);
                OutPutExpression = String.Format("$1{0}{1}{2}{3}", (char)BengaliLetter.ekar, WordRight, (char)BengaliLetter.O, (char)BengaliLetter.YYA);
                OutPutWord = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
            }
            return OutPutWord;
        }

        static public string VerbSondhiActive(string WordLeft, string WordRight, bool IsImperative)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;

            string OutPutWord = String.Empty;

            if (WordRight.Equals((char)BengaliLetter.oikar))
            {
                return VerbSondhiActiveSuffixOkar(WordLeft, WordRight);
            }

            #region de => di
            ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, Consonant, (char)BengaliLetter.ekar);
            OutPutExpression = String.Format("$1$2{0}", (char)BengaliLetter.ikar);
            WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);

            #endregion

            #region lekh =>  likh
            ExpressionLeft = String.Format("([{0}]*)({1})({2})({3})$", Any, (char)BengaliLetter.LA, (char)BengaliLetter.ekar, Consonant);
            OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ikar);
            WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
            #endregion

            OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

            #region 3rd person, present simple 3, present, s  XX + "ekar"
            if (WordRight.Equals(((char)BengaliLetter.ekar).ToString()))
            {
                #region likh =>  lekh
                ExpressionLeft = String.Format("([{0}]*)({1})({2})({3})$", Any, (char)BengaliLetter.LA, (char)BengaliLetter.ikar, Consonant);
                OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ekar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                #endregion

                #region di =>  de
                ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, Consonant, (char)BengaliLetter.ikar);
                OutPutExpression = String.Format("$1$2{0}", (char)BengaliLetter.ekar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                #endregion


                ExpressionLeft = String.Format("([{0}]*)({1}|{2})({1})$", Any, Consonant, Matra);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
                else
                {
                    OutPutWord = String.Format("{0}{1}", WordLeft, MapToProperBengaliLetter(WordRight));
                }
            }
            #endregion

            #region  2nd person, present simple  2, present, s  XX + "ekar NA"
            ExpressionRight = String.Format("{0}{1}", (char)BengaliLetter.ekar, (char)BengaliLetter.NA);
            if (WordRight.Equals(ExpressionRight))
            {
                #region likh =>  lekh
                ExpressionLeft = String.Format("([{0}]*)({1})({2})({3})$", Any, (char)BengaliLetter.LA, (char)BengaliLetter.ikar, Consonant);
                OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ekar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                #endregion

                ExpressionLeft = String.Format("([{0}]*)({1}|{2}|{3})({1})$", Any, Consonant, Vowel, (char)BengaliLetter.ekar);
                if (Regex.IsMatch(WordLeft, ExpressionLeft)) 
                {
                    if (IsImperative)
                    {
                        ExpressionRight = String.Format("({0})([{1}])", (char)BengaliLetter.ekar, Consonant);
                        OutPutExpression = String.Format("{0}$2", (char)BengaliLetter.ukar);
                        WordRight = Regex.Replace(WordRight, ExpressionRight, OutPutExpression);
                        OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    }
                    else
                    {
                        OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    }

                    if (OutPutWord.Length > 0)
                        return OutPutWord;
                }
                else
                {
                    ExpressionLeft = String.Format("([{0}]*)({1})({2})$", Any, Matra, Consonant);

                    if (Regex.IsMatch(WordLeft, ExpressionLeft))
                    {
                        OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                        if (OutPutWord.Length > 0)
                            return OutPutWord;
                    }
                    else
                    {
                        OutPutWord = String.Format("{0}{1}", WordLeft, MapToProperBengaliLetter(WordRight));
                    }
                }

            }
            #endregion

            #region 1st/2nd/3rd person, future simple  XX + "BA"   XX + "be"  XX + "ben"
            ExpressionRight = String.Format("({0})([{1}]*)", (char)BengaliLetter.BA, Any);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                ExpressionLeft = String.Format("([{1}]*)({0})$", (char)BengaliLetter.ikar, Any);
                OutPutExpression = String.Format("$1{0}", (char)BengaliLetter.ikar);

                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);

                OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);

                if (OutPutWord.Length > 0)
                    return OutPutWord;

            }
            #endregion

            #region 1st/2nd/3rd person, past simple  XX + "lam"    XX + "len"    XX + "la"
            if (WordRight.EndsWith(((char)BengaliLetter.LA).ToString())
                || (WordRight.EndsWith(String.Format("{0}{1}{2}", (char)BengaliLetter.LA, (char)BengaliLetter.akar, (char)BengaliLetter.MA)))
                || (WordRight.EndsWith(String.Format("{0}{1}{2}", (char)BengaliLetter.LA, (char)BengaliLetter.ekar, (char)BengaliLetter.NA))))
            {
                #region // YA akar + LA  =  GA akar + LA
                ExpressionLeft = String.Format("( )({0})({1})$", (char)BengaliLetter.YA, (char)BengaliLetter.akar);
                OutPutExpression = String.Format("$1{0}$3", (char)BengaliLetter.GA);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);


                ExpressionLeft = String.Format("([{0}]*)({1})$", Any, (char)BengaliLetter.akar);
                OutPutExpression = String.Format("$1{0}{1}", (char)BengaliLetter.ekar,(char)BengaliLetter.YA);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                }
                OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                if (OutPutWord.Length > 0)
                    return OutPutWord;
                #endregion
            }
            #endregion

            #region 1st person, present simple XX + "ikar"
            if (WordRight.Equals((char)BengaliLetter.ikar))
            {
                ExpressionLeft = String.Format("([{0}]*)({1})({2})({3})$", Any, (char)BengaliLetter.LA, (char)BengaliLetter.ekar, Consonant);
                OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ikar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);

                ExpressionLeft = String.Format("([{0}]*)({1}|{2})({1})$", Any, Consonant, Matra);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                }
                else
                {
                    OutPutWord = String.Format("{0}{1}", WordLeft, MapToProperBengaliLetter(WordRight));
                }
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion

            #region 1st/2nd/3rd person, present/past continuous  XX + "chi"    XX + "che"  XX + "chen"
            //ExpressionRight = String.Format("({1})([{0}]*)", (char)BengaliLetter.CHA, (char)BengaliLetter.ikar);
            //string ExpressionRight1 = String.Format("({1})([{0}]*)", (char)BengaliLetter.CHA, (char)BengaliLetter.ekar);
            
            if(WordRight.StartsWith(String.Format("{0}{1}", (char)BengaliLetter.CHA, (char)BengaliLetter.ekar))
                || WordRight.StartsWith(String.Format("{0}{1}", (char)BengaliLetter.CHA, (char)BengaliLetter.ikar)))

            {
                #region  ...ha + chi   ... => ...ha + ch + chi ..
                ExpressionLeft = String.Format("({1})({0})$", Consonant, Any);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                }
                #endregion

                #region  ...kh+ akar + chh + ... => ...kh + akar + ch+ chh..
                else
                {
                    ExpressionLeft = String.Format("([{0}]*)({1})({2})({1})$", Any, Consonant, (char)BengaliLetter.akar);
                    OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ekar);
                    if (Regex.IsMatch(WordLeft, ExpressionLeft))
                    {
                        WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                        OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                    }
                    else
                    {
                        OutPutWord = String.Format("{0}{2}{3}{1}", WordLeft, WordRight, (char)BengaliLetter.CA, (char)BengaliLetter.hasant);
                    }
                }
                #endregion
                if (OutPutWord.Length > 0)
                    return OutPutWord;
            }
            #endregion

            #region 1st/2nd/3rd person, present/past, perfect  XX + "echhi"    XX + "echhe"    XX + "echhen"
            ExpressionRight = String.Format("({0})({1})({0}|{2})$",(char)BengaliLetter.ekar, (char)BengaliLetter.CHA, (char)BengaliLetter.ikar);
            if (Regex.IsMatch(WordRight, ExpressionRight))
            {
                #region  YA akar + LA  =  GA ikar + LA
                ExpressionLeft = String.Format("( )({0})({1})$", (char)BengaliLetter.YA, (char)BengaliLetter.akar);
                OutPutExpression = String.Format("{0}{1}", (char)BengaliLetter.GA, (char)BengaliLetter.ikar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                #endregion

                #region bas  =>  bes
                ExpressionLeft = String.Format("({0})({1})({0})$", Consonant, (char)BengaliLetter.akar);
                OutPutExpression = String.Format("$1$2{0}$4", (char)BengaliLetter.ikar);
                WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                #endregion

                #region  ...ha + ekar   ... => ...ha + Ya+ ekar ..
                ExpressionLeft = String.Format("( )({0})$", Consonant);
                if (Regex.IsMatch(WordLeft, ExpressionLeft))
                {
                    OutPutWord = String.Format("{0}{2}{1}", WordLeft, WordRight, (char)BengaliLetter.YYA);
                }
                else //...ha + ekar   ... => ...ha + Ya+ ekar ..
                {
                    ExpressionLeft = String.Format("([{2}]*)({0})({1})$", Consonant, (char)BengaliLetter.ikar, Any);
                    if (Regex.IsMatch(WordLeft, ExpressionLeft))
                    {
                        OutPutWord = String.Format("{0}{2}{1}", WordLeft, WordRight, (char)BengaliLetter.YYA);
                    }
                    else
                    {
                        ExpressionLeft = String.Format("([{3}]*)({0}|{1})({0})({2})$", Consonant, Matra, (char)BengaliLetter.akar, Any);
                        if (Regex.IsMatch(WordLeft, ExpressionLeft))
                        {
                            ExpressionLeft = String.Format("([{0}]*)({1})([{0}])([{0}])$", (char)BengaliLetter.oikar, Any);
                            OutPutExpression = String.Format("$1{0}$3{1}", (char)BengaliLetter.ukar, (char)BengaliLetter.ikar);
                            WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                            OutPutWord = String.Format("{0}{2}{1}", WordLeft, WordRight, (char)BengaliLetter.YYA);

                            ExpressionLeft = String.Format("([{0}]*)({1})$", (char)BengaliLetter.akar, Any);
                            OutPutExpression = String.Format("$1{0}", (char)BengaliLetter.ekar);
                            WordLeft = Regex.Replace(WordLeft, ExpressionLeft, OutPutExpression);
                            OutPutWord = String.Format("{0}{2}{1}", WordLeft, WordRight, (char)BengaliLetter.YYA);

                            if (OutPutWord.Length > 0)
                                return OutPutWord;
                            else
                                return String.Format("{0}{1}", WordLeft, WordRight);
                        }


                    }
                }
                #endregion
            }
            #endregion


            return OutPutWord;
        }

        static private string MapToProperBengaliLetter(string WordRight)
        {
            string OutPutWord = Constant.Map.Table(WordRight);

            return OutPutWord;
        }

        static private string VerbSondhiActiveSuffixOkar(string WordLeft, string WordRight)
        {
            string OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
            return OutPutWord;
        }

        internal static string SondhiTO(string WordLeft, string WordRight)
        {
            string ExpressionLeft = String.Empty;
            string ExpressionRight = String.Empty;
            string OutPutExpression = String.Empty;
            WordLeft = WordLeft.Trim();

            string OutPutWord = String.Empty;
            OutPutWord = String.Format("{0} {1}", WordLeft, WordRight);

            ExpressionLeft = String.Format("([{0}]*)({1})$", Any, Consonant);

            if (Regex.IsMatch(WordLeft, ExpressionLeft)) // Last word Consonent
            {
                if (Regex.IsMatch(WordRight, String.Format("{0}", (char)BengaliLetter.YYA)))
                    return String.Format("{0}{1}{2}", WordLeft, WordRight, (char)BengaliLetter.ekar);
                if (Regex.IsMatch(WordRight, String.Format("{0}{1}", (char)BengaliLetter.E, (char)BengaliLetter.RA)))
                    return  String.Format("{0}{1}{2}", WordLeft, (char)BengaliLetter.ekar, (char)BengaliLetter.RA);
                else if (Regex.IsMatch(WordRight, String.Format("^([{0}])",Matra)))
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
                else if (Regex.IsMatch(WordRight, String.Format("^([{0}])", Consonant)))
                    OutPutWord = String.Format("{0}{2}{3} {1}", WordLeft, WordRight, (char)BengaliLetter.ekar, (char)BengaliLetter.RA);
            }
            else
            {
                if (Regex.IsMatch(WordRight, String.Format("{0}", (char)BengaliLetter.YYA)))
                    OutPutWord = String.Format("{0}{1}", WordLeft, WordRight);
            }
            return OutPutWord;
        }
    }
}
