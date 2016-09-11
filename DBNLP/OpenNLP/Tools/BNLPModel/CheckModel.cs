using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNLP.Tools
{
    static class CheckModel
    {
        internal static SharpEntropy.IMaximumEntropyModel GetModel(string dataDirectory)
        {
            SharpEntropy.IO.ModelReader checkModelReader = new SharpEntropy.IO.ModelReader(dataDirectory + "Parser/check.bnlp");
            SharpEntropy.GisModel checkModel = new SharpEntropy.GisModel(checkModelReader);

            return checkModel;
        }
    }
}
