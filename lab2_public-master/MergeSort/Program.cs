using System;
using System.Diagnostics;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {

            
            int ARRAY_SIZE = 1000;
            int min = 0;
            int max = 10000;
            int[] arraySingleThread = new int[ARRAY_SIZE];



            Random r = new Random();
            // TODO : Use the "Random" class in a for loop to initialize an array
            for(int i=0; i<ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = r.Next(min, max);
            }

            // copy array by value.. You can also use array.copy()
            int[] arrayMultiThread = new int[ARRAY_SIZE]; // = arraySingleThread.Slice(0,arraySingleThread.Length);
            arraySingleThread.CopyTo(arrayMultiThread, 0);

            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */
            Stopwatch sw = new Stopwatch();



            //TODO :start the stopwatch
            sw.Start();
            MergeSort(arraySingleThread);
            //TODO :Stop the stopwatch
            sw.Stop();



            //TODO: Multi Threading Merge Sort







             /*********************** Methods **********************
              *****************************************************/
             /*
             implement Merge method. This method takes two sorted array and
             and constructs a sorted array in the size of combined arrays
             */

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
            static int[] MergeSort(int[] A)
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


    }
}
