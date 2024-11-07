

namespace TLWD1.Task2.Tests
{
	public class UnitTest2
	{
		[Fact]
		public void TestGetNumberOfPeople_ValidInput_ReturnsCorrectValue()
		{
			var input = new StringReader("5");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();
			
			int number = analyzer.GetNumberOfPeople();

			Assert.Equal(5, number);
		}

		[Fact]
		public void TestGetNumberOfPeople_InvalidInput_ReturnsValidValue()
		{
			var input = new StringReader("30\n5");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			int number = analyzer.GetNumberOfPeople();

			Assert.Equal(5, number);
		}

		[Fact]
		public void TestGetPeopleData_ValidInput_ReturnsCorrectList()
		{
			var input = new StringReader("John\nDoe\n25");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			var people = analyzer.GetPeopleData(1);

			Assert.Single(people);
			Assert.Equal("John", people[0].Name);
			Assert.Equal("Doe", people[0].Surname);
			Assert.Equal(25, people[0].Age);
		}

		[Fact]
		public void TestGetValidName_ValidInput_ReturnsCorrectName()
		{
			var input = new StringReader("John");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			string name = analyzer.GetValidName("Enter name");

			Assert.Equal("John", name);
		}

		[Fact]
		public void TestGetValidName_InvalidInput_RetriesUntilValid()
		{
			var input = new StringReader("123\n\nJohn");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			string name = analyzer.GetValidName("Enter name");

			Assert.Equal("John", name);
		}

		[Fact]
		public void TestGetValidAge_ValidInput_ReturnsCorrectAge()
		{
			var input = new StringReader("25");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			int age = analyzer.GetValidAge("Enter age");

			Assert.Equal(25, age);
		}

		[Fact]
		public void TestGetValidAge_InvalidInput_RetriesUntilValid()
		{
			var input = new StringReader("abc\n-1\n125\n25");
			Console.SetIn(input);

			var analyzer = new HumanResourceAnalyzer();

			int age = analyzer.GetValidAge("Enter age");

			Assert.Equal(25, age);
		}

		[Fact]
		public void TestDisplayStatistics_CalculatesCorrectStatistics()
		{
			var people = new List<Person>
			{
				new Person { Name = "John", Surname = "Doe", Age = 25 },
				new Person { Name = "Jane", Surname = "Smith", Age = 30 },
				new Person { Name = "Max", Surname = "Brown", Age = 22 }
			};

			var analyzer = new HumanResourceAnalyzer();

			var stringWriter = new StringWriter();
			Console.SetOut(stringWriter);

			analyzer.DisplayStatistics(people);

			var output = stringWriter.ToString();
			Assert.Contains("Youngest age: 22", output);
			Assert.Contains("Oldest age: 30", output);
			Assert.Contains("Average age: 25,67", output);
		}
	}
}
