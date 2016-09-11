using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpEntropy.IO
{
    public class ModelReader : IModelReader
    {
        private char[] mSpaces;

        private int mCorrectionConstant;
        private double mCorrectionParameter;
        private string[] mOutcomeLabels;
        private int[][] mOutcomePatterns;
        private int mPredicateCount;
        private Dictionary<string, PatternedPredicate> mPredicates;

        private StreamReader mInput;
        private byte[] mBuffer;
        private int mStringLength = 0;
        private System.Text.Encoding mEncoding = System.Text.Encoding.UTF8;

        public ModelReader(StreamReader dataInputStream)
		{
			using (mInput = dataInputStream)
			{
				mBuffer = new byte[256];
				ReadModel();
			}
		}

        public ModelReader(string fileName)
		{
			using (mInput = new StreamReader(fileName))
			{
				mBuffer = new byte[256];
				ReadModel();
			}
		}

        protected int PredicateCount
        {
            get
            {
                return mPredicateCount;
            }
        }

        public int CorrectionConstant
        {
            get
            {
                return mCorrectionConstant;
            }
        }

        public double CorrectionParameter
        {
            get
            {
                return mCorrectionParameter;
            }
        }

        protected void ReadModel()
        {
            mSpaces = new Char[] { ' ' }; 
            mCorrectionConstant = ReadCorrectionConstant();
            mCorrectionParameter = ReadCorrectionParameter();
            mOutcomeLabels = ReadOutcomes();
            mOutcomePatterns = ReadOutcomePatterns();
            mPredicates = ReadPredicates();
        }

        protected int ReadCorrectionConstant()
        {
            return ReadInt32();
        }

        protected double ReadCorrectionParameter()
        {
            return ReadDouble();
        }

        protected string[] ReadOutcomes()
        {
            string _temp = ReadString();
            string[] outcomeLabels = _temp.Split(',');
            return outcomeLabels;
        }

        protected int[][] ReadOutcomePatterns()
        {
            List<int[]> _tPat = new List<int[]>();

            string _temp = ReadString();
            foreach (string _pat in _temp.Split('|'))
            {
                int[] _pats = _pat.Split(',').ToIntArray();
                _tPat.Add(_pats);
            }

            int[][] outcomePatterns = _tPat.ToArray();

            return outcomePatterns;
        }

        protected Dictionary<string, PatternedPredicate> ReadPredicates()
        {
            Dictionary<string, PatternedPredicate> predicates = new Dictionary<string, PatternedPredicate>();

            foreach (string s in ReadLines())
            {
                try
                {
                    string[] _temp = s.Split('\t');
                    double[] _d = _temp[2].Split(',').ToDoubleArray();
                    predicates.Add(_temp[0], new PatternedPredicate(_temp[1], _d));
                }
                catch { }
            }

            return predicates;
        }

        protected int ReadInt32()
        {
            return int.Parse(ReadLine());
        }

        protected double ReadDouble()
        {
            return double.Parse(ReadLine());
        }

        protected string ReadString()
        {
            return ReadLine();
        }

        protected string ReadLine()
        {
            return mInput.ReadLine();
        }

        protected string[] ReadLines()
        {
            return mInput.ReadLines();
        }

        

        public string[] GetOutcomeLabels()
        {
            return mOutcomeLabels;
        }

        public int[][] GetOutcomePatterns()
        {
            return mOutcomePatterns;
        }

        public Dictionary<string, PatternedPredicate> GetPredicates()
        {
            return mPredicates;
        }

        public void GetPredicateData(string predicateLabel, int[] featureCounts, double[] outcomeSums)
        {
            if (mPredicates.ContainsKey(predicateLabel))
            {
                PatternedPredicate predicate = mPredicates[predicateLabel];
                int[] activeOutcomes = mOutcomePatterns[predicate.OutcomePattern];

                for (int currentActiveOutcome = 1; currentActiveOutcome < activeOutcomes.Length; currentActiveOutcome++)
                {
                    int outcomeIndex = activeOutcomes[currentActiveOutcome];
                    featureCounts[outcomeIndex]++;
                    outcomeSums[outcomeIndex] += predicate.GetParameter(currentActiveOutcome - 1);
                }
            }
        }

    }
}
