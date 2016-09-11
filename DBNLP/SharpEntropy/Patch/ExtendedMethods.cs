using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpEntropy.IO
{
    public static class ExtendedMethods
    {
        /*
        public static string ReadLine(this Stream Stream)
        {
            string line;
            using (var reader = new StreamReader(Stream))
            {
                line = reader.ReadLine();
            }

            return line;
        }

        public static string[] ReadLines(this Stream Stream)
        {
            List<string> lines = new List<string>();
            using (var reader = new StreamReader(Stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }
        */
        public static string[] ReadLines(this StreamReader Stream)
        {
            List<string> lines = new List<string>();
            string line;
            while ((line = Stream.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines.ToArray();
        }
        

        public static int[] ToIntArray(this string[] arrayToConvert)
        {
            int[] resultingArray = new int[arrayToConvert.Length];

            int itemValue;

            resultingArray = Array.ConvertAll<string, int>
                (
                    arrayToConvert,
                    delegate(string strParameter)
                    {
                        int.TryParse(strParameter, out itemValue);
                        return itemValue;
                    }
                );

            return resultingArray;
        }

        public static double[] ToDoubleArray(this string[] arrayToConvert)
        {
            double[] resultingArray = new double[arrayToConvert.Length];

            double itemValue;

            resultingArray = Array.ConvertAll<string, double>
                (
                    arrayToConvert,
                    delegate(string strParameter)
                    {
                        double.TryParse(strParameter, out itemValue);
                        return itemValue;
                    }
                );

            return resultingArray;
        }
    }
}
