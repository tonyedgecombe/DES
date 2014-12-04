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
            Assert.That(DES.LeftShift(0x7654321000000000, 0x04), Is.EqualTo(0x6543217000000000));
            Assert.That(DES.LeftShift(0xF654321000000000, 0x04), Is.EqualTo(0x654321F000000000));
            Assert.That(DES.LeftShift(0xFFFFFFF000000000, 0x01), Is.EqualTo(0xFFFFFFF000000000));
            Assert.That(DES.LeftShift(0x8000000000000000, 0x01), Is.EqualTo(0x0000001000000000));
        }

        [Test]
        public void TestConcat()
        {
            const ulong left = 0x1234567100000000;
            const ulong right = 0x89ABCDE100000000;

            Assert.That(DES.Concat(left, right), Is.EqualTo(0x123456789ABCDE00));
        }

        [Test]
        public void TestKeySchedule()
        {
            var result = DES.KeySchedule(0x133457799BBCDFF1);

            Assert.That(result[1], Is.EqualTo(0x1B02EFFC70720000));
            Assert.That(result[2], Is.EqualTo(0x79AED9DBC9E50000));
            Assert.That(result[3], Is.EqualTo(0x55FC8A42CF990000));
            
            Assert.That(result[16], Is.EqualTo(0xCB3D8B0E17F50000));
        }
    }
}
