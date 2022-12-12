using System;
using QuickSort;

namespace ArrayGenerator
{
    internal static class Program
    {
        private const int MaxRandomValue = 4000000;
        
        public static void Main(string[] args)
        {
            var size = int.Parse(args[0]);
            CustomArray.SaveArray(FillArray(size), args.Length > 1 ? args[1] : "startArr.txt");
        }

        private static int[] FillArray(int size)
        {
            var rand = new Random();
            var randomArray = new int[size];
            for (var i = 1; i < size; i++)
            {
                randomArray[i] = rand.Next(MaxRandomValue);
            }

            return randomArray;
        }
    }
}