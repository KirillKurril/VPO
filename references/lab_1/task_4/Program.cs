string htmlFilePath = "C:\\University\\course_3\\sem_1\\VPO\\labs\\lab_1\\lab_1\\task_4\\createdFile.html";
using (StreamWriter streamWriter = new StreamWriter(htmlFilePath))
{
    streamWriter.WriteLine("<!DOCTYPE html>");
    streamWriter.WriteLine("<html>");
    streamWriter.WriteLine("<head>");
    streamWriter.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
    streamWriter.WriteLine("<title>Index</title>");
    streamWriter.WriteLine("</head>");
    streamWriter.WriteLine("<body style=\"margin: 0px; padding: 0px 0px 0px 0px\">");
    streamWriter.WriteLine("<table style=\"width: 100%; border-collapse: collapse;\">");
    for (int i = 255; i > -1; i--)
    {
        string color = $"rgb({i}, {i}, {i})";
        streamWriter.WriteLine($"<tr style=\"background-color: {color}; height: 3px\"><td></td></tr>");
    }
    streamWriter.WriteLine("</table>");
    streamWriter.WriteLine("</body>");
    streamWriter.WriteLine("</html>");
}
Console.WriteLine($"HTML-file created. Path: {htmlFilePath}");
