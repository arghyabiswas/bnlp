using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNLP.Tools
{
    static class BuildModel
    {
        static internal SharpEntropy.GisModel GetModel(string dataDirectory)
        {
            SharpEntropy.IO.ModelReader buildModelReader = new SharpEntropy.IO.ModelReader(dataDirectory + "Parser/build.bnlp");

            //SharpEntropy.IO.ModelReader buildModelReader = GetModel();

            SharpEntropy.GisModel buildModel = new SharpEntropy.GisModel(buildModelReader);

            return buildModel;
        }

        private static SharpEntropy.IO.ModelReader GetModel()
        {
            throw new NotImplementedException();
        }
    }
}
