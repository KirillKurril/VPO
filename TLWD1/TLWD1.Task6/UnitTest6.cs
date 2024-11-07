using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TLWD1.Task6.Tests
{
	public class DocumentDownloaderTests
	{
		Mock<IHttpClient> _mockHttpClient = new();
		Mock<IFileService> _mockFileService = new();
		private readonly TextWriter _originalConsoleOut = Console.Out;

		[Fact]
		public void AskForURLAndSaveDirectory_ValidInput_SetsURLAndSaveDir()
		{
			_mockFileService.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);

			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			using (var reader = new StringReader("http://example.com/document.pdf C:\\Downloads"))
			{
				Console.SetIn(reader);
				downloader.AskForURLAndSaveDirectory();
			}

			Assert.Equal("http://example.com/document.pdf", downloader.URL);
			Assert.Equal("C:\\Downloads", downloader.SaveDir);
		}

		[Fact]
		public void AskForURLAndSaveDirectory_InvalidURL_ShowsErrorMessage()
		{
			_mockFileService.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);

			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			string input = "invalid-url C:\\Downloads\nhttp://example.com/document.pdf C:\\Downloads";

			using (var reader = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				Console.SetIn(reader);
			
				downloader.AskForURLAndSaveDirectory();
				
				var output = consoleOutput.ToString();
				Assert.Contains("Invalid URL format", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void AskForURLAndSaveDirectory_DirectoryDoesNotExist_CreatesDirectory()
		{
			_mockFileService.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(false);
			_mockFileService.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Returns(new DirectoryInfo("C:\\Downloads"));

			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			using (var reader = new StringReader("http://example.com/document.pdf C:\\Downloads"))
			{
				Console.SetIn(reader);
				downloader.AskForURLAndSaveDirectory();
			}

			_mockFileService.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Once);
		}

		[Fact]
		public async Task DownloadAndSaveDocumentAsync_ValidUrlAndDirectory_DownloadsAndSavesDocument()
		{
			_mockFileService.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);

			byte[] expectedContent = new byte[] { 1, 2, 3, 4, 5 };
			_mockHttpClient.Setup(http => http.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(expectedContent)
			});

			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			var url = "http://example.com/document.pdf";
			var saveDir = "C:\\Downloads";
			var input = url + ' ' + saveDir;

			using (var reader = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				Console.SetIn(reader);

				await downloader.DownloadAndSaveDocumentAsync(url, saveDir);

				var output = consoleOutput.ToString();
				Assert.Contains($"Document from {url} resource was downloaded successfully to {saveDir}", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public async Task DownloadAndSaveDocumentAsync_InvalidUrl_ThrowsArgumentException()
		{
			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			var exception = await Assert.ThrowsAsync<ArgumentException>(() => downloader.DownloadAndSaveDocumentAsync("invalid-url", "C:\\Downloads"));
			Assert.Equal("Invalid URL.", exception.Message);
		}

		[Fact]
		public void SaveFile_AccessDenied_ShowsErrorMessage()
		{
			_mockFileService.Setup(fs => fs.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>())).Throws(new UnauthorizedAccessException("UnauthorizedAccessException error"));
			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			byte[] content = { 1, 2, 3, 4, 5 };

			string input = "C:\\restricted\\file.pdf";

			using (var reader = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				Console.SetIn(reader);

				downloader.SaveFile(input, content);
					
				var output = consoleOutput.ToString();
				Assert.Contains("Access to the path is denied.", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void SaveFile_IoException_ShowsErrorMessage()
		{
			_mockFileService.Setup(fs => fs.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>())).Throws(new IOException("I/O error"));
			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			byte[] content = { 1, 2, 3, 4, 5 };

			string input = "C:\\restricted\\file.pdf";

			using (var reader = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				Console.SetIn(reader);

				downloader.SaveFile(input, content);

				var output = consoleOutput.ToString();
				Assert.Contains("I/O error:", output);
			}
			Console.SetOut(_originalConsoleOut);
		}

		[Fact]
		public void SaveFile_UnexpectedException_ShowsErrorMessage()
		{
			_mockFileService.Setup(fs => fs.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>())).Throws(new Exception("Unexpected error"));
			var downloader = new DocumentDownloader(_mockHttpClient.Object, _mockFileService.Object);

			byte[] content = { 1, 2, 3, 4, 5 };

			string input = "C:\\restricted\\file.pdf";

			using (var reader = new StringReader(input))
			using (var consoleOutput = new StringWriter())
			{
				Console.SetOut(consoleOutput);
				Console.SetIn(reader);

				downloader.SaveFile(input, content);

				var output = consoleOutput.ToString();
				Assert.Contains("Unexpected error when saving file:", output);
			}
			Console.SetOut(_originalConsoleOut);
		}
	}
}
