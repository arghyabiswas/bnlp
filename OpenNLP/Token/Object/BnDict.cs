using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace NLPToken
{
    public class BnDict : Hashtable
    {
        public BnDict()
        {
            // Read main dictionary
            string _DictLocation = String.Format("{0}/bdict.db", GlobalVariable.DictionaryLocation);
            LoadDict(_DictLocation);

            // Read user dictionary
            _DictLocation = String.Format("{0}/bdictuser.db", GlobalVariable.DictionaryLocation);
            LoadDict(_DictLocation);

            // Read junk user dictionary
            _DictLocation = String.Format("{0}/bdictuserjunc.db", GlobalVariable.DictionaryLocation);
            LoadDict(_DictLocation);
        }

        private void LoadDict(string _DictLocation)
        {
            using (FileStream fileStream = new FileStream(_DictLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string LineValue = streamReader.ReadLine();
                        if (LineValue.Length > 0)
                        {
                            string[] db_data = LineValue.Split('\t');
                            this.Add(db_data[0], db_data[1]);
                        }
                    }
                }
            }
        }

        private void WriteDict(string _DictLocation, string Word)
        {
            try
            {
                if (File.Exists(_DictLocation))
                {
                    using (StreamWriter streamWriter = File.AppendText(_DictLocation))
                    {
                        streamWriter.WriteLine(Word);
                    }
                }
            }
            catch { }
        }
      
        public void AddWord(string EnglishWord, string BengaliWord)
        {
            this.Add(EnglishWord, BengaliWord);
            string Word = String.Format("{0}\t{1}", EnglishWord, BengaliWord);
            string _DictLocation = String.Format("{0}/bdictuser.db", GlobalVariable.DictionaryLocation);
            WriteDict(_DictLocation, Word);
        }

        public void AddJunkWord(string EnglishWord, string BengaliWord)
        {
            this.Add(EnglishWord, BengaliWord);
            string Word = String.Format("{0}\t{1}", EnglishWord, BengaliWord);
            string _DictLocation = String.Format("{0}/bdictuserjunc.db", GlobalVariable.DictionaryLocation);
            WriteDict(_DictLocation, Word);
        }
    }
}
