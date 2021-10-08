using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace Pi
{
    class Program
    {
        static List<double[]> samples;
        public static long hits;
        static void Main(string[] args)
        {
            Console.WriteLine("Core count: {0}\n", Environment.ProcessorCount);
            long numberOfSamples = 100000000;
            int threadNum = 20;
            hits = 0;
            samples = GenerateSamples(numberOfSamples);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            double pi = EstimatePI(numberOfSamples,ref hits);
            long singleThreadTime = sw.ElapsedMilliseconds;

            Console.WriteLine("Single threading PI: {0}\nTime: {1}\n", pi, singleThreadTime);

            hits = 0;
            List<Thread> threadList = new List<Thread>();
            sw.Restart();
            for(int i = 0; i < threadNum; i++)
            {
                threadWork tw = new threadWork();
                Thread th = new Thread(() => tw.estimatePI(numberOfSamples / threadNum));
                th.Start();
            }
            foreach(Thread t in threadList)
            {
                t.Join();
            }

            double multiThreadPi = 4 * Convert.ToDouble(hits) / numberOfSamples;
            long multiThreadTime = sw.ElapsedMilliseconds;
            sw.Stop();
            Console.WriteLine("Multi threading PI: {0}\nTime: {1}\n", multiThreadPi, multiThreadTime);
            Console.WriteLine("Speed up factor: {0}", singleThreadTime / Convert.ToDouble(multiThreadTime));
        }
        public static double EstimatePI(long numberOfSamples, ref long hits)
        {
            for(int i = 0; i < numberOfSamples; i++)
            {
                double magnitude = Math.Sqrt((samples[i][0] * samples[i][0]) + (samples[i][1] * samples[i][1]));
                if(magnitude <= 1)
                {
                    hits++;
                }
            }
            return 4 * Convert.ToDouble(hits) / numberOfSamples;
        }

        public static List<double[]> GenerateSamples(long numberOfSamples)
        {
            double min = -1;
            double max = 1;
            Random r = new Random();
            List<double[]> randomSamples = new List<double[]>();
            for (int i = 0; i < numberOfSamples; i++)
            {
                randomSamples.Add(new double[] {r.NextDouble() * (max - min) + min, r.NextDouble() * (max - min) + min});
            }
            return randomSamples;
        }
    }

    public class threadWork
    {
        public void estimatePI(object numberOfSamplesObj)
        {
            Mutex mut = new Mutex();
            long numberOfSamples = Convert.ToInt64(numberOfSamplesObj);
            List<double[]> samples = Program.GenerateSamples(Convert.ToInt64(numberOfSamplesObj));
            for (int i = 0; i < numberOfSamples; i++)
            {
                double magnitude = Math.Sqrt((samples[i][0] * samples[i][0]) + (samples[i][1] * samples[i][1]));
                if (magnitude <= 1)
                {
                    mut.WaitOne();
                    Program.hits++;
                    mut.ReleaseMutex();
                }
            }
        }
    }
}
