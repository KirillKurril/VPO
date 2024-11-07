using Moq;
using System;
using System.IO;
using Xunit;

namespace TLWD1.Task5.Tests
{
	public class UnitTest5
	{
		private readonly Mock<IDirectoryWrapper> _mockDirectoryWrapper;
		private readonly FileSearcher _fileSearcher;
		private readonly TextWriter _originalConsoleOut;

		public UnitTest5()
		{
			_originalConsoleOut = Console.Out;
			_mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
			_fileSearcher = new FileSearcher(_mockDirectoryWrapper.Object);
		}

		[Fact]
		public void Constructor_ShouldThrowArgumentNullException_WhenDirectoryWrapperIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new FileSearcher(null));
		}

		[Fact]
		public void AskForDirectoryAndExtension_ShouldSetExtensionAndDirectoryPath_WhenValidInputIsProvided()
		{
			var input = "C:\\myfolder .txt";
			var expectedPath = "C:\\myfolder";
			var expectedExtension = ".txt";

			using (var consoleInput = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetIn(consoleInput);
				Console.SetOut(consoleOutput);
				_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
				_fileSearcher.AskForDirectoryAndExtension();
			}

			Assert.Equal(expectedPath, _fileSearcher.DirectoryPath);
			Assert.Equal(expectedExtension, _fileSearcher.Extension);
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void AskForDirectoryAndExtension_ShouldNotSetValues_WhenInvalidInputIsProvided()
		{
			var input = "C:\\myfolder";
			string invalidMessage = "Invalid format. Please provide both directory and extension.";

			using (var consoleInput = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetIn(consoleInput);
				Console.SetOut(consoleOutput);
				_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
				_fileSearcher.AskForDirectoryAndExtension();
				var output = consoleOutput.ToString();

				Assert.Contains(invalidMessage, output);
				Assert.Null(_fileSearcher.DirectoryPath);
				Assert.Null(_fileSearcher.Extension);
				Console.SetOut(_originalConsoleOut);
			}
		}

		[Fact]
		public void SearchFilesInDirectory_ShouldPrintError_WhenDirectoryDoesNotExist()
		{
			
			_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
			string expectedMessage = "The directory 'C:\\myfolder' does not exist.";

			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.DirectoryPath = "C:\\myfolder";
				_fileSearcher.Extension = ".txt";
				_fileSearcher.SearchFilesInDirectory();
				var output = consoleOutput.ToString();

				Assert.Contains(expectedMessage, output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void SearchFilesInDirectory_ShouldCallSearchFiles_WhenDirectoryExists()
		{
			_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

			int callCount = 0;
			_mockDirectoryWrapper
				.Setup(x => x.GetDirectories(It.IsAny<string>()))
				.Returns(() =>
				{
					if (callCount == 1)
					{
						callCount++;
						return new string[] { "subdir1", "subdir2" };
					}
					else
					{
						return new string[] {};
					}
				});

			_mockDirectoryWrapper
				.Setup(x => x.GetFiles(It.IsAny<string>(), "*.txt"))
				.Returns(() =>
				{
					if (callCount == 0)
					{
						callCount++;
						return new string[] { "file1.txt", "file2.txt" };
					}
					else
					{
						return new string[] {};
					}
				});



			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.DirectoryPath = "C:\\myfolder";
				_fileSearcher.Extension = ".txt";
				_fileSearcher.SearchFilesInDirectory();
				var output = consoleOutput.ToString();

				Assert.Contains("file1.txt", output);
				Assert.Contains("file2.txt", output);
				Assert.Contains("subdir1", output);
				Assert.Contains("subdir2", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void SearchFiles_ShouldPrintError_WhenIOExceptionIsThrown()
		{
			
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Throws(new IOException("I/O error"));

			
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.SearchFiles("C:\\myfolder");
				var output = consoleOutput.ToString();

				
				Assert.Contains("Error fetching files from directory:", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void GetFilesWithExtension_ShouldReturnFiles_WhenValidFilesArePresent()
		{
			
			var files = new[] { "file1.txt", "file2.txt" };
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), "*.txt")).Returns(files);

			_fileSearcher.Extension = ".txt";
			var result = _fileSearcher.GetFilesWithExtension("C:\\myfolder");

			
			Assert.Equal(files, result);
		}

		[Fact]
		public void GetFilesWithExtension_ShouldReturnEmptyArray_WhenExceptionOccurs()
		{
			
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Some error"));

			
			var result = _fileSearcher.GetFilesWithExtension("C:\\myfolder");

			
			Assert.Empty(result);
		}

		[Fact]
		public void GetSubDirectories_ShouldReturnSubDirectories_WhenValidDirectoriesArePresent()
		{
			
			var directories = new[] { "subdir1", "subdir2" };
			_mockDirectoryWrapper.Setup(x => x.GetDirectories(It.IsAny<string>())).Returns(directories);

			
			var result = _fileSearcher.GetSubDirectories("C:\\myfolder");

			
			Assert.Equal(directories, result);
		}

		[Fact]
		public void GetSubDirectories_ShouldReturnEmptyArray_WhenExceptionOccurs()
		{
			
			_mockDirectoryWrapper.Setup(x => x.GetDirectories(It.IsAny<string>())).Throws(new Exception("Some error"));

			
			var result = _fileSearcher.GetSubDirectories("C:\\myfolder");

			
			Assert.Empty(result);
		}
	}
}
