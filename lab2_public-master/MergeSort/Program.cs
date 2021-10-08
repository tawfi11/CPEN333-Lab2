using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int ARRAY_SIZE = 10000000;
            int threadCount = 10;
            int min = 0;
            int max = ARRAY_SIZE;
            int[] arraySingleThread = new int[ARRAY_SIZE];



            Random r = new Random();
            // TODO : Use the "Random" class in a for loop to initialize an array
            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = r.Next(min, max);
            }

            // copy array by value.. You can also use array.copy()
            int[] arrayMultiThread = new int[ARRAY_SIZE]; // = arraySingleThread.Slice(0,arraySingleThread.Length);
            Array.Copy(arraySingleThread, arrayMultiThread, ARRAY_SIZE);

            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */
            Stopwatch sw = new Stopwatch();



            //TODO :start the stopwatch
            sw.Start();
            MergeSort(arraySingleThread);
            //TODO :Stop the stopwatch
            Console.WriteLine("Is single threaded array sorted: {0}\nTime elapsed: {1}ms\n",IsSorted(arraySingleThread), sw.ElapsedMilliseconds);



            //TODO: Multi Threading Merge Sort
            List<int[]> multiThreadArrayList = new List<int[]>();
            for (int i = 0; i < threadCount; i++)
            {
                int[] multiThreadArray = new int[ARRAY_SIZE / 10];
                Array.Copy(arrayMultiThread, i * multiThreadArray.Length, multiThreadArray, 0, multiThreadArray.Length);
                multiThreadArrayList.Add(multiThreadArray);
            }
            List<Thread> threadList = new List<Thread>();

            sw.Restart();
            for (int i = 0; i < threadCount; i++)
            {
                threadWork tw = new threadWork();
                tw.unsortedArray = multiThreadArrayList[i];
                Thread thread = new Thread(tw.process);
                thread.Start();
                threadList.Add(thread);
            }
            foreach(Thread t in threadList)
            {
                t.Join();
            }
            MultiThreadMerge(multiThreadArrayList, arrayMultiThread, max);
            Console.WriteLine("Is multithreaded array sorted? {0}\nTime elapsed: {1}ms", IsSorted(arrayMultiThread), sw.ElapsedMilliseconds);
        }

            /*********************** Methods **********************
            *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */
        static int[] MultiThreadMerge(List<int[]> sortedArraysList, int[] arr, int max)
        {
            int[] indeces = new int[sortedArraysList.Count]; //indeces of the arrays themselves
            //k = arr index, i = sorted array index, j = indeces index 
            for(int k = 0; k < arr.Length; k++)
            {
                int lowestIndex=0; //gotten from j
                int lowest = max+1;
                for (int i = 0; i < sortedArraysList.Count; i++)
                {
                    if (indeces[i] < sortedArraysList[0].Length)
                    {
                        if (sortedArraysList[i][indeces[i]] < lowest)
                        {
                            lowest = sortedArraysList[i][indeces[i]];
                            lowestIndex = i;
                        }
                    }
                }
                indeces[lowestIndex]++;
                arr[k] = lowest;
            }
            return arr;
        }

        static int[] Merge(int[] LA, int[] RA, int[] A)
        {
            // TODO :implement
            int lengthL = LA.Length;
            int lengthR = RA.Length;
            int i = 0;
            int j = 0;
            int k = 0;

            while(i < lengthL && j < lengthR)
            {
                if(LA[i] <= RA[j])
                {
                    A[k] = LA[i];
                    i++;
                }
                else
                {
                    A[k] = RA[j];
                    j++;
                }
                k++;
            }

            while(i < lengthL)
            {
                A[k] = LA[i];
                i++;
                k++;
            }

            while(j < lengthR)
            {
                A[k] = RA[j];
                j++;
                k++;
            }

            return A;
        }


            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            */
        public static int[] MergeSort(int[] A)
        {
            // TODO :implement
            int lengthA = A.Length;
            if (lengthA < 2)
                return A;
            int mid = lengthA / 2;
            int[] L = new int[mid];
            int[] R = new int[lengthA - mid];
            Array.Copy(A, 0, L, 0, mid);
            Array.Copy(A, mid, R, 0, lengthA - mid);
            MergeSort(L);
            MergeSort(R);
            Merge(L, R, A);
            return A;
        }


        // a helper function to print your array
        static void PrintArray(int[] myArray)
        {
            Console.Write("[");
            for (int i = 0; i < myArray.Length; i++)
            {
                Console.Write("{0} ", myArray[i]);

            }
            Console.Write("]");
            Console.WriteLine();

        }

        // a helper function to confirm your array is sorted
        // returns boolean True if the array is sorted
        static bool IsSorted(int[] a)
        {
            int j = a.Length - 1;
            if (j < 1) return true;
            int ai = a[0], i = 1;
            while (i <= j && ai <= (ai = a[i])) i++;
            return i > j;
        }


        
    }

    public class threadWork
    {
        public int[] unsortedArray;
        public int[] sortedArray;

        public void process()
        {
            sortedArray = Program.MergeSort(unsortedArray);   
        }
    }
}
