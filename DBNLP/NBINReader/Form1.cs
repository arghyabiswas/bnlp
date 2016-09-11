using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SharpEntropy.IO;
using SharpEntropy;
using OpenNLP.Tools.Parser;

namespace NBINReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = @"D:\software\Project\BNLP\DBNLP\NBINReader\data\";
            StringBuilder sblog = new StringBuilder();
            foreach (string file in Directory.GetFiles(folder))
            {
                BinaryGisModelReader buildModelReader = new BinaryGisModelReader(file);

                StringBuilder sb = new StringBuilder();
                int ParameterCount = 0;
                foreach (KeyValuePair<string, PatternedPredicate> s in buildModelReader.GetPredicates())
                {
                    StringBuilder sp = new StringBuilder();
                    for (int i = 0; i < s.Value.ParameterCount; i++)
                    {
                        sp.AppendFormat("{0},", s.Value.GetParameter(i));
                    }
                    sb.AppendLine(s.Key +"\t"+s.Value.OutcomePattern+ "\t" + sp.ToString().TrimEnd(','));
                    ParameterCount = s.Value.ParameterCount;
                }


                //Dictionary<string, PatternedPredicate> s =  buildModelReader.GetPredicates();

                FileInfo fi = new FileInfo(file);
                sblog.AppendFormat("File Name : {0}\t", fi.Name.Replace(".nbin", ""));
                sblog.AppendFormat("CorrectionConstant : {0}\t", buildModelReader.CorrectionConstant);
                sblog.AppendFormat("CorrectionParameter : {0}\t", buildModelReader.CorrectionParameter);

                string _temp = string.Empty;
                StringBuilder st = new StringBuilder(); 
                foreach (string s in buildModelReader.GetOutcomeLabels())
                {
                    st.AppendFormat("{0},", s);
                }
                _temp = st.ToString().TrimEnd(',');

                

                sblog.AppendFormat("OutcomeLabels : {0}\t", _temp);

                string _temp1 = string.Empty;

                st = new StringBuilder();
                foreach (int[] s in buildModelReader.GetOutcomePatterns())
                {
                    foreach (int t in s)
                    {
                        st.AppendFormat("{0},", t);
                    }
                    st.Append("|");
                }
                _temp1 = st.ToString().Replace(",|", "|").TrimEnd('|'); ;

                sblog.AppendFormat("OutcomePatterns : {0}\t", _temp1);

                sb.Insert(0, Environment.NewLine);
                sb.Insert(0, _temp1);
                sb.Insert(0, Environment.NewLine);
                sb.Insert(0, _temp);
                sb.Insert(0, Environment.NewLine);
                sb.Insert(0, buildModelReader.CorrectionParameter);
                sb.Insert(0, Environment.NewLine);
                sb.Insert(0, buildModelReader.CorrectionConstant);
                StreamWriter sw = new StreamWriter(file.Replace(".nbin", ".bnlp"));
                sw.Write(sb.ToString());
                sw.Close();

            }

            StreamWriter swlog = new StreamWriter(folder + "log.txt");
            swlog.Write(sblog.ToString());
            swlog.Close();
        }

        private static bool ConvertFolder(string file)
        {
            try
            {
                BinaryGisModelWriter writer = new BinaryGisModelWriter();

                writer.Persist(new GisModel(new JavaBinaryGisModelReader(file)), file.Replace(".bin", ".nbin"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
