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

namespace DbWriter
{
    public partial class Form1 : Form
    {
        //DBNLPEntities oDb = new DBNLPEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = @"D:\software\Project\BNLP\DBNLP\NBINReader\data\";
            //StringBuilder sblog = new StringBuilder();
            foreach (string file in Directory.GetFiles(folder))
            {

                //ModelReader buildModelReader = new ModelReader(file);
                //FileInfo fi = new FileInfo(file);
                /*
                                // Add Modules
                                Module oModule;
                                string name = fi.Name.Replace(".bnlp", "");
                                oModule = oDb.Modules.FirstOrDefault(p => p.Name.Equals(name));
                                if (oModule == null)
                                {
                                    oModule = new Module();
                                    oModule.Name = name;
                                    oModule.CorrectionConstant = buildModelReader.CorrectionConstant;
                                    oModule.CorrectionParameter = (decimal)buildModelReader.CorrectionParameter;
                                    oDb.AddToModules(oModule);
                                    oDb.SaveChanges();
                                }

                
                                // Add Label 
                                OutcomeLabel oLabel;
                                foreach (string s in buildModelReader.GetOutcomeLabels())
                                {
                                    oLabel = new OutcomeLabel();
                                    oLabel.Value = s;
                                    oLabel.ModuleID = oModule.ID;
                                    oDb.AddToOutcomeLabels(oLabel);
                                }
                                oDb.SaveChanges();

                                OutcomePattern oPattern;
                                Item oItem;
                                foreach (int[] s in buildModelReader.GetOutcomePatterns())
                                {
                                    // Add OutcomePattern
                                    oPattern = new OutcomePattern();
                                    oPattern.ModuleID = oModule.ID;
                                    oDb.AddToOutcomePatterns(oPattern);
                                    oDb.SaveChanges();

                                    foreach (int t in s)
                                    {
                                        // Add Item
                                        oItem = new Item();
                                        oItem.Value = t;
                                        oItem.PatternID = oPattern.ID;
                                        oDb.AddToItems(oItem);
                                    }
                                } 
                
                                oDb.SaveChanges();

                                /*
                                // Add Predicate
                                Predicate oPredicate;
                                Parameter oParameter;
                                foreach (KeyValuePair<string, PatternedPredicate> s in buildModelReader.GetPredicates())
                                {
                                    oPredicate = new Predicate();
                                    oPredicate.Name = s.Key;
                                    oPredicate.OutcomePattern = s.Value.OutcomePattern;
                                    oPredicate.ModuleID = oModule.ID;
                                    oDb.AddToPredicates(oPredicate);
                                    oDb.SaveChanges();

                                    for (int i = 0; i < s.Value.ParameterCount; i++)
                                    {
                                        // Add Parameter
                                        oParameter = new Parameter();
                                        oParameter.Value = (decimal)s.Value.GetParameter(i);
                                        oParameter.PredicateID = oPredicate.ID;
                                        oDb.AddToParameters(oParameter);
                       
                                    }
                                }

                                oDb.SaveChanges();
               

                                using (StreamReader oReader = new StreamReader(file))
                                {
                                    oReader.ReadLine();
                                    oReader.ReadLine();
                                    oReader.ReadLine();
                                    oReader.ReadLine();
                                    while (!oReader.EndOfStream)
                                    {
                                        string t = oReader.ReadLine();
                                        Temp oTemp = new Temp();
                                        oTemp.ModuleID = oModule.ID;
                                        oTemp.Value = t;
                                        oDb.AddToTemps(oTemp);
                                    }

                                    oDb.SaveChanges();
                                } */
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            string file = @"D:\software\Project\BNLP\DBNLP\NBINReader\data\tag.nbin";
            BinaryGisModelReader oModelReader = new BinaryGisModelReader(file);
            GisModel oModel = new GisModel(oModelReader);
            SQLGisModelWriter oSQLGISModelWriter = new SQLGisModelWriter();

            oSQLGISModelWriter.WriteDB(oModel);
            */

            string folder = @"D:\software\Project\BNLP\DBNLP\NBINReader\data\";
            //StringBuilder sblog = new StringBuilder();
            foreach (string file in Directory.GetFiles(folder))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Extension.Equals(".nbin"))
                {
                    BinaryGisModelReader oModelReader = new BinaryGisModelReader(file);
                    GisModel oModel = new GisModel(oModelReader);
                    SQLGisModelWriter oSQLGISModelWriter = new SQLGisModelWriter();

                    oSQLGISModelWriter.PersistFile(oModel, folder, fi.Name.Replace(".nbin", ""));
                }
            }
        }



    }
}
