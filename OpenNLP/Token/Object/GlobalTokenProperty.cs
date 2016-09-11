using System;
using System.Collections.Generic;
using System.Text;

namespace NLPToken
{
    public class GlobalTokenProperty
    {
        
        public bool ConfusionPurpose;
        public bool IsInvertedSentence;
        public bool IsWhWordPresent;
        public bool IsBePresent;
        public bool IsHavePresent;
        public bool IsPastParticipleOfBe;
        public bool IsProgressiveOfBe;
        public bool IsPastParticipleOfVerb;
        public bool IsProgressiveOfHave;
        public bool IsVerbDetected;
        public bool IsObject;
        public bool IsNegetive;
        public bool IsExistentialThere;
        public bool IsVoiceDetected;
        public bool IsPersonDetected = false;
        public bool IsNegetiveApplied = false;
        public bool IsBengaliKiAdded = false;
        public bool IsBeginDetected = false;

        public int TokenPropertys = 0;

        public Tense Tense;
        public TenseType TenseType;
        public Voice Voice;
        public TypesByPurpose Purpose;
        public FirstWord FirstWord;
        public Person Person = Person.Third;
        public SentenceNumber Number = SentenceNumber.Singuler;
        public MainVerb MainVerb = MainVerb.Other;
        public ModalType ModalType = ModalType.None;

        public string ModalWord;
        public string BengaliNegationWord;
        public string BengaliPreDeterminer;
        public string BengaliDeterminerSuffix = String.Empty;
        public int TempWordPosition = 0;
        public string BengaliAdverb;
        
        //public List<string> WordUsed = new List<string>();

        public string Text
        {
            get
            {
				/*
                return String.Format("Purpose=\"{0}\" ConfusionPurpose=\"{1}\" InvertedSentence=\"{2}\" <br />WhWordPresent=\"{3}\" " +
                    "FirstWord=\"{4}\" BePresent=\"{5}\" HavePresent=\"{6}\" PastParticipleOfBe=\"{7}\" ProgressiveOfBe=\"{8}\" <br />" +
                    "PastParticipleOfVerb=\"{9}\" ProgressiveOfHave=\"{10}\" Tense=\"{11}\" TenseType=\"{12}\" Voice=\"{13}\" <br />" +
                    "Person=\"{14}\" Number=\"{15}\" VerbDetected=\"{16}\" Negetive=\"{17}\" ExistentialThere=\"{18}\" MainVerb=\"{19}\"",
                    Purpose,
                    ConfusionPurpose,
                    IsInvertedSentence,
                    IsWhWordPresent,
                    FirstWord,
                    IsBePresent,
                    IsHavePresent,
                    IsPastParticipleOfBe,
                    IsProgressiveOfBe,
                    IsPastParticipleOfVerb,
                    IsProgressiveOfHave,
                    Tense,
                    TenseType,
                    Voice,
                    Person,
                    Number,
                    IsVerbDetected,
                    IsNegetive,
                    IsExistentialThere,
                    MainVerb
                    );
				*/
				return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}", Purpose,
					ConfusionPurpose,
					IsInvertedSentence,
					IsWhWordPresent,
					FirstWord,
					IsBePresent,
					IsHavePresent,
					IsPastParticipleOfBe,
					IsProgressiveOfBe,
					IsPastParticipleOfVerb,
					IsProgressiveOfHave,
					Tense,
					TenseType,
					Voice,
					Person,
					Number,
					IsVerbDetected,
					IsNegetive,
					IsExistentialThere,
									 MainVerb);
									
				//return Purpose.ToString();
            }
            set
            {
            }
        }

        public void DetectSentenceType(Token Token)
        {
            if (Token.WordPosition == 1)
            {
                // If First word is a Wh word or Be/Have verb The sentence is an Interrogative (?) word.
                switch (Token.Type)
                {
                    case TagsType.WDT:
                    case TagsType.WRB:
                    case TagsType.WP:
                    case TagsType.WPP:
                        if (Token.Lemma.StartsWith("how"))
                        {
                            this.FirstWord = FirstWord.How;
                        }
                        else
                        {
                            this.FirstWord = FirstWord.WhWord;
                        }
                        this.IsWhWordPresent = true;
                        this.Purpose = TypesByPurpose.Interrogative;
                        break;
                    case TagsType.MD:
                        this.FirstWord = FirstWord.ModalWord;
                        this.Purpose = TypesByPurpose.Interrogative;
                        break;
                    case TagsType.VB:
                    case TagsType.VBP:
                    case TagsType.VBZ:
                    case TagsType.VBG:
                    case TagsType.VBD:
                    case TagsType.VBN:
                        this.FirstWord = FirstWord.Be;
                        this.Purpose = TypesByPurpose.Interrogative;
                        this.IsVerbDetected = true;
                        break;
                    case TagsType.VH:
                    case TagsType.VHP:
                    case TagsType.VHZ:
                    case TagsType.VHG:
                    case TagsType.VHD:
                    case TagsType.VHN:
                        this.FirstWord = FirstWord.Have;
                        this.Purpose = TypesByPurpose.Interrogative;
                        this.IsVerbDetected = true;
                        break;
                    case TagsType.UH:
                        this.Purpose = TypesByPurpose.Interrogative;
                        break;
                    case TagsType.VV: // If forst word is a verb other than Be/Have the word is an Imperative word.
                    case TagsType.VVP:
                        this.Purpose = TypesByPurpose.Imperative;
                        this.IsVerbDetected = true;
                        break;
                }
                if (Token.Lemma.Equals("do", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Purpose = TypesByPurpose.Interrogative;
                    this.FirstWord = FirstWord.Do;
                }
                else if (Token.English.Equals("please", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Purpose = TypesByPurpose.Imperative;
                }
                else if (Token.English.Equals("you", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.FirstWord = FirstWord.You;
                }
                return;
            }
            else if (Token.WordPosition == 2)
            {
                if (this.FirstWord == FirstWord.Do)
                {
                    if (!Token.Lemma.Equals("it", StringComparison.CurrentCultureIgnoreCase)
                        && !Token.Lemma.Equals("this", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.Purpose = TypesByPurpose.Interrogative;
                    }
                }
                else if (this.FirstWord == FirstWord.You)
                {
                    switch (Token.Type)
                    {
                        case TagsType.VV:
                        case TagsType.VVP:
                            this.Purpose = TypesByPurpose.Imperative;
                            this.IsVerbDetected = true;
                            break;
                    }
                }
            }
            else
            {
                return;
            }
        }

        internal void DetectSentenceType(Token Token, bool IsPhaseTwo)
        {
            if (!IsPhaseTwo)
                DetectSentenceType(Token);

            switch (Token.Type)
            {
                case TagsType.S:
                    if (this.Purpose == TypesByPurpose.Interrogative)
                    {
                        this.ConfusionPurpose = true;
                    }
                    break;
                case TagsType.SBAR:
                    // Not sure
                    break;
                case TagsType.SBARQ:
                    if (this.Purpose != TypesByPurpose.Interrogative)
                    {
                        this.ConfusionPurpose = true;
                    }
                    this.Purpose = TypesByPurpose.Interrogative;
                    break;
                case TagsType.SINV:
                    if (this.Purpose == TypesByPurpose.Interrogative)
                    {
                        this.ConfusionPurpose = true;
                    }
                    this.IsInvertedSentence = true;
                    break;
                case TagsType.SQ:
                    if (this.Purpose != TypesByPurpose.Interrogative)
                    {
                        this.ConfusionPurpose = true;
                    }
                    this.Purpose = TypesByPurpose.Interrogative;
                    this.IsInvertedSentence = true;
                    break;
            }
        }

        public void DetectTenseVoiceTenseType(Token Token)
        {
            switch (Token.Type)
            {
                case TagsType.RB:
                case TagsType.RBR:
                case TagsType.RBS:
                    this.BengaliNegationWord = Constant.AdverbNegation.Table(Token.Lemma);
                    if (this.BengaliNegationWord.Length > 0)
                        this.IsNegetive = true;
                    break;
                case TagsType.MD:
                    if (this.Tense == Tense.None)
                        if (Token.Lemma.Equals("shall", StringComparison.CurrentCultureIgnoreCase)
                            || Token.Lemma.Equals("will", StringComparison.CurrentCultureIgnoreCase)
                            || Token.Lemma.Equals("would", StringComparison.CurrentCultureIgnoreCase)
                            || Token.Lemma.Equals("must", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.Tense = Tense.Future;
                        }
                    break;

                case TagsType.VB:
                case TagsType.VBP:
                case TagsType.VBZ:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Present;
                    }
                    this.IsBePresent = true;
                    this.TenseType = TenseType.Simple;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VBD:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Past;
                    }
                    this.IsBePresent = true;
                    this.TenseType = TenseType.Simple;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VBG:
                    this.IsBePresent = true;
                    this.IsProgressiveOfBe = true;
                    this.TenseType = TenseType.Progressive;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VBN:
                    this.IsBePresent = true;
                    this.IsPastParticipleOfBe = true;
                    this.TenseType = TenseType.Perfect;
                    //this.IsVerbDetected = true;
                    break;


                case TagsType.VHZ:
                case TagsType.VH:
                case TagsType.VHP:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Present;
                    }
                    this.IsHavePresent = true;
                    this.TenseType = TenseType.Simple;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VHD:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Past;
                    }
                    this.IsHavePresent = true;
                    this.TenseType = TenseType.Simple;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VHN:
                    this.IsHavePresent = true;
                    this.IsPastParticipleOfVerb = true;
                    this.TenseType = TenseType.Perfect;
                    //this.IsVerbDetected = true;
                    break;
                case TagsType.VHG:
                    this.IsHavePresent = true;
                    this.IsProgressiveOfHave = true;
                    this.TenseType = TenseType.Progressive;
                    //this.IsVerbDetected = true;
                    break;

                case TagsType.VVG:
                    this.TenseType = TenseType.Progressive;
                    this.IsVerbDetected = true;
                    break;
                case TagsType.VV:
                case TagsType.VVP:
                case TagsType.VVZ:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Present;
                    }
                    this.TenseType = TenseType.Simple;
                    this.IsVerbDetected = true;
                    break;
                case TagsType.VVD:
                    if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Past;
                    }
                    this.TenseType = TenseType.Simple;
                    this.IsVerbDetected = true;
                    if (this.IsHavePresent)
                    {
                        this.IsPastParticipleOfVerb = true; // For some verb past form and past participle are same.
                    }
                    break;
                case TagsType.VVN:
                    this.IsPastParticipleOfVerb = true;
                    if (this.IsHavePresent)
                    {
                        this.TenseType = TenseType.Perfect;
                    }
                    else if (this.Tense == Tense.None)
                    {
                        this.Tense = Tense.Past;
                        this.TenseType = TenseType.Simple;
                        this.IsVoiceDetected = true;
                        this.Voice = Voice.Active;
                    }
                    this.IsVerbDetected = true;
                    break;
                /*
                case TagsType.PPP:
                case TagsType.WPP:
                    this.Voice = Voice.Passive;
                    break;
                */
                default:
                    break;
            }
        }

        public void DetectTenseVoiceTenseType(Token Token, bool IsPhaseTwo)
        {
            if (!IsPhaseTwo)
                DetectTenseVoiceTenseType(Token);

            // Parse Parents
            if (Token.English.ToLower().Contains("have to "))
            {
                this.Voice = Voice.Passive;
            }

            if (!this.IsHavePresent)
            {
                if (this.IsPastParticipleOfBe || this.IsPastParticipleOfVerb)
                {
                    if (!this.IsVoiceDetected)
                    {
                        this.Voice = Voice.Passive;
                    }
                }
            }
            else
            {
                if (this.TenseType == TenseType.Progressive)
                {
                    this.TenseType = TenseType.PerfectProgressive;
                }
                else if (this.IsPastParticipleOfVerb)
                {
                    this.TenseType = TenseType.Perfect;
                }
                if (this.IsPastParticipleOfBe && this.IsPastParticipleOfVerb)
                    this.Voice = Voice.Passive;
            }
        }

        public void DetectPerson(Token Token)
        {
            switch (Token.Type)
            {
                case TagsType.DT:
                    if (!IsPersonDetected)
                    {
                        if (!Token.English.ToLower().Equals("the") && !this.IsVerbDetected)
                        {
                            this.Person = Person.Third;
                            IsPersonDetected = true;
                        }
                    }
                    break;
                case TagsType.PRP:
                case TagsType.PRPP:
                case TagsType.PP:
                case TagsType.PPP:
                    if (!IsPersonDetected)
                    {
                        if (this.Purpose == TypesByPurpose.Interrogative)
                        {
                            this.Person = Constant.Person.Table(Token.English);
                            IsPersonDetected = true;
                        }
                        else
                        {
                            if (!this.IsVerbDetected)
                            {
                                this.Person = Constant.Person.Table(Token.English);
                                IsPersonDetected = true;
                            }
                            else
                            {
                                if (this.Person == Person.Third)
                                {
                                    this.Person = (Person)GuessSubjectPerson(Token.English);
                                    IsPersonDetected = true;
                                }
                            }
                        }
                    }
                    break;
                case TagsType.NNP:
                case TagsType.NNPS:
                case TagsType.NP:
                case TagsType.NPS:
                case TagsType.NNS:
                case TagsType.NN:
                    if (!IsPersonDetected)
                    {
                        if (!this.IsVerbDetected && this.FirstWord != FirstWord.WhWord)
                        {
                            this.Person = Person.Third;
                            IsPersonDetected = true;
                        }
                    }
                    break;
            }
        }

        public void DetectPerson(Token Token, bool IsPhaseTwo)
        {
            if (this.Purpose == TypesByPurpose.Imperative)
                this.Person = Person.Second;
        }

        private Person GuessSubjectPerson(string EnglishWord)
        {
            Person _Person = Constant.Person.Table(EnglishWord);
            if (this.Purpose != TypesByPurpose.Interrogative)
            {
                if (_Person == Person.First)
                    return Person.Second;
                if (_Person == Person.Second)
                    return Person.First;
                else
                    return Person.Third;
            }
            else
                return this.Person;
        }

        public void DetectMainVerb(Token Token)
        {
            switch (Token.Type)
            {
                case TagsType.EX:
                    this.IsExistentialThere = true;
                    break;
                case TagsType.VB:
                case TagsType.VBG:
                case TagsType.VBN:
                case TagsType.VBP:
                case TagsType.VBZ:
                case TagsType.VBD:
                    if (!this.IsVerbDetected)
                    {
                        this.MainVerb = MainVerb.Be;
                    }
                    else
                    {
                        if (Token.English.Equals("be", StringComparison.CurrentCultureIgnoreCase)
                            || Token.English.Equals("been", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.MainVerb = MainVerb.Be;
                        }
                    }
                    break;
                case TagsType.MD:
                    this.MainVerb = MainVerb.Modal;
                    switch (Token.English.ToLower())
                    {
                        case "should":
                        case "ought":
                        case "must":
                            this.ModalType = ModalType.Should;
                            this.ModalWord = Token.English.ToLower();
                            break;
                        case "can":
                        case "could":
                            this.ModalType = ModalType.Can;
                            this.ModalWord = Token.English.ToLower();
                            break;
                        case "may":
                        case "might":
                            this.ModalType = ModalType.May;
                            this.ModalWord = Token.English.ToLower();
                            break;
                        default:
                            this.ModalWord = Token.English.ToLower();
                            break;
                    }
                    break;
                case TagsType.VV:
                case TagsType.VVZ:
                case TagsType.VVD:
                case TagsType.VVG:
                case TagsType.VVP:
                case TagsType.VVN:
                    //if (Token.WordPosition != 1)
                    {
                        if (Token.Lemma.Equals("do", StringComparison.CurrentCultureIgnoreCase))
                            this.MainVerb = MainVerb.Do;
                        else
                            this.MainVerb = MainVerb.Other;
                    }
                    break;
                case TagsType.VHZ:
                case TagsType.VHP:
                case TagsType.VHD:
                case TagsType.VHN:
                case TagsType.VHG:
                case TagsType.VH:
                    this.MainVerb = MainVerb.Have;
                    break;
            }
        }

        public void DetectMainVerb(Token Token, bool IsPhaseTwo)
        {
            DetectMainVerb(Token);
        }
    }
}
