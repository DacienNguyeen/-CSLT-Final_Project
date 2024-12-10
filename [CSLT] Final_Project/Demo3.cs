using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PersonalFinanceApp
{
    class Program
    {
        public class Transaction
        {
            public int ID { get; set; }
            public string Flow { get; set; } // "IN" or "OUT"
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
            public DateTime Date { get; set; }
            public string Category { get; set; }
            public double Amount { get; set; }
        }

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
            ShowTypeOfRecord();
        }

        static void ShowTypeOfRecord()
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

            decimal amount = ReadDecimalInput();

            string category = GetSpendingCategory();

            string method = GetMethod();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Spending Record Added: Outflow {FormatCurrency(amount)} VND, On {category}, By {method}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddIncomeRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Income Record ===");

            decimal amount = ReadDecimalInput();

            string category = GetIncomeCategory();

            string method = GetMethod();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Income Record Added: Inflow {FormatCurrency(amount)} VND, From {category}, By {method}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddLoanRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Loan Record ===");

            decimal amount = ReadDecimalInput();

            Console.Write("Enter who you lend to: ");
            string Borrower = GetValidName();

            string method = GetMethod();

            Console.Write("Enter description (e.g: what they borrow for): ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Loan Record Added: {FormatCurrency(amount)} VND, To {Borrower}, By {method}, For {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static void AddDebitRecord()
        {
            Console.Clear();
            Console.WriteLine("=== Add Debit Record ===");

            decimal amount = ReadDecimalInput();

            Console.Write("Enter who you take loan from: ");
            string lender = GetValidName();

            string method = GetMethod();

            Console.Write("Enter description (e.g: What you used it for?): ");
            string description = Console.ReadLine();

            DateTime date = GetDateInput("Enter date (leave blank for today): ");

            Console.WriteLine($"Debit Record Added: {FormatCurrency(amount)} VND, From {lender}, By {method}, To {description}, On {date.ToShortDateString()}");
            // TODO: Store this record in a collection or file
        }

        static decimal ReadDecimalInput()
        {
            Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
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
                while (true)
                {
                    Console.Write("Enter the month and year to filter with format MM/yyyy (e.g., 12/2024): ");
                    string input = Console.ReadLine();
                    Console.WriteLine();
                    if (TryParseMonthYear(input, out month, out year))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid format. Please enter a valid month and year (MM/yyyy).");
                    Console.ResetColor();
                }

                var filteredTransactions = FilterByMonthYear(transactions, month, year);

                if (filteredTransactions.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"No transaction data in {month:D2}/{year}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Filtered Results for {month:D2}/{year}:");
                    Console.ResetColor();
                    DisplayBalancesByMethod(filteredTransactions);
                    Console.WriteLine($"Total Balance: {FormatCurrency(CalculateTotalBalance(filteredTransactions))}");

                }

                Console.WriteLine("\nDo you want to refilter?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");
            } while (GetYorN_Selection() == 1);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for using the transaction filter!");
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
            Console.WriteLine("Track your savings and spending forecasts here.");
            Console.WriteLine("TODO: Implement saving features");
        }
    }
}
