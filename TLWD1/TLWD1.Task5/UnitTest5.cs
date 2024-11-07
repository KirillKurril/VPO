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

		public UnitTest5()
		{
			_mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
			_fileSearcher = new FileSearcher(_mockDirectoryWrapper.Object);
		}

		[Fact]
		public void Constructor_ShouldThrowArgumentNullException_WhenDirectoryWrapperIsNull()
		{
			// Arrange & Act & Assert
			Assert.Throws<ArgumentNullException>(() => new FileSearcher(null));
		}

		[Fact]
		public void AskForDirectoryAndExtension_ShouldSetExtensionAndDirectoryPath_WhenValidInputIsProvided()
		{
			// Arrange
			var input = "C:\\myfolder .txt";
			var expectedPath = "C:\\myfolder";
			var expectedExtension = ".txt";

			// Act
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
				_fileSearcher.AskForDirectoryAndExtension();
			}

			// Assert
			Assert.Equal(expectedPath, _fileSearcher.DirectoryPath);
			Assert.Equal(expectedExtension, _fileSearcher.Extension);
		}

		[Fact]
		public void AskForDirectoryAndExtension_ShouldNotSetValues_WhenInvalidInputIsProvided()
		{
			// Arrange
			var input = "C:\\myfolder";
			string invalidMessage = "Invalid format. Please provide both directory and extension.";

			// Act
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
				_fileSearcher.AskForDirectoryAndExtension();
				var output = consoleOutput.ToString();

				// Assert
				Assert.Contains(invalidMessage, output);
				Assert.Null(_fileSearcher.DirectoryPath);
				Assert.Null(_fileSearcher.Extension);
			}
		}

		[Fact]
		public void SearchFilesInDirectory_ShouldPrintError_WhenDirectoryDoesNotExist()
		{
			// Arrange
			_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
			string expectedMessage = "The directory 'C:\\myfolder' does not exist.";

			// Act
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.DirectoryPath = "C:\\myfolder";
				_fileSearcher.Extension = ".txt";
				_fileSearcher.SearchFilesInDirectory();
				var output = consoleOutput.ToString();

				// Assert
				Assert.Contains(expectedMessage, output);
			}
		}

		[Fact]
		public void SearchFilesInDirectory_ShouldCallSearchFiles_WhenDirectoryExists()
		{
			_mockDirectoryWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), "*")).Returns(new string[] { "file1.txt", "file2.txt" });
			_mockDirectoryWrapper.Setup(x => x.GetDirectories(It.IsAny<string>())).Returns(new string[] { "subdir1", "subdir2" });

			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.DirectoryPath = "C:\\myfolder";
				_fileSearcher.Extension = ".txt";
				_fileSearcher.SearchFilesInDirectory();
				var output = consoleOutput.ToString();

				// Assert
				Assert.Contains("file1.txt", output);
				Assert.Contains("file2.txt", output);
				Assert.Contains("subdir1", output);
				Assert.Contains("subdir2", output);
			}
		}

		[Fact]
		public void SearchFiles_ShouldPrintError_WhenIOExceptionIsThrown()
		{
			// Arrange
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Throws(new IOException("I/O error"));

			// Act
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				_fileSearcher.SearchFiles("C:\\myfolder");
				var output = consoleOutput.ToString();

				// Assert
				Assert.Contains("I/O error while accessing the directory", output);
			}
		}

		[Fact]
		public void GetFilesWithExtension_ShouldReturnFiles_WhenValidFilesArePresent()
		{
			// Arrange
			var files = new[] { "file1.txt", "file2.txt" };
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), "* .txt")).Returns(files);

			// Act
			var result = _fileSearcher.GetFilesWithExtension("C:\\myfolder");

			// Assert
			Assert.Equal(files, result);
		}

		[Fact]
		public void GetFilesWithExtension_ShouldReturnEmptyArray_WhenExceptionOccurs()
		{
			// Arrange
			_mockDirectoryWrapper.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Some error"));

			// Act
			var result = _fileSearcher.GetFilesWithExtension("C:\\myfolder");

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void GetSubDirectories_ShouldReturnSubDirectories_WhenValidDirectoriesArePresent()
		{
			// Arrange
			var directories = new[] { "subdir1", "subdir2" };
			_mockDirectoryWrapper.Setup(x => x.GetDirectories(It.IsAny<string>())).Returns(directories);

			// Act
			var result = _fileSearcher.GetSubDirectories("C:\\myfolder");

			// Assert
			Assert.Equal(directories, result);
		}

		[Fact]
		public void GetSubDirectories_ShouldReturnEmptyArray_WhenExceptionOccurs()
		{
			// Arrange
			_mockDirectoryWrapper.Setup(x => x.GetDirectories(It.IsAny<string>())).Throws(new Exception("Some error"));

			// Act
			var result = _fileSearcher.GetSubDirectories("C:\\myfolder");

			// Assert
			Assert.Empty(result);
		}
	}
}
