using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using CsvHelper;
using static System.Net.Mime.MediaTypeNames;

namespace PersonalFinanceApp
{
    class Program2
    {
        public class Transaction
        {
            public int ID { get; set; }
            public string Source { get; set; }
            public string Flow { get; set; } // "IN" or "OUT"
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
            public DateTime Date { get; set; }
            public string Category { get; set; }
            public double Amount { get; set; }
        }

        static void Main(string[] args)
        {
            NavigationBar();
        }

        static void NavigationBar()
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
                        return; // Exit the program
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }

                if (choice != "6")
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
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
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("=== Add Record ===");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Select the type of record to add:");
                Console.ResetColor();

                Console.WriteLine("[1] Spending");
                Console.WriteLine("[2] Income");
                Console.WriteLine("[3] Loan");
                Console.WriteLine("[4] Debit");
                Console.WriteLine("[0] Return to Navigation");

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
                    case "0":
                        return; // Exit to NavigationBar without recursion
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nPress any key to return to the Add Record menu...");
                Console.ReadKey();
            }
        }

        static void AddSpendingRecord()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("=== Add Spending Record ===");
                Console.ResetColor();

                // Step 1: Read amount
                decimal amount = ReadDecimalInput();

                // Step 2: Get payment method
                string method = GetMethod();

                // Step 3: Get spending category
                string category = GetSpendingCategory();

                // Step 4: Enter a note
                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                // Step 5: Get the date (adjusted for DD/MM format and default year)
                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                // Step 6: Choose session of the day
                string session = GetSessionOfDay();

                // Step 7: Display result
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSpending Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Category: {category}");
                Console.WriteLine($"  - Note: {note}");

                // Step 8: Confirm save
                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    StoreSpendingRecord(date, session, note, category, method, amount);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Record saved successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Record not saved.");
                    Console.ResetColor();
                }

                // Step 9: Ask to add another record
                Console.WriteLine("\nDo you want to add another record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 0)
                {
                    break; // Exit to AddRecord menu
                }
            }
        }

        static void StoreSpendingRecord(DateTime date, string session, string note, string category, string method, decimal amount)
        {
            const string fileName = "Spending.csv";

            // Check if the file exists
            bool fileExists = File.Exists(fileName);

            // Open the file for appending
            using (var writer = new StreamWriter(fileName, append: true))
            {
                if (!fileExists)
                {
                    // Write header row if file is newly created
                    writer.WriteLine("ID,Date,Session,Note,Category,Method,Amount");
                }

                // Determine the next ID
                int nextId = GetNextId(fileName);

                // Write the record
                writer.WriteLine($"{nextId},{date:dd/MM/yyyy},{session},{note},{category},{method},{amount}");
            }
        }

        static int GetNextId(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return 1; // Start ID from 1 if file doesn't exist
            }

            // Read all lines from the file
            var lines = File.ReadAllLines(fileName);

            // Extract the last ID (skip the header)
            if (lines.Length <= 1)
            {
                return 1;
            }

            // Get the ID of the last record
            var lastLine = lines[^1];
            var lastId = int.Parse(lastLine.Split(',')[0]);
            return lastId + 1;
        }

        static void AddIncomeRecord()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=== Add Income Record ===");
                Console.ResetColor();

                decimal amount = ReadDecimalInput();

                string category = GetIncomeCategory();

                string method = GetMethod();

                // Step 4: Enter a note
                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                // Step 5: Get the date (adjusted for DD/MM format and default year)
                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                // Step 6: Choose session of the day
                string session = GetSessionOfDay();

                // Step 7: Display result
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nIncome Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Category: {category}");
                Console.WriteLine($"  - Note: {note}");

                // Step 8: Confirm save
                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    StoreSpendingRecord(date, session, note, category, method, amount);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Record saved successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Record not saved.");
                    Console.ResetColor();
                }

                // Step 9: Ask to add another record
                Console.WriteLine("\nDo you want to add another record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 0)
                {
                    break; // Exit the loop to return to AddRecord()
                }
            }
        }

        static void AddLoanRecord()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== Add Loan Record ===");
                Console.ResetColor();

                decimal amount = ReadDecimalInput();

                Console.Write("Enter who you lend to: ");
                string borrower = GetValidName();

                string method = GetMethod();

                // Step 4: Enter a note
                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                // Step 5: Get the date (adjusted for DD/MM format and default year)
                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                // Step 6: Choose session of the day
                string session = GetSessionOfDay();

                // Step 7: Display result
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nLoan Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Borrower: {borrower}");
                Console.WriteLine($"  - Note: {note}");

                // Step 8: Confirm save
                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    StoreSpendingRecord(date, session, note, borrower, method, amount);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Record saved successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Record not saved.");
                    Console.ResetColor();
                }

                // Step 9: Ask to add another record
                Console.WriteLine("\nDo you want to add another record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 0)
                {
                    break; // Exit the loop to return to AddRecord()
                }
            }
        }

        static void AddDebitRecord()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== Add Debit Record ===");
                Console.ResetColor();

                decimal amount = ReadDecimalInput();

                Console.Write("Enter who you borrow from: ");
                string lender = GetValidName();

                string method = GetMethod();

                // Step 4: Enter a note
                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                // Step 5: Get the date (adjusted for DD/MM format and default year)
                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                // Step 6: Choose session of the day
                string session = GetSessionOfDay();

                // Step 7: Display result
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDebit Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Borrower: {lender}");
                Console.WriteLine($"  - Note: {note}");

                // Step 8: Confirm save
                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    StoreSpendingRecord(date, session, note, lender, method, amount);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Record saved successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Record not saved.");
                    Console.ResetColor();
                }

                // Step 9: Ask to add another record
                Console.WriteLine("\nDo you want to add another record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 0)
                {
                    break; // Exit the loop to return to AddRecord()
                }
            }
        }

        static decimal ReadDecimalInput()
        {
            while (true)
            {
                Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
                string input = Console.ReadLine().Trim().ToLower();
                try
                {
                    decimal multiplier = 1;

                    // Handle suffix multipliers
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
                        amount *= multiplier;

                        // Reject values less than or equal to 0
                        if (amount <= 0)
                        {
                            throw new ArgumentOutOfRangeException("Amount must be greater than 0.");
                        }

                        return amount;
                    }

                    throw new FormatException("Invalid numeric format.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid amount. The value must be greater than 0. Please try again.");
                    Console.ResetColor();
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid amount format. Please try again (e.g., 30k, 3m, 3b, 30.000).");
                    Console.ResetColor();
                }
            }
        }

        static string GetMethod()
        {
            string method = "";
            Console.WriteLine("Spend by (IN three following types: Banking, Cash or E-Wallet)");
            Console.WriteLine("1. Banking");
            Console.WriteLine("2. Cash");
            Console.WriteLine("3. E-Wallet");

            do
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int selection))
                {
                    if (selection == 1)
                    {
                        method = "Banking";
                        break;
                    }
                    else if (selection == 2)
                    {
                        method = "Cash";
                        break;
                    }
                    else if (selection == 3)
                    {
                        method = "E-Wallet";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Please choose 1, 2, or 3.");
                    }
                }
                else
                {
                    Console.WriteLine("Make sure to enter an integer value!");
                }
            } while (method == "");
            return method;
        }

        static string GetSpendingCategory()
        {
            string category = "";
            Console.WriteLine("Enter category of spending (adjusted for typical spending habits):");
            Console.WriteLine("1. Food & Groceries"); // Essential
            Console.WriteLine("2. Housing & Utilities"); // Essential
            Console.WriteLine("3. Transportation"); // Essential for many
            Console.WriteLine("4. Healthcare"); // Important
            Console.WriteLine("5. Education"); // Investment
            Console.WriteLine("6. Personal & Family Care"); // Broad category
            Console.WriteLine("7. Social & Entertainment"); // Leisure
            Console.WriteLine("8. Savings & Investments"); // Future-oriented
            Console.WriteLine("9. Debt Payments"); // Financial responsibility

            do
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int selection))
                {
                    switch (selection)
                    {
                        case 1:
                            category = "Food & Groceries";
                            break;
                        case 2:
                            category = "Housing & Utilities";
                            break;
                        case 3:
                            category = "Transportation";
                            break;
                        case 4:
                            category = "Healthcare";
                            break;
                        case 5:
                            category = "Education";
                            break;
                        case 6:
                            category = "Personal & Family Care";
                            break;
                        case 7:
                            category = "Social & Entertainment";
                            break;
                        case 8:
                            category = "Savings & Investments";
                            break;
                        case 9:
                            category = "Debt Payments";
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please choose from 1 to 9.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Make sure to enter an integer value!");
                }
            } while (category == "");
            return category;
        }

        static string GetIncomeCategory()
        {
            string category = "";
            Console.WriteLine("Enter income category:");
            Console.WriteLine("1. Family Support");
            Console.WriteLine("2. Main Job (Full-time/Part-time)");
            Console.WriteLine("3. Freelancing/Gig Work");
            Console.WriteLine("4. Scholarship/Grant");
            Console.WriteLine("5. Investments/Interest");
            Console.WriteLine("6. Other");

            do
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int selection))
                {
                    switch (selection)
                    {
                        case 1:
                            category = "Family Support";
                            break;
                        case 2:
                            category = "Main Job";
                            break;
                        case 3:
                            category = "Freelancing/Gig Work";
                            break;
                        case 4:
                            category = "Scholarship/Grant";
                            break;
                        case 5:
                            category = "Investments/Interest";
                            break;
                        case 6:
                            category = "Other";
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please choose from 1 to 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Make sure to enter an integer value!");
                }
            } while (category == "");

            // Optionally, get more details for "Other" category:
            if (category == "Other")
            {
                Console.Write("Please specify the 'Other' income source: ");
                category = Console.ReadLine(); // Overwrite "Other" with the user's input
            }


            return category;
        }

        static string GetValidName()
        {
            string name = "";

            do
            {
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name)) // Check for empty or whitespace-only input
                {
                    Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                    Console.Write("Retry: ");
                }
                else if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$")) // Use regex for validation
                {
                    Console.WriteLine("Name contains invalid characters. Please use letters and spaces only.");
                    Console.Write("Retry: ");
                }

            } while (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[a-zA-Z\s]+$"));

            return name;
        }

        static string FormatCurrency(decimal amount)
        {
            // Format as Vietnamese currency with dot separator and add ₫ symbol
            return $"{amount:N0}".Replace(",", ".");
        }

        static string GetSessionOfDay()
        {
            Console.WriteLine("\nPlease choose a session of the day:");
            Console.WriteLine("  1. Morning (6:00-10:59)");
            Console.WriteLine("  2. Midday (11:00-12:59)");
            Console.WriteLine("  3. Afternoon (13:00-17:59)");
            Console.WriteLine("  4. Evening (18:00-23:59)");
            Console.WriteLine("  5. Night (0:00-5:59)");
            Console.WriteLine("  0. Null (no specific session)");

            while (true)
            {
                Console.Write("Your selection: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": return "Morning (6:00-10:59)";
                    case "2": return "Midday (11:00-12:59)";
                    case "3": return "Afternoon (13:00-17:59)";
                    case "4": return "Evening (18:00-23:59)";
                    case "5": return "Night (0:00-5:59)";
                    case "0": return "No specific session";
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Please choose a valid option.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static DateTime GetDateInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return DateTime.Now;
            }

            // Try parsing input as DD/MM with the current year
            if (DateTime.TryParseExact(input + $"/{DateTime.Now.Year}", "dd/MM/yyyy",
                                       null, System.Globalization.DateTimeStyles.None, out DateTime date))
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

            // Get Input Data from transactions
            List<Transaction> transactions = new List<Transaction>
    {
        new Transaction { ID = 1, Flow = "IN", Method = "Banking", Date = DateTime.Parse("1/12/2024 8:00"), Category = "Subsidy", Amount = 3000000 },
        new Transaction { ID = 2, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 8:30"), Category = "Withdrawal", Amount = 500000 },
        new Transaction { ID = 3, Flow = "IN", Method = "Cash", Date = DateTime.Parse("1/12/2024 8:30"), Category = "Withdrawal", Amount = 500000 },
        new Transaction { ID = 4, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 9:00"), Category = "Food", Amount = 30000 },
        new Transaction { ID = 5, Flow = "IN", Method = "E-Wallet", Date = DateTime.Parse("1/12/2024 10:00"), Category = "Loan", Amount = 28000 },
        new Transaction { ID = 6, Flow = "OUT", Method = "Cash", Date = DateTime.Parse("1/12/2024 11:00"), Category = "Debit", Amount = 15000 },
        new Transaction { ID = 7, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 12:00"), Category = "Food", Amount = 35000 },
        new Transaction { ID = 8, Flow = "OUT", Method = "Cash", Date = DateTime.Parse("1/12/2024 13:00"), Category = "Snack", Amount = 20000 },
        new Transaction { ID = 9, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 18:00"), Category = "Food", Amount = 35000 }
    };

            do
            {
                int month, year;

                // Menu for date selection
                Console.WriteLine("\nSelect date filter option:");
                Console.WriteLine("1. Current month and year");
                Console.WriteLine("2. Customize date");
                Console.Write("Your selection: ");
                string dateChoice = Console.ReadLine();

                if (dateChoice == "1")
                {
                    // Use current month and year
                    month = DateTime.Now.Month;
                    year = DateTime.Now.Year;
                }
                else if (dateChoice == "2")
                {
                    // Customize date
                    year = GetValidYear();
                    month = GetValidMonth();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Please select 1 or 2.");
                    Console.ResetColor();
                    continue;
                }

                // Filter transactions
                var filteredTransactions = FilterByMonthYear(transactions, month, year);

                if (filteredTransactions.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"No transaction data in {month:D2}/{year}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Using the current month: {month:D2}/{year}");
                    Console.ResetColor();

                    PerformNextAction(filteredTransactions, month, year);
                }

                Console.WriteLine("\nDo you want to refilter?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");
            } while (GetYorN_Selection() == 1);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for using the transaction filter!");
            Console.ResetColor();
        }


        static int GetValidYear()
        {
            while (true)
            {
                Console.Write("Enter the year (e.g., 2024): ");
                if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
                {
                    return year;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a valid year (e.g., 2024).");
                Console.ResetColor();
            }
        }

        static int GetValidMonth()
        {
            while (true)
            {
                Console.Write("Enter the month (1-12): ");
                if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
                {
                    return month;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a valid month (1-12).");
                Console.ResetColor();
            }
        }

        static void PerformNextAction(List<Transaction> filteredTransactions, int month, int year)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== Budget ===");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Using the current month: {month:D2}/{year}");
                Console.ResetColor();

                Console.WriteLine("\nWhat would you like to do next?");
                Console.WriteLine("1. View your budget (balances by method)");
                Console.WriteLine("2. View your expenditure by category");
                Console.WriteLine("3. View your income by category");
                Console.WriteLine("0. Exit this menu");
                Console.Write("Your selection: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("=== Budget (Balances by Method) ===");
                            DisplayBalancesByMethod(filteredTransactions);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Total Balance: {FormatCurrency(CalculateTotalBalance(filteredTransactions))}");
                            Console.ResetColor();
                            break;

                        case 2:
                            Console.Clear();
                            Console.WriteLine("=== Expenditure by Category ===");
                            DisplayOutFlowByCategory(filteredTransactions);
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("=== Income by Category ===");
                            DisplayInFlowByCategory(filteredTransactions);
                            break;

                        case 0:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Exiting the current menu...");
                            Console.ResetColor();
                            return;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            Console.ResetColor();
                            break;
                    }

                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the options.");
                    Console.ResetColor();
                }
            }
        }

        static void DisplayOutFlowByCategory(List<Transaction> transactions)
        {
            Console.WriteLine("\nOutflow by Category:");
            var expenditure = transactions
                .Where(t => t.Flow == "OUT")
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

            double totalExpenditure = 0;

            foreach (var item in expenditure)
            {
                Console.WriteLine($"  {item.Category}: {FormatCurrency(item.Total)}");
                totalExpenditure += item.Total;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Expenditure: {FormatCurrency(totalExpenditure)}");
            Console.ResetColor();
        }

        static void DisplayInFlowByCategory(List<Transaction> transactions)
        {
            Console.WriteLine("\nInflow by Category:");
            var income = transactions
                .Where(t => t.Flow == "IN")
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

            double totalIncome = 0;

            foreach (var item in income)
            {
                Console.WriteLine($"  {item.Category}: {FormatCurrency(item.Total)}");
                totalIncome += item.Total;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Income: {FormatCurrency(totalIncome)}");
            Console.ResetColor();
        }

        static int GetYorN_Selection()
        {
            while (true)
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int selection))
                {
                    if (selection == 1 || selection == 0)
                        return selection;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection. Please choose 0 or 1.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Make sure to enter an integer value!");
                    Console.ResetColor();
                }
                Console.Clear();
            }
        }
        static bool TryParseMonthYear(string input, out int month, out int year)
        {
            month = 0;
            year = 0;
            if (DateTime.TryParseExact(input, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                month = result.Month;
                year = result.Year;
                return true;
            }
            return false;
        }

        static List<Transaction> FilterByMonthYear(List<Transaction> transactions, int month, int year)
        {
            return transactions.Where(t => t.Date.Month == month && t.Date.Year == year).ToList();
        }

        static void DisplayBalancesByMethod(List<Transaction> transactions)
        {
            var methods = transactions.Select(t => t.Method).Distinct();
            foreach (var method in methods)
            {
                double inflow = transactions.Where(t => t.Method == method && t.Flow == "IN").Sum(t => t.Amount);
                double outflow = transactions.Where(t => t.Method == method && t.Flow == "OUT").Sum(t => t.Amount);
                Console.WriteLine($"{method} Balance: {FormatCurrency(inflow - outflow)}");
            }
        }

        static double CalculateTotalBalance(List<Transaction> transactions)
        {
            return transactions.Where(t => t.Flow == "IN").Sum(t => t.Amount) -
                   transactions.Where(t => t.Flow == "OUT").Sum(t => t.Amount);
        }
        static string FormatCurrency(double amount)
        {
            return $"{amount:N0}".Replace(",", ".") + " vnd";
        }

        static void DisplayTransactions(List<Transaction> transactions)
        {
            foreach (var t in transactions)
            {
                Console.WriteLine($"{t.ID} | {t.Flow} | {t.Method} | {t.Date} | {t.Category} | {FormatCurrency(t.Amount)}");
            }
        }
        static void ShowSaving()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Saving ===");
            Console.ResetColor();
            Console.WriteLine("Please choose an action:");
            Console.WriteLine("1. Set a daily spending constraint");
            Console.WriteLine("2. Set up a financial plan (scenario)");
            Console.WriteLine("3. Use spending forecast function");
            Console.WriteLine("4. Use spending suggestions function");
            Console.WriteLine("0. Return to Main Menu");

            int choice = GetSavingMenuSelection();

            // Stub for next steps; this can be expanded later to call specific functions
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Set a daily spending constraint - functionality coming soon.");
                    break;
                case 2:
                    Console.WriteLine("Set up a financial plan (scenario) - functionality coming soon.");
                    break;
                case 3:
                    Console.WriteLine("Use spending forecast function - functionality coming soon.");
                    break;
                case 4:
                    Console.WriteLine("Use spending suggestions function - functionality coming soon.");
                    break;
                case 0:
                    Console.WriteLine("Returning to Main Menu...");
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }
        static void SetDailySpendingConstraint(double totalIncome)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Set a Daily Spending Constraint ===");
            Console.ResetColor();

            // Default daily spending limit: 55% of income
            double defaultLimit = totalIncome * 0.55 / 30; // Assuming 30 days in a month
            double spendingLimit = defaultLimit;

            // Step 1: Ask user to input spending limit
            while (true)
            {
                Console.Write($"Enter the daily spending limit (default: {FormatCurrency(defaultLimit)}) or leave blank: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    spendingLimit = defaultLimit;
                    Console.WriteLine($"Daily spending limit set to default: {FormatCurrency(spendingLimit)}");
                    break;
                }
                else if (TryParseAmount(input, out double limit))
                {
                    spendingLimit = limit;
                    Console.WriteLine($"Daily spending limit set to: {FormatCurrency(spendingLimit)}");
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number or leave blank.");
                    Console.ResetColor();
                }
            }

            // Step 2: Ask user for the number of reminders
            int defaultReminderCount = 3;
            int reminderCount = defaultReminderCount;

            while (true)
            {
                Console.Write($"Enter the number of reminders for overspending (default: {defaultReminderCount}) or leave blank: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    reminderCount = defaultReminderCount;
                    Console.WriteLine($"Reminder count set to default: {defaultReminderCount}");
                    break;
                }
                else if (int.TryParse(input, out int count) && count > 0)
                {
                    reminderCount = count;
                    Console.WriteLine($"Reminder count set to: {count}");
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid positive integer or leave blank.");
                    Console.ResetColor();
                }
            }

            // Final confirmation
            Console.WriteLine("\nSummary:");
            Console.WriteLine($"  Daily Spending Limit: {FormatCurrency(spendingLimit)}");
            Console.WriteLine($"  Reminder Count: {reminderCount}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Your spending constraint settings have been saved!");
            Console.ResetColor();
        }
        static bool TryParseAmount(string input, out double amount)
        {
            input = input.Trim().ToLower();
            if (input.EndsWith("k")) input = input.Replace("k", "000");
            else if (input.EndsWith("m")) input = input.Replace("m", "000000");
            else if (input.EndsWith("b")) input = input.Replace("b", "000000000");
            input = input.Replace(",", "").Replace("₫", "").Trim();

            return double.TryParse(input, out amount);
        }
        static int GetSavingMenuSelection()
        {
            while (true)
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection >= 0 && selection <= 4)
                {
                    return selection;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice. Please enter a number between 0 and 4.");
                Console.ResetColor();
            }
        }
    }
}
