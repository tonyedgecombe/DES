using System.Linq;
using NUnit.Framework;

namespace DES.Test
{
    [TestFixture]
    public class PermuteTest
    {
        [Test]
        public void TestSimplePermute()
        {
            var permutation = Enumerable.Range(1, 64).ToArray();

            Assert.That(DES.Permute(0, permutation), Is.EqualTo(0));

            Assert.That(DES.Permute(0xFFFFFFFFFFFFFFFF, permutation), Is.EqualTo(0xFFFFFFFFFFFFFFFF));
        }


        [Test]
        public void TestPermuteOnPC1()
        {
            const ulong before = 0x133457799BBCDFF1;
            const ulong expected = 0xF0CCAAF556678F00;
            var result = DES.Permute(before, DES.PC1);

            Assert.That(result, Is.EqualTo(expected), string.Format("Actual {0:x16}\nExpected {1:x16}", result, expected));
        }

        [Test]
        public void TestLeft()
        {
            Assert.That(DES.Left(0x0123456789ABCDEF), Is.EqualTo(0x0123456000000000));
        }

        [Test]
        public void TestRight()
        {
            Assert.That(DES.Right(0x0123456789ABCDEF), Is.EqualTo(0x789ABCD000000000));
        }

        [Test]
        public void TestLeftShift()
        {
            Assert.That(DES.LeftShift(0x01000000000, 0x01), Is.EqualTo(0x02000000000));
            Assert.That(DES.LeftShift(0x01000000000, 0x02), Is.EqualTo(0x04000000000));
            Assert.That(DES.LeftShift(0x7654321000000000, 0x04), Is.EqualTo(0x6543210000000000));
            Assert.That(DES.LeftShift(0xF654321000000000, 0x04), Is.EqualTo(0x6543211000000000));
        }
    }
}
