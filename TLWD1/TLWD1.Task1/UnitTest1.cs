

using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TLWD1.Task1
{
	public class UnitTest1
	{
		[Fact]
        public void GenerateMessage_ShouldContainThreeLines()
        {
            var message = HelloPrinter.GenerateMessage();
            var lines = message.Split('\n');
            Assert.Equal(3, lines.Length);
        }

		[Fact]
		public void GenerateMessage_ShouldStartWithHelloWorld()
		{
			var message = HelloPrinter.GenerateMessage();
			var lines = message.Split('\n');
			Assert.Equal("Hello, world!", lines[0]);
		}

		[Fact]
		public void GenerateMessage_ShouldContainAndHiAgain()
		{
			var message = HelloPrinter.GenerateMessage();
			var lines = message.Split('\n');
			Assert.Equal("And hi again!", lines[1]);
		}

		[Fact]
		public void GenerateMessage_ShouldHaveExclamationMarksBetween5And50()
		{
			var random = new Random(0);
			var message = HelloPrinter.GenerateMessage(random);
			var lines = message.Split('\n');
			int exclamationCount = lines[2].Length;
			Assert.InRange(exclamationCount, 5, 50);
		}
	}
}