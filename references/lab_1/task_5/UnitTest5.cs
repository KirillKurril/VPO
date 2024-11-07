using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_5
{
    public class UnitTest5
    {
        string func(string[] args)
        {
            if (args.Length > 2 || args.Length < 2)
            {
                return "Enter <path> <extension>";
            }
            string path = args[0];
            string extension = args[1];
            if (!Directory.Exists(path))
            {
                return "Directory does not exist";
            }
            try
            {
                string[] files = Directory.GetFiles(path, $"*{extension}", SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    string result_str = "";
                    foreach (var file in files)
                    {
                        result_str += file + "\n";

                    }
                    if (result_str.EndsWith("\n"))
                    {
                        result_str = result_str.Remove(result_str.Length - 1);
                    }
                    return result_str;
                }
                else
                {
                    return "Extension not found";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.ToString()}";
            }
        }

        [Fact]
        public void Test1()
        {
            string[] args = ["C:\\input", "asm"];
            var resText = func(args);
            Assert.Equal("C:\\input\\mycode.asm", resText);
        }

        [Fact]
        public void Test2()
        {
            string[] args = [".", "asm", "jpeg"];
            var resText = func(args);
            Assert.Equal("Enter <path> <extension>", resText);
        }

        [Fact]
        public void Test3()
        {
            string[] args = ["."];
            var resText = func(args);
            Assert.Equal("Enter <path> <extension>", resText);
        }

        [Fact]
        public void Test4()
        {
            string[] args = ["C:\\gamess", "jpeg"];
            var resText = func(args);
            Assert.Equal("Directory does not exist", resText);
        }

        [Fact]
        public void Test5()
        {
            string[] args = ["C:\\games", "mp4"];
            var resText = func(args);
            Assert.Equal("Extension not found", resText);
        }
    }
}
