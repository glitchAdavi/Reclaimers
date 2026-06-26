using System.Collections.Generic;

namespace Reclaimers.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this IList<T> list, int sourceIndex, int destinationIndex, out T newDestinationValue)
        {
            newDestinationValue = list[sourceIndex];
            list[sourceIndex] = list[destinationIndex];
            list[destinationIndex] = newDestinationValue;
        }

        public static void RemoveAtSwapBack<T>(this IList<T> list, int index, out T swappedValue)
        {
            int lastIndex = list.Count - 1;
            if (lastIndex > 0 && lastIndex != index)
            {
                list[index] = list[lastIndex];
                swappedValue = list[index];
            }
            else swappedValue = default;
            list.RemoveAt(lastIndex);
        }
    }
}

