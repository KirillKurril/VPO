string func(string[] args)
{
    if(args.Length > 2 || args.Length < 2)
    {
        return "Enter <path> <extension>";
    }
    string path = args[0];
    string extension = args[1];
    if(!Directory.Exists(path))
    {
        return "Directory does not exist";
    }
    try
    {
        string[] files = Directory.GetFiles(path, $"*{extension}", SearchOption.AllDirectories);
        if(files.Length > 0)
        {
            string result_str = "";
            foreach(var file in files)
            {
                result_str += file + "\n";
                
            }
            if(result_str.EndsWith("\n"))
            {
                result_str = result_str.Remove(result_str.Length - 1);
            }
            return result_str;
        }
        else
        {
            return "Extension not found";
        }
    }
    catch (Exception ex)
    {
        return $"Error: {ex.ToString()}";
    }
}

Console.WriteLine(func(args));