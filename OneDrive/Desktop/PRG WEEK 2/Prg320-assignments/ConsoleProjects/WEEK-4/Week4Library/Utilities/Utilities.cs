using System;

namespace Week4Library.Utilities
{
    public static class Utilities
    {
        // Shows the main menu in pink color
        public static void PrintMenu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("==================================");
            Console.WriteLine("      Library Management System   ");
            Console.WriteLine("==================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Add Magazine");
            Console.WriteLine("3. Add Newspaper");
            Console.WriteLine("4. Display All Items");
            Console.WriteLine("5. Remove Item");
            Console.WriteLine("6. Search by Title");
            Console.WriteLine("7. Search by Author");
            Console.WriteLine("8. Sort by Title");
            Console.WriteLine("9. Sort by Year");
            Console.WriteLine("10. Exit");
            Console.ResetColor();

            Console.WriteLine();
        }

        // Prints text on the screen using pink color
        public static void ColorWrite(string message, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(message);
            Console.ResetColor();
        }

        // Stops the screen until the user presses ENTER
        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ResetColor();
            Console.ReadLine();
        }

        // Gets a number from the user and checks if it is valid
        public static int ReadInt(string prompt, bool mustBePositive)
        {
            while (true)
            {
                ColorWrite(prompt, ConsoleColor.Magenta);
                string? input = Console.ReadLine();

                // Check if the input is a number
                if (int.TryParse(input, out int value))
                {
                    // Check if the number must be positive
                    if (mustBePositive && value <= 0)
                    {
                        ColorWrite("Please enter a positive number.\n", ConsoleColor.Magenta);
                        continue;
                    }
                    return value;
                }

                // Shows error if input is not a number
                ColorWrite("Please enter a valid number.\n", ConsoleColor.Magenta);
            }
        }
    }
}
