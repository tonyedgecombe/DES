using System.Collections.Generic;
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
            var permutation = Enumerable.Range(1, 64).ToList();

            Assert.That(Program.Permute(0, 64, permutation), Is.EqualTo(0));

            Assert.That(Program.Permute(0xFFFFFFFFFFFFFFFF, 64, permutation), Is.EqualTo(0xFFFFFFFFFFFFFFFF));
        }

        [Test]
        public void TestPermute()
        {
            var permutation = new List<int> {4, 3, 2, 1};
            Assert.That(Program.Permute(10, 4, permutation), Is.EqualTo(5));
            Assert.That(Program.Permute(8, 4, permutation), Is.EqualTo(1));
            Assert.That(Program.Permute(12, 4, permutation), Is.EqualTo(3));
            Assert.That(Program.Permute(4, 4, permutation), Is.EqualTo(2));
        }
    }
}
