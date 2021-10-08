using System;

namespace MultiThreadPi
{
    class MainClass
    {
        static void Main(string[] args)
        {
            long numberOfSamples = 1000;
            long hits;
            double[,] samples = GenerateSamples(numberOfSamples);

            //double pi = EstimatePI(numberOfSamples, ref long hits);
        }
        /*static double EstimatePI(long numberOfSamples, ref long hits)
        {
            //implement
        }*/

        static double[,] GenerateSamples(long numberOfSamples)
        {
            double min = -1;
            double max = 1;
            Random r = new Random();
            double[,] randomSamples = new double[numberOfSamples, numberOfSamples];
            for (int i = 0; i < Math.Sqrt(numberOfSamples); i++)
            {
                for (int j = 0; j < Math.Sqrt(numberOfSamples); j++)
                {
                    randomSamples[i, j] = r.NextDouble() * (max - min) + min;
                }
            }
            return randomSamples;
        }
    }
    
}
