using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using CsvHelper;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;
using MathNet.Numerics.Statistics;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Linear;
using System.Data;

namespace PersonalFinanceApp
{
    class Program2
    {
        static double dailyBudgetConstraint;
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
            public string Note { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public double Constraint { get; set; }
            public int OverSpendLimit { get; set; }
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
        public class DailyBudgetConstraint
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public double Constraint { get; set; }
            public int OverSpendLimit { get; set; }
        }

        class OutFlowEvent
        {
            public DateTime Date { get; set; }
            public string Type { get; set; } // "Spending", "Loan"
            public double Amount { get; set; }
        }

        public class FinancialEvent
        {
            public DateTime Date { get; set; } // Date of the financial event
            public string Type { get; set; }   // Type of event (e.g., Spending, Debit, Loan)
            public double Amount { get; set; } // Amount of the event
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
                Console.WriteLine("[0] Exit");
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
                    case "0":
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
                Console.WriteLine("2. Change the Reminder Threshold");
                Console.WriteLine("3 . Exit to Main Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                double exp = 0;
                string healthStatus = string.Empty;
                string growthStage = string.Empty;
                switch (choice)
                {
                    case "1":
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
                        SaveGameFile(gamefilepath, progress); // Save updated progress
                        Console.WriteLine("Daily constraint updated. Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "2":
                        reminderThreshold = GetReminderThreshold();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
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

            double dailyBudgetConstraint = transactions
                .Where(t => t.Month == DateTime.Now.Month && t.Year == DateTime.Now.Year)
                .Select(t => t.Amount)
                .LastOrDefault(); 
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
            var trees = progress.Trees;
            int numberOfLines = trees / 5;
            int numberTreeOfLastLine = trees % 5;
            // Display trees in a grid format (up to 5 per row)
            int count = 0;
            string treeFirstLine = "";
            string tree2ndLine = "";
            string tree3rdLine = "";
            string tree4thLine = "";
            string tree5thLine = "";
            string tree6thLine = "";
            string tree7thLine = "";
            for (int line = 0; line < numberOfLines; line++)
            {
                treeFirstLine = "";
                tree2ndLine = "";
                tree3rdLine = "";
                tree4thLine = "";
                tree5thLine = "";
                tree6thLine = "";
                tree7thLine = "";
                for (int i = 0; i < 5; i++)
                {
                    treeFirstLine += @"   \|/   ";
                    tree2ndLine += @"  --*--  ";
                    tree3rdLine += "   /|\\   ";
                    tree4thLine += "    |    ";
                    tree5thLine += @"   /|\   ";
                    tree6thLine += @"  / | \  ";
                    tree7thLine += @" /  |  \ ";
                }


                Console.WriteLine(treeFirstLine);
                Console.WriteLine(tree2ndLine);
                Console.WriteLine(tree3rdLine);
                Console.WriteLine(tree4thLine);
                Console.WriteLine(tree5thLine);
                Console.WriteLine(tree6thLine);
                Console.WriteLine(tree7thLine);
            }
            treeFirstLine = "";
            tree2ndLine = "";
            tree3rdLine = "";
            tree4thLine = "";
            tree5thLine = "";
            tree6thLine = "";
            tree7thLine = "";
            for (int i = 0; i < numberTreeOfLastLine; i++)
            {
                treeFirstLine += @"   \|/   ";
                tree2ndLine += @"  --*--  ";
                tree3rdLine += "   /|\\   ";
                tree4thLine += "    |    ";
                tree5thLine += @"   /|\   ";
                tree6thLine += @"  / | \  ";
                tree7thLine += @" /  |  \ ";
            }


            Console.WriteLine(treeFirstLine);
            Console.WriteLine(tree2ndLine);
            Console.WriteLine(tree3rdLine);
            Console.WriteLine(tree4thLine);
            Console.WriteLine(tree5thLine);
            Console.WriteLine(tree6thLine);
            Console.WriteLine(tree7thLine);



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
            public double EXP { get; set; }
            public int Health { get; set; }    // Health is an integer
            public string Stage { get; set; }
            public int Trees { get; set; } // Comma-separated list of trees
        }
        static void Home_ShowReminder(List<Transaction> transactions, double dailyBudget, int overspendingLimit)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Home ===");
            Console.ResetColor();

            DateTime today = DateTime.Now.Date;

            // Calculate today's expenditure
            double todayExpenditure = transactions
                .Where(t => t.Source == "Spending" && t.Date.Date == today)
                .Sum(t => t.Amount);

            // Check overspending occurrences
            int overspendingCount = CalculateOverspendingCount(transactions, dailyBudget);
            Console.WriteLine($"Daily Budget: {dailyBudget:N0} VND");
            Console.WriteLine($"Today's Expenditure: {todayExpenditure:N0} VND");

            // Display reminder if overspending limit is exceeded
            if (overspendingCount > overspendingLimit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Reminder: You have exceeded your overspending limit {overspendingLimit} times this month!");
                Console.ResetColor();
            }

            Console.WriteLine("\n[1] View Transactions");
            Console.WriteLine("[2] Add Record");
            Console.WriteLine("[3] View Budget");
            Console.WriteLine("[4] Exit");
            Console.Write("Choose an option: ");
        }

        static void ShowHome(List<Transaction> transactions, double dailyBudget, int overspendingLimit)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Home ===");
            Console.ResetColor();

            // Gamification introduction
            Console.WriteLine("Gamify your expense tracking by maintaining a streak.");
            Console.WriteLine("TODO: Add streak tracking logic using System.DateTime");

            // Call the overspending reminder logic
            Home_ShowReminder(transactions, dailyBudget, overspendingLimit);

            // Prompt user for the next action
            Console.WriteLine("\n[1] View Transactions");
            Console.WriteLine("[2] Add Record");
            Console.WriteLine("[3] View Budget");
            Console.WriteLine("[4] Exit");
            Console.Write("Choose an option: ");
        }

        static int CalculateOverspendingCount(List<Transaction> transactions, double dailyBudget)
        {
            // Tolerance for overspending (e.g., 10%)
            double overspendingThreshold = dailyBudget * 1.1;

            // Group spending by day
            var dailySpending = transactions
                .Where(t => t.Source == "Spending")
                .GroupBy(t => t.Date.Date)
                .Select(g => g.Sum(t => t.Amount))
                .ToList();

            // Calculate overspending occurrences
            double count = 0;
            foreach (var spending in dailySpending)
            {
                if (spending > overspendingThreshold)
                    count += 1; // Full overspending
                else if (spending > dailyBudget)
                    count += 0.5; // Partial overspending
            }

            return (int)count; // Return the integer part as overspending count
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
                Console.WriteLine("[0] Return to Main Menu");

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

                // Apply the date filter
                var filteredTransactions = transactions
                    .Where(t => t.Date >= startDate && t.Date <= endDate)
                    .OrderByDescending(t => t.Date)
                    .ToList();

                // Action menu loop
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Select an action:");
                    Console.WriteLine("[1] Transaction data");
                    Console.WriteLine("[2] Summary");
                    Console.WriteLine("[0] Return to Filter Menu");

                    string actionChoice = Console.ReadLine();

                    if (actionChoice == "0") // Return to Filter Menu
                        break;

                    switch (actionChoice)
                    {
                        case "1": // Show transaction data
                            ShowTransactionTable(filteredTransactions);
                            break;
                        case "2": // Show spending summary
                            ShowSpendingSummary(filteredTransactions);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice. Please try again.");
                            Console.ResetColor();
                            break;
                    }
                }
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
                            Amount = spending.Amount,
                            Note = spending.Note
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
                            Amount = income.Amount,
                            Note = income.Note
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
                            Amount = loan.Amount,
                            Note = loan.Note
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
                            Amount = debit.Amount,
                            Note = debit.Note
                        });
                    }
                }
            }
            //Load DailyBudgetConstraints.csv
            if (File.Exists("DailyBudgetConstraints.csv"))
            {
                using (var reader = new StreamReader("DailyBudgetConstraints.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Rename the variable to avoid conflict with the type name
                    var dailyBudgetConstraints = csv.GetRecords<DailyBudgetConstraint>().ToList();

                    foreach (var constraintRecord in dailyBudgetConstraints) // Use a distinct name for loop variable
                    {
                        transactions.Add(new Transaction
                        {
                            Constraint = constraintRecord.Constraint, // Assuming property name matches the class
                            OverSpendLimit = constraintRecord.OverSpendLimit, // Adjust based on property naming
                            Month = constraintRecord.Month,
                            Year = constraintRecord.Year
                        });
                    }
                }
            }

            return transactions;
        }

        static void ShowTransactionTable(List<Transaction> filteredTransactions)
        {
            // Create a Spectre.Console Table
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("ID");
            table.AddColumn("Session");
            table.AddColumn("Date");
            table.AddColumn("Flow");
            table.AddColumn("Amount");
            table.AddColumn("Source");
            table.AddColumn("Note");

            int sequentialId = 1;
            foreach (var transaction in filteredTransactions)
            {
                table.AddRow(
                    sequentialId++.ToString(),
                    transaction.Session,
                    transaction.Date.ToString("dd/MM/yyyy"),
                    transaction.Flow,
                    $"{transaction.Amount:N0}",
                    transaction.Source,
                    transaction.Note ?? "N/A" // Default to "N/A" if Note is null
                );
            }

            // Render the table
            Console.Clear();
            AnsiConsole.MarkupLine($"[cyan]=== Transactions ({filteredTransactions.Count} records) ===[/]");
            AnsiConsole.Write(table);

            Console.WriteLine("\nPress any key to return menu of actions...");
            Console.ReadKey();
        }

        static void ShowSpendingSummary(List<Transaction> filteredTransactions)
        {
            DateTime today = DateTime.Now.Date;

            // Calculate total expenditure by day (including today)
            var expenditureByDay = filteredTransactions
                .Where(t => t.Source == "Spending")
                .GroupBy(t => t.Date.Date) // Group by day
                .Select(g => new { Date = g.Key, TotalExpenditure = g.Sum(t => t.Amount) })
                .OrderBy(g => g.Date)
                .ToList();

            // Calculate total expenditure
            double totalExpenditure = expenditureByDay.Sum(t => t.TotalExpenditure);

            // Calculate average daily expenditure (excluding today)
            var expenditureExcludingToday = expenditureByDay
                .Where(t => t.Date < today) // Exclude today's data for average calculation
                .ToList();

            int totalDaysExcludingToday = expenditureExcludingToday.Count;
            double averageExpenditure = totalDaysExcludingToday > 0
                ? expenditureExcludingToday.Sum(t => t.TotalExpenditure) / totalDaysExcludingToday
                : 0;

            // Create a Spectre.Console Table for total expenditure by day
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Date");
            table.AddColumn("Expenditure");

            foreach (var entry in expenditureByDay)
            {
                table.AddRow(entry.Date.ToString("dd/MM/yyyy"), $"{entry.TotalExpenditure:N0} VND");
            }

            // Render the table
            Console.Clear();
            AnsiConsole.MarkupLine("[cyan]=== Total Expenditure by Day ===[/]");
            AnsiConsole.Write(table);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Expenditure: {totalExpenditure:N0} VND");
            Console.WriteLine($"Average Daily Expenditure (excluding today): {averageExpenditure:N0} VND");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static double CalculateAverageDailyExpenditure(List<Transaction> transactions)
        {
            DateTime today = DateTime.Now.Date;

            // Group expenditures by day where source is "Spending"
            var expenditureByDay = transactions
                .Where(t => t.Source == "Spending") // Filter by source
                .GroupBy(t => t.Date.Date) // Group by day
                .Select(g => new { Date = g.Key, TotalExpenditure = g.Sum(t => t.Amount) })
                .ToList();

            // Filter out today's expenditure for average calculation
            var expenditureExcludingToday = expenditureByDay
                .Where(e => e.Date < today) // Exclude today
                .ToList();

            // Calculate average
            int totalDaysExcludingToday = expenditureExcludingToday.Count;
            double averageExpenditure = totalDaysExcludingToday > 0
                ? expenditureExcludingToday.Sum(e => e.TotalExpenditure) / totalDaysExcludingToday
                : 0;

            return averageExpenditure;
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
            Console.WriteLine("3. Transportation"); // Essential
            Console.WriteLine("4. Snack & Drinks"); // Optional
            Console.WriteLine("5. Optional Spending (eating out, movie ticket, ...)");
            Console.WriteLine("6. Healthcare"); // Important
            Console.WriteLine("7. Education"); // Investment
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
                            category = "Snack & Drinks";
                            break;
                        case 5:
                            category = "Optional Spending";
                            break;
                        case 6:
                            category = "Healthcare";
                            break;
                        case 7:
                            category = "Education";
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
                Console.WriteLine("7. Set a daily budget constraint");
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
                            DisplaySpendingByCategory("Spending.csv");
                            break;

                        case 4:
                            Console.Clear();
                            Console.WriteLine("=== Income by Category ===");
                            DisplayIncomeByCategory("Income.csv");
                            break;

                        case 5:
                            Console.Clear();
                            Console.WriteLine("=== Loan by Borrower ===");
                            DisplayLoansByBorrower("Loan.csv");
                            break;

                        case 6:
                            Console.Clear();
                            Console.WriteLine("=== Debit by Lender ===");
                            DisplayDebitByLender("Debit.csv");
                            break;

                        case 7:
                            Console.Clear();
                            SetDailyBudgetConstraint(filteredTransactions, month, year); // Pass the month and year
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

        static void SetDailyBudgetConstraint(List<Transaction> filteredTransactions, int month, int year)
        {
            // Ensure the average expenditure is calculated using filtered transactions
            double averageExpenditure = CalculateAverageDailyExpenditure(filteredTransactions);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Average Daily Expenditure (excluding today): {averageExpenditure:N0} VND");
            Console.ResetColor();

            Console.WriteLine("\nSet a daily budget constraint:");
            Console.WriteLine("1. Use current average daily expenditure as the budget constraint");
            Console.WriteLine("2. Set a fixed daily expenditure constraint");
            Console.WriteLine("0. Return to Budget Menu");
            Console.Write("Your selection: ");

            double dailyBudgetConstraint = 0;
            if (int.TryParse(Console.ReadLine(), out int budgetChoice))
            {
                switch (budgetChoice)
                {
                    case 1:
                        dailyBudgetConstraint = averageExpenditure;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Daily Budget Constraint set to: {dailyBudgetConstraint:N0} VND");
                        Console.ResetColor();
                        break;

                    case 2:
                        Console.Write("Enter your desired daily budget constraint: ");
                        while (!double.TryParse(Console.ReadLine(), out dailyBudgetConstraint) || dailyBudgetConstraint <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a positive number.");
                            Console.ResetColor();
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Daily Budget Constraint set to: {dailyBudgetConstraint:N0} VND");
                        Console.ResetColor();
                        break;

                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Returning to Budget Menu...");
                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Returning to Budget Menu.");
                        Console.ResetColor();
                        return;
                }
            }

            if (dailyBudgetConstraint > 0)
            {
                Console.Write("\nHow many times of overspending should trigger a reminder? (Default: 3): ");
                int maxOverspendingCount;
                if (!int.TryParse(Console.ReadLine(), out maxOverspendingCount) || maxOverspendingCount <= 0)
                {
                    maxOverspendingCount = 3;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Overspending Reminder set to trigger after {maxOverspendingCount} times.");
                Console.ResetColor();

                // Save the daily budget constraint and max overspending count
                SaveDailyBudgetConstraint(dailyBudgetConstraint, maxOverspendingCount, month, year);
            }
        }

        static void SaveDailyBudgetConstraint(double constraint, int overspendLimit, int month, int year)
        {
            const string fileName = "DailyBudgetConstraints.csv";

            bool fileExists = File.Exists(fileName);
            using (var writer = new StreamWriter(fileName, append: true))
            {
                if (!fileExists)
                {
                    // Write headers if file is new
                    writer.WriteLine("Month,Year,Constraint,MaxOverspendLimit");
                }

                // Save the constraint data
                writer.WriteLine($"{month},{year},{constraint},{overspendLimit}");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Daily Budget Constraint saved successfully!");
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
        static void ShowSaving()
        {
            const string transactionFilePath = "Transaction.csv";

            // Ensure Transaction.csv exists; only create it if missing
            if (!File.Exists(transactionFilePath))
            {
                CreateTransactionFile(transactionFilePath);
            }

            while (true) // Keep the user in the Saving menu
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=== Saving ===");
                Console.ResetColor();
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("1. Set up a financial plan (scenario)");
                Console.WriteLine("2. Use spending forecast function");
                Console.WriteLine("3. Use spending suggestions function");
                Console.WriteLine("0. Return to Main Menu");

                int choice = GetSavingMenuSelection();

                // Retrieve required data for the Scenario function
                double currentBalance = GetCurrentBalance(transactionFilePath);
                double dailyAverageExpenditure = CalculateAverageDailyExpenditure(transactionFilePath);

                // Menu handling
                switch (choice)
                {
                    case 1:
                        Scenario(currentBalance, dailyAverageExpenditure); // Return to ShowSaving after execution
                        break;
                    case 2:
                        SpendingForecast(transactionFilePath); // Return to ShowSaving after execution
                        break;
                    case 3:
                        SpendingSuggestions(transactionFilePath); // Return to ShowSaving after execution
                        break;
                    case 0:
                        Console.WriteLine("Returning to Main Menu...");
                        return; // Exit to main menu
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Please try again.");
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\nPress any key to return to the Saving menu...");
                Console.ReadKey();
            }
        }


        static void CreateTransactionFile(string transactionFilePath)
        {
            // Load transactions from individual CSV files
            var transactions = LoadTransactionsFromFiles();

            // Write the transactions to Transaction.csv (overwrite instead of append)
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            try
            {
                using (var writer = new StreamWriter(transactionFilePath, append: false)) // Overwrite mode
                using (var csvWriter = new CsvWriter(writer, config))
                {
                    // Write header
                    csvWriter.WriteHeader<Transaction>();
                    csvWriter.NextRecord();

                    // Write transactions
                    csvWriter.WriteRecords(transactions);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Transaction data successfully written to {transactionFilePath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to CSV: {ex.Message}");
                Console.ResetColor();
            }
        }
        static int GetSavingMenuSelection()
        {
            while (true)
            {
                Console.Write("Your selection: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= 3)
                {
                    return choice;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please select a number between 0 and 3.");
                Console.ResetColor();
            }
        }


        // Example implementation for GetCurrentBalance
        static double GetCurrentBalance(string transactionFilePath)
        {
            if (!File.Exists(transactionFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {transactionFilePath} not found.");
                Console.ResetColor();
                return 0;
            }

            // Load transactions from file
            List<Transaction> transactions;
            using (var reader = new StreamReader(transactionFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                transactions = csv.GetRecords<Transaction>().ToList();
            }

            // Get current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            // Filter transactions for the current month and year
            var filteredTransactions = transactions
                .Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear)
                .ToList();

            // Calculate balance
            return filteredTransactions.Where(t => t.Flow == "IN").Sum(t => t.Amount) -
                   filteredTransactions.Where(t => t.Flow == "OUT").Sum(t => t.Amount);
        }

        // Example implementation for CalculateAverageDailyExpenditure
        static double CalculateAverageDailyExpenditure(string transactionFilePath)
        {
            if (!File.Exists(transactionFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {transactionFilePath} not found.");
                Console.ResetColor();
                return 0;
            }

            // Load transactions from file
            List<Transaction> transactions;
            using (var reader = new StreamReader(transactionFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                transactions = csv.GetRecords<Transaction>().ToList();
            }

            // Get current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            DateTime today = DateTime.Now.Date;

            // Filter transactions for the current month and year
            var filteredTransactions = transactions
                .Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear && t.Source == "Spending")
                .GroupBy(t => t.Date.Date) // Group by day
                .Select(g => new { Date = g.Key, TotalExpenditure = g.Sum(t => t.Amount) })
                .OrderBy(g => g.Date)
                .ToList();

            // Calculate average daily expenditure excluding today
            var expenditureExcludingToday = filteredTransactions
                .Where(t => t.Date < today) // Exclude today's data
                .ToList();

            int totalDaysExcludingToday = expenditureExcludingToday.Count;
            return totalDaysExcludingToday > 0
                ? expenditureExcludingToday.Sum(t => t.TotalExpenditure) / totalDaysExcludingToday
                : 0;
        }

        static void Scenario(double currentBalance, double dailyAverageExpenditure)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=== Budget Scenario Planning ===");
                Console.ResetColor();

                DateTime today = DateTime.Now.Date;
                int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                DateTime endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);

                Console.WriteLine($"Today is {today:dd/MM/yyyy}");
                Console.WriteLine($"Your current balance: {currentBalance:N0} VND");
                Console.WriteLine($"Your daily average expenditure: {dailyAverageExpenditure:N0} VND");

                var futureEvents = new List<FinancialEvent>();

                while (true)
                {
                    Console.WriteLine("\nAdd a financial event in the future:");
                    Console.WriteLine("1. Spending");
                    Console.WriteLine("2. Debit");
                    Console.WriteLine("3. Loan");
                    Console.WriteLine("0. Finish adding events");
                    Console.Write("Your selection: ");
                    string choice = Console.ReadLine();

                    if (choice == "0") break;

                    string eventType = choice switch
                    {
                        "1" => "Spending",
                        "2" => "Debit",
                        "3" => "Loan",
                        _ => null
                    };

                    if (eventType == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write($"Enter the amount for {eventType.ToLower()} (e.g., 200000): ");
                    if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid amount. Please try again.");
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write($"Enter the date for {eventType.ToLower()} (dd/MM/yyyy) (within {today:dd/MM/yyyy} - {endOfMonth:dd/MM/yyyy}): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime eventDate) || eventDate < today || eventDate > endOfMonth)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid date. Please try again.");
                        Console.ResetColor();
                        continue;
                    }

                    futureEvents.Add(new FinancialEvent
                    {
                        Date = eventDate,
                        Type = eventType,
                        Amount = amount
                    });

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{eventType} of {amount:N0} VND on {eventDate:dd/MM/yyyy} added successfully!");
                    Console.ResetColor();
                }

                // Calculate future balance
                double futureBalance = currentBalance;
                if (futureEvents.Any())
                {
                    foreach (var eventDate in futureEvents.Select(e => e.Date).Distinct().OrderBy(d => d))
                    {
                        int daysToEvent = (eventDate - today).Days;
                        double dailyExpenditureTotal = daysToEvent * dailyAverageExpenditure;

                        double eventImpact = futureEvents
                            .Where(e => e.Date == eventDate)
                            .Sum(e => e.Type == "Debit" ? e.Amount : -e.Amount);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nOn {eventDate:dd/MM/yyyy}:");
                        Console.WriteLine($"  Daily Expenditure Total: {dailyExpenditureTotal:N0} VND");
                        Console.WriteLine($"  Event Impact: {eventImpact:N0} VND");

                        futureBalance -= dailyExpenditureTotal;
                        futureBalance += eventImpact;

                        Console.WriteLine($"  Balance After Events: {futureBalance:N0} VND");
                    }
                }
                else
                {
                    // If no events are added, deduct daily expenditures until the end of the month
                    int remainingDays = (endOfMonth - today).Days;
                    double totalDailyExpenditure = remainingDays * dailyAverageExpenditure;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nNo events added. Deducting total daily expenditures for the remaining {remainingDays} days.");
                    Console.WriteLine($"  Total Daily Expenditure: {totalDailyExpenditure:N0} VND");

                    futureBalance -= totalDailyExpenditure;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFinal Balance at the end of the month: {futureBalance:N0} VND");
                Console.ResetColor();

                // Return to ShowSaving
                return;
            }
        }

        static void SpendingForecast(string transactionFilePath)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Spending Forecast Function ===");
            Console.ResetColor();

            if (!File.Exists(transactionFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {transactionFilePath} not found.");
                Console.ResetColor();
                return;
            }

            DateTime today = DateTime.Now.Date;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Start of the week (Sunday)

            // Load transactions from all files and filter for Spending
            var transactions = LoadTransactionsFromFiles()
                .Where(t => t.Source == "Spending" && t.Date.Date >= startOfWeek && t.Date.Date <= today)
                .OrderBy(t => t.Date)
                .ToList();

            if (!transactions.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No spending data available for the current week.");
                Console.ResetColor();
                return;
            }

            // Extract spending amounts by day
            var spendingData = transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new Tuple<DateTime, double>(g.Key, g.Sum(t => t.Amount)))
                .OrderBy(t => t.Item1)
                .ToList();

            // Convert spending amounts to an array
            double[] amounts = spendingData.Select(t => t.Item2).ToArray();

            // Calculate a 3-day moving average using MathNet.Numerics
            var movingAverage3 = new double[amounts.Length];
            for (int i = 0; i < amounts.Length; i++)
            {
                if (i < 2)
                    movingAverage3[i] = amounts.Take(i + 1).Average(); // Average for first few days
                else
                    movingAverage3[i] = amounts.Skip(i - 2).Take(3).Average(); // 3-day moving average
            }

            // Predict spending for the next 7 days based on the last moving average value
            double lastMA3 = movingAverage3.Last();
            var forecastedSpending = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                forecastedSpending.Add(lastMA3);
            }

            // Display the forecast results
            var forecastTable = new Table();
            forecastTable.Border(TableBorder.Rounded);
            forecastTable.AddColumn("Date");
            forecastTable.AddColumn("Predicted Spending (VND)");

            for (int i = 0; i < 7; i++)
            {
                DateTime futureDate = today.AddDays(i + 1);
                forecastTable.AddRow(futureDate.ToString("dd/MM/yyyy"), $"{forecastedSpending[i]:N0}");
            }

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[cyan]=== Forecast for Next 7 Days ===[/]");
            AnsiConsole.Write(forecastTable);

            Console.WriteLine("\nPress any key to return to the saving menu...");
            Console.ReadKey();
        }

        static void SpendingSuggestions(string transactionFilePath)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Spending Suggestions ===");
            Console.ResetColor();

            DateTime today = DateTime.Now;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            // Step 1: Load Transactions
            var transactions = LoadTransactionsFromFiles(); // Use the correct function name

            // Step 2: Filter for Current Month
            var currentMonthTransactions = transactions
                .Where(t => t.Date >= startOfMonth && t.Date <= endOfMonth)
                .ToList();

            // Step 3: Aggregate Data
            double totalSpending = currentMonthTransactions
                .Where(t => t.Source == "Spending")
                .Sum(t => t.Amount);

            double totalIncome = currentMonthTransactions
                .Where(t => t.Source == "Income")
                .Sum(t => t.Amount);

            double totalLoan = currentMonthTransactions
                .Where(t => t.Source == "Loan")
                .Sum(t => t.Amount);

            double totalDebt = currentMonthTransactions
                .Where(t => t.Source == "Debit")
                .Sum(t => t.Amount);

            // Calculate net savings
            double netSavings = totalIncome - totalSpending + totalLoan - totalDebt;

            // Display financial breakdown
            Console.WriteLine($"Total Spending: {totalSpending:N0} VND");
            Console.WriteLine($"Total Income: {totalIncome:N0} VND");
            Console.WriteLine($"Total Loan Given: {totalLoan:N0} VND");
            Console.WriteLine($"Total Debt Taken: {totalDebt:N0} VND");
            Console.WriteLine($"Net Saving (Spending - Income + Loan - Debt): {netSavings:N0} VND");

            // Step 4: Calculate Suggestions
            if (netSavings > 0)
            {
                double suggestedSavingLow = netSavings * 0.1; // Save 10%
                double suggestedSavingHigh = netSavings * 0.3; // Save 20%

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSuggested Monthly Saving Goal: {suggestedSavingLow:N0} - {suggestedSavingHigh:N0} VND");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou are spending more than your income!");
                Console.WriteLine("Suggestions:");
                Console.WriteLine("- Reduce discretionary expenses.");
                Console.WriteLine("- Avoid additional loans.");
                Console.ResetColor();
            }
        }

    }
}