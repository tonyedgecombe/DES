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
        public static ulong Left(ulong val)
        {
            return val & 0xFFFFFFF000000000;
        }

        // Retrieve the right half of the 56 bit value
        public static ulong Right(ulong val)
        {
            return (val << 28) & 0xFFFFFFF000000000;
        }

        // 56 bit left shift
        public static ulong LeftShift(ulong val, int count)
        {
            var msb = val & 0x8000000000000000;
            return (val << count) & 0xFFFFFFF000000000 | msb >> 27;
        }

        public static int[] LeftShifts = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};


    }

    public struct Pair
    {
        public uint Left;
        public uint Right;
    }
}