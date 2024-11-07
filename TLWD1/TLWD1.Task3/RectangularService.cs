

namespace TLWD1.Task3
{
	internal class RectangularService
	{
        public double MaxValue { get; set; } = 1e100;
		public void Run()
		{
			double length = GetDimension("Enter the length of the rectangle (positive number less than 1e100):");
			double width = GetDimension("Enter the width of the rectangle (positive number less than 1e100):");

			double area = CalculateArea(length, width);
			Console.WriteLine($"The area of the rectangle is: {area}");
		}

		private double GetDimension(string prompt)
		{
			double dimension;
			while (true)
			{
				Console.WriteLine(prompt);

				string input = Console.ReadLine();
				bool isValid = double.TryParse(input, out dimension);

				if (isValid && dimension > 0 && dimension <= MaxValue)
				{
					break; 
				}
				else
				{
					Console.WriteLine("Invalid input. Please enter a positive number greater than 0 and less than or equal to " + MaxValue);
				}
			}

			return dimension;
		}

		public double CalculateArea(double length, double width)
		{
			if (width <= 0 || width > MaxValue )
			{
				throw new ArgumentOutOfRangeException("width", "Width must be positive value less than or equal to " + MaxValue);
			}
			if (length <= 0 || length > MaxValue)
			{
				throw new ArgumentOutOfRangeException("lenght", "Length must be positive value less than or equal to " + MaxValue);
			}

			return length * width;
		}
	}
}
