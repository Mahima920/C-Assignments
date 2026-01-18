using System;

// Simple Banking System
public static class Banking
{
    // This is the main method that runs the entire banking system.
    // It first verifies the user using a PIN and, if successful,
    // continuously displays a menu allowing the user to perform
    // banking operations until they choose to exit.
    public static void RunSimpleBankSystem()
    {
        const int correctPin = 4444; // Hardcoded PIN used only for demonstration
        const int maxAttempts = 3;   // Maximum number of login attempts allowed

        // Calls the authentication method to check if the user
        // enters the correct PIN within the allowed attempts.
        if (!AuthenticateUser(correctPin, maxAttempts))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("System access denied due to multiple invalid PIN entries.");
            Console.ResetColor();
            return;
        }

        bool exitBank = false;   // Controls when the user exits the banking system
        decimal balance = 0m;    // Stores the user's account balance, initially zero

        // This loop keeps the banking menu running
        // until the user selects the exit option.
        while (!exitBank)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n==============================");
            Console.WriteLine("       SIMPLE BANK SYSTEM      ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Deposit Money");
            Console.WriteLine("2. Withdraw Money");
            Console.WriteLine("3. Check Balance");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            Console.ResetColor();

            string? choice = Console.ReadLine();

            // Switch statement determines which banking
            // operation to perform based on user input.
            switch (choice)
            {
                case "1":
                    balance = Deposit(balance);
                    break;

                case "2":
                    balance = Withdraw(balance);
                    break;

                case "3":
                    ShowBalance(balance);
                    break;

                case "4":
                    exitBank = true;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Thank you for using the Simple Banking System.");
                    Console.ResetColor();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Invalid selection. Please choose a valid menu option.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    // This method handles user authentication.
    // The user is prompted to enter a PIN and is given
    // a limited number of attempts to enter the correct one.
    // Returns true if authentication is successful.
    private static bool AuthenticateUser(int correctPin, int maxAttempts)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            Console.Write("Enter your PIN: ");
            string? input = Console.ReadLine();

            // Checks whether the input is numeric and matches the correct PIN
            if (int.TryParse(input, out int pin) && pin == correctPin)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Authentication successful. Access granted.");
                Console.ResetColor();
                return true;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Incorrect PIN. Attempt {attempt} of {maxAttempts}.");
            Console.ResetColor();
        }

        return false;
    }

    // This method allows the user to deposit money.
    // The entered amount is added to the existing balance
    // and the updated balance is displayed to the user.
    private static decimal Deposit(decimal balance)
    {
        Console.Write("Enter amount to deposit: ");
        decimal amount = decimal.Parse(Console.ReadLine());

        balance += amount;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Deposit successful. Amount deposited: Nrs {amount}");
        Console.WriteLine($"Updated account balance: Nrs {balance}");
        Console.ResetColor();

        return balance;
    }

    // This method processes a withdrawal request.
    // It checks whether the user has sufficient funds
    // before allowing the withdrawal to proceed.
    private static decimal Withdraw(decimal balance)
    {
        Console.Write("Enter amount to withdraw: ");
        decimal amount = decimal.Parse(Console.ReadLine());

        // Prevents the user from withdrawing more than the available balance
        if (amount > balance)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Withdrawal failed. Insufficient balance.");
            Console.ResetColor();
            return balance;
        }

        balance -= amount;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Withdrawal successful. Amount withdrawn: Nrs {amount}");
        Console.WriteLine($"Remaining balance: Nrs {balance}");
        Console.ResetColor();

        return balance;
    }

    // This method simply displays the current balance
    // so the user can view their available funds.
    private static void ShowBalance(decimal balance)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"Your current balance is: Nrs {balance}");
        Console.ResetColor();
    }
}
