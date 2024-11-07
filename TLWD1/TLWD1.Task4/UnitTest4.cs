namespace TLWD1.Task4
{
	public class UnitTest4
	{
		[Fact]
		public void GenerateTableRows_ShouldGenerateCorrectNumberOfRows()
		{
			var printer = new GradientPrinter();

			var result = printer.GenerateTableRows();

			var rowCount = result.Split("<tr").Length - 1; 
			Assert.Equal(256, rowCount);
		}

		[Fact]
		public void GenerateTableRows_ShouldGenerateCorrectColorForFirstRow()
		{
			var printer = new GradientPrinter();

			var result = printer.GenerateTableRows();

			Assert.Contains("rgb(255, 255, 255)", result); 
		}

		[Fact]
		public void GenerateTableRows_ShouldGenerateCorrectColorForLastRow()
		{
			var printer = new GradientPrinter();

			var result = printer.GenerateTableRows();

			Assert.Contains("rgb(0, 0, 0)", result); 
		}

		[Fact]
		public void SaveHtmlToFile_ShouldThrowArgumentNullException_WhenContentIsNull()
		{
			var printer = new GradientPrinter();

			var exception = Assert.Throws<ArgumentNullException>(() => printer.SaveHtmlToFile(null));
			Assert.Equal("content", exception.ParamName);
		}

		[Fact]
		public void SaveHtmlToFile_ShouldThrowInvalidOperationException_WhenDirectoryNotFound()
		{
			var printer = new GradientPrinter();

			printer.FilePath = @"Z:\nonexistent\path\file.html"; 

			var exception = Assert.Throws<InvalidOperationException>(() => printer.SaveHtmlToFile("test content"));
			Assert.Contains("The specified directory was not found", exception.Message);
		}

		[Fact]
		public void SaveHtmlToFile_ShouldThrowInvalidOperationException_WhenNoPermissionToWrite()
		{
			var printer = new GradientPrinter();
			printer.FilePath = @"C:\Windows\system32\file.html"; 

			var exception = Assert.Throws<InvalidOperationException>(() => printer.SaveHtmlToFile("test content"));
			Assert.Contains("You do not have permission to write", exception.Message);
		}

		[Fact]
		public void SaveHtmlToFile_ShouldCreateFile_WhenValidContentIsProvided()
		{
			string filePath = "table_with_gradient.html";

			var printer = new GradientPrinter();
			printer.FilePath = filePath;
			string validContent = "<html><body>Test content</body></html>";

			printer.SaveHtmlToFile(validContent);

			Assert.True(File.Exists(filePath), $"File was not created at {filePath}");

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
	}
}