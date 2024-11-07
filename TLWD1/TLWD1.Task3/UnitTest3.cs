using System;
using System.IO;
using Xunit;

namespace TLWD1.Task3
{
	public class UnitTest3
	{
		[Fact]
		public void Test_CorrectInput_ValidLengthAndWidth()
		{
			var input = "5\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("The area of the rectangle is: 50", result);
		}

		[Fact]
		public void Test_IncorrectInput_NonNumericValueForLength()
		{
			var input = "abc\n10\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_IncorrectInput_NonNumericValueForWidth()
		{
			var input = "10\nxyz\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_IncorrectInput_ZeroLength()
		{
			var input = "0\n10\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_IncorrectInput_ZeroWidth()
		{
			var input = "10\n0\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_IncorrectInput_NegativeLength()
		{
			var input = "-5\n10\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_IncorrectInput_NegativeWidth()
		{
			var input = "10\n-5\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_MaximumBoundaryInput()
		{
			var input = "1e100\n1e100\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("The area of the rectangle is:", result);
		}

		[Fact]
		public void Test_InputExceedsMaxBoundary()
		{
			var input = "1e101\n10\n10\n10\n";
			SetConsoleInput(input);
			var service = new RectangularService();

			var result = CaptureConsoleOutput(() => service.Run());

			Assert.Contains("Invalid input. Please enter a positive number", result);
		}

		[Fact]
		public void Test_CalculateArea_ThrowsException_WhenNegativeLength()
		{
			var service = new RectangularService();
			double length = -1;
			double width = 10;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateArea(length, width));
			Assert.Equal($"Length must be positive value less than or equal to {service.MaxValue} (Parameter 'lenght')", exception.Message);
		}

		[Fact]
		public void Test_CalculateArea_ThrowsException_WhenNegativeWidth()
		{
			var service = new RectangularService();
			double length = 10;
			double width = -1;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateArea(length, width));
			Assert.Equal($"Width must be positive value less than or equal to {service.MaxValue} (Parameter 'width')", exception.Message);
		}

		[Fact]
		public void Test_CalculateArea_ThrowsException_WhenLengthExceedsMaxValue()
		{
			var service = new RectangularService();
			double length = 1e101;
			double width = 10;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateArea(length, width));
			Assert.Equal($"Length must be positive value less than or equal to {service.MaxValue} (Parameter 'lenght')", exception.Message);
		}

		[Fact]
		public void Test_CalculateArea_ThrowsException_WhenWidthExceedsMaxValue()
		{
			var service = new RectangularService();
			double length = 10;
			double width = 1e101;

			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateArea(length, width));
			Assert.Equal($"Width must be positive value less than or equal to {service.MaxValue} (Parameter 'width')", exception.Message);
		}

		private void SetConsoleInput(string input)
		{
			var stringReader = new StringReader(input);
			Console.SetIn(stringReader);
		}

		private string CaptureConsoleOutput(Action action)
		{
			using (var stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);
				action.Invoke();
				return stringWriter.ToString();
			}
		}
	}
}
