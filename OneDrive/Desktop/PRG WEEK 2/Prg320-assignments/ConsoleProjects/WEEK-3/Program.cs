using System;
using WEEK3.CustomException;
using WEEK3.Model;
using WEEK3.Service;

namespace WEEK3
{
    /// -----------------------------------------------------------------
    /// LIBRARY MANAGEMENT SYSTEM (Menu Driven)
    /// -----------------------------------------------------------------
    /// • Starts the program and shows a menu in a loop
    /// • Allows user to add Books and Magazines
    /// • Displays all items stored in the library
    /// • Uses try/catch/finally to handle errors safely
    /// -----------------------------------------------------------------
    internal class Program
    {
        private static void Main()
        {
            // • LibraryService is the main controller for storing and managing items
            // • Program calls this service to add/display items
            var libraryService = new LibraryService();

            // • exit flag controls when the menu loop should stop
            bool exit = false;

            // -------------------------------------------------------------
            // MAIN MENU LOOP
            // -------------------------------------------------------------
            // • Runs until user selects Exit (option 4)
            // • Reads menu choice and executes selected operation
            // • Prevents the program from stopping unexpectedly
            // -------------------------------------------------------------
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n=================================================");
                Console.WriteLine("             LIBRARY MANAGEMENT SYSTEM            ");
                Console.WriteLine("=================================================");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Add a Magazine");
                Console.WriteLine("3. View All Library Items");
                Console.WriteLine("4. Exit Program");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Select your option: ");
                Console.ResetColor();

                // • Read user choice safely (null-safe)
                // • If user presses Enter without typing, choice becomes empty string
                string choice = Console.ReadLine() ?? string.Empty;

                // -------------------------------------------------------------
                // TRY–CATCH–FINALLY
                // -------------------------------------------------------------
                // • try: executes the selected option
                // • catch: handles validation/duplicate/system errors
                // • finally: always runs to show a return-to-menu message
                // -------------------------------------------------------------
                try
                {
                    switch (choice)
                    {
                        case "1":
                            // • Collect user input for a Book
                            // • Create Book object using the model constructor
                            // • Add to library collection through service
                            var book = CreateBookFromInput();
                            libraryService.AddItem(book);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("✅ Book has been added successfully.");
                            Console.ResetColor();
                            break;

                        case "2":
                            // • Collect user input for a Magazine
                            // • Create Magazine object using the model constructor
                            // • Add to library collection through service
                            var magazine = CreateMagazineFromInput();
                            libraryService.AddItem(magazine);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("✅ Magazine has been added successfully.");
                            Console.ResetColor();
                            break;

                        case "3":
                            // • Display all items currently available in the library
                            // • Service will handle formatting/printing
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n--- Library Items ---");
                            Console.ResetColor();

                            libraryService.DisplayAllItems();
                            break;

                        case "4":
                            // • Stop the menu loop and exit program
                            exit = true;

                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("👋 Program closed. Thank you!");
                            Console.ResetColor();
                            break;

                        default:
                            // • Handles invalid menu inputs like 0, 5, abc, empty, etc.
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("⚠ Invalid selection. Please choose between 1 and 4.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (InvalidItemDataException ex)
                {
                    // • Handles validation issues from Book/Magazine constructors
                    // • Examples: empty title, invalid year, invalid issue number
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Validation Error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (DuplicateEntryException ex)
                {
                    // • Handles duplicate item attempts
                    // • Prevents storing same item more than once
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Duplicate Entry Error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    // • Handles unexpected errors safely
                    // • Ensures program continues running instead of crashing
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"❌ System Error: {ex.Message}");
                    Console.ResetColor();
                }
                finally
                {
                    // • Always runs after each menu operation
                    // • Gives user clear feedback that program is still active
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nReturning to menu...\n");
                    Console.ResetColor();
                }
            }
        }

        // -------------------------------------------------------------
        // CREATE BOOK FROM USER INPUT
        // -------------------------------------------------------------
        // • Asks user for Book details (title, publisher, year, author)
        // • Uses helper method to validate year
        // • Returns a Book object
        // -------------------------------------------------------------
        private static Book CreateBookFromInput()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter book name: ");
            Console.ResetColor();
            string title = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter publisher name: ");
            Console.ResetColor();
            string publisher = Console.ReadLine() ?? string.Empty;

            int year = ReadPublicationYear("Enter year of publication (YYYY): ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter author name: ");
            Console.ResetColor();
            string author = Console.ReadLine() ?? string.Empty;

            return new Book(title, publisher, year, author);
        }

        // -------------------------------------------------------------
        // CREATE MAGAZINE FROM USER INPUT
        // -------------------------------------------------------------
        // • Asks user for Magazine details (title, publisher, year, issue)
        // • Validates year and ensures issue number is positive
        // • Returns a Magazine object
        // -------------------------------------------------------------
        private static Magazine CreateMagazineFromInput()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter magazine title: ");
            Console.ResetColor();
            string title = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter publisher name: ");
            Console.ResetColor();
            string publisher = Console.ReadLine() ?? string.Empty;

            int year = ReadPublicationYear("Enter year of publication (YYYY): ");

            int issueNumber = ReadInt("Enter magazine issue number: ", mustBePositive: true);

            return new Magazine(title, publisher, year, issueNumber);
        }

        // -------------------------------------------------------------
        // YEAR INPUT VALIDATION
        // -------------------------------------------------------------
        // • Keeps asking until user enters a valid year
        // • Only accepts numbers
        // • Only accepts a 4-digit range (1000–9999)
        // -------------------------------------------------------------
        private static int ReadPublicationYear(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(prompt);
                Console.ResetColor();

                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int year))
                {
                    if (year >= 1000 && year <= 9999)
                        return year;

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("⚠ Please enter a valid 4-digit year (e.g., 2024).");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("⚠ Invalid input. Numbers only.");
                    Console.ResetColor();
                }
            }
        }

        // -------------------------------------------------------------
        // INTEGER INPUT VALIDATION (Generic)
        // -------------------------------------------------------------
        // • Reads integer input safely
        // • Optionally checks that value must be positive (> 0)
        // • Loops until user enters a valid integer
        // -------------------------------------------------------------
        private static int ReadInt(string prompt, bool mustBePositive = false)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(prompt);
                Console.ResetColor();

                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int value))
                {
                    if (!mustBePositive || value > 0)
                        return value;

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("⚠ Number must be greater than zero.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("⚠ Invalid input. Please enter a number.");
                    Console.ResetColor();
                }
            }
        }
    }
}
