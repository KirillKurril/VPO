using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TLWD1.Task6
{
	public class DocumentDownloader
	{
		private readonly IHttpClient _httpClient;
		private readonly IFileService _fileService;

		public string URL { get; private set; }
		public string SaveDir { get; private set; }

		public DocumentDownloader(IHttpClient httpClient, IFileService fileService)
		{
			_httpClient = httpClient;
			_fileService = fileService;
		}

		public void AskForURLAndSaveDirectory()
		{
			while (true)
			{
				Console.WriteLine("Enter <url> <directory> (e.g. http://example.com/document.pdf C:\\Downloads)");

				string input = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(input))
				{
					Console.WriteLine("Invalid input. Please try again.");
					continue;
				}

				var inputParts = input.Split(' ', 2);
				if (inputParts.Length != 2)
				{
					Console.WriteLine("Invalid format. Please provide both URL and directory.");
					continue;
				}

				URL = inputParts[0];
				SaveDir = inputParts[1];

				if (!IsValidUrl(URL))
				{
					Console.WriteLine("Invalid URL format. Please try again.");
					URL = null;
					continue;
				}

				if (!_fileService.Exists(SaveDir))
				{
					Console.WriteLine("Directory does not exist. Creating directory...");
					try
					{
						_fileService.CreateDirectory(SaveDir);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Failed to create directory: {ex.Message}");
						continue;
					}
				}
				break;
			}
		}

		public bool IsValidUrl(string url)
		{
			var pattern = @"^(http|https)://";
			return Uri.TryCreate(url, UriKind.Absolute, out _) && Regex.IsMatch(url, pattern);
		}

		public string GetFileNameFromUrl(string url)
		{
			try
			{
				string fileName = Path.GetFileName(new Uri(url).LocalPath);
				return string.IsNullOrEmpty(fileName) ? "downloaded_document" : fileName;
			}
			catch (UriFormatException)
			{
				return "downloaded_document";
			}
		}

		public async Task<byte[]> DownloadFileAsync(string url)
		{
			HttpResponseMessage response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsByteArrayAsync();
		}

		public void SaveFile(string filePath, byte[] content)
		{
			try
			{
				_fileService.WriteAllBytes(filePath, content);
			}
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine("Access to the path is denied.");
			}
			catch (IOException ex)
			{
				Console.WriteLine($"I/O error: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unexpected error when saving file: {ex.Message}");
			}
		}

		public async Task DownloadAndSaveDocumentAsync(string url, string saveDirectory)
		{
			if (string.IsNullOrWhiteSpace(saveDirectory) || string.IsNullOrWhiteSpace(url))
			{
				Console.WriteLine("Directory path or URL cannot be empty.");
				return;
			}

			if (!IsValidUrl(url))
				throw new ArgumentException("Invalid URL.");

			string fileName = GetFileNameFromUrl(url);
			string filePath = Path.Combine(saveDirectory, fileName);

			try
			{
				byte[] fileContents = await DownloadFileAsync(url);
				SaveFile(filePath, fileContents);
				Console.WriteLine($"Document saved successfully to: {filePath}");
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Error downloading document: {ex.Message}");
			}
			catch (IOException ex)
			{
				Console.WriteLine($"Error saving document: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An unexpected error occurred: {ex.Message}");
			}

			Console.WriteLine($"Document from {url} resource was downloaded successfully to {saveDirectory}");
		}
	}

	public interface IHttpClient
	{
		Task<HttpResponseMessage> GetAsync(string requestUri);
	}

	public interface IFileService
	{
		void WriteAllBytes(string path, byte[] bytes);
		bool Exists(string? path);
		DirectoryInfo CreateDirectory(string? path);

	}
}
