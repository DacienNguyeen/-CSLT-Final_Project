using System;

namespace PersonalFinanceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black; // Set background
                Console.ForegroundColor = ConsoleColor.Cyan; // Set primary text color
                Console.WriteLine(new string('=', 40));
                Console.WriteLine("       Personal Finance App");
                Console.WriteLine(new string('=', 40));
                Console.ResetColor();

                // Menu options with colors
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[1] Home");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[2] Transaction");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[3] Add Record");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[4] Budget");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[5] Saving");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[6] Exit");
                Console.ResetColor();

                // Prompt
                Console.WriteLine(new string('-', 40));
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                // Menu handling
                switch (choice)
                {
                    case "1":
                        ShowHome();
                        break;
                    case "2":
                        ShowTransactions();
                        break;
                    case "3":
                        AddRecord();
                        break;
                    case "4":
                        ShowBudget();
                        break;
                    case "5":
                        ShowSaving();
                        break;
                    case "6":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void ShowHome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Home ===");
            Console.ResetColor();
            Console.WriteLine("Gamify your expense tracking by maintaining a streak.");
            Console.WriteLine("TODO: Add streak tracking logic using System.DateTime");
        }

        static void ShowTransactions()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== Transactions ===");
            Console.ResetColor();
            Console.WriteLine("View all your transactions here.");
            Console.WriteLine("TODO: Display transactions from data storage");
        }

        static void AddRecord()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=== Add Record ===");
            Console.ResetColor();

            Console.WriteLine("Select the type of record to add:");
            Console.WriteLine("[1] Spending");
            Console.WriteLine("[2] Income");
            Console.WriteLine("[3] Loan");
            Console.WriteLine("[4] Debit");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddSpendingRecord();
                    break;
                case "2":
                    AddIncomeRecord();
                    break;
                case "3":
                    AddLoanRecord();
                    break;
                case "4":
                    AddDebitRecord();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Returning to the main menu.");
                    Console.ResetColor();
                    break;
            }
        }

        static void AddSpendingRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Spending Record ===");

            Console.Write("Enter amount (e.g., 30k, 3m, 3b, 30.000): ");
            decimal amount = ReadDecimalInput();

            Console.Write("Enter category (e.g., Food, Shopping, Housing): ");
            string category = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Spending Record Added: {FormatCurrency(amount)} VND, On {category}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddIncomeRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Income Record ===");

            Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
            decimal amount = ReadDecimalInput();

            Console.Write("Enter category (e.g., Main Job, Part-Time Job, Savings): ");
            string category = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Income Record Added: {FormatCurrency(amount)} VND, From {category}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddLoanRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Loan Record ===");

            Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
            decimal amount = ReadDecimalInput();

            Console.Write("Enter who you lend to: ");
            string Borrower = Console.ReadLine();

            Console.Write("Enter description (e.g: what they borrow for): ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Loan Record Added: {FormatCurrency(amount)} VND, To {Borrower}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddDebitRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Debit Record ===");

            Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
            decimal amount = ReadDecimalInput();

            Console.Write("Enter who you take loan from (e.g: bank's name, a person's name): ");
            string lender = Console.ReadLine();

            Console.Write("Enter description (e.g: What you used it for?): ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Debit Record Added: {FormatCurrency(amount)} VND, From {lender}, To {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static decimal ReadDecimalInput()
        {
            while (true)
            {
                string input = Console.ReadLine().Trim().ToLower();
                try
                {
                    decimal multiplier = 1;
                    if (input.EndsWith("k")) // Handle 'k' for thousands
                    {
                        input = input.Substring(0, input.Length - 1);
                        multiplier = 1000;
                    }
                    else if (input.EndsWith("m")) // Handle 'm' for millions
                    {
                        input = input.Substring(0, input.Length - 1);
                        multiplier = 1_000_000;
                    }
                    else if (input.EndsWith("b")) // Handle 'b' for billions
                    {
                        input = input.Substring(0, input.Length - 1);
                        multiplier = 1_000_000_000;
                    }

                    // Remove non-numeric characters (e.g., "vnd", ",", ".")
                    string sanitizedInput = input.Replace(".", "").Replace(",", "").Replace("vnd", "").Trim();

                    if (decimal.TryParse(sanitizedInput, out decimal amount))
                    {
                        return amount * multiplier;
                    }

                    throw new FormatException("Invalid numeric format.");
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid amount format. Please try again (e.g., 30k, 3m, 3b, 30.000).");
                    Console.ResetColor();
                }
            }
        }

        static string FormatCurrency(decimal amount)
        {
            // Format as Vietnamese currency with dot separator and add ₫ symbol
            return $"{amount:N0}".Replace(",", ".");
        }

        static DateTime GetDateInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                return DateTime.Now;
            }

            if (DateTime.TryParse(input, out DateTime date))
            {
                return date;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date. Using today's date instead.");
            Console.ResetColor();
            return DateTime.Now;
        }

        static void ShowBudget()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Budget ===");
            Console.ResetColor();
            Console.WriteLine("Manage your budget here.");
            Console.WriteLine("TODO: Implement budget tracking and analytics");
        }

        static void ShowSaving()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Saving ===");
            Console.ResetColor();
            Console.WriteLine("Track your savings and spending forecasts here.");
            Console.WriteLine("TODO: Implement saving features");
        }
    }
}
