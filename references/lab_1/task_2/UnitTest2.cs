using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_2
{
    public class UnitTest2
    {
        public string func(string number, List<(string name, string surname, string ageStr)> peopleData)
        {
            var people = new List<Person>();
            int num = 0;
            try
            {
                num = Convert.ToInt32(number);
                if (num < 1 || num > 20)
                {
                    return("Enter a number in the correct range");
                }
            }
            catch
            {
                return("Enter correct number!");
            }
            for (int i = 0; i < num; i++)
            {
                var (name, surname, age) = peopleData[i];
                if (!int.TryParse(age, out int age_int))
                {
                    return "Enter correct age!";
                }
                if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter) || !char.IsUpper(name[0]))
                {
                    return("Enter the correct name!");
                }
                if (string.IsNullOrWhiteSpace(surname) || !surname.All(char.IsLetter) || !char.IsUpper(surname[0]))
                {
                    return("Enter the correct surname!");
                }
                if (age_int < 1 || age_int > 120)
                {
                    return ("Enter an age in the correct range");
                }
                people.Add(new Person
                {
                    Name = name,
                    Surname = surname,
                    Age = age_int
                });
            }
            int? youngest_age = 100;
            int? oldest_age = 1;
            double? middle_age = 0;
            var text = "";
            for (int i = 0; i < people.Count; i++)
            {
                if (people[i].Age < youngest_age)
                    youngest_age = people[i].Age;
                if (people[i].Age > oldest_age)
                    oldest_age = people[i].Age;
                middle_age += people[i].Age;
                Console.WriteLine($"{people[i].Surname} {people[i].Name} {people[i].Age}");
                text += $"{people[i].Surname} {people[i].Name} {people[i].Age}\n";
            }
            middle_age /= people.Count;
            var str = $"Youngest age: {youngest_age}, Oldest age: {oldest_age}, Middle age: {middle_age:F2}";
            text += str;
            return text;
        }

        [Fact]
        public void Test1()
        {
            var unitTest = new UnitTest2();
            string number = "3";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Alice", "Johnson", "25"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Johnson Alice 25\nSmith Bob 30\nBrown Charlie 22\nYoungest age: 22, Oldest age: 30, Middle age: 25,67", text);
        }

        [Fact]
        public void Test2()
        {
            var unitTest = new UnitTest2();
            string number = "-5";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Alice", "Johnson", "25"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter a number in the correct range", text);
        }

        [Fact]
        public void Test3()
        {
            var unitTest = new UnitTest2();
            string number = "zxc";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Alice", "Johnson", "25"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter correct number!", text);
        }

        [Fact]
        public void Test4() 
        {
            var unitTest = new UnitTest2();
            string number = "3";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("zxc", "1zxc", "25"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter the correct name!", text);
        }

        [Fact]
        public void Test5()
        {
            var unitTest = new UnitTest2();
            string number = "3";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Joe", "Meow", "133"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter an age in the correct range", text);
        }

        [Fact]
        public void Test6()
        {
            var unitTest = new UnitTest2();
            string number = "3";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Joe", "Meow", "0"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter an age in the correct range", text);
        }

        [Fact]
        public void Test7()
        {
            var unitTest = new UnitTest2();
            string number = "3";
            var peopleData = new List<(string name, string surname, string age)>
            {
                ("Joe", "Meow", "zxc"),
                ("Bob", "Smith", "30"),
                ("Charlie", "Brown", "22")
            };
            var text = unitTest.func(number, peopleData);
            Assert.Equal("Enter correct age!", text);
        }
    }


}
