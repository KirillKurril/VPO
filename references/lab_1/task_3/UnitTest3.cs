using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_3
{
    public class UnitTest3
    {
        public string func(string[] args)
        {
            if (args.Length > 2)
            {
                return("Too many arguments!");
            }
            else if (args.Length < 2)
            {
                return("Too few arguments!");
            }
            string first_arg = args[0];
            string second_arg = args[1];
            double length;
            double width;
            try
            {
                length = Convert.ToDouble(first_arg);
                width = Convert.ToDouble(second_arg);

                if (length <= 0 || width <= 0)
                {
                    return("Enter numbers in correct range!");
                }
                double area = length * width;
                return($"Area: {area:F2}");
            }
            catch
            {
                return("Enter correct numbers!");
            }
        }

        [Fact]
        public void Test1()
        {
            var resText = func(["3", "6"]);
            Assert.Equal("Area: 18,00", resText);
        }

        [Fact]
        public void Test2()
        {
            var resText = func(["0", "4"]);
            Assert.Equal("Enter numbers in correct range!", resText);
        }

        [Fact]
        public void Test3()
        {
            var resText = func(["zxc", "4"]);
            Assert.Equal("Enter correct numbers!", resText);
        }

        [Fact]
        public void Test4()
        {
            var resText = func(["", ""]);
            Assert.Equal("Enter correct numbers!", resText);
        }

        [Fact]
        public void Test5()
        {
            var resText = func(["12", "6,4", "9,1", "4,7"]);
            Assert.Equal("Too many arguments!", resText);
        }

        [Fact]
        public void Test6()
        {
            var resText = func(["4"]);
            Assert.Equal("Too few arguments!", resText);
        }
    }
}
