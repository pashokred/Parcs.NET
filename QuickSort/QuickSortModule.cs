using System.Threading;
using Parcs;

namespace QuickSort
{
    public class QuickSortModule : IModule
    {
        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            var array = (CustomArray)info.Parent.ReadObject(typeof(CustomArray));
            info.Parent.WriteObject(SortArray(array, 0, array.Length - 1));
        }
        
        private static CustomArray SortArray(CustomArray array, int leftIndex, int rightIndex)
        {
            int i = leftIndex, j = rightIndex;
            
            for (var pivot = array[leftIndex]; i <= j; i++, j--)
            {
                while (array[i] < pivot)
                    i++;
                
                while (array[j] > pivot)
                    j--;
                
                if (i > j) continue;

                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            if (leftIndex < j)
                SortArray(array, leftIndex, j);
            if (i < rightIndex)
                SortArray(array, i, rightIndex);
            
            return array;
        }
    }
}