using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_1
{
    public class UnitTest1
    {
        public string func(Random random = null)
        {
            random ??= new Random();
            var number = random.Next(5, 51);
            var text = $"Hello, world!\nAndhiagain!\n{new string('!', number)}";
            return text;
        }

        [Fact]
        public void Test1()
        {
            var expectedCount = 40;
            var random = new Random(0);
            var unitTestInstance = new UnitTest1();
            var result = unitTestInstance.func(random);
            var count = result.Split("!").Length - 1;

            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void Test2()
        {
            var random = new Random(0);
            var unitTestInstance = new UnitTest1();
            var result = unitTestInstance.func();
            var count = result.Split('\n').Length;

            Assert.Equal(3, count);
        }

        [Fact]
        public void Test3()
        {
            var unitTestInstance = new UnitTest1();
            var result = unitTestInstance.func();

            Assert.True(result.Contains("Hello, world!", StringComparison.Ordinal));
        }

        [Fact]
        public void Test4()
        {
            var unitTestInstance = new UnitTest1();
            var result = unitTestInstance.func();

            Assert.NotEmpty(result);
        }
    }
}
