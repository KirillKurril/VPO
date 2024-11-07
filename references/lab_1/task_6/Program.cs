async Task func()
{
    if(args.Length < 2 || args.Length > 2)
    {
        Console.WriteLine("Enter <URL> <path_directory>");
        return;
    }
    string url = args[0];
    string path = args[1];

    if(!Directory.Exists(path))
    {
        Console.WriteLine("Directory does not exist");
        return;
    }
    if(!Uri.IsWellFormedUriString(url, UriKind.Absolute)) 
    {
        Console.WriteLine("URL is not correct");
        return;
    }
    var uri = new Uri(url);
    string absPath = uri.AbsolutePath;
    if (string.IsNullOrEmpty(Path.GetExtension(absPath)))
    {
        Console.WriteLine ("There is no file at the specified link");
        return;
    }
    string fileName = Path.GetFileName(new Uri(url).AbsolutePath);
    string filePath = Path.Combine(path, fileName);
    try
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string htmlContent = await response.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(filePath, htmlContent);

        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await response.Content.CopyToAsync(fs);
        }
        if(File.Exists(filePath))
        {
            Console.WriteLine($"File saved successfully. File path: {filePath}");
        }
        else
        {
            Console.WriteLine("File not saved");
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Error while receiving document: {ex.Message}");
    }
    catch (ArgumentNullException ex)
    {
        Console.WriteLine($"Argument null: {ex.Message}");
    }
    catch (UriFormatException ex)
    {
        Console.WriteLine($"Invalid URI format: {ex.Message}");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine($"Directory not found: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"Access denied: {ex.Message}");
    }
    catch (IOException ex)
    {
        Console.WriteLine($"I/O error: {ex.Message}");
    }
    catch (TaskCanceledException ex)
    {
        Console.WriteLine($"Request was canceled: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected Error: {ex.Message}");
    }
}

await func();
