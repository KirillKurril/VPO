void func() 
{
    if (args.Length > 2)
    {
        Console.WriteLine("Too many arguments!");
        return;
    }
    else if (args.Length < 2)
    {
        Console.WriteLine("Too few arguments!");
        return;
    }
    string first_arg = args[0];
    string second_arg = args[1];
    double length;
    double width;
    try
    {
        length = Convert.ToDouble(first_arg);
        width = Convert.ToDouble(second_arg);

        if(length <= 0 || width <= 0)
        {
            Console.WriteLine("Enter correct numbers!");
            return;
        }
        double area = length * width;
        Console.WriteLine($"Area: {area:F2}");
    }
    catch
    {
        Console.WriteLine("Enter correct numbers!");
    }
}

func();
