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
            63, 55, 47, 39, 31, 23, 15, 7
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
            33, 1, 41, 9, 49, 17, 57, 25
        };

        public static int[] E =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        public static int[] P =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        public static byte[,] SBoxes =
        {
            {
                14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
            },
            {
                15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
            },
            {
                10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
            },
            {
                7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
            },
            {
                2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
            },
            {
                12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
            },
            {
                4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
            },
            {
                13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
            }
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
                ulong msb = val & 0x8000000000000000;
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

        public static byte SBoxLookup(byte val, int table)
        {
            int index = ((val & 0x80) >> 2) | ((val & 0x04) << 2) | ((val & 0x78) >> 3);
            return SBoxes[table, index];
        }

        public static List<ulong> KeySchedule(ulong key)
        {
            ulong p = Permute(key, PC1);
            ulong c = Left28(p);
            ulong d = Right28(p);

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
                ulong joined = Concat56(schedule[i].Left, schedule[i].Right);
                ulong permuted = Permute(joined, PC2);

                result.Add(permuted);
            }
            return result;
        }

        public static ulong Encode(ulong message, ulong key)
        {
            ulong ip = Permute(message, IP);
            List<ulong> schedule = KeySchedule(key);

            var pair = new Pair
            {
                Left = ip & 0xFFFFFFFF00000000,
                Right = (ip & 0x00000000FFFFFFFF) << 32
            };

            for (int i = 0; i < 16; i++)
            {
                var newPair = new Pair
                {
                    Left = pair.Right,
                    Right = pair.Left ^ F(pair.Right, schedule[i+1])
                };

                pair = newPair;
            }

            ulong joined = pair.Right | (pair.Left >> 32);

            return Permute(joined, IPINV);
        }

        public static ulong F(ulong right, ulong key)
        {
            ulong e = Permute(right, E);

            ulong x = e ^ key;

            var bs = Split(x);

            ulong boxLookup = 0;

            for (int i = 0; i < 8; i++)
            {
                boxLookup <<= 4;
                boxLookup |= SBoxLookup(bs[i], i);
            }

            boxLookup <<= 32;

            var result = Permute(boxLookup, P);

            return result;
        }
    }

    public struct Pair
    {
        public ulong Left;
        public ulong Right;
    }
}