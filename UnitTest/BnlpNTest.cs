
using System;
using NUnit.Framework;

namespace NunitTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase ()
		{
		}

		[DeploymentItem("..\\UnitTest\\TestCases\\all_persons_modal_can_may.xml"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestCases\\all_persons_modal_can_may.xml", "sentence", DataAccessMethod.Sequential)]
		[Test]
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
	}
}

