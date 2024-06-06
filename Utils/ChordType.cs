using System;

namespace Netychords.Utils
{
    public enum ChordType
    {
        Maj,
        Min,
        Maj7th,
        Min7th,
        Dom7th,
        Dim7th,
        Sus2,
        Sus4,
        Dom9th,     // WRONG!
        Dom11th,    // WRONG!
        SemiDim,
        Maj6th,
        Min6th
    }

    public static class Extensions
    {
        public static ChordType Next(this ChordType chordType)
        {
            var count = Enum.GetNames(typeof(ChordType)).Length;

            ChordType num = chordType + 1;

            if((int)num < count)
            {
                return num;
            }
            else
            {
                return 0;
            }
        }

        public static ChordType Previous(this ChordType chordType)
        {
            var count = Enum.GetNames(typeof(ChordType)).Length;

            ChordType num = chordType - 1;

            if ((int)num >= 0)
            {
                return num;
            }
            else
            {
                return (ChordType)(count - 1);
            }
        }

        //public static T Next<T>(this T src) where T : struct
        //{
        //    if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        //    T[] Arr = (T[])Enum.GetValues(src.GetType());
        //    int j = Array.IndexOf<T>(Arr, src) + 1;
        //    return (Arr.Length == j) ? Arr[0] : Arr[j];
        //}
    }
}
