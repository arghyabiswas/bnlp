using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using NLPToken;
using OpenNLP.Tools.Util;
using OpenNLP.Tools.Parser;
using System.Web.Script.Serialization;



namespace NLPToken
{
    /// <summary>
    /// Class for holding constituents.
    /// </summary>
    public class ParseToken : Parse
    {
        public ParseToken()
        {
        }

        public ParseToken(string parseText, Span span, string type, double probability)
            : base(parseText, span, type, probability)
        {
        }

        public ParseToken(string parseText, Span span, string type, double probability, ParseToken head)
            : this(parseText, span, type, probability)
        {
            mHead = head;
        }


        //public Token ToToken()
        //{
        //    foreach (TranslationFormatText _Ps in Constant.Phase)
        //    {
        //        mText = mText.Replace(_Ps.Expression, _Ps.Replace);
        //    }

        //    Token _Token = ToToken(new GlobalTokenProperty());
        //    _Token.Translate();
        //    return _Token;
        //}

        //public Token ToToken(GlobalTokenProperty GlobalTokenProperty)
        //{
        //    Token _Token = new Token();

        //    int start = mSpan.Start;

        //    if (mType != MaximumEntropyParser.TokenNode)
        //    {
        //        _Token.Type = mType;
        //    }
        //    if ((!GlobalTokenProperty.IsBeginDetected && _Token.IsBeginSentense) || _Token.IsIntermediateBegin)
        //    {
        //        GlobalTokenProperty = new GlobalTokenProperty();
        //        Parent.WordPosition = 0;
        //        GlobalTokenProperty.IsBeginDetected = true;
        //    }

        //    foreach (ParseToken childParse in ToPasrseTokenList(mParts))
        //    {
        //        Span childSpan = childParse.mSpan;

        //        // Forword Loop, Flow data parent to child.
        //        //if (Parent != null && WordPosition < Parent.WordPosition)
        //        //    WordPosition = Parent.WordPosition;
        //        // End Forword Loop

        //        Token _SubToken = childParse.ToToken(GlobalTokenProperty);

        //        // Backword Loop, Flow data child to parent
        //        if (childParse.ChildCount == 0) // Change value in End Node
        //        {
        //            GlobalTokenProperty.TempWordPosition++;
        //            childParse.WordPosition = GlobalTokenProperty.TempWordPosition;
        //        }
        //        WordPosition = childParse.WordPosition;
        //        _Token.WordPosition = childParse.WordPosition;
        //        // Backword Loop

        //        if (mType != MaximumEntropyParser.TokenNode)
        //        {
        //            //string _RawText = mText.Substring(0, mSpan.End);
        //            if (!String.IsNullOrEmpty(_SubToken.Type) && _SubToken.Tokens.Count == 0)
        //            {
        //                _SubToken.GetFormattedType();
        //                GlobalTokenProperty.DetectSentenceType(_SubToken);
        //                GlobalTokenProperty.DetectTenseVoiceTenseType(_SubToken);
        //                GlobalTokenProperty.DetectPerson(_SubToken);
        //                GlobalTokenProperty.DetectMainVerb(_SubToken);
        //            }
        //        }

        //        if (_SubToken.Type != null)
        //        {
        //            //_Token.GlobalTokenProperty = _SubToken.GlobalTokenProperty;
        //            _Token.Tokens.Add(_SubToken);
        //        }
        //        start = childSpan.End;
        //    }

        //    if (start < mSpan.End)
        //    {
        //        string _RawText = mText.Substring(start, (mSpan.End) - (start));
        //        _Token.English = _RawText;
        //    }
        //    else
        //    {
        //        string _RawText = mText.Substring(mSpan.Start, (mSpan.End - mSpan.Start));
        //        _Token.English = _RawText;
        //    }
        //    if (_Token.IsBeginSentense)
        //    {
        //        GlobalTokenProperty.DetectSentenceType(_Token, true);
        //        GlobalTokenProperty.DetectTenseVoiceTenseType(_Token, true);
        //        GlobalTokenProperty.DetectPerson(_Token, true);
        //        GlobalTokenProperty.DetectMainVerb(_Token, true);
        //        _Token.GlobalTokenProperty = GlobalTokenProperty;
        //        //GlobalTokenProperty = new GlobalTokenProperty();
        //        //Parent.WordPosition = 0;
        //    }
        //    return _Token;
        //}

        //private List<ParseToken> ToPasrseTokenList(List<Parse> mParts)
        //{
        //    List<ParseToken> _ParseTokens = new List<ParseToken>();
        //    foreach (Parse _Parse in mParts)
        //    {
        //        _ParseTokens.Add(ParseToken.ToParseToken(_Parse));
        //    }
        //    return _ParseTokens;
        //}

        //public static ParseToken ToParseToken(Parse _Parse)
        //{
        //    ParseToken _ParseToken = new ParseToken();

        //    _ParseToken.mDerivation = _Parse.mDerivation;
        //    _ParseToken.mHead = _Parse.mHead;
        //    _ParseToken.mLabel = _Parse.mLabel;
        //    _ParseToken.mParent = _Parse.mParent;
        //    _ParseToken.mParts = _Parse.mParts;
        //    _ParseToken.mProbability = _Parse.mProbability;
        //    _ParseToken.mSpan = _Parse.mSpan;
        //    _ParseToken.mText = _Parse.mText;
        //    _ParseToken.mType = _Parse.mType;
        //    _ParseToken.WordPosition = _Parse.WordPosition;

        //    return _ParseToken;
        //}
    }
}
