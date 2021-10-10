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
        public static Mutex mut;
        static Random r;
        static void Main(string[] args)
        {
            long numberOfSamples = 10000000;
            int threadNum = 1000;
            Console.WriteLine("Core count: {0}\nNumber of Samples: {1}\nThread Number: {2}\n\n", Environment.ProcessorCount, numberOfSamples, threadNum);
            hits = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            samples = GenerateSamples(numberOfSamples);
            double pi = EstimatePI(numberOfSamples,ref hits);
            sw.Stop();
            long singleThreadTime = sw.ElapsedMilliseconds;

            Console.WriteLine("Single threading PI: {0}\nTime: {1}\n", pi, singleThreadTime);

            hits = 0;
            List<Thread> threadList = new List<Thread>();
            mut = new Mutex();
            sw.Restart();
            for (int i = 0; i < threadNum; i++)
            {
                threadWork tw = new threadWork();
                Thread th = new Thread(() => tw.estimatePI(numberOfSamples / threadNum));
                threadList.Add(th);
                th.Start();
            }
            
            foreach(Thread t in threadList)
            {
                t.Join();
            }

            double multiThreadPi = 4 * Convert.ToDouble(hits) / numberOfSamples;
            sw.Stop();
            long multiThreadTime = sw.ElapsedMilliseconds;
            
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
            List<double[]> randomSamples = new List<double[]>();
            for (int i = 0; i < numberOfSamples; i++)
            {
                randomSamples.Add(new double[] {StaticRandom.Rand(max, min), StaticRandom.Rand(max,min)});
            }
            return randomSamples;
        }
    }

    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static double Rand(double max, double min)
        {
            return random.Value.NextDouble() * (max-min) + min;
        }
    }

    public class threadWork
    {
        public void estimatePI(object numberOfSamplesObj)
        {
            int hits = 0;
            long numberOfSamples = Convert.ToInt64(numberOfSamplesObj);
            List<double[]> samples = Program.GenerateSamples(Convert.ToInt64(numberOfSamplesObj));
            for (int i = 0; i < numberOfSamples; i++)
            {
                //Console.Write("Point: {0},{1}\r", samples[i][0], samples[i][1]);
                double magnitude = Math.Sqrt((samples[i][0] * samples[i][0]) + (samples[i][1] * samples[i][1]));
                if (magnitude <= 1)
                {
                    hits++;
                }
            }
            Interlocked.Add(ref Program.hits, hits);
        }
    }
}
