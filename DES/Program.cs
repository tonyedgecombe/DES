using System.Collections.Generic;
using System.Diagnostics;

namespace DES
{
    public class Program
    {
        static void Main()
        {
        }

        static public ulong Permute(ulong val, int size, List<int> changes)
        {
            Debug.Assert(size == changes.Count);

            ulong result = 0;

            for (int i = size - 1; val != 0; i--)
            {
                result |= (val & 1) << size - changes[i];
                val >>= 1;
            }

            return result;
        }
    }
}
