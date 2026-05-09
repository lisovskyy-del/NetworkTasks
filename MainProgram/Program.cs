namespace MainProgram;

class Program
{
    public static void Main(string[] args)
    {
        try
        {
            while (true)
            {
                Console.WriteLine("\nВиберіть завдання:");
                Console.WriteLine("1. Завдання 5");
                Console.WriteLine("2. Завдання 6");
                Console.WriteLine("0. Exit");
                string? input = InputHelpers.StringInput("\nВаш вибір: ");

                if (int.TryParse(input, out int userChoice))
                {
                    if (userChoice == 0)
                    {
                        Console.WriteLine("\nВихід...");
                        return;
                    }
                    else if (userChoice == 1)
                    {
                        Task5.Run();
                    }
                    else if (userChoice == 2)
                    {
                        Task6.Run();
                    }
                    else
                    {
                        Console.WriteLine("\nВведіть цифру між 0-2!");
                    }
                }
                else
                {
                    Console.WriteLine("\nВведіть цифру!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex}");
            return;
        }
    }
}