using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RayTracerChallenge.Helpers
{
    public static class ArrayExtension
    {
        //public static T[] GetRow<T>(this T[,] array, int row)
        //{
        //    if (!typeof(T).IsPrimitive)
        //        throw new InvalidOperationException("Not supported for managed types.");

        //    if (array == null)
        //        throw new ArgumentNullException("array");

        //    int cols = array.GetUpperBound(1) + 1;
        //    T[] result = new T[cols];

        //    int size;

        //    if (typeof(T) == typeof(bool))
        //        size = 1;
        //    else if (typeof(T) == typeof(char))
        //        size = 2;
        //    else
        //        size = Marshal.SizeOf<T>();

        //    Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

        //    return result;
        //}

        public static double[] GetRow(this double[,] array, int row)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                list.Add(array[row, i]);
            }

            return list.ToArray();
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }
}