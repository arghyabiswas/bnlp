using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;


namespace UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BnlpTest
    {
        BNLPService.BNLPServiceSoapClient oService;
        public BnlpTest()
        {
            oService = new BNLPService.BNLPServiceSoapClient();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region all_persons_modal_can_may
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_modal_can_may.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_modal_can_may.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_modal_can_may()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region all_persons_modal_can_may_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_modal_can_may_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_modal_can_may_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_modal_can_may_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region all_persons_modal_should
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_modal_should.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_modal_should.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_modal_should()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region all_persons_modal_should_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_modal_should_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_modal_should_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_modal_should_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region all_persons_tenses_passive
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_tenses_passive.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_tenses_passive.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_tenses_passive()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region all_persons_tenses_passive_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\all_persons_tenses_passive_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_tenses_passive_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void all_persons_tenses_passive_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region auxiliary_verbs_as_main_verbs
        [DeploymentItem("..\\UnitTest\\TestCases\\auxiliary_verbs_as_main_verbs.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\auxiliary_verbs_as_main_verbs.xml", "sentence", DataAccessMethod.Sequential)]

        public void auxiliary_verbs_as_main_verbs()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region first_person_tenses_active
        [DeploymentItem("..\\UnitTest\\TestCases\\first_person_tenses_active.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\first_person_tenses_active.xml", "sentence", DataAccessMethod.Sequential)]

        public void first_person_tenses_active()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region first_person_tenses_active_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\first_person_tenses_active_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\first_person_tenses_active_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void first_person_tenses_active_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_aux
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_aux.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_aux.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_aux()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_do
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_do.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_do.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_do()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_how
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_how.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_how.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_how()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_what
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_what.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_what.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_what()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_where
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_where.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_where.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_where()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_which
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_which.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_which.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_which()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_who
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_who.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_who.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_who()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_whom
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_whom.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_whom.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_whom()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region interrogative_sentences_why
        [DeploymentItem("..\\UnitTest\\TestCases\\interrogative_sentences_why.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\interrogative_sentences_why.xml", "sentence", DataAccessMethod.Sequential)]

        public void interrogative_sentences_why()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region prepositions_made_with_three_words
        [DeploymentItem("..\\UnitTest\\TestCases\\prepositions_made_with_three_words.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\prepositions_made_with_three_words.xml", "sentence", DataAccessMethod.Sequential)]

        public void prepositions_made_with_three_words()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region prepositions_made_with_two_words
        [DeploymentItem("..\\UnitTest\\TestCases\\prepositions_made_with_two_words.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\prepositions_made_with_two_words.xml", "sentence", DataAccessMethod.Sequential)]

        public void prepositions_made_with_two_words()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region second_person_tenses_active
        [DeploymentItem("..\\UnitTest\\TestCases\\second_person_tenses_active.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\second_person_tenses_active.xml", "sentence", DataAccessMethod.Sequential)]

        public void second_person_tenses_active()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region second_person_tenses_active_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\second_person_tenses_active_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\second_person_tenses_active_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void second_person_tenses_active_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region special_handling_do_verb
        [DeploymentItem("..\\UnitTest\\TestCases\\special_handling_do_verb.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\special_handling_do_verb.xml", "sentence", DataAccessMethod.Sequential)]

        public void special_handling_do_verb()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region third_person_tenses_active
        [DeploymentItem("..\\UnitTest\\TestCases\\third_person_tenses_active.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\third_person_tenses_active.xml", "sentence", DataAccessMethod.Sequential)]

        public void third_person_tenses_active()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region third_person_tenses_active_negation
        [DeploymentItem("..\\UnitTest\\TestCases\\third_person_tenses_active_negation.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\third_person_tenses_active_negation.xml", "sentence", DataAccessMethod.Sequential)]

        public void third_person_tenses_active_negation()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

        #region RandomWord
        [DeploymentItem("..\\UnitTest\\TestCases\\wikipedia.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\wikipedia.xml", "sentence", DataAccessMethod.Sequential)]

        public void RandomWord()
        {

            string input = (string)TestContext.DataRow["english"];
            string expected = (string)TestContext.DataRow["expected_bengali"];
            input = input.Trim();
            expected = expected.Trim();
            string output = oService.Translate(input);
            output = output.Trim();
            Assert.AreEqual(expected, output, input);

        }
        #endregion

    }
}
