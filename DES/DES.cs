namespace DES
{
    public static class DES
    {
        public static int[] PC1 =
        {
            57, 49, 41, 33, 25, 17,  9,
             1, 58, 50, 42, 34, 26, 18,
            10,  2, 59, 51, 43, 35, 27,
            19, 11,  3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
             7, 62, 54, 46, 38, 30, 22,
            14,  6, 61, 53, 45, 37, 29,
            21, 13,  5, 28, 20, 12,  4
        };


        static public ulong Permute(ulong val, int[] changes)
        {
            ulong result = 0;
            const int size = sizeof (ulong) * 8;

            for (int i = 0; i < changes.Length; i++)
            {
                ulong bit = val >> size - changes[i] & 1;
                result |= bit << size - i - 1;
            }

            return result;
        }

        // Retrieve the left half of the 56 bit value
        static public uint Left(ulong val)
        {
            return (uint) ((val & 0xFFFFFFF000000000) >> 36);
        }

        // Retrieve the right half of the 56 bit value
        static public uint Right(ulong val)
        {
            return (uint)((val & 0x0000000FFFFFFF00) >> 8);
        }

        // 56 bit left shift
        static public uint LeftShift(uint val, int count)
        {
            var msb = val & 0x08000000;
            return (val << count) & 0x0FFFFFFF | msb >> 27;
        }
    }
}