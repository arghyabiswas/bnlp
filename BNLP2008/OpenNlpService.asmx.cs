using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using OpenNLP.Tools;
using NLPToken;

namespace BNLP2008
{
    /// <summary>
    /// Summary description for OpenNlpService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OpenNlpService : System.Web.Services.WebService
    {
        private OpenNLP.Tools.Parser.EnglishTreebankParser mParser;

        [WebMethod]
        public string ParseJSON(string Sentence)
        {
            StringBuilder output = new StringBuilder();

            output.Append(Common.Parser.DoParse(Sentence).Show());

            return output.ToString();
        }
    }
}
