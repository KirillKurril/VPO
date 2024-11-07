
string func()
{
    Random random = new Random();
    var number = random.Next(5, 51);
    var text = $"Hello, world!\nAndhiagain!\n{new string('!', number)}";
    return text;
}

Console.WriteLine(func());





