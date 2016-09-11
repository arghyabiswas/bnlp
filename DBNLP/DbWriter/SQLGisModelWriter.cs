using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpEntropy.IO;
using SharpEntropy;
using System.IO;

namespace DbWriter
{
    class SQLGisModelWriter : GisModelWriter
    {
        public void PersistFile(GisModel model, string fileName)
        {
            Initialize(model);
            PatternedPredicate[] predicates = GetPredicates();


            using (StreamWriter file = new StreamWriter(fileName))
            {
                //"CREATE TABLE Model(CorrectionConstant INTEGER NOT NULL, CorrectionParameter FLOAT NOT NULL)"
                file.WriteLine("Module:");
                file.WriteLine("CorrectionConstant\tCorrectionParameter");
                file.WriteLine(String.Format("{0}\t{1}", model.CorrectionConstant, model.CorrectionParameter));
                file.WriteLine("----------------------------------");

                //"CREATE TABLE Outcome(OutcomeID INTEGER NOT NULL PRIMARY KEY UNIQUE, OutcomeLabel VARCHAR(255) NOT NULL)"
                file.WriteLine("Outcome:");
                file.WriteLine("OutcomeID\tOutcomeLabel");
                string[] outcomeLabels = model.GetOutcomeNames();
                for (int currentOutcomeId = 0; currentOutcomeId < outcomeLabels.Length; currentOutcomeId++)
                {
                    file.WriteLine(String.Format("{0}\t{1}", currentOutcomeId, outcomeLabels[currentOutcomeId]));
                }
                file.WriteLine("----------------------------------");

                //"CREATE TABLE Predicate(PredicateID INTEGER NOT NULL PRIMARY KEY UNIQUE, PredicateLabel VARCHAR(255) NOT NULL)"
                //"CREATE UNIQUE INDEX IX_PredicateLabel ON Predicate (PredicateLabel ASC)"
                file.WriteLine("Predicate:");
                file.WriteLine("PredicateID\tPredicateLabel");

                for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
                {
                    file.WriteLine(String.Format("{0}\t{1}", currentPredicate, predicates[currentPredicate].Name));
                }
                file.WriteLine("----------------------------------");


                //"CREATE TABLE PredicateParameter(PredicateID INTEGER NOT NULL, OutcomeID INTEGER NOT NULL, Parameter FLOAT NOT NULL, PRIMARY KEY(PredicateID, OutcomeID))"
                file.WriteLine("PredicateParameter:");
                file.WriteLine("PredicateID\tOutcomeID\tParameter");

                int[] currentOutcomePattern;
                int[][] outcomePatterns = model.GetOutcomePatterns();
                for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
                {
                    currentOutcomePattern = outcomePatterns[predicates[currentPredicate].OutcomePattern];
                    for (int currentParameter = 0; currentParameter < predicates[currentPredicate].ParameterCount; currentParameter++)
                    {
                        file.WriteLine(String.Format("{0}\t{1}\t{2}", currentPredicate, currentOutcomePattern[currentParameter + 1], predicates[currentPredicate].GetParameter(currentParameter)));
                    }
                }
                file.WriteLine("----------------------------------");
            }


        }

        protected override void WriteString(string data)
        {
            throw new NotImplementedException();
        }

        protected override void WriteInt32(int data)
        {
            throw new NotImplementedException();
        }

        protected override void WriteDouble(double data)
        {
            throw new NotImplementedException();
        }

        public void WriteDB(GisModel model)
        {
            Initialize(model);
            PatternedPredicate[] predicates = GetPredicates();

            Temp_DBNLPEntities db = new Temp_DBNLPEntities();

            //Add Model
            Model oModel = new Model();
            oModel.CorrectionConstant = model.CorrectionConstant;
            oModel.CorrectionParameter = model.CorrectionParameter;
            db.Models.AddObject(oModel);
            db.SaveChanges();



            // Outcome
            string[] outcomeLabels = model.GetOutcomeNames();
            for (int currentOutcomeId = 0; currentOutcomeId < outcomeLabels.Length; currentOutcomeId++)
            {
                Outcome oOutcome = new Outcome();
                oOutcome.OutcomeID = currentOutcomeId;
                oOutcome.OutcomeLabel = outcomeLabels[currentOutcomeId];
                db.Outcomes.AddObject(oOutcome);
            }
            db.SaveChanges();

            //Predicate
            for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
            {
                Predicate oPredicate = new Predicate();
                oPredicate.PredicateID = currentPredicate;
                oPredicate.PredicateLabel = predicates[currentPredicate].Name;
                db.Predicates.AddObject(oPredicate);
            }
            db.SaveChanges();


            //PredicateParameter
            int[] currentOutcomePattern;
            int[][] outcomePatterns = model.GetOutcomePatterns();
            for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
            {
                currentOutcomePattern = outcomePatterns[predicates[currentPredicate].OutcomePattern];
                for (int currentParameter = 0; currentParameter < predicates[currentPredicate].ParameterCount; currentParameter++)
                {
                    PredicateParameter oPredicateParameter = new PredicateParameter();
                    oPredicateParameter.PredicateID = currentPredicate;
                    oPredicateParameter.OutcomeID = currentOutcomePattern[currentParameter + 1];
                    oPredicateParameter.Parameter = predicates[currentPredicate].GetParameter(currentParameter);
                    db.PredicateParameters.AddObject(oPredicateParameter);
                }
            }
            db.SaveChanges();
        }

        public void PersistFile(GisModel model, string Dir, string ModelName)
        {
            Initialize(model);
            PatternedPredicate[] predicates = GetPredicates();

            string dirPath = String.Format(@"{0}\{1}", Dir, ModelName);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string fileName = "";

            //"CREATE TABLE Model(CorrectionConstant INTEGER NOT NULL, CorrectionParameter FLOAT NOT NULL)"
            fileName = String.Format(@"{0}\Module.txt", dirPath);
            using (StreamWriter file = new StreamWriter(fileName))
            {
                //file.WriteLine("CorrectionConstant\tCorrectionParameter");
                file.WriteLine(String.Format("{0}\t{1}\t{2}", model.CorrectionConstant, model.CorrectionParameter, ModelName));

            }

            //"CREATE TABLE Outcome(OutcomeID INTEGER NOT NULL PRIMARY KEY UNIQUE, OutcomeLabel VARCHAR(255) NOT NULL)"
            fileName = String.Format(@"{0}\Outcome.txt", dirPath);
            using (StreamWriter file = new StreamWriter(fileName))
            {
                file.WriteLine("OutcomeID\tOutcomeLabel");
                string[] outcomeLabels = model.GetOutcomeNames();
                for (int currentOutcomeId = 0; currentOutcomeId < outcomeLabels.Length; currentOutcomeId++)
                {
                    file.WriteLine(String.Format("{0}\t{1}", currentOutcomeId, outcomeLabels[currentOutcomeId]));
                }
            }

            //"CREATE TABLE Predicate(PredicateID INTEGER NOT NULL PRIMARY KEY UNIQUE, PredicateLabel VARCHAR(255) NOT NULL)"
            //"CREATE UNIQUE INDEX IX_PredicateLabel ON Predicate (PredicateLabel ASC)"
            fileName = String.Format(@"{0}\Predicate.txt", dirPath);
            using (StreamWriter file = new StreamWriter(fileName))
            {
                file.WriteLine("PredicateID\tPredicateLabel");

                for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
                {
                    file.WriteLine(String.Format("{0}\t{1}", currentPredicate, predicates[currentPredicate].Name));
                }
            }

            //"CREATE TABLE PredicateParameter(PredicateID INTEGER NOT NULL, OutcomeID INTEGER NOT NULL, Parameter FLOAT NOT NULL, PRIMARY KEY(PredicateID, OutcomeID))"
            fileName = String.Format(@"{0}\PredicateParameter.txt", dirPath);
            using (StreamWriter file = new StreamWriter(fileName))
            {
                file.WriteLine("PredicateID\tOutcomeID\tParameter");

                int[] currentOutcomePattern;
                int[][] outcomePatterns = model.GetOutcomePatterns();
                for (int currentPredicate = 0; currentPredicate < predicates.Length; currentPredicate++)
                {
                    currentOutcomePattern = outcomePatterns[predicates[currentPredicate].OutcomePattern];
                    for (int currentParameter = 0; currentParameter < predicates[currentPredicate].ParameterCount; currentParameter++)
                    {
                        file.WriteLine(String.Format("{0}\t{1}\t{2}", currentPredicate, currentOutcomePattern[currentParameter + 1], predicates[currentPredicate].GetParameter(currentParameter)));
                    }
                }
            }
        }
    }
}
