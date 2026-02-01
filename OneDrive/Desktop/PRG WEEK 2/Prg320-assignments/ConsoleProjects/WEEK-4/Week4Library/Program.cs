using System;
using Week4Library.Model;
using Week4Library.Service;
using Week4Library.CustomExpectation;
using Week4Library.Utilities;

namespace Week4Library
{
    class Program
    {
        static void Main()
        {
            // This object controls all library actions like add, remove, search, and sort
            LibraryService service = new LibraryService();

            // This variable checks if the user wants to exit the program
            bool exit = false;

            // This loop keeps the program running until the user chooses to exit
            while (!exit)
            {
                // Show the menu options on the screen
                Utilities.Utilities.PrintMenu();

                // Ask the user to choose a menu option
                Utilities.Utilities.ColorWrite("Choose an option: ", ConsoleColor.White);
                string? choice = Console.ReadLine();

                try
                {
                    // Check the user's choice and perform the correct action
                    switch (choice)
                    {
                        case "1":
                            // Add a new book to the library
                            service.AddItem(CreateBook());
                            Utilities.Utilities.ColorWrite("Book added successfully.", ConsoleColor.Green);
                            break;

                        case "2":
                            // Add a new magazine to the library
                            service.AddItem(CreateMagazine());
                            Utilities.Utilities.ColorWrite("Magazine added successfully.", ConsoleColor.Green);
                            break;

                        case "3":
                            // Add a new newspaper to the library
                            service.AddItem(CreateNewspaper());
                            Utilities.Utilities.ColorWrite("Newspaper added successfully.", ConsoleColor.Green);
                            break;

                        case "4":
                            // Display all library items
                            service.DisplayAllItems();
                            break;

                        case "5":
                            // Remove an item by its title
                            Utilities.Utilities.ColorWrite("Enter title to remove: ", ConsoleColor.White);
                            service.RemoveItem(Console.ReadLine() ?? "");
                            break;

                        case "6":
                            // Search for an item using its title
                            Utilities.Utilities.ColorWrite("Enter title to search: ", ConsoleColor.White);
                            service.SearchByTitle(Console.ReadLine() ?? "");
                            break;

                        case "7":
                            // Search for books by author name
                            Utilities.Utilities.ColorWrite("Enter author to search (Books only): ", ConsoleColor.White);
                            service.SearchByAuthor(Console.ReadLine() ?? "");
                            break;

                        case "8":
                            // Sort all items by title
                            service.SortByTitle();
                            break;

                        case "9":
                            // Sort all items by publication year
                            service.SortByYear();
                            break;

                        case "10":
                            // Exit the program
                            exit = true;
                            Utilities.Utilities.ColorWrite("Goodbye! Program closed.", ConsoleColor.Green);
                            break;

                        default:
                            // Show error message if option is not valid
                            Utilities.Utilities.ColorWrite("Invalid option. Try again.", ConsoleColor.Red);
                            break;
                    }
                }
                catch (InvalidItemException ex)
                {
                    // Runs when the user enters incorrect or invalid input
                    Utilities.Utilities.ColorWrite("Input Error: " + ex.Message, ConsoleColor.Red);
                }
                catch (DuplicateItemException ex)
                {
                    // Runs when the user tries to add a duplicate item
                    Utilities.Utilities.ColorWrite("Duplicate Error: " + ex.Message, ConsoleColor.Red);
                }
                catch (Exception ex)
                {
                    // Runs when an unexpected error happens
                    Utilities.Utilities.ColorWrite("Unexpected Error: " + ex.Message, ConsoleColor.Red);
                }

                // Pause the program so the user can read the output
                Utilities.Utilities.Pause();
            }
        }

        // This method collects book details from the user and creates a Book object
        static Book CreateBook()
        {
            Utilities.Utilities.ColorWrite("Enter Title: ", ConsoleColor.White);
            string title = Console.ReadLine() ?? "";

            Utilities.Utilities.ColorWrite("Enter Publisher: ", ConsoleColor.White);
            string publisher = Console.ReadLine() ?? "";

            // Read a valid year value from the user
            int year = Utilities.Utilities.ReadInt("Enter Publication Year (YYYY): ", mustBePositive: true);

            Utilities.Utilities.ColorWrite("Enter Author: ", ConsoleColor.White);
            string author = Console.ReadLine() ?? "";

            // Return a new Book object with user input
            return new Book(title, publisher, year, author);
        }

        // This method collects magazine details and creates a Magazine object
        static Magazine CreateMagazine()
        {
            Utilities.Utilities.ColorWrite("Enter Title: ", ConsoleColor.White);
            string title = Console.ReadLine() ?? "";

            Utilities.Utilities.ColorWrite("Enter Publisher: ", ConsoleColor.White);
            string publisher = Console.ReadLine() ?? "";

            // Read publication year and issue number
            int year = Utilities.Utilities.ReadInt("Enter Publication Year (YYYY): ", mustBePositive: true);
            int issue = Utilities.Utilities.ReadInt("Enter Issue Number: ", mustBePositive: true);

            // Return a new Magazine object
            return new Magazine(title, publisher, year, issue);
        }

        // This method collects newspaper details and creates a Newspaper object
        static Newspaper CreateNewspaper()
        {
            Utilities.Utilities.ColorWrite("Enter Title: ", ConsoleColor.White);
            string title = Console.ReadLine() ?? "";

            Utilities.Utilities.ColorWrite("Enter Publisher: ", ConsoleColor.White);
            string publisher = Console.ReadLine() ?? "";

            // Read the publication year
            int year = Utilities.Utilities.ReadInt("Enter Publication Year (YYYY): ", mustBePositive: true);

            Utilities.Utilities.ColorWrite("Enter Editor: ", ConsoleColor.White);
            string editor = Console.ReadLine() ?? "";

            // Return a new Newspaper object
            return new Newspaper(title, publisher, year, editor);
        }
    }
}
