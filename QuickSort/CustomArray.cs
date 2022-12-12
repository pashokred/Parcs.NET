using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuickSort
{
    [Serializable]
    public class CustomArray
    {
        public int[] Values { get; set; }
        public int Length { get { return Values.Length; } }

        public CustomArray(int size)
        {
            Values = new int[size];
        }
        public CustomArray(int[] values)
        {
            Values = values;
        }

        public int this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }

        public static void SaveArray(int[] resArr, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var extension = Path.GetExtension(fileName);
                if (extension.Equals(".txt"))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(resArr.Length);
                        Array.ForEach(resArr, value => writer.WriteLine(value));
                    }
                }
                else
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(resArr.Length);
                        Array.ForEach(resArr, value => writer.Write(value));
                    }
                }
            }
        }

        public static int[] LoadFromFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            if (extension.Equals(".txt"))
            {
                return File.ReadAllLines(filePath).Skip(1).Select(int.Parse).ToArray();
            }

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var l = reader.ReadInt32();
                    var array = new int[l];
                    for (var i = 0; i < l; i++)
                    {
                        array[i] = reader.ReadInt32();
                    }

                    return array;
                }
            }
        }

        public static IEnumerable<CustomArray> Split(IReadOnlyCollection<int> array, int amount)
        {
            var length = array.Count / amount;
            var arrOfLengths = Enumerable.Repeat(length, amount).ToArray();

            var left = array.Count % amount;
            for (var i = 0; i < left; i++)
                arrOfLengths[i]++;

            var cursor = 0;
            foreach (var pieceLength in arrOfLengths)
            {
                var piece = new CustomArray(array.Skip(cursor).Take(pieceLength).ToArray());
                cursor += pieceLength;
                yield return piece;
            }
        }
    }
}
