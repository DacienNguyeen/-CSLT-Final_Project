using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Spectre.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Figgle;
namespace PersonalFinanceApp
{
    class TestHome
    {
        static double dailyBudgetConstraint = 100000; // Default daily budget constraint
        public class Transaction
        {
            public int ID { get; set; }
            public string Source { get; set; }
            public string Flow { get; set; } // "IN" or "OUT"
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"

            public string Session { get; set; }
            public DateTime Date { get; set; }

            public string Category { get; set; }
            public double Amount { get; set; }
        }
        public class Spending
        {
            public int ID { get; set; }
            public string Session { get; set; }
            public DateTime Date { get; set; }
            public double Amount { get; set; }
            public string Method { get; set; }
            public string Category { get; set; }
            public string Note { get; set; }
        }

        public class Income
        {
            public int ID { get; set; }
            public string Session { get; set; }
            public DateTime Date { get; set; }
            public double Amount { get; set; }
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
            public string Category { get; set; }
            public string Note { get; set; }
        }

        public class Loan
        {
            public int ID { get; set; }
            public string Session { get; set; }
            public DateTime Date { get; set; }
            public double Amount { get; set; }
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
            public string Borrower { get; set; }
            public string Note { get; set; }
        }
        public class Debit
        {
            public int ID { get; set; }
            public string Session { get; set; }
            public DateTime Date { get; set; }
            public double Amount { get; set; }
            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
            public string Lender { get; set; }
            public string Note { get; set; }

        }

        static void Main34(string[] args)
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

                string choice = Console.ReadLine(); // Trim spaces to ensure clean input

                // Menu handling
                switch (choice)
                {
                    case "1":
                        ShowHome();
                        break;
                    case "2":
                        ShowTransactions(); // Stay in ShowTransactions loop
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
            }
        }

        static void ShowHome()
        {
            string gamefilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gameprogress.csv");
            Gameprogress progress = LoadLatestGameProgress(gamefilepath);
            int reminderThreshold = 5;  
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== Virtual Garden Menu ===");
                Console.ResetColor();
                Console.WriteLine("1. View Garden Status");
                Console.WriteLine("2. Update Daily Constraint");
                Console.WriteLine("3. Change the Reminder Threshold");
                Console.WriteLine("4. Exit to Main Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                double exp = 0;
                string healthStatus = string.Empty;
                string growthStage = string.Empty;
                switch (choice)
                {
                    case "1":
                        if (dailyBudgetConstraint <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Daily budget constraint is not set. Please go back to the menu and set it first.");
                            Console.ResetColor();
                            Console.WriteLine("\nPress any key to return to the menu...");
                            Console.ReadKey();
                        }
                        else
                        {
                            progress = UpdateGameProgress(gamefilepath, 1.0, 0); // Example: add 1.0 EXP, no health change
                            DisplayGardenStatus();
                            DisplayTreeStatus(progress); // Display updated garden status

                            // Reminder system: Check budget exceedances
                            int exceedCount = CheckBudgetExceedances();
                            if (exceedCount >= reminderThreshold)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Reminder: You have exceeded your daily budget {exceedCount} times this period. Please adjust your spending!");
                                Console.ResetColor();
                            }
                        }
                        break;
                    case "2":
                        UpdateDailyBudgetConstraint();
                        SaveGameFile(gamefilepath, progress); // Save updated progress
                        Console.WriteLine("Daily constraint updated. Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "3":
                        reminderThreshold = GetReminderThreshold();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static int CheckBudgetExceedances()
        {
            // Placeholder implementation: Replace with actual tracking logic
            // Example: Count the number of days in a given period where spending > dailyBudgetConstraint
            var transactions = LoadTransactionsFromFiles();
            var exceedances = transactions
                .Where(t => t.Date >= DateTime.Now.AddDays(-7) && t.Flow == "OUT") // Example: last 7 days
                .GroupBy(t => t.Date.Date)
                .Count(g => g.Sum(t => t.Amount) > dailyBudgetConstraint);

            return exceedances;
        }

        static int GetReminderThreshold()
        {
            int defaultThreshold = 5;

            // Ask the user if they want to change the reminder threshold
            Console.Clear();
            Console.Write($"Current reminder threshold is {defaultThreshold}. Would you like to change it? (y/n): ");
            string userInput = Console.ReadLine().Trim().ToLower();

            if (userInput == "y" || userInput == "yes")
            {
                // Allow the user to input a new threshold
                Console.Write($"Enter the number of exceedances to trigger a reminder (default: {defaultThreshold}): ");

                if (int.TryParse(Console.ReadLine(), out int threshold) && threshold > 0)
                {
                    return threshold;
                }
                else
                {
                    Console.WriteLine("Invalid input. Using default threshold of 5.");
                }
            }
            else
            {
                Console.WriteLine("Using default threshold of 5.");
            }

            return defaultThreshold;
        }
        static void DisplayGardenStatus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Virtual Garden ===");
            Console.ResetColor();

            // Simulate the plant growth stage
            string[] growthStages = { "Seed Stage", "Sapling Stage", "Mature Stage" };
            string[] healthStates = { "Healthy", "Moderate", "Ill" };
            double[] expThresholds = { 5, 10, 15 }; // EXP thresholds for stages

            // Load data from CSV files
            var transactions = LoadTransactionsFromFiles();

            double dailySpending = transactions.Where(t => t.Date.Date == DateTime.Now.Date && t.Flow == "OUT").Sum(t => t.Amount);
            double income = transactions.Where(t => t.Date.Month == DateTime.Now.Month && t.Date.Year == DateTime.Now.Year && t.Flow == "IN" && t.Source == "Income").Sum(t => t.Amount);
            double monthlySpending = transactions.Where(t => t.Date.Month == DateTime.Now.Month && t.Flow == "OUT").Sum(t => t.Amount);
            double loan = transactions.Where(t => t.Date.Month == DateTime.Now.Month && t.Date.Year == DateTime.Now.Year && t.Source == "Loan" && t.Flow == "OUT").Sum(t => t.Amount);
            double debit = transactions.Where(t => t.Date.Month == DateTime.Now.Month && t.Date.Year == DateTime.Now.Year && t.Source == "Debit" && t.Flow == "IN").Sum(t => t.Amount);

            double balance = income - monthlySpending;
            double debtBalance = loan - debit;
            double savings = balance + debtBalance;
            double savingsPercentage = (savings / income) * 100;

            // Daily EXP calculation
            double exp = 0;
            if (dailySpending <= dailyBudgetConstraint)
            {
                exp = 1.0;
            }
            else if (dailySpending <= dailyBudgetConstraint * 1.2)
            {
                exp = 0.5;
            }

            // Monthly Health calculation
            string healthStatus;
            if (savingsPercentage > 10)
            {
                healthStatus = healthStates[0]; // Healthy
                exp += 1.0; // Level up bonus
            }
            else if (savingsPercentage >= 0)
            {
                healthStatus = healthStates[1]; // Moderate
                exp += 0.5; // Limited EXP
            }
            else
            {
                healthStatus = healthStates[2]; // Ill
            }

            // Determine growth stage based on EXP
            string growthStage = growthStages[0]; // Default to Seed Stage
            if (exp > expThresholds[1])
            {
                growthStage = growthStages[2]; // Mature Stage
            }
            else if (exp > expThresholds[0])
            {
                growthStage = growthStages[1]; // Sapling Stage
            }

            // Display results
            Console.Clear();
            Console.WriteLine($"Growth Stage: {growthStage}");
            // Draw the plant growth stage
            DrawPlantGrowth(growthStage);
            Console.WriteLine($"Health Status: {healthStatus}");
            // Display a health bar for health status
            DrawHealthBar(healthStatus);
            Console.WriteLine($"Daily Spending: {FormatCurrency(dailySpending)} / {FormatCurrency(dailyBudgetConstraint)}");

            // Display warning if daily budget exceeded
            if (dailySpending > dailyBudgetConstraint)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Warning: You have exceeded your daily budget!");
                Console.ResetColor();
            }
            else Console.WriteLine("Good job! Keep going :33");

            Console.WriteLine($"Savings: {FormatCurrency(savings)} ({savingsPercentage:F2}% of income)");
            Console.WriteLine($"Total EXP: {exp:F1}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Keep tracking your spending to help your plant grow!");
            Console.ResetColor();
        }

        static void UpdateDailyBudgetConstraint()
        {
            Console.Clear();
            Console.WriteLine("=== Update Daily Budget Constraint ===");
            while (true)
            {
                Console.Write("Enter your new daily budget constraint (e.g., 30k, 3m, 3b, 30.000): ");
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

                        dailyBudgetConstraint = (double)amount;
                        Console.WriteLine("Daily budget constraint updated successfully!");
                        break;
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

        static void DrawPlantGrowth(string growthStage)
        {
            Console.WriteLine();
            switch (growthStage)
            {
                case "Seed Stage":
                    Console.WriteLine("  ( )");
                    Console.WriteLine("   | ");
                    Console.WriteLine(@"  / \");

                    Console.WriteLine("You poor nigga");
                    break;
                case "Sapling Stage":
                    Console.WriteLine(@"   \|/");
                    Console.WriteLine("  --*--");
                    Console.WriteLine("   /|\\");
                    Console.WriteLine("    | ");
                    Console.WriteLine(@"   / \");

                    Console.WriteLine("Keep up with your goal!");
                    break;
                case "Mature Stage":
                    Console.WriteLine(@"   \|/");
                    Console.WriteLine("  --*--");
                    Console.WriteLine("   /|\\");
                    Console.WriteLine("    | ");
                    Console.WriteLine(@"   /|\");
                    Console.WriteLine(@"  / | \");
                    Console.WriteLine(@" /  |  \");
                    Console.WriteLine("Congratulation!");
                    break;
            }
            Console.WriteLine();
        }

        static void DrawHealthBar(string healthStatus)
        {
            Console.WriteLine();
            Console.Write("Health: ");
            switch (healthStatus)
            {
                case "Healthy":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[##########]");
                    Console.WriteLine("You are healthy, keep going!");
                    break;
                case "Moderate":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[#####-----]");
                    Console.WriteLine("Please improve your health TT~TT");
                    break;
                case "Ill":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[##--------]");
                    Console.WriteLine("Joke's over, you're dead!");
                    break;
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        static Gameprogress UpdateGameProgress(string gamefilepath, double expChange, double healthChange)
        {
            var progress = LoadLatestGameProgress(gamefilepath);

            // Update EXP and Health
            progress.EXP += expChange;
            progress.Health = (int)Math.Clamp(progress.Health + healthChange, 0, 100);

            // Level up logic
            if (progress.EXP >= 10 && progress.Stage == "Seed Stage")
                progress.Stage = "Sapling Stage";

            if (progress.EXP >= 20 && progress.Stage == "Sapling Stage")
                progress.Stage = "Mature Stage";

            // Check if max level and stage reached
            if (progress.Stage == "Mature Stage" && progress.EXP >= 30)
            {
                // Parse Trees (CSV) into a List<string>
                var trees = string.IsNullOrEmpty(progress.Trees)
                    ? new List<string>()
                    : progress.Trees.Split(',').ToList();

                trees.Add($"Tree {trees.Count + 1}: Mature");

                // Serialize the updated list back into a CSV string
                progress.Trees = string.Join(",", trees);

                // Reset for a new tree
                progress.EXP = 0;
                progress.Stage = "Seed Stage";
            }

            SaveGameFile(gamefilepath, progress);
            return progress;
        }
        static Gameprogress LoadLatestGameProgress(string gamefilepath)
        {
            if (!File.Exists(gamefilepath))
            {
                // Return a default progress if no file exists
                return new Gameprogress { EXP = 0, Health = 100, Stage = "Seed Stage" };
            }

            using (var reader = new StreamReader(gamefilepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Gameprogress>().ToList();
                return records.LastOrDefault() ?? new Gameprogress { EXP = 0, Health = 100, Stage = "Seed Stage" };
            }
        }
        static void DisplayTreeStatus(Gameprogress progress)
        {
            // Parse trees from CSV (comma-separated values)
            var trees = string.IsNullOrEmpty(progress.Trees)
                ? new List<string>()
                : progress.Trees.Split(',').ToList();

            // Display trees in a grid format (up to 5 per row)
            int count = 0;
            foreach (var tree in trees)
            {
                Console.Write($"| {tree} ");
                count++;
                if (count % 5 == 0) Console.WriteLine("|"); // New line after 5 trees
            }

            if (count % 5 != 0) Console.WriteLine("|"); // Close the last row if incomplete

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
        static void SaveGameFile(string gamefilepath, Gameprogress progress)
        {
            bool fileExists = File.Exists(gamefilepath);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists
            };

            using (var writer = new StreamWriter(gamefilepath, append: true))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                if (!fileExists)
                {
                    csvWriter.WriteHeader<Gameprogress>();
                    csvWriter.NextRecord();
                }

                csvWriter.WriteRecord(progress);
                csvWriter.NextRecord();
            }

            Console.WriteLine("Game progress saved successfully.");
        }
        public class Gameprogress
        {
            public double EXP {  get; set; }
            public int Health { get; set; }    // Health is an integer
            public string Stage {  get; set; }
            public string Trees { get; set; } // Comma-separated list of trees
        }
        static void ShowTransactions()
            {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== View Transactions ===");

                // Select a filter by time
                Console.WriteLine("Select filter by time:");
                Console.WriteLine("[1] This Week");
                Console.WriteLine("[2] This Month");
                Console.WriteLine("[3] This Year");
                Console.WriteLine("[4] Custom Date Range");
                Console.WriteLine("[5] Show all");
                Console.WriteLine("[0] Return to Main Menu"); // Add an option to return to the main menu

                string filterChoice = Console.ReadLine();

                if (filterChoice == "0") // Exit to NavigationBar
                    break;

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                switch (filterChoice)
                {
                    case "1":
                        startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                        endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
                        break;
                    case "2":
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
                        break;
                    case "3":
                        startDate = new DateTime(DateTime.Now.Year, 1, 1);
                        endDate = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
                        break;
                    case "4":
                        Console.Write("Enter start date (dd/mm/yyyy): ");
                        while (!DateTime.TryParse(Console.ReadLine(), out startDate))
                        {
                            Console.Write("Invalid start date format. Please enter in dd/mm/yyyy format: ");
                        }

                        Console.Write("Enter end date (dd/mm/yyyy): ");
                        while (!DateTime.TryParse(Console.ReadLine(), out endDate))
                        {
                            Console.Write("Invalid end date format. Please enter in dd/mm/yyyy format: ");
                        }
                        endDate = endDate.Date.AddDays(1).AddTicks(-1);
                        break;
                    case "5":
                        // No filtering needed
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Showing all records.");
                        Console.ResetColor();
                        continue; // Skip the rest of the code and re-loop
                }

                // Load transactions from all CSV files
                var transactions = LoadTransactionsFromFiles();

                // Apply the date filter and sort by date (latest to oldest)
                var filteredTransactions = transactions
                    .Where(t => t.Date >= startDate && t.Date <= endDate)
                    .OrderByDescending(t => t.Date)
                    .ToList();

                // Create a Spectre.Console Table
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Session");
                table.AddColumn("Date");
                table.AddColumn("Flow");
                table.AddColumn("Amount");
                table.AddColumn("Source");

                int sequentialId = 1;
                foreach (var transaction in filteredTransactions)
                {
                    table.AddRow(
                        sequentialId++.ToString(),
                        transaction.Session,
                        transaction.Date.ToString("dd/MM/yyyy"),
                        transaction.Flow,
                        $"{transaction.Amount:N0}",
                        transaction.Source
                    );
                }

                // Render the table
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[cyan]=== Transactions ({filteredTransactions.Count} records) ===[/]");
                AnsiConsole.Write(table);

                Console.WriteLine("\nPress any key to filter again...");
                Console.ReadKey();
            }
        }
        static List<Transaction> LoadTransactionsFromFiles()
        {
            var transactions = new List<Transaction>();

            // Load Spending.csv
            if (File.Exists("Spending.csv"))
            {
                using (var reader = new StreamReader("Spending.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var spendings = csv.GetRecords<Spending>().ToList();
                    foreach (var spending in spendings)
                    {
                        transactions.Add(new Transaction
                        {
                            ID = spending.ID,
                            Source = "Spending",
                            Flow = "OUT",
                            Method = spending.Method,
                            Date = spending.Date,
                            Session = spending.Session, // Directly map Session from Spending record
                            Amount = spending.Amount
                        });
                    }
                }
            }

            // Load Income.csv
            if (File.Exists("Income.csv"))
            {
                using (var reader = new StreamReader("Income.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var incomes = csv.GetRecords<Income>().ToList();
                    foreach (var income in incomes)
                    {
                        transactions.Add(new Transaction
                        {
                            ID = income.ID,
                            Source = "Income",
                            Flow = "IN",
                            Method = income.Method,
                            Date = income.Date,
                            Session = income.Session, // Directly map Session from Income record
                            Amount = income.Amount
                        });
                    }
                }
            }

            // Load Loan.csv
            if (File.Exists("Loan.csv"))
            {
                using (var reader = new StreamReader("Loan.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var loans = csv.GetRecords<Loan>().ToList();
                    foreach (var loan in loans)
                    {
                        transactions.Add(new Transaction
                        {
                            ID = loan.ID,
                            Source = "Loan",
                            Flow = "OUT",
                            Method = loan.Method,
                            Date = loan.Date,
                            Session = loan.Session, // Directly map Session from Loan record
                            Amount = loan.Amount
                        });
                    }
                }
            }

            // Load Debit.csv
            if (File.Exists("Debit.csv"))
            {
                using (var reader = new StreamReader("Debit.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var debits = csv.GetRecords<Debit>().ToList();
                    foreach (var debit in debits)
                    {
                        transactions.Add(new Transaction
                        {
                            ID = debit.ID,
                            Source = "Debit",
                            Flow = "IN",
                            Method = debit.Method,
                            Date = debit.Date,
                            Session = debit.Session, // Directly map Session from Debit record
                            Amount = debit.Amount
                        });
                    }
                }
            }

            return transactions;
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
                    // Generate a new ID for the record
                    int id = GetNextId("Spending.csv");

                    // Save the record
                    StoreSpendingRecord(id, session, date, (double)amount, method, category, note);
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

        static void StoreSpendingRecord(int id, string session, DateTime date, double amount, string method, string category, string note)
        {
            const string fileName = "Spending.csv";

            // Check if the file exists
            bool fileExists = File.Exists(fileName);

            var spending = new Spending
            {
                ID = id,
                Session = session,
                Date = date,
                Amount = amount,
                Method = method,
                Category = category,
                Note = note
            };

            var configSpendings = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists // Only add headers if the file is new
            };

            try
            {
                using (var writer = new StreamWriter(fileName, append: true))
                using (var csvWriter = new CsvWriter(writer, configSpendings))
                {
                    if (!fileExists)
                    {
                        // Write header if it's a new file
                        csvWriter.WriteHeader<Spending>();
                        csvWriter.NextRecord();
                    }

                    // Write the spending record
                    csvWriter.WriteRecord(spending);
                    csvWriter.NextRecord();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data written to CSV successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
                Console.ResetColor();
            }
        }

        static int GetNextId(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return 1; // Start ID from 1 if the file doesn't exist
            }

            // Read all lines from the file
            var lines = File.ReadAllLines(fileName);

            // Skip header and parse the last ID
            if (lines.Length <= 1) return 1;

            var lastLine = lines[^1];
            if (int.TryParse(lastLine.Split(',')[0], out int lastId))
            {
                return lastId + 1;
            }

            return 1; // Default to 1 if parsing fails
        }

        static void AddIncomeRecord()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("=== Add Income Record ===");
                Console.ResetColor();

                // Step 1: Read amount
                decimal amount = ReadDecimalInput();

                // Step 2: Get payment method
                string method = GetMethod();

                // Step 3: Get spending category
                string category = GetIncomeCategory();

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
                    // Generate a new ID for the record
                    int id = GetNextId("Income.csv");

                    // Save the record
                    StoreIncomeRecord(id, session, date, (double)amount, method, category, note);
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

        static void StoreIncomeRecord(int id, string session, DateTime date, double amount, string method, string category, string note)
        {
            const string fileName = "Income.csv";

            // Check if the file exists
            bool fileExists = File.Exists(fileName);

            var income = new Income
            {
                ID = id,
                Session = session,
                Date = date,
                Amount = amount,
                Method = method,
                Category = category,
                Note = note
            };

            var configSpendings = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists // Only add headers if the file is new
            };

            try
            {
                using (var writer = new StreamWriter(fileName, append: true))
                using (var csvWriter = new CsvWriter(writer, configSpendings))
                {
                    if (!fileExists)
                    {
                        // Write header if it's a new file
                        csvWriter.WriteHeader<Income>();
                        csvWriter.NextRecord();
                    }

                    // Write the spending record
                    csvWriter.WriteRecord(income);
                    csvWriter.NextRecord();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data written to CSV successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
                Console.ResetColor();
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

                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                string session = GetSessionOfDay();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nLoan Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Borrower: {borrower}");
                Console.WriteLine($"  - Note: {note}");

                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    int id = GetNextId("Loan.csv");
                    StoreLoanRecord(id, session, date, (double)amount, method, borrower, note);
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

                Console.WriteLine("\nDo you want to add another record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 0)
                {
                    break; // Exit to AddRecord menu
                }
            }
        }

        static void StoreLoanRecord(int id, string session, DateTime date, double amount, string method, string borrower, string note)
        {
            const string fileName = "Loan.csv";

            bool fileExists = File.Exists(fileName);

            var loan = new Loan
            {
                ID = id,
                Session = session,
                Date = date,
                Amount = amount,
                Method = method,
                Borrower = borrower,
                Note = note
            };

            var configLoans = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists
            };

            try
            {
                using (var writer = new StreamWriter(fileName, append: true))
                using (var csvWriter = new CsvWriter(writer, configLoans))
                {
                    if (!fileExists)
                    {
                        csvWriter.WriteHeader<Loan>();
                        csvWriter.NextRecord();
                    }

                    csvWriter.WriteRecord(loan);
                    csvWriter.NextRecord();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data written to CSV successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
                Console.ResetColor();
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

                // Step 1: Get amount
                decimal amount = ReadDecimalInput();

                // Step 2: Get lender's name
                Console.Write("Enter who you borrow from: ");
                string lender = GetValidName();

                // Step 3: Get payment method
                string method = GetMethod();

                // Step 4: Enter a note
                Console.Write("Enter Note: ");
                string note = Console.ReadLine();

                // Step 5: Get the date
                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

                // Step 6: Choose session of the day
                string session = GetSessionOfDay();

                // Step 7: Display record summary
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDebit Record:");
                Console.ResetColor();
                Console.WriteLine($"  - Session: {session}");
                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
                Console.WriteLine($"  - Method: {method}");
                Console.WriteLine($"  - Lender: {lender}");
                Console.WriteLine($"  - Note: {note}");

                // Step 8: Confirm save
                Console.WriteLine("\nDo you want to save this record?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("0. No");

                if (GetYorN_Selection() == 1)
                {
                    // Generate a new ID for the record
                    int id = GetNextId("Debit.csv");

                    // Save the record
                    StoreDebitRecord(id, session, date, (double)amount, method, lender, note);
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
        static void StoreDebitRecord(int id, string session, DateTime date, double amount, string method, string lender, string note)
        {
            const string fileName = "Debit.csv";

            // Check if the file exists
            bool fileExists = File.Exists(fileName);

            // Create a Debit object
            var debit = new Debit
            {
                ID = id,
                Session = session,
                Date = date,
                Amount = amount,
                Method = method,
                Lender = lender,
                Note = note
            };

            var configDebits = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !fileExists // Write headers only if the file is new
            };

            try
            {
                using (var writer = new StreamWriter(fileName, append: true))
                using (var csvWriter = new CsvWriter(writer, configDebits))
                {
                    if (!fileExists)
                    {
                        // Write header for a new file
                        csvWriter.WriteHeader<Debit>();
                        csvWriter.NextRecord();
                    }

                    // Write the debit record
                    csvWriter.WriteRecord(debit);
                    csvWriter.NextRecord();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data written to CSV successfully.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
                Console.ResetColor();
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

            // Load transactions from all CSV files
            List<Transaction> transactions = LoadTransactionsFromFiles();

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
            Console.WriteLine("Thank you for using the budget tool!");
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
                Console.WriteLine("2. View your balance book (lending/borrowing)");
                Console.WriteLine("3. View spending by category");
                Console.WriteLine("4. View income by category");
                Console.WriteLine("5. View loans by borrower");
                Console.WriteLine("6. View borrowing lender");
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
                            Console.WriteLine("=== Balance Book (Lending/Borrowing) ===");
                            DisplayBalanceBook(filteredTransactions);
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("=== Spending by Category ===");

                            // Provide the Spending.csv file path
                            const string spendingFilePath = "Spending.csv";

                            if (File.Exists(spendingFilePath))
                            {
                                DisplaySpendingByCategory(spendingFilePath); // Call the correct function
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Spending.csv file not found.");
                                Console.ResetColor();
                            }
                            break;

                        case 4:
                            Console.Clear();
                            Console.WriteLine("=== Income by Category ===");

                            // Provide the Spending.csv file path
                            const string incomeFilePath = "Income.csv";

                            if (File.Exists(spendingFilePath))
                            {
                                DisplayIncomeByCategory(incomeFilePath);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Income.csv file not found.");
                                Console.ResetColor();
                            }
                            break;

                        case 5:
                            Console.Clear();
                            Console.WriteLine("=== Loan by Borrower ===");

                            // Provide the Spending.csv file path
                            const string loanFilePath = "Loan.csv";

                            if (File.Exists(spendingFilePath))
                            {
                                DisplayLoansByBorrower(loanFilePath);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Loan.csv file not found.");
                                Console.ResetColor();
                            }
                            break;

                        case 6:
                            Console.Clear();
                            Console.WriteLine("=== Debit by Lender ===");

                            // Provide the Spending.csv file path
                            const string debitFilePath = "debit.csv";

                            if (File.Exists(spendingFilePath))
                            {
                                DisplayDebitByLender(debitFilePath);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: Debit.csv file not found.");
                                Console.ResetColor();
                            }
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


        static void DisplaySpendingByCategory(string spendingFilePath)
        {
            if (!File.Exists(spendingFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Spending.csv file not found.");
                Console.ResetColor();
                return;
            }

            // Read data from the CSV file
            var spendingRecords = new List<Spending>();
            using (var reader = new StreamReader(spendingFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                spendingRecords = csv.GetRecords<Spending>().ToList();
            }

            // Process data: Group by Category, sum Amount, and sort by total in descending order
            var expenditure = spendingRecords
                .GroupBy(s => s.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(s => s.Amount) })
                .OrderByDescending(e => e.Total)
                .ToList();

            // Calculate total expenditure
            double totalExpenditure = expenditure.Sum(e => e.Total);

            // Create a Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Category");
            table.AddColumn("Amount");

            foreach (var item in expenditure)
            {
                table.AddRow(item.Category, FormatCurrency(item.Total));
            }

            // Display table and total
            Console.Clear();
            Console.WriteLine("=== Spending by Category ===\n");
            AnsiConsole.Write(table);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Expenditure: {FormatCurrency(totalExpenditure)}");
            Console.ResetColor();
        }

        static void DisplayIncomeByCategory(string incomeFilePath)
        {
            if (!File.Exists(incomeFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Income.csv file not found.");
                Console.ResetColor();
                return;
            }

            // Read data from the CSV file
            var incomeRecords = new List<Income>(); // Use the Income class
            using (var reader = new StreamReader(incomeFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                incomeRecords = csv.GetRecords<Income>().ToList();
            }


            // Process data: Group by Category, sum Amount, and sort by total in descending order
            var incomeByCategory = incomeRecords
                .GroupBy(i => i.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(i => i.Amount) })
                .OrderByDescending(e => e.Total)
                .ToList();

            // Calculate total income
            double totalIncome = incomeByCategory.Sum(e => e.Total);

            // Create a Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Category");
            table.AddColumn("Amount");

            foreach (var item in incomeByCategory)
            {
                table.AddRow(item.Category, FormatCurrency(item.Total));
            }

            // Display table and total
            Console.Clear();
            Console.WriteLine("=== Income by Category ===\n"); // Changed title
            AnsiConsole.Write(table);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Income: {FormatCurrency(totalIncome)}"); // Changed label
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
            var balances = transactions
                .GroupBy(t => t.Method)
                .Select(g => new { Method = g.Key, Balance = g.Sum(t => t.Flow == "IN" ? t.Amount : -t.Amount) });

            // Create Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Wallet Type");
            table.AddColumn("Balance");

            foreach (var balance in balances)
            {
                table.AddRow(balance.Method, FormatCurrency(balance.Balance));
            }

            // Render the table
            Console.Clear();
            Console.WriteLine("=== Balances by Method ===\n");
            AnsiConsole.Write(table);
        }


        static void DisplayBalanceBook(List<Transaction> transactions)
        {
            double lending = transactions
                .Where(t => t.Source == "Loan")
                .Sum(t => t.Amount);

            double borrowing = transactions
                .Where(t => t.Source == "Debit")
                .Sum(t => t.Amount);

            // Create Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Category");
            table.AddColumn("Amount");

            table.AddRow("Lending", FormatCurrency(lending));
            table.AddRow("Borrowing", FormatCurrency(borrowing));
            table.AddRow("Balance", FormatCurrency(lending - borrowing));

            // Render the table
            Console.Clear();
            Console.WriteLine("=== Balance Book ===\n");
            AnsiConsole.Write(table);
        }

        static void DisplayLoansByBorrower(string loanFilePath)
        {
            if (!File.Exists(loanFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Loan.csv file not found.");
                Console.ResetColor();
                return;
            }

            // Read data from the CSV file
            var loanRecords = new List<Loan>(); // Use the Loan class
            using (var reader = new StreamReader(loanFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                loanRecords = csv.GetRecords<Loan>().ToList();
            }

            // Process data: Group by Borrower (assuming 'Method' is the Borrower),
            // sum Amount, and sort by total in descending order
            var loansByBorrower = loanRecords
                .GroupBy(l => l.Borrower) // Group by Borrower
                .Select(g => new { Borrower = g.Key, Total = g.Sum(l => l.Amount) })
                .OrderByDescending(e => e.Total)
                .ToList();

            // Create a Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Borrower");  // Changed column name
            table.AddColumn("Amount");

            foreach (var item in loansByBorrower)
            {
                table.AddRow(item.Borrower, FormatCurrency(item.Total));
            }

            // Display table
            Console.Clear();
            Console.WriteLine("=== Loans by Borrower ===\n"); // Changed title
            AnsiConsole.Write(table);

            // Calculate and display total loan amount (optional)
            double totalLoanAmount = loansByBorrower.Sum(l => l.Total);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Loan Amount: {FormatCurrency(totalLoanAmount)}");
            Console.ResetColor();

        }

        static void DisplayDebitByLender(string debitFilePath)
        {
            if (!File.Exists(debitFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Debit.csv file not found.");
                Console.ResetColor();
                return;
            }

            // Read data from the CSV file
            var debitRecords = new List<Debit>(); // Use the Debit class
            using (var reader = new StreamReader(debitFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                debitRecords = csv.GetRecords<Debit>().ToList();
            }

            // Assuming 'Method' property represents the Lender in Debit records
            var borrowingByLender = debitRecords
                .GroupBy(d => d.Lender) // Group by Lender (Method)
                .Select(g => new { Lender = g.Key, Total = g.Sum(d => d.Amount) })
                .OrderByDescending(e => e.Total)
                .ToList();


            // Create a Spectre.Console table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Lender"); // Changed column name
            table.AddColumn("Amount");

            foreach (var item in borrowingByLender)
            {
                table.AddRow(item.Lender, FormatCurrency(item.Total));
            }

            // Display table
            Console.Clear();
            Console.WriteLine("=== Borrowing by Lender ===\n"); // Changed title
            AnsiConsole.Write(table);

            // Calculate and display total borrowed amount (optional)
            double totalBorrowedAmount = borrowingByLender.Sum(b => b.Total);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Borrowed Amount: {FormatCurrency(totalBorrowedAmount)}");
            Console.ResetColor();
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
