using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace NLPToken
{
    public static class Constant
    {
        public static ConstantPronoun Pronoun;
        public static ConstantPerson Person;
        public static ConstantDeterminer Determiner;
        public static ConstantPreposition Preposition;
        public static ConstantConjunction Conjunction;
        public static ConstantHaveHas HaveHas;
        public static ConstantAdverbNegation AdverbNegation;
        public static ConstantModalHaveHas ModalHaveHas;
        public static ConstantVerb Verb;
        public static ConstantModal Modal;
        public static ConstantMatra Matra;
        public static ConstantNumbers Numbers;
        public static ConstantWh Wh;
        public static ConstantPunctuation Punctuation;
        public static MapWord Map;
        public static Hashtable Lexicon;
        public static Hashtable Lemma;
        public static StringCollection TheRules;
        public static StringCollection TheContext;
        public static BnDict Dict;
        public static List<TranslationFormatText> PreTranslationFormat;
        public static List<TranslationFormatText> PostTaggedFormat;
        public static List<TranslationFormatText> PostProcessFormat;
        public static List<TranslationFormatText> Phase;

        public static Hashtable KnownVvg;

        static string[] have_verbs = { "have", "has", "had", "having", "'ve", "'d" };
        static string[] be_verbs = { "be", "been", "being", "am", "is", "are", "was", "were", "'m" };

        static string[] verb_map = {
		"VB", "VH", "VV", 
		"VBD", "VHD", "VVD", 
		"VBG", "VHG", "VVG", 
		"VBN", "VHN", "VVN", 
		"VBP", "VHP", "VVP", 
		"VBZ", "VHZ", "VVZ" };

        static string[] tag_map = {
		"PRP", "PP",
		"PRP$", "PP$",
		"NNP", "NP",
		"NNPS", "NPS"};

        public static bool IsBeVerbPresent(string _w)
        {
            foreach (string _s in be_verbs)
            {
                if (_s.Equals(_w.ToLower()))
                    return true;
            }
            return false;
        }

        public static bool IsHaveVerbPresent(string _w)
        {
            foreach (string _s in have_verbs)
            {
                if (_s.Equals(_w.ToLower()))
                    return true;
            }
            return false;
        }

        static Constant()
        {
            Pronoun = new ConstantPronoun();
            Person = new ConstantPerson();
            Determiner = new ConstantDeterminer();
            Preposition = new ConstantPreposition();
            Conjunction = new ConstantConjunction();
            HaveHas = new ConstantHaveHas();
            AdverbNegation = new ConstantAdverbNegation();
            ModalHaveHas = new ConstantModalHaveHas();
            Verb = new ConstantVerb();
            Modal = new ConstantModal();
            Matra = new ConstantMatra();
            Numbers = new ConstantNumbers();
            Wh = new ConstantWh();
            Punctuation = new ConstantPunctuation();
            Map = new MapWord();
            Dict = new BnDict();

            PreTranslationFormat = new List<TranslationFormatText>();
            StreamReader SR = new StreamReader(String.Format("{0}//PreTranslationFormat.bnlp", GlobalVariable.DictionaryLocation));
            string _FormatExpression = SR.ReadLine();
            string _FormatToReplace = SR.ReadLine();
            while (_FormatExpression != null && _FormatExpression.Length > 0 && _FormatToReplace != null && _FormatToReplace.Length > 0)
            {
                TranslationFormatText _PreTranslationFormatText = new TranslationFormatText();
                _PreTranslationFormatText.Expression = _FormatExpression;
                _PreTranslationFormatText.Replace = _FormatToReplace;
                PreTranslationFormat.Add(_PreTranslationFormatText);

                _FormatExpression = SR.ReadLine();
                _FormatToReplace = SR.ReadLine();
            }

            PostTaggedFormat = new List<TranslationFormatText>();

            SR = new StreamReader(String.Format("{0}/PostTaggedFormat.bnlp", GlobalVariable.DictionaryLocation));
            _FormatExpression = SR.ReadLine();
            _FormatToReplace = SR.ReadLine();
            while (_FormatExpression != null && _FormatExpression.Length > 0 && _FormatToReplace != null && _FormatToReplace.Length > 0)
            {
                TranslationFormatText _PreTranslationFormatText = new TranslationFormatText();
                _PreTranslationFormatText.Expression = _FormatExpression.Replace("\\t", "\t");
                _PreTranslationFormatText.Replace = _FormatToReplace.Replace("\\t", "\t");
                PostTaggedFormat.Add(_PreTranslationFormatText);

                _FormatExpression = SR.ReadLine();
                _FormatToReplace = SR.ReadLine();
            }

            Phase = new List<TranslationFormatText>();
            SR = new StreamReader(String.Format("{0}/Phase.bnlp", GlobalVariable.DictionaryLocation));
            _FormatExpression = SR.ReadLine();
            _FormatToReplace = SR.ReadLine();

            while (_FormatExpression != null && _FormatExpression.Length > 0 && _FormatToReplace != null && _FormatToReplace.Length > 0)
            {
                TranslationFormatText _PreTranslationFormatText = new TranslationFormatText();
                _PreTranslationFormatText.Expression = _FormatExpression;
                _PreTranslationFormatText.Replace = _FormatToReplace;
                Phase.Add(_PreTranslationFormatText);

                _FormatExpression = SR.ReadLine();
                _FormatToReplace = SR.ReadLine();
            }


            PostProcessFormat = new List<TranslationFormatText>();

            SR = new StreamReader(String.Format("{0}/PostProcessFormat.bnlp", GlobalVariable.DictionaryLocation));
            _FormatExpression = SR.ReadLine();
            _FormatToReplace = SR.ReadLine();
            while (_FormatExpression != null && _FormatExpression.Length > 0 && _FormatToReplace != null && _FormatToReplace.Length > 0)
            {
                TranslationFormatText _PreTranslationFormatText = new TranslationFormatText();
                _PreTranslationFormatText.Expression = _FormatExpression;
                _PreTranslationFormatText.Replace = _FormatToReplace;
                PostProcessFormat.Add(_PreTranslationFormatText);

                _FormatExpression = SR.ReadLine();
                _FormatToReplace = SR.ReadLine();
            }

            SR.Close();

            ConstructLexicon();
            //KnownVvg = new Hashtable();
            //KnownVvg.Add("including", "IN");
            //KnownVvg.Add("using", "IN");
        }

        public class ConstantPronoun
        {
            private readonly Hashtable _Modal;
            private readonly Hashtable _Passive;
            private readonly Hashtable _Table;

            public string Modal(string Key)
            {
                Key = Key.ToLower();
                return (_Modal[Key] != null) ? _Modal[Key].ToString() : Key;
            }

            public string Passive(string Key)
            {
                Key = Key.ToLower();
                return (_Passive[Key] != null) ? _Passive[Key].ToString() : "";
            }

            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
            }

            public ConstantPronoun()
            {
                _Modal = new Hashtable();
                _Modal.Add("i", "my");
                _Modal.Add("we", "our");
                _Modal.Add("you", "your");
                _Modal.Add("he", "his");
                _Modal.Add("she", "her");
                _Modal.Add("they", "their");

                _Passive = new Hashtable();
                _Passive.Add("i", "me");
                _Passive.Add("we", "us");
                _Passive.Add("you", "you_obj");
                _Passive.Add("he", "him");
                _Passive.Add("she", "him");
                _Passive.Add("they", "them");
                _Passive.Add("me", "my");
                _Passive.Add("us", "our");
                _Passive.Add("you_obj", "your");
                _Passive.Add("him", "his");
                _Passive.Add("her", "his");
                _Passive.Add("them", "their");

                _Table = new Hashtable();
                _Table.Add("i", "আমি");
                _Table.Add("me", "আমাকে");
                _Table.Add("my", "আমার");
                _Table.Add("mine", "আমার");
                _Table.Add("we", "আমরা");
                _Table.Add("our", "আমাদের");
                _Table.Add("ours", "আমাদের");
                _Table.Add("us", "আমাদেরকে");
                _Table.Add("you", "আপনি");
                _Table.Add("you_obj", "আপনাকে");
                _Table.Add("your", "আপনার");
                _Table.Add("he", "সে");
                _Table.Add("she", "সে");
                _Table.Add("him", "তাকে");
                _Table.Add("his", "তার");
                _Table.Add("her", "তার");
                _Table.Add("they", "তারা");
                _Table.Add("their", "তাদের");
                _Table.Add("them", "তাদের");
                _Table.Add("myself", "নিজে");
                _Table.Add("ourselves", "নিজেরা");
                _Table.Add("yourself", "নিজে");
                _Table.Add("yourselves", "নিজেরা");
                _Table.Add("himself", "নিজে");
                _Table.Add("herself", "নিজে");
                _Table.Add("themselves", "নিজেরা");
                _Table.Add("it", "এইটি");
                _Table.Add("its", "এর");
                _Table.Add("itself", "নিজেরা");
                _Table.Add("let\"s", "আসুন");
                _Table.Add("lets", "আসুন");
                _Table.Add("each.other", "একেঅপরকে");
            }
        }

        public class ConstantPerson
        {
            private readonly Hashtable _Table;

            public Person Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? (NLPToken.Person)_Table[Key] : NLPToken.Person.Third;
            }

            public ConstantPerson()
            {
                _Table = new Hashtable();
                _Table.Add("i", NLPToken.Person.First);
                _Table.Add("me", NLPToken.Person.First);
                _Table.Add("my", NLPToken.Person.Third);
                _Table.Add("mine", NLPToken.Person.First);
                _Table.Add("we", NLPToken.Person.First);
                _Table.Add("us", NLPToken.Person.First);
                _Table.Add("you", NLPToken.Person.Second);
                _Table.Add("he", NLPToken.Person.Third);
                _Table.Add("she", NLPToken.Person.Third);
                _Table.Add("him", NLPToken.Person.Third);
                _Table.Add("his", NLPToken.Person.Third);
                _Table.Add("her", NLPToken.Person.Third);
                _Table.Add("they", NLPToken.Person.Third);
                _Table.Add("them", NLPToken.Person.Third);
                _Table.Add("it", NLPToken.Person.Third);
                _Table.Add("our", NLPToken.Person.Third);
                _Table.Add("your", NLPToken.Person.Third);
                _Table.Add("their", NLPToken.Person.Third);
                _Table.Add("let's", NLPToken.Person.First);
                _Table.Add("lets", NLPToken.Person.First);
            }
        }

        public class ConstantDeterminer
        {
            private readonly Hashtable _Table;
            private readonly Hashtable _Suffix;
            private readonly Hashtable _SubjectSuffix;
            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : "";
            }

            public string Suffix(string Key, bool IsObject)
            {
                if (IsObject)
                {
                    return Suffix(Key);
                }
                else
                {
                }
                Key = Key.ToLower();
                return (_SubjectSuffix[Key] != null) ? _SubjectSuffix[Key].ToString() : Suffix(Key);
            }

            public string Suffix(string Key)
            {
                Key = Key.ToLower();
                return (_Suffix[Key] != null) ? _Suffix[Key].ToString() : "";
            }
            public ConstantDeterminer()
            {
                _Table = new Hashtable();
                _Table.Add("a", "একটি");
                _Table.Add("an", "একটি");
                _Table.Add("the", " ");
                _Table.Add("this", "এই");
                _Table.Add("that", "সেই");
                _Table.Add("these", "এই");
                _Table.Add("those", "সেই");
                _Table.Add("some", "কিছু");
                _Table.Add("every", "প্রতি");
                _Table.Add("no", "কোনও");
                _Table.Add("no.such", "তেমনকোনও");
                _Table.Add("neither", "কোনও");
                _Table.Add("no.longer", "আর");
                _Table.Add("no.more", "আর");

                _Suffix = new Hashtable();
                _Suffix.Add("a", " ");
                _Suffix.Add("an", " ");
                _Suffix.Add("the", "টি");
                _Suffix.Add("this", "টি");
                _Suffix.Add("that", "টার");
                _Suffix.Add("these", "গুলি");
                _Suffix.Add("those", "গুলি");
                _Suffix.Add("some", " ");
                _Suffix.Add("every", " ");
                _Suffix.Add("no", "না");
                _Suffix.Add("no.such", "নেই");
                _Suffix.Add("neither", "না");
                _Suffix.Add("no.longer", "না");
                _Suffix.Add("no.more", "না");

                _SubjectSuffix = new Hashtable();
                //_SubjectSuffix.Add("a", " ");
                //_SubjectSuffix.Add("an", " ");
                //_SubjectSuffix.Add("the", "টি");
                //_SubjectSuffix.Add("this", "টির");
                //_SubjectSuffix.Add("that", "টার");
                //_SubjectSuffix.Add("these", "গুলি");
                //_SubjectSuffix.Add("those", "গুলি");
                //_SubjectSuffix.Add("some", " ");
                //_SubjectSuffix.Add("every", " ");
                //_SubjectSuffix.Add("no", "না");
                //_SubjectSuffix.Add("no.such", "নেই");
                //_SubjectSuffix.Add("neither", "না");
                //_SubjectSuffix.Add("no.longer", "না");
                //_SubjectSuffix.Add("no.more", "না");
            }
        }

        public class ConstantPreposition
        {
            private readonly Hashtable _Table;
            private readonly Hashtable _Negation;

            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
            }

            public string Negation(string Key)
            {
                Key = Key.ToLower();
                return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
            }

            public ConstantPreposition()
            {
                _Table = new Hashtable();
                _Table.Add("of", "ের");
                _Table.Add("on", "ে");
                _Table.Add("into", "েরদিকে");
                _Table.Add("in.to", "েরদিকে");
                _Table.Add("with", "েরসঙ্গে");
                _Table.Add("than", "েরচেয়ে");
                _Table.Add("from", "থেকে");
                _Table.Add("towards", "ারপ্রতি");
                _Table.Add("at", "তে");
                _Table.Add("against", "েরবিরুদ্ধে");
                _Table.Add("about", "েরসম্বন্ধে");
                _Table.Add("like", "েরমত");
                _Table.Add("similar.to", "েরমত");
                _Table.Add("unlike", "েরমতনয়");
                _Table.Add("despite", "সত্ত্বেও");
                _Table.Add("in.spite.of", "সত্ত্বেও");
                _Table.Add("in", "তে");
                _Table.Add("as", "হিসেবে");
                _Table.Add("not.to", "তে");
                _Table.Add("to", "তে");
                _Table.Add("for", "েরজন্য");
                _Table.Add("by", "েরমধ্যে");
                _Table.Add("by_nn", "েরদ্বারা");
                _Table.Add("consist.of", "েরদ্বারাগঠিত");
                _Table.Add("between", "েরমধ্যে");
                _Table.Add("under", "েরঅধীনে");
                _Table.Add("below", "েরনিচে");
                _Table.Add("since", "থেকে");
                _Table.Add("while", "ারসময়");
                _Table.Add("whilst", "ারসময়");
                _Table.Add("in.case.of", "ারসময়");
                _Table.Add("before", "ারপূর্বে");
                _Table.Add("inside", "েরভেতরে");
                _Table.Add("outside", "েরবাইরে");
                _Table.Add("near", "েরকাছে");
                _Table.Add("during", "েরসময়");
                _Table.Add("amid", "েরমধ্যে");
                _Table.Add("within", "েরমধ্যে");
                _Table.Add("among", "েরমধ্যে");
                _Table.Add("amongst", "েরমধ্যে");
                _Table.Add("onto", "েরওপর");
                _Table.Add("on.to", "েরওপর");
                _Table.Add("on.top.of", "েরওপর");
                _Table.Add("over", "েরওপর");
                _Table.Add("upon", "েরওপর");
                _Table.Add("above", "েরওপর");
                _Table.Add("alongside", "েরপাশাপাশি");
                _Table.Add("around", "েরধারেকাছে");
                _Table.Add("through", "েরমধ্যদিয়ে");
                _Table.Add("beyond", "েরপরেও");
                _Table.Add("till", "পর্যন্ত");
                _Table.Add("until", "পর্যন্ত");
                _Table.Add("up.to", "পর্যন্ত");
                _Table.Add("after", "েরপরে");
                _Table.Add("behind", "েরপেছনে");
                _Table.Add("across", "জুড়ে");
                _Table.Add("via", "হয়ে");
                _Table.Add("throughout", "জুড়ে");
                _Table.Add("without", "ব্যতীত");
                _Table.Add("with.no", "ব্যতীত");
                _Table.Add("along", "বরাবর");
                _Table.Add("due.to", "েরদরুন");
                _Table.Add("according.to", "অনুসারে");
                _Table.Add("accord.to", "অনুসারে");
                _Table.Add("in.accordance.with", "অনুযায়ী");
                _Table.Add("besides", "ছাড়াও");
                _Table.Add("in.addition.to", "েরসাথে");
                _Table.Add("about.to", "তেচলেছে");
                _Table.Add("including", "সহ");
                _Table.Add("using ", "ব্যবহারকরে");
                _Table.Add("by.means.of", "ব্যবহারকরে");
                _Table.Add("by.mean.of", "ব্যবহারকরে");
                _Table.Add("because.of", "েরকারণে");
                _Table.Add("with.respect.to", "েরসাপেক্ষে");
                _Table.Add("in.front.of", "েরসামনে");
                _Table.Add("instead.of", "েরপরিবর্তে");
                _Table.Add("in.lieu.of", "েরপরিবর্তে");
                _Table.Add("in.place.of", "েরপরিবর্তে");
                _Table.Add("on.behalf.of", "েরপক্ষথেকে");
                _Table.Add("on.account.of", "েরপরিপ্রেক্ষিতে");
                _Table.Add("ahead.of", "েরআগে");
                _Table.Add("aside.from", "ছাড়া");

                _Table.Add("so", "সুতরাং");
                _Table.Add("to_from", "থেকে");
                _Table.Add("that", "যে");
                _Table.Add("if", "যদি");
                _Table.Add("then", "যখন");
                _Table.Add("although", "যদিও");
                _Table.Add("though", "যদিও");
                _Table.Add("whereas", "কিন্তুঅন্যদিকে");
                _Table.Add("albeit", "যদিও");
                _Table.Add("because", "কারণ");
                _Table.Add("per", "প্রতি");
                _Table.Add("unless", "যদিনা");
                _Table.Add("once", "একবার");
                _Table.Add("ago", "আগে");
                _Table.Add("such.as", "যেমন");
                _Table.Add("about_approx", "প্রায়");
                _Table.Add("as.well.as", "ও");
                _Table.Add("as.far.as", "যতদূর");
                _Table.Add("the", " ");

                _Table.Add("whether", "কিনা");

                _Negation = new Hashtable();
                _Negation.Add("not.to", "না");
            }
        }

        public class ConstantConjunction
        {
            private readonly Hashtable _Table;

            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : "";
            }

            public ConstantConjunction()
            {
                _Table = new Hashtable();
                _Table.Add("neither", "না");
                _Table.Add("nor", "না");
                _Table.Add("either", "হয়");
                _Table.Add("or", "অথবা");
                _Table.Add("and", "এবং");
            }
        }

        public class ConstantHaveHas
        {
            private readonly Hashtable _Table;
            private readonly Hashtable _Negation;

            /// <summary>
            /// Tense.Word
            /// </summary>
            /// <param name="Key"></param>
            /// <returns></returns>
            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
            }
            /// <summary>
            /// Tense(1,2,3).Word
            /// </summary>
            /// <param name="Key"></param>
            /// <returns></returns>
            public string Negation(string Key)
            {
                Key = Key.ToLower();
                return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
            }

            public ConstantHaveHas()
            {
                _Table = new Hashtable();
                _Table.Add("1.have", "রয়েছে");
                _Table.Add("2.have", "ছিল");
                _Table.Add("3.have", "থাকবে");
                _Table.Add("1.have.to", "হবে");
                _Table.Add("2.have.to", "হয়েছিল");
                _Table.Add("3.have.to", "হবে");

                _Negation = new Hashtable();
                _Negation.Add("1.have", "নেই");
                _Negation.Add("2.have", "ছিলনা");
                _Negation.Add("3.have", "থাকবেনা");
            }
        }

        public class ConstantAdverbNegation
        {
            private readonly Hashtable _Table;
            private readonly Hashtable _Person;

            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : "";
            }

            public string Person(string Key)
            {
                Key = Key.ToLower();
                return (_Person[Key] != null) ? _Person[Key].ToString() : Key;
            }

            public ConstantAdverbNegation()
            {
                _Table = new Hashtable();
                _Table.Add("no", "না");
                _Table.Add("not", "না");

                _Person = new Hashtable();
                _Person.Add("1", "নই");
                _Person.Add("2", "নন");
                _Person.Add("3", "নয়");
            }
        }

        public class ConstantWh
        {
            private readonly Hashtable _Question;
            private readonly Hashtable _Relation;

            public string Question(string Key)
            {
                Key = Key.ToLower();
                return (_Question[Key] != null) ? _Question[Key].ToString() : "";
            }

            public string Relation(string Key)
            {
                Key = Key.ToLower();
                return (_Relation[Key] != null) ? _Relation[Key].ToString() : "";
            }

            public ConstantWh()
            {
                _Question = new Hashtable();
                _Question.Add("what", "কি");
                _Question.Add("who", "কে");
                _Question.Add("whom", "কাকে");
                _Question.Add("whose", "কার");
                _Question.Add("when", "কখন");
                _Question.Add("which", "কোন");
                _Question.Add("why", "কেন");
                _Question.Add("where", "কোথায়");
                _Question.Add("to.whom", "কাকে");
                _Question.Add("that", "যে");
                _Question.Add("whatever", "সব কিছু");
                _Question.Add("how", "কিভাবে");
                _Question.Add("how.to", "হয় কিভাবে");
                _Question.Add("how.to.not", "না হয় কিভাবে");
                _Question.Add("how.do", "কিভাবে");
                _Question.Add("how.be", "কেমন");

                _Relation = new Hashtable();

                _Relation.Add("what", "যার");
                _Relation.Add("who", "যে");
                _Relation.Add("whom", "যাকে");
                _Relation.Add("whose", "যার");
                _Relation.Add("when", "যখন");
                _Relation.Add("which", "যেটি");
                _Relation.Add("why", "জন্য");
                _Relation.Add("where", "যেখানে");
                _Relation.Add("that", "যে");
                _Relation.Add("to.whom", "যাকে");
                _Relation.Add("whatever", "সব কিছু");
                _Relation.Add("how", "যেমন");
                _Relation.Add("how.much", "কত");
                _Relation.Add("how.long", "কতক্ষন");
                _Relation.Add("how.many", "কতগুলি");
            }
        }

        public class ConstantModalHaveHas
        {
            private readonly Hashtable _Table;
            private readonly Hashtable _Negation;

            /// <summary>
            /// Tense(1,2,3).word
            /// </summary>
            /// <param name="Key"></param>
            /// <returns></returns>
            public string Table(string Key)
            {
                Key = Key.ToLower();
                return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
            }
            /// <summary>
            /// Tense(1,2,3).word
            /// </summary>
            /// <param name="Key"></param>
            /// <returns></returns>
            public string Negation(string Key)
            {
                Key = Key.ToLower();
                return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
            }

            public ConstantModalHaveHas()
            {
                _Table = new Hashtable();
                //present
                _Table.Add("1.should", "থাকা উচিত");
                _Table.Add("1.can", "থাকতে পারে");
                _Table.Add("1.may", "থাকতে পারে");
                _Table.Add("1.could", "থাকতে পারে");

                //past
                _Table.Add("2.should", "থাকা উচিত ছিল");
                _Table.Add("2.can", "থাকতে পারে");
                _Table.Add("2.may", "থাকতে পারে");
                _Table.Add("2.could", "থাকতে পারে");

                //future
                _Table.Add("3.should", "থাকা উচিত");

                _Negation = new Hashtable();
                _Negation.Add("1.should", "থাকা উচিত নয়");
                _Negation.Add("1.may", "থাকতে নাও পারে");

                _Negation.Add("2.should", "থাকা উচিতহয়নি");
                _Negation.Add("2.may", "থাকতে নাও পারে");

                _Negation.Add("3.should", "থাকা উচিত নয়");
            }
        }

        public class ConstantVerb
        {
            public Mod VerbMod;
            public SuffixPassive VerbSuffixPassive;
            public Be BeVerb;
            public ConstantVerb()
            {
                VerbMod = new Mod();
                VerbSuffixPassive = new SuffixPassive();
                BeVerb = new Be();
            }
            public class Mod
            {
                private readonly Hashtable _Table;
                private readonly Hashtable _Negation;
                /// <summary>
                /// Person(1,2,3).Tense(1,2,3).Tensetype(1,2,3,4)
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Table(string Key)
                {
                    Key = Key.ToLower();
                    return (_Table[Key] != null) ? _Table[Key].ToString() : "";
                }
                /// <summary>
                /// Person(1,2,3).Tense(1,2,3).Tensetype(1,2,3,4)
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Negation(string Key)
                {
                    Key = Key.ToLower();
                    return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                }

                public Mod()
                {
                    _Table = new Hashtable();
                    //1stperson;present
                    _Table.Add("1.0.1", "ি");
                    _Table.Add("1.1.1", "ি");
                    _Table.Add("1.1.2", "ছি");
                    _Table.Add("1.1.3", "েছি");
                    _Table.Add("1.1.4", "ছি");

                    //1stperson;past
                    _Table.Add("1.2.1", "েছিলাম");
                    _Table.Add("1.2.2", "ছিলাম");
                    _Table.Add("1.2.3", "েছিলাম");
                    _Table.Add("1.2.4", "ছিলাম");

                    //1stperson;future
                    _Table.Add("1.3.1", "ব");
                    _Table.Add("1.3.2", "ব");
                    _Table.Add("1.3.3", "ব");
                    _Table.Add("1.3.4", "ব");

                    //2ndperson;present
                    _Table.Add("2.1.1", "েন");
                    _Table.Add("2.1.2", "ছেন");
                    _Table.Add("2.1.3", "েছেন");
                    _Table.Add("2.1.4", "ছেন");

                    //2ndperson;past
                    _Table.Add("2.2.1", "েছিলেন");
                    _Table.Add("2.2.2", "ছিলেন");
                    _Table.Add("2.2.3", "েছিলেন");
                    _Table.Add("2.2.4", "ছিলেন");

                    //2ndperson;future
                    _Table.Add("2.3.1", "বেন");
                    _Table.Add("2.3.2", "বেন");
                    _Table.Add("2.3.3", "বেন");
                    _Table.Add("2.3.4", "বেন");

                    //Arghya
                    _Table.Add("3.0.2", "া");
                    //3rdperson;present
                    _Table.Add("3.1.1", "ে");
                    _Table.Add("3.1.2", "ছে");
                    _Table.Add("3.1.3", "েছে");
                    _Table.Add("3.1.4", "ছে");

                    //3rdperson;past
                    _Table.Add("3.2.1", "েছিল");
                    _Table.Add("3.2.2", "ছিল");
                    _Table.Add("3.2.3", "েছিল");
                    _Table.Add("3.2.4", "ছিল");

                    //3rdperson;future
                    _Table.Add("3.3.1", "বে");
                    _Table.Add("3.3.2", "বে");
                    _Table.Add("3.3.3", "বে");
                    _Table.Add("3.3.4", "বে");



                    _Negation = new Hashtable();

                    //1stperson;present
                    _Negation.Add("1.1.3", "িনি");

                    //1stperson;past
                    _Negation.Add("1.2.1", "িনি");
                    _Negation.Add("1.2.3", "িনি");

                    //1stperson;future

                    //2ndperson;present
                    _Negation.Add("2.1.1", "েন না");
                    _Negation.Add("2.1.3", "েননি");

                    //2ndperson;past
                    _Negation.Add("2.2.1", "েননি");
                    _Negation.Add("2.2.3", "েননি");

                    //2ndperson;future

                    //3rdperson;present
                    _Negation.Add("3.1.3", "েনি");

                    //3rdperson;past
                    _Negation.Add("3.2.1", "েনি");
                    _Negation.Add("3.2.3", "েনি");
                }
            }
            public class SuffixPassive
            {
                private readonly Hashtable _Table;
                private readonly Hashtable _Negation;

                /// <summary>
                /// Tense(1,2,3).TenseType(1,2,3,4)
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Table(string Key)
                {
                    Key = Key.ToLower();
                    return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
                }
                /// <summary>
                /// Tense(1,2,3).TenseType(1,2,3,4)
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Negation(string Key)
                {
                    Key = Key.ToLower();
                    return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                }

                public SuffixPassive()
                {
                    _Table = new Hashtable();
                    //present
                    _Table.Add("1.1", "া হয়");
                    _Table.Add("1.2", "া হচ্ছে");
                    _Table.Add("1.3", "া হয়েছে");
                    _Table.Add("1.4", "া হচ্ছে");

                    //past
                    _Table.Add("2.1", "া হয়েছিল");
                    _Table.Add("2.2", "া হচ্ছিল");
                    _Table.Add("2.3", "া হয়েছিল");
                    _Table.Add("2.4", "া হচ্ছিল");

                    //future
                    _Table.Add("3.1", "া হবে");
                    _Table.Add("3.2", "া হবে");
                    _Table.Add("3.3", "া হবে");
                    _Table.Add("3.4", "া হবে");


                    _Negation = new Hashtable();
                    _Negation.Add("1.3", "া হয়নি");
                    _Negation.Add("2.1", "া হয়নি");
                    _Negation.Add("2.3", "া হয়নি");
                }
            }
            public class Be
            {
                public Main MainVerb;
                public Auxileary AuxilearyVerb;
                public ExtendedAuxileary ExtendedAuxilearyVerb;
                public Be()
                {
                    MainVerb = new Main();
                    AuxilearyVerb = new Auxileary();
                    ExtendedAuxilearyVerb = new ExtendedAuxileary();
                }
                public class Main
                {
                    private readonly Hashtable _Table;
                    private readonly Hashtable _Negation;

                    /// <summary>
                    /// Person(1,2,3).Tense(1,2,3)
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Table(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
                    }
                    /// <summary>
                    /// Person(1,2,3).Tense(1,2,3)
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Negation(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Negation[Key] != null) ? _Negation[Key].ToString() : Key;
                    }

                    public Main()
                    {
                        _Table = new Hashtable();
                        //Present
                        _Table.Add("1.1", " ");
                        _Table.Add("2.1", " ");
                        _Table.Add("3.1", " ");

                        //Present
                        _Table.Add("1.2", "ছিলাম");
                        _Table.Add("2.2", "ছিলেন");
                        _Table.Add("3.2", "ছিল");

                        //Future
                        _Table.Add("1.3", "হব");
                        _Table.Add("2.3", "হবেন");
                        _Table.Add("3.3", "হবে");

                        _Negation = new Hashtable();
                    }
                }

                public class Auxileary
                {
                    private readonly Hashtable _Table;
                    private readonly Hashtable _Negation;

                    /// <summary>
                    /// Person(1,2,3).Word
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Table(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Table[Key] != null) ? _Table[Key].ToString() : "";
                    }
                    /// <summary>
                    /// Person(1,2,3).Word
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Negation(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                    }

                    public Auxileary()
                    {
                        _Table = new Hashtable();
                        //1stperson
                        _Table.Add("1.am", " ");
                        _Table.Add("1.is", " ");
                        _Table.Add("1.are", "আছি");
                        _Table.Add("1.was", "ছিলাম");
                        _Table.Add("1.were", "ছিলাম");
                        _Table.Add("1.will", "থাকব");
                        _Table.Add("1.shall", "থাকব");

                        //2ndperson
                        _Table.Add("2.are", " ");
                        _Table.Add("2.is", " ");
                        _Table.Add("2.were", "ছিলেন");
                        _Table.Add("2.will", "থাকবেন");
                        _Table.Add("2.shall", "থাকবেন");

                        //3rdperson
                        _Table.Add("3.is", " ");
                        _Table.Add("3.are", "আছে");
                        _Table.Add("3.was", "ছিল");
                        _Table.Add("3.were", "ছিল");
                        _Table.Add("3.will", "থাকবে");
                        _Table.Add("3.shall", "থাকবে");


                        _Negation = new Hashtable();

                        //1stPerson
                        _Negation.Add("1.am", "নই");
                        _Negation.Add("1.are", "নই");
                        _Negation.Add("1.was", "ছিলামনা");
                        _Negation.Add("1.were", "ছিলামনা");
                        _Negation.Add("1.will", "থাকবনা");
                        _Negation.Add("1.shall", "থাকবনা");

                        //2ndPerson
                        _Negation.Add("2.are", "নন");
                        _Negation.Add("2.were", "ছিলেননা");
                        _Negation.Add("2.will", "থাকবেননা");
                        _Negation.Add("2.shall", "থাকবেননা");

                        //3rdPerson
                        _Negation.Add("3.is", "নয়");
                        _Negation.Add("3.are", "নয়");
                        _Negation.Add("3.was", "ছিলনা");
                        _Negation.Add("3.were", "ছিলনা");
                        _Negation.Add("3.will", "থাকবেনা");
                        _Negation.Add("3.shall", "থাকবেনা");
                    }
                }

                public class ExtendedAuxileary
                {
                    private readonly Hashtable _Table;
                    private readonly Hashtable _Negation;

                    /// <summary>
                    /// Tense(1,2,3).Word
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Table(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Table[Key] != null) ? _Table[Key].ToString() : "";
                    }
                    /// <summary>
                    /// Tense(1,2,3).Word
                    /// </summary>
                    /// <param name="Key"></param>
                    /// <returns></returns>
                    public string Negation(string Key)
                    {
                        Key = Key.ToLower();
                        return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                    }

                    public ExtendedAuxileary()
                    {
                        _Table = new Hashtable();
                        _Table.Add("1.is", "আছে");
                        _Table.Add("1.are", "আছে");

                        _Negation = new Hashtable();
                        _Negation.Add("1.is", "নেই");
                        _Negation.Add("1.are", "নেই");

                    }
                }
            }
        }

        public class ConstantModal
        {
            public ModalActive Active;
            public ModalPassive Passive;

            public ConstantModal()
            {
                Active = new ModalActive();
                Passive = new ModalPassive();
            }

            public class ModalActive
            {
                private readonly Hashtable _Table;
                private readonly Hashtable _Negation;

                /// <summary>
                /// TenseType(0,1,2,3,4).Word
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Table(string Key)
                {
                    Key = Key.ToLower();
                    return (_Table[Key] != null) ? _Table[Key].ToString() : "";
                }
                /// <summary>
                /// TenseType(0,1,2,3,4).Word
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Negation(string Key)
                {
                    return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                }
                public ModalActive()
                {
                    _Table = new Hashtable();
                    _Table.Add("0.can", "তে পার");
                    _Table.Add("0.may", "তে পার");
                    _Table.Add("0.could", "তে পার");
                    _Table.Add("0.might", "তে পার");

                    //present
                    _Table.Add("1.should", "া উচিত");
                    _Table.Add("2.should", "া উচিত");
                    _Table.Add("3.should", "া উচিত ছিল");
                    _Table.Add("4.should", "া উচিত ছিল");



                    //present
                    _Table.Add("1.ought", "া উচিত");
                    _Table.Add("2.ought", "া উচিত");
                    _Table.Add("3.ought", "া উচিত ছিল");
                    _Table.Add("4.ought", "া উচিত ছিল");
                    _Table.Add("1.must", "া উচিত");
                    _Table.Add("2.must", "া উচিত");
                    _Table.Add("3.must", "া উচিত ছিল");
                    _Table.Add("4.must", "া উচিত ছিল");


                    //Negation
                    _Negation = new Hashtable();
                    _Negation.Add("0.may", "তে নাও পার");
                    _Negation.Add("0.might", "তে নাও পার");

                    //presentinnegation
                    _Negation.Add("1.should", "া উচিত নয়");
                    _Negation.Add("2.should", "া উচিত নয়");
                    _Negation.Add("3.should", "া উচিত হয়নি");
                    _Negation.Add("4.should", "া উচিত হয়নি");

                    //presentinnegation
                    _Negation.Add("1.ought", "া উচিত নয়");
                    _Negation.Add("2.ought", "া উচিত নয়");
                    _Negation.Add("3.ought", "া উচিত হয়নি");
                    _Negation.Add("4.ought", "া উচিত হয়নি");

                    //presentinnegation
                    _Negation.Add("1.must", "া উচিত নয়");
                    _Negation.Add("2.must", "া উচিত নয়");
                    _Negation.Add("3.must", "া উচিত হয়নি");
                    _Negation.Add("4.must", "া উচিত হয়নি");
                }
            }

            public class ModalPassive
            {
                private readonly Hashtable _Table;
                private readonly Hashtable _Negation;

                /// <summary>
                /// TenseType(1,2,3,4).Word
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Table(string Key)
                {
                    Key = Key.ToLower();
                    return (_Table[Key] != null) ? _Table[Key].ToString() : Key;
                }
                /// <summary>
                /// TenseType(1,2,3,4).Word
                /// </summary>
                /// <param name="Key"></param>
                /// <returns></returns>
                public string Negation(string Key)
                {
                    Key = Key.ToLower();
                    return (_Negation[Key] != null) ? _Negation[Key].ToString() : "";
                }
                public ModalPassive()
                {
                    _Table = new Hashtable();
                    //present
                    _Table.Add("1.can", "া যাবে");
                    _Table.Add("2.can", "া যাবে");
                    _Table.Add("3.can", "া যেত");
                    _Table.Add("4.can", "া যেত");

                    //present
                    _Table.Add("1.may", "া হয়ত যাবে");
                    _Table.Add("2.may", "া হয়ত যাবে");
                    _Table.Add("3.may", "া যেত");
                    _Table.Add("4.may", "া যেত");

                    //present
                    _Table.Add("1.could", "া যাবে");
                    _Table.Add("2.could", "া যাবে");
                    _Table.Add("3.could", "া যেত");
                    _Table.Add("4.could", "া যেত");

                    //present
                    _Table.Add("1.might", "া যাবে");
                    _Table.Add("2.might", "া যাবে");
                    _Table.Add("3.might", "া যেত");
                    _Table.Add("4.might", "া যেত");

                    //present
                    _Table.Add("1.should", "া উচিত");
                    _Table.Add("2.should", "া উচিত");
                    _Table.Add("3.should", "া উচিত ছিল");
                    _Table.Add("4.should", "া উচিত ছিল");

                    //present
                    _Table.Add("1.ought", "া উচিত");
                    _Table.Add("2.ought", "া উচিত");
                    _Table.Add("3.ought", "া উচিত ছিল");
                    _Table.Add("4.ought", "া উচিত ছিল");

                    //present
                    _Table.Add("1.must", "া উচিত");
                    _Table.Add("2.must", "া উচিত");
                    _Table.Add("3.must", "া উচিত ছিল");
                    _Table.Add("4.must", "া উচিত ছিল");

                    _Negation = new Hashtable();
                    //presentinnegation
                    _Negation.Add("1.should", "া উচিত নয়");
                    _Negation.Add("2.should", "া উচিত নয়");
                    _Negation.Add("3.should", "া উচিত হয়নি");
                    _Negation.Add("4.should", "াউচিত হয়নি");
                    //presentinnegation
                    _Negation.Add("1.ought", "া উচিত নয়");
                    _Negation.Add("2.ought", "া উচিত নয়");
                    _Negation.Add("3.ought", "া উচিত হয়নি");
                    _Negation.Add("4.ought", "া উচিত হয়নি");
                    //presentinnegation
                    _Negation.Add("1.must", "া উচিত নয়");
                    _Negation.Add("2.must", "া উচিত নয়");
                    _Negation.Add("3.must", "া উচিত হয়নি");
                    _Negation.Add("4.must", "া উচিত হয়নি");
                }
            }

        }

        public class ConstantMatra : Hashtable
        {
            public BengaliMatra Table(char Key)
            {
                return (this[Key] != null) ? (BengaliMatra)this[Key] : BengaliMatra.None;
            }

            public ConstantMatra()
            {
                this.Add('অ', BengaliMatra.Vowel);
                this.Add('আ', BengaliMatra.Vowel);
                this.Add('ই', BengaliMatra.Vowel);
                this.Add('ঈ', BengaliMatra.Vowel);
                this.Add('উ', BengaliMatra.Vowel);
                this.Add('ঊ', BengaliMatra.Vowel);

                this.Add('ঋ', BengaliMatra.Vowel);
                this.Add('এ', BengaliMatra.Vowel);
                this.Add('ঐ', BengaliMatra.Vowel);
                this.Add('ও', BengaliMatra.Vowel);
                this.Add('ঔ', BengaliMatra.Vowel);

                this.Add('ক', BengaliMatra.Consonant);
                this.Add('খ', BengaliMatra.Consonant);
                this.Add('গ', BengaliMatra.Consonant);
                this.Add('ঘ', BengaliMatra.Consonant);
                this.Add('ঙ', BengaliMatra.Consonant);

                this.Add('চ', BengaliMatra.Consonant);
                this.Add('ছ', BengaliMatra.Consonant);
                this.Add('জ', BengaliMatra.Consonant);
                this.Add('ঝ', BengaliMatra.Consonant);
                this.Add('ঞ', BengaliMatra.Consonant);

                this.Add('ট', BengaliMatra.Consonant);
                this.Add('ঠ', BengaliMatra.Consonant);
                this.Add('ড', BengaliMatra.Consonant);
                this.Add('ঢ', BengaliMatra.Consonant);
                this.Add('ণ', BengaliMatra.Consonant);

                this.Add('ত', BengaliMatra.Consonant);
                this.Add('থ', BengaliMatra.Consonant);
                this.Add('দ', BengaliMatra.Consonant);
                this.Add('ধ', BengaliMatra.Consonant);
                this.Add('ন', BengaliMatra.Consonant);

                this.Add('প', BengaliMatra.Consonant);
                this.Add('ফ', BengaliMatra.Consonant);
                this.Add('ব', BengaliMatra.Consonant);
                this.Add('ভ', BengaliMatra.Consonant);
                this.Add('ম', BengaliMatra.Consonant);


                this.Add('য', BengaliMatra.Consonant);
                this.Add('র', BengaliMatra.Consonant);
                this.Add('ল', BengaliMatra.Consonant);
                this.Add('শ', BengaliMatra.Consonant);
                this.Add('ষ', BengaliMatra.Consonant);
                this.Add('স', BengaliMatra.Consonant);
                this.Add('হ', BengaliMatra.Consonant);
                this.Add('ড়', BengaliMatra.Consonant);
                this.Add('ঢ়', BengaliMatra.Consonant);
                this.Add('য়', BengaliMatra.Consonant);

                this.Add('া', BengaliMatra.Matra);
                this.Add('ি', BengaliMatra.Matra);
                this.Add('ী', BengaliMatra.Matra);
                this.Add('ু', BengaliMatra.Matra);
                this.Add('ূ', BengaliMatra.Matra);
                this.Add('ৃ', BengaliMatra.Matra);
                this.Add('ে', BengaliMatra.Matra);
                this.Add('ৈ', BengaliMatra.Matra);
                this.Add('ো', BengaliMatra.Matra);
                this.Add('ৌ', BengaliMatra.Matra);

                this.Add('ঁ', BengaliMatra.Hasant);
                this.Add('ং', BengaliMatra.Hasant);
                this.Add(':', BengaliMatra.Hasant);
                this.Add('্', BengaliMatra.Hasant);


            }
        }

        public class ConstantNumbers : Hashtable
        {
            public char Table(char Key)
            {
                return (this[Key] != null) ? (char)this[Key] : Key;
            }

            public ConstantNumbers()
            {
                this.Add('0', '০');
                this.Add('1', '১');
                this.Add('2', '২');
                this.Add('3', '৩');
                this.Add('4', '৪');
                this.Add('5', '৫');
                this.Add('6', '৬');
                this.Add('7', '৭');
                this.Add('8', '৮');
                this.Add('9', '৯');
            }
        }

        public class ConstantPunctuation : Hashtable
        {
            public char Table(char Key)
            {
                return (this[Key] != null) ? (char)this[Key] : Key;
            }

            public ConstantPunctuation()
            {
                this.Add('.', '।');
                this.Add('?', '?');
                this.Add('!', '!');
            }
        }

        public class MapWord : Hashtable
        {
            public string Table(string Key)
            {
                return (this[Key] != null) ? this[Key].ToString() : Key;
            }

            public MapWord()
            {
                this.Add("ি", "ই");
                this.Add("ে", "য়");
                this.Add("েন","ন");
            }
        }

        private static void ConstructLexicon()
        {
            Lexicon = new Hashtable();
            Lemma = new Hashtable();
            TheRules = new StringCollection();
            TheContext = new StringCollection();


            string _LexiconFile = "Lexicon";
            string _LexicalRuleFile = "LexiconRule";
            string _ContextualRuleFile = "ContextualRule";

            string lx;
            string[] lv;
            string[] tlist;
            int i = 0;
            int j;
            //StreamReader SR = new StreamReader((Application.StartupPath + "//Lexiconlong.txt"));
            StreamReader SR = new StreamReader(String.Format("{0}/{1}", GlobalVariable.DictionaryLocation, _LexiconFile));
            // I usually read in long files with SR.ReadToEnd then do a Split on VBNewLine
            // But in this case it is MUCH slower than doing it via ReadLine
            // And reading and hashing this way is MUCH faster than saving the serialized hash table
            // especially for a very long lexicon. Serializing a big hash table is VERY, VERY slow
            string s = SR.ReadLine();
            // TDMS 20 Oct 2005 - added check for empty string
            while (!(s == null) && !((string)s == ""))
            {
                string[] _WordsWithTagLemma = s.Split(' ');
                string _Word = _WordsWithTagLemma[0];
                string _Lemma = _WordsWithTagLemma[1];

                j = s.IndexOf(_Lemma);

                string[] _Tags = new string[_WordsWithTagLemma.Length - 2];

                for (int c = 2; c < _WordsWithTagLemma.Length; c++)
                {
                    _Tags[c - 2] = _WordsWithTagLemma[c];
                }

                //tlist = s.Substring((j + 1)).Split(' ');
                if (Lexicon[_Word] == null)
                    Lexicon.Add(_Word, _Tags);
                if (Lemma[_Word] == null)
                    Lemma.Add(_Word, _Lemma);

                s = SR.ReadLine();
            }
            SR.Close();
            //SR = new StreamReader((Application.StartupPath + "//LexicalRuleFile.txt"));
            SR = new StreamReader(String.Format("{0}/{1}", GlobalVariable.DictionaryLocation, _LexicalRuleFile));
            lx = SR.ReadToEnd();
            SR.Close();
            lv = lx.Split('\n');

            for (i = 0; i <= (lv.Length - 1); i++)
            {
                // TheRules[i] = lv[i];
                // TDMS 20 Oct 2005 - added check for empty string
                if ((string)lv[i] != "")
                    TheRules.Add((string)lv[i].Replace("\r", ""));
            }
            //SR = new StreamReader((Application.StartupPath + "//ContextualRuleFile.txt"));
            SR = new StreamReader(String.Format("{0}/{1}", GlobalVariable.DictionaryLocation, _ContextualRuleFile));
            lx = SR.ReadToEnd();
            SR.Close();
            lv = lx.Split('\n');
            //object TheContext;
            for (i = 0; (i <= (lv.Length - 1)); i++)
            {
                // TDMS 20 Oct 2005 - added check for empty string
                if ((string)lv[i] != "")
                    //TDMS 17 Nov 2005 - fixed the rules matching by removing \r from each entry in the array - the last entry contained \r which breaks contextual comparisons
                    TheContext.Add(lv[i].Replace("\r", ""));
            }
        }
    }

    public static class GlobalVariable
    {
        public static string DictionaryLocation { get { return String.Format("{0}Dict", _ResourceLocation); } }


        private static string _ResourceLocation = String.Format("{0}//Data//", Environment.CurrentDirectory);
        public static string ResourceLocation
        {
            get
            {
                return _ResourceLocation;
            }
            set
            {
                _ResourceLocation = value;
            }
        }
    }

    public class Interrogative
    {
        private bool _ISQuestionMark = false;
        public bool ISQuestionMark { get { return _ISQuestionMark; } set { _ISQuestionMark = value; } }
        private bool _ISWhQuestionMark = false;
        public bool ISWhQuestionMark { get { return _ISWhQuestionMark; } set { _ISWhQuestionMark = value; } }
    }

    public class TranslationFormatText
    {
        public string Expression;
        public string Replace;
    }



    public enum BengaliMatra
    {
        None,
        Vowel,
        Consonant,
        Matra,
        Hasant
    }

    public class TagsType
    {
        public const string CC = "CC"; // Coordinating conjunction 
        public const string CD = "CD"; // Cardinal number
        public const string DT = "DT"; // Determiner 
        public const string EX = "EX"; // Existential there 
        public const string FW = "FW"; // Foreign word 
        public const string IN = "IN"; // Preposition/subord. conjunction
        public const string JJ = "JJ"; // Adjective 
        public const string JJR = "JJR"; // Adjective, comparative 
        public const string JJS = "JJS"; // Adjective, superlative
        public const string LS = "LS"; // List item marker 
        public const string MD = "MD"; // Modal 
        public const string NN = "NN"; // Noun, singular or mass
        public const string NNS = "NNS"; // Noun, plural 
        public const string NNP = "NNP"; // Proper noun, singular 
        public const string NNPS = "NNPS"; // Proper noun, plural 
        public const string PDT = "PDT"; // Predeterminer 
        public const string POS = "POS"; // Possessive ending 
        public const string PRP = "PRP"; // Personal pronoun 
        public const string PRPP = "PRP$"; // Possessive pronoun 
        public const string RB = "RB"; // Adverb 
        public const string RBR = "RBR"; // Adverb, comparative 
        public const string RBS = "RBS"; // Adverb, superlative 
        public const string RP = "RP"; // Particle
        public const string SYM = "SYM"; // Symbol (mathematical or scientific) 
        public const string TO = "TO"; // to 
        public const string UH = "UH"; // Interjection
        public const string VB = "VB"; // Verb, base form 
        public const string VBD = "VBD"; // Verb, past tense 
        public const string VBG = "VBG"; // Verb, gerund/present participle
        public const string VBN = "VBN"; // Verb, past participle 
        public const string VBP = "VBP"; // Verb, non-3rd ps. sing. present 
        public const string VBZ = "VBZ"; // Verb, 3rd ps. sing. present 
        public const string WDT = "WDT"; // wh-determiner 
        public const string WP = "WP"; // wh-pronoun 
        public const string WPP = "WP$"; // Possessive wh-pronoun 
        public const string WRB = "WRB"; //  wh-adverb 
        public const string Pound = "#";
        public const string Dollar = "$";
        public const string Punctuation = ".";
        public const string Comma = ",";
        public const string SemiColon = ";";
        public const string Colon = ":";
        public const string LeftBracket = "(";
        public const string RightBracket = ")";
        public const string DoubleQuote = "\"";
        public const string LeftOpenSingleQuote = "`";
        public const string RightCloseSingleQuote = "'";

        public const string VV = "VV";
        public const string VVZ = "VVZ";
        public const string VVD = "VVD";
        public const string VVG = "VVG";
        public const string VVP = "VVP";
        public const string VVN = "VVN";
        public const string VHZ = "VHZ";
        public const string VHP = "VHP";
        public const string VHD = "VHD";
        public const string VHN = "VHN";
        public const string VHG = "VHG";
        public const string VH = "VH";
        public const string IGNR = "IGNR";
        public const string PP = "PP";
        public const string PPP = "PP$";
        public const string NP = "NP";
        public const string NPS = "NPS";


        public const string S = "S";// simple declarative clause, i.e. one that is not introduced by a (possible empty) subordinating conjunction or a wh-word and that does not exhibit subject-verb inversion.
        public const string SBAR = "SBAR";//Clause introduced by a (possibly empty) subordinating conjunction.
        public const string SBARQ = "SBARQ";// Direct question introduced by a wh-word or a wh-phrase. Indirect questions and relative clauses should be bracketed as SBAR, not SBARQ.
        public const string SINV = "SINV";// Inverted declarative sentence, i.e. one in which the subject follows the tensed verb or modal.
        public const string SQ = "SQ";// Inverted yes/no question, or main clause of a wh-question, following the wh-phrase in SBARQ.
        public const string FRAG = "FRAG";
        public const string TOP = "TOP";

    }

    public enum BengaliLetter
    {
        chandrabindu = 'ঁ',
        anusvar = 'ং',
        bisarga = ':',
        A = 'অ',
        AA = 'আ',
        I = 'ই',
        II = 'ঈ',
        U = 'উ',
        UU = 'ঊ',
        RI = 'ঋ',
        E = 'এ',
        AI = 'ঐ',
        O = 'ও',
        AU = 'ঔ',
        KA = 'ক',
        KHA = 'খ',
        GA = 'গ',
        GHA = 'ঘ',
        NGA = 'ঙ',
        CA = 'চ',
        CHA = 'ছ',
        JA = 'জ',
        JHA = 'ঝ',
        NYA = 'ঞ',
        TTA = 'ট',
        TTHA = 'ঠ',
        DDA = 'ড',
        DDHA = 'ঢ',
        NNA = 'ণ',
        TA = 'ত',
        THA = 'থ',
        DA = 'দ',
        DHA = 'ধ',
        NA = 'ন',
        PA = 'প',
        PHA = 'ফ',
        BA = 'ব',
        BHA = 'ভ',
        MA = 'ম',
        YA = 'য়',
        RA = 'র',
        LA = 'ল',
        SHA = 'শ',
        SSA = 'ষ',
        SA = 'স',
        HA = 'হ',
        RRA = 'ড়',
        RHA = 'ঢ়',
        YYA = 'য়',
        akar = 'া',
        ikar = 'ি',
        iikar = 'ী',
        ukar = 'ু',
        uukar = 'ূ',
        rikar = 'ৃ',
        ekar = 'ে',
        oikar = 'ৈ',
        okar = 'ো',
        aukar = 'ৌ',
        hasant = '্'
    }

    public enum TypesByStructure
    {
        Simple = 1,
        Compound,
        Complex,
        ComplexCompound
    }

    public enum FirstWord
    {
        Other,
        Do,
        Be,
        You,
        Have,
        WhWord,
        How,
        ModalWord
    }

    public enum TypesByPurpose
    {
        Declarative = 0,
        Interrogative,
        Exclamatory,
        Imperative,
        Conditional
    }

    public enum Tense
    {
        None = 0,
        Present,
        Past,
        Future,
        Conditional
    }

    public enum TenseType
    {
        Simple = 1,
        Progressive,
        Perfect,
        PerfectProgressive,
    }

    public enum Voice
    {
        Active = 0,
        Passive
    }

    public enum ModalType
    {
        None = 0,
        Should,
        Ought,
        Can,
        May
    }

    public enum Person
    {
        First = 1,
        Second,
        Third
    }

    public enum SentenceNumber
    {
        Singuler = 1,
        Plural
    }

    public enum MainVerb
    {
        Other = 0,
        Be,
        Do,
        Have,
        Modal
    }

    public enum Formality
    {
        Apni = 1,
        Tumi,
        Tui
    }

    public enum SVOO
    {
        NotDetected,
        Subject,
        Verb,
        Object,
        Others
    }

    public class BengaliWord
    {
        public const string te = "তে";
        public const string ache = "আছে";
        public const string chilo = "ছিল";
        public const string thakbe = "থাকবে";
        public const string na = "না";
        public const string nei = "নেই";
        public const string ni = "নি";
        public const string ti = "টি";
        public const string gulo = "গুলি";
        public const string er = "ের"; // *
        public const string ki = " কি"; // *
        public const string a_jai_ni = "া যায় নি";
        public const string object_suffix = "কে";
        public const string subject_hhh_suffix = "ের"; // *
    }

    public enum TokenProperty
    {
        ConfusionPurpose = 1, 
        IsInvertedSentence = 2,
        IsWhWordPresent = 4,
        IsBePresent = 8,
        IsHavePresent = 16,
        IsPastParticipleOfBe = 32,
        IsProgressiveOfBe = 64,
        IsPastParticipleOfVerb = 128,
        IsProgressiveOfHave = 256,
        IsVerbDetected = 512,
        IsObject = 1024,
        IsNegetive = 2148,
        IsExistentialThere = 4096,
        IsVoiceDetected = 8192,
        IsPersonDetected = 16384,
        IsNegetiveApplied = 32768,
        IsBengaliKiAdded = 65536,
        IsBeginDetected = 131072
    }
}
