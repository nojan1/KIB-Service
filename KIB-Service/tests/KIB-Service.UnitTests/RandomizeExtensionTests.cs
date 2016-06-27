using KIB_Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KIB_Service.Tests
{
    public class RandomizeExtensionTests
    {
        [Fact]
        public void ShufflingTheSameListTwiceShouldNotReturnTheSameResult()
        {
            var numbers = Enumerable.Range(0, 10);

            var pass1 = numbers.Shuffle();
            var pass2 = numbers.Shuffle();

            Assert.False(numbers.SequenceEqual(pass1));
            Assert.False(pass1.SequenceEqual(pass2));
        }
    }
}
