using System.Collections.Generic;
using System.Linq;

namespace DES
{
    public static class DES
    {
        public static int[] PC1 =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        public static int[] PC2 =
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };


        public static ulong Permute(ulong val, int[] changes)
        {
            ulong result = 0;
            const int size = sizeof (ulong)*8;

            for (int i = 0; i < changes.Length; i++)
            {
                ulong bit = val >> size - changes[i] & 1;
                result |= bit << size - i - 1;
            }

            return result;
        }

        // Retrieve the left half of the 56 bit value
        public static ulong Left28(ulong val)
        {
            return val & 0xFFFFFFF000000000;
        }

        // Retrieve the right half of the 56 bit value
        public static ulong Right28(ulong val)
        {
            return (val << 28) & 0xFFFFFFF000000000;
        }

        // Join two 56 bit values
        public static ulong Concat56(ulong left, ulong right)
        {
            return (left & 0xFFFFFFF000000000) | ((right & 0xFFFFFFF000000000) >> 28);
        }

        // 56 bit left shift
        public static ulong LeftShift56(ulong val, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var msb = val & 0x8000000000000000;
                val = (val << 1) & 0xFFFFFFE000000000 | msb >> 27;
            }

            return val;
        }

        public static int[] LeftShifts = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};

        public static List<ulong> KeySchedule(ulong key)
        {
            var p = Permute(key, PC1);
            var c = Left28(p);
            var d = Right28(p);

            var schedule = new List<Pair> {new Pair {Left = c, Right = d}};

            for (int i = 1; i <= LeftShifts.Count(); i++)
            {
                schedule.Add(new Pair
                {
                    Left = LeftShift56(schedule[i - 1].Left, LeftShifts[i - 1]),
                    Right = LeftShift56(schedule[i - 1].Right, LeftShifts[i - 1])
                });
            }

            var result = new List<ulong>();

            for (int i = 0; i < schedule.Count; i++)
            {
                var joined = Concat56(schedule[i].Left, schedule[i].Right);
                var permuted = Permute(joined, PC2);

                result.Add(permuted);
            }
            return result;
        }
    }

    public struct Pair
    {
        public ulong Left;
        public ulong Right;
    }
}