using System;
using System.IO;

namespace TLWD1.Task4
{
    internal class GradientPrinter
    {
        public string FilePath { get; set; } = "D:\\uni\\VPO\\TLWD1\\TLWD1.Task4\\table_with_gradient.html";

        public string Print()
        {
            string htmlContent = GenerateHtmlContent();
            SaveHtmlToFile(htmlContent);
            return $"HTML-file created. Path: {FilePath}";
        }

        public string GenerateHtmlContent()
        {
            string tableRows = GenerateTableRows();
            return $"""
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />
                        <title>Index</title>
                    </head>
                    <body style=\"margin: 0px; padding: 0px 0px 0px 0px\">
                        <table style=\"width: 100%; border-collapse: collapse;\">{tableRows}</table>
                    </body>
                    </html>
                    """;
        }

        public string GenerateTableRows()
        {
            var rows = string.Empty;

            for (int i = 255; i > -1; i--)
            {
                string color = $"rgb({i}, {i}, {i})";
                rows += $"<tr style=\"background-color: {color}; height: 3px\"><td></td></tr>";
            }

            return rows;
        }

		public void SaveHtmlToFile(string content)
		{
			if (content == null)
			{
				throw new ArgumentNullException(nameof(content), "Content cannot be null");
			}

			try
			{
				using (StreamWriter streamWriter = new StreamWriter(FilePath))
				{
					streamWriter.WriteLine(content);
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new InvalidOperationException("You do not have permission to write to this file path.", ex);
			}
			catch (DirectoryNotFoundException ex)
			{
				throw new InvalidOperationException("The specified directory was not found.", ex);
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("An error occurred while writing to the file.", ex);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("An unexpected error occurred.", ex);
			}
		}

	}
}

