using task_2;
void func()
{
    int number = 0;
    while (true)
    {
        try
        {
            Console.WriteLine("Enter the number of people (1-20):");
            number = Convert.ToInt32(Console.ReadLine());
            if (number < 1 || number > 20)
            {
                Console.Clear();
                Console.WriteLine("Enter a number in the correct range");
                continue;
            }
            else
                break;
        }
        catch
        {
            Console.Clear();
            Console.WriteLine("Enter correct number!");
        }
    }
    var people = new List<Person>();
    for (int i = 0; i < number; i++)
    {
        string? surname;
        string? name;
        int age;
        while (true)
        {
            Console.WriteLine($"Enter name of the {i + 1} person");
            name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter) && char.IsUpper(name![0]))
            {
                break;
            }
            else
                Console.WriteLine("Enter the correct name!");
        }

        while (true)
        {
            Console.WriteLine($"Enter surname of the {i + 1} person");
            surname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(surname) && surname.All(char.IsLetter) && char.IsUpper(surname![0]))
            {
                break;
            }
            else
                Console.WriteLine("Enter the correct surname");
        }

        while (true)
        {
            try
            {
                Console.WriteLine($"Enter age of the {i + 1} person");
                age = Convert.ToInt32(Console.ReadLine());
                if (age < 1 || age > 120)
                {
                    Console.WriteLine("Enter an age in the correct range");
                    continue;
                }
                else
                    break;

            }
            catch
            {
                Console.WriteLine("Enter correct number!");
            }
        }
        people.Add(new Person
        {
            Name = name,
            Surname = surname,
            Age = age
        });
    }
    int? youngest_age = 100;
    int? oldest_age = 1;
    double? middle_age = 0;
    for (int i = 0; i < people.Count; i++)
    {
        if (people[i].Age < youngest_age)
            youngest_age = people[i].Age;
        if (people[i].Age > oldest_age)
            oldest_age = people[i].Age;
        middle_age += people[i].Age;
        Console.WriteLine($"{people[i].Surname} {people[i].Name} {people[i].Age}");
    }
    middle_age /= people.Count;
    Console.WriteLine($"Youngest age: {youngest_age}, Oldest age: {oldest_age}, Middle age: {middle_age:F2}");
}

func();






