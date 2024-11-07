
namespace TLWD1.Task5
{
	public class FileSearcher
	{
		public readonly IDirectoryWrapper _directoryWrapper;
		public string Extension { get; set; }
		public string DirectoryPath { get; set; }

		public FileSearcher(IDirectoryWrapper directoryWrapper)
		{
			_directoryWrapper = directoryWrapper ?? throw new ArgumentNullException(nameof(directoryWrapper));
		}

		public void AskForDirectoryAndExtension()
		{
			Console.WriteLine("Enter <path> <extension> (e.g. C:\\myfolder .txt)");

			string input = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(input))
			{
				Console.WriteLine("Invalid input.");
				return;
			}

			var inputParts = input.Split(' ');

			if (inputParts.Length != 2)
			{
				Console.WriteLine("Invalid format. Please provide both directory and extension.");
				return;
			}

			DirectoryPath = inputParts[0];
			Extension = inputParts[1].StartsWith(".") ? inputParts[1] : $".{inputParts[1]}";

			SearchFilesInDirectory();
		}

		public void SearchFilesInDirectory()
		{
			if (string.IsNullOrWhiteSpace(DirectoryPath) || string.IsNullOrWhiteSpace(Extension))
			{
				Console.WriteLine("Directory path or extension cannot be empty.");
				return;
			}

			if (!_directoryWrapper.Exists(DirectoryPath))
			{
				Console.WriteLine($"The directory '{DirectoryPath}' does not exist.");
				return;
			}

			try
			{
				SearchFiles(DirectoryPath);
			}
			catch (UnauthorizedAccessException ex)
			{
				Console.WriteLine($"Access denied to directory: {DirectoryPath}. {ex.Message}");
			}
			catch (IOException ex)
			{
				Console.WriteLine($"I/O error while accessing the directory: {DirectoryPath}. {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error: {ex.Message}");
			}
		}

		public void SearchFiles(string directoryPath)
		{
			try
			{
				string[] files = GetFilesWithExtension(directoryPath);
				foreach (string file in files)
				{
					Console.WriteLine(file);
				}

				string[] directories = GetSubDirectories(directoryPath);
				foreach (string directory in directories)
				{
					Console.WriteLine(directory);
					SearchFiles(directory);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during file search in directory: {directoryPath}. {ex.Message}");
			}
		}

		public string[] GetFilesWithExtension(string directoryPath)
		{
			try
			{
				return _directoryWrapper.GetFiles(directoryPath, "*" + Extension);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching files from directory: {directoryPath}. {ex.Message}");
				return Array.Empty<string>();
			}
		}

		public string[] GetSubDirectories(string directoryPath)
		{
			try
			{
				return _directoryWrapper.GetDirectories(directoryPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching subdirectories from directory: {directoryPath}. {ex.Message}");
				return Array.Empty<string>();
			}
		}
	}

	public interface IDirectoryWrapper
	{
		string[] GetFiles(string path, string searchPattern);
		string[] GetDirectories(string path);
		bool Exists(string path);
	}
}
