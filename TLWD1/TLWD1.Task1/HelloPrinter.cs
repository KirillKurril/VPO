

namespace TLWD1.Task1
{
	internal class HelloPrinter
	{
		public static void PrintMessage()
		{
			string message = GenerateMessage();
			Console.WriteLine(message);
		}
		public static string GenerateMessage(Random random = null)
		{
			random ??= new Random();
			int numberOfExclamations = random.Next(5, 51);
			return $"Hello, world!\nAnd hi again!\n{new string('!', numberOfExclamations)}";
		}
	}
}
