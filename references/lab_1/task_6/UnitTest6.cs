using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_6
{
    public class UnitTest6
    {
        async Task<string> func(string[] args)
        {
            if (args.Length < 2 || args.Length > 2)
            {
                return ("Enter <URL> <path_directory>");
            }
            string url = args[0];
            string path = args[1];

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return ("URL is not correct");
            }
            if (!Directory.Exists(path))
            {
                return ("Directory does not exist");
            }
            var uri = new Uri(url);
            string absPath = uri.AbsolutePath;
            if (string.IsNullOrEmpty(Path.GetExtension(absPath)))
            {
                return ("There is no file at the specified link");
            }
            string fileName = Path.GetFileName(new Uri(url).AbsolutePath);
            string filePath = Path.Combine(path, fileName);
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fs);
                }
                if (File.Exists(filePath))
                {
                    return($"File saved successfully, file path: {filePath}");
                }
                else
                {
                    return("File not saved");
                }
            }
            catch (HttpRequestException ex)
            {
                return($"Error while receiving document: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return ($"Argument null: {ex.Message}");
            }
            catch (UriFormatException ex)
            {
                return ($"Invalid URI format: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                return ($"Directory not found: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return($"Access denied: {ex.Message}");
            }
            catch (IOException ex)
            {
                return ($"I/O error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return($"Request was canceled: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ($"Unexpected Error: {ex.Message}");
            }
        }

        [Fact]
        public async Task Test1()
        {
            string[] args = ["https://public.dhe.ibm.com/support/knowledgecenter/StoredIQ/7.6.0.21/IBMSIQAdministratorAdminGuide76021.pdf", "."];
            var resText = await func(args);
            Assert.Equal($"File saved successfully, file path: .\\IBMSIQAdministratorAdminGuide76021.pdf", resText);
        }

        [Fact]
        public async Task Test2()
        {
            string[] args = ["https://docs.pexip.com/files/v35/Pexip_Infinity_Upgrading_Quickguide_v35.a.pdf"];
            var resText = await func(args);
            Assert.Equal("Enter <URL> <path_directory>", resText);
        }

        [Fact]
        public async Task Test3()
        {
            string[] args = ["https://docs.pexip.com/files/v35/Pexip_Infinity_Upgrading_Quickguide_v35.a.pdf", ".", "https://public.dhe.ibm.com/support/knowledgecenter/StoredIQ/7.6.0.21/IBMSIQAdministratorAdminGuide76021.pdf"];
            var resText = await func(args);
            Assert.Equal("Enter <URL> <path_directory>", resText);
        }

        [Fact]
        public async Task Test4()
        {
            string[] args = ["https://docs.pexip.com", "."];
            var resText = await func(args);
            Assert.Equal("There is no file at the specified link", resText);
        }

        [Fact]
        public async Task Test5()
        {
            string[] args = ["https://public.dhe.ibm.com/support/knowledgecenter/StoredIQ/7.6.0.21/IBMSIQAdministratorAdminGuide76021.pdf", "."];
            var resText = await func(args);
            var path = resText[resText.IndexOf(".")..];
            Assert.True(File.Exists(path));
        }

        [Fact]
        public async Task Test6()
        {
            string[] args = ["https://public.dhe.ibm.com/support/knowledgecenter/StoredIQ/7.6.0.21/IBMSIQAdministratorAdminGuide76021.pdf", "."];
            var resText = await func(args);
            Assert.Equal("Error while receiving document: Этот хост неизвестен. (public.dhe.ibm.com:443)", resText);
        }
    }
}
