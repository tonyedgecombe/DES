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

        public static int[] IP =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7,
        };

        public static int[] IPINV =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41,  9, 49, 17, 57, 25
        };

        public static int[] E =
        {
            32,  1,  2,  3,  4,  5,
             4,  5,  6,  7,  8,  9,
             8,  9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32,  1,
        };

        public static int[] LeftShifts = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};


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

        // Input is left aligned 48 bit value
        // Output is 8 left aligned 6 bit values
        public static List<byte> Split(ulong val)
        {
            var result = new List<byte>();

            for (int i = 0; i < 8; i++)
            {
                result.Add((byte) ((val & 0xFC00000000000000) >> 56));

                val <<= 6;
            }

            return result;
        } 

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

        public static ulong Encode(ulong message, ulong key)
        {
            var ip = Permute(message, IP);
            var schedule = KeySchedule(key);

            var pair = new Pair
            {
                Left = ip & 0xFFFFFFFF00000000,
                Right = ip & 0x00000000FFFFFFFF << 32
            };

            for (int i = 0; i < 16; i++)
            {
                pair = new Pair
                {
                    Left = pair.Right,
                    Right = pair.Left ^ F(pair.Right, schedule[i])
                };
            }

            ulong joined = pair.Right | pair.Left >> 32;

            return Permute(joined, IPINV);
        }

        private static ulong F(ulong right, ulong key)
        {
            var e = Permute(right, E);

            var x = e ^ key;

            return x;
        }
    }

    public struct Pair
    {
        public ulong Left;
        public ulong Right;
    }
}