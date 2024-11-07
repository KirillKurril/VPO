

namespace TLWD1.Task2
{
	internal class HumanResourceAnalyzer
	{
		public void Run()
		{
			int numberOfPeople = GetNumberOfPeople();
			List<Person> people = GetPeopleData(numberOfPeople);

			DisplayPeople(people);
			DisplayStatistics(people);
		}

		public int GetNumberOfPeople()
		{
			int number;
			while (true)
			{
				try
				{
					Console.WriteLine("Enter the number of people (1-20):");
					number = Convert.ToInt32(Console.ReadLine());
					if (number >= 1 && number <= 20)
					{
						break;
					}
					Console.WriteLine("Enter a number in the correct range (1-20).");
				}
				catch
				{
					Console.WriteLine("Enter a valid number!");
				}
			}
			return number;
		}

		public List<Person> GetPeopleData(int number)
		{
			var people = new List<Person>();
			for (int i = 0; i < number; i++)
			{
				string name = GetValidName($"Enter name of the {i + 1} person");
				string surname = GetValidSurname($"Enter surname of the {i + 1} person");
				int age = GetValidAge($"Enter age of the {i + 1} person");

				people.Add(new Person { Name = name, Surname = surname, Age = age });
			}
			return people;
		}

		public string GetValidName(string prompt)
		{
			while (true)
			{
				Console.WriteLine(prompt);
				string? name = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter) && char.IsUpper(name[0]))
				{
					return name;
				}
				Console.WriteLine("Enter a valid name (starting with an uppercase letter and only letters)!");
			}
		}

		public string GetValidSurname(string prompt)
		{
			while (true)
			{
				Console.WriteLine(prompt);
				string? surname = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(surname) && surname.All(char.IsLetter) && char.IsUpper(surname[0]))
				{
					return surname;
				}
				Console.WriteLine("Enter a valid surname (starting with an uppercase letter and only letters)!");
			}
		}

		public int GetValidAge(string prompt)
		{
			while (true)
			{
				try
				{
					Console.WriteLine(prompt);
					int age = Convert.ToInt32(Console.ReadLine());
					if (age >= 1 && age <= 120)
					{
						return age;
					}
					Console.WriteLine("Enter an age in the correct range (1-120).");
				}
				catch
				{
					Console.WriteLine("Enter a valid number for age!");
				}
			}
		}

		public void DisplayPeople(List<Person> people)
		{
			foreach (var person in people)
			{
				Console.WriteLine($"{person.Surname} {person.Name} {person.Age}");
			}
		}

		public void DisplayStatistics(List<Person> people)
		{
			int youngestAge = people.Min(p => p.Age);
			int oldestAge = people.Max(p => p.Age);
			double averageAge = people.Average(p => p.Age);

			Console.WriteLine($"Youngest age: {youngestAge}, Oldest age: {oldestAge}, Average age: {averageAge:F2}");
		}
	}
}
