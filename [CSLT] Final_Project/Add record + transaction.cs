//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.IO;
//using System.Transactions;
//using static FinancialManager.Program;
//using static FinancialManager.Program.GamificationManager;
//using System.Globalization;
//using CsvHelper.Configuration;
//using CsvHelper;
//using System.Diagnostics.CodeAnalysis;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using static System.Collections.Specialized.BitVector32;
//using System.Text.RegularExpressions;
//namespace FinancialManager
//{
//    class Program
//    {
//        private static List<Transaction> transactions = new List<Transaction>();
//        private static List<Transaction> Transactions = new List<Transaction>();
//        static GamificationManager gamificationManager = new GamificationManager();
//        private static ChallengeManager challengeManager = new ChallengeManager();
//        private static string transactionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Transactions.csv");
//        static void Main(string[] args)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.BackgroundColor = ConsoleColor.Black; // Set background
//                Console.ForegroundColor = ConsoleColor.Cyan; // Set primary text color
//                Console.WriteLine(new string('=', 40));
//                Console.WriteLine("       Personal Finance App");
//                Console.WriteLine(new string('=', 40));
//                Console.ResetColor();

//                // Menu options with colors
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("[1] Home");
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("[2] Transaction");
//                Console.ForegroundColor = ConsoleColor.Blue;
//                Console.WriteLine("[3] Add Record");
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("[4] Budget");
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("[5] Saving");
//                Console.ForegroundColor = ConsoleColor.White;
//                Console.WriteLine("[6] Exit");
//                Console.ResetColor();

//                // Prompt
//                Console.WriteLine(new string('-', 40));
//                Console.Write("Select an option: ");

//                string choice = Console.ReadLine();

//                // Menu handling
//                switch (choice)
//                {
//                    case "1":
//                        ShowHome();
//                        break;
//                    case "2":
//                        Transactions.Clear();
//                        LoadTransactionsFromFile("Spending.csv");
//                        LoadTransactionsFromFile("Income.csv");
//                        LoadTransactionsFromFile("Loan.csv");
//                        LoadTransactionsFromFile("Debit.csv");
//                        SaveTransactionsToFile();
//                        ShowTransactions();
//                        break;
//                    case "3":
//                        AddRecord();
//                        break;
//                    case "4":
//                        ShowBudget();
//                        break;
//                    case "5":
//                        ShowSaving();
//                        break;
//                    case "6":
//                        Console.WriteLine("Exiting the application. Goodbye!");
//                        return; // Exit the program
//                    default:
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine("Invalid choice. Please try again.");
//                        Console.ResetColor();
//                        break;
//                }

//                if (choice != "6")
//                {
//                    Console.WriteLine("\nPress any key to return to the menu...");
//                    Console.ReadKey();
//                }
//            }
//        }

//        static void AddRecord()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Blue;
//                Console.WriteLine("=== Add Record ===");
//                Console.ResetColor();
//                Console.ForegroundColor = ConsoleColor.Blue;
//                Console.WriteLine("Select the type of record to add:");
//                Console.ResetColor();
//                Console.WriteLine("[1] Spending");
//                Console.WriteLine("[2] Income");
//                Console.WriteLine("[3] Loan");
//                Console.WriteLine("[4] Debit");
//                Console.WriteLine("[0] Return to Navigation");

//                string choice = Console.ReadLine();
//                // Declare the variables outside of the switch statement
//                decimal amount = 0;
//                string category = string.Empty;
//                string description = string.Empty;
//                string borrower = string.Empty;
//                string lender = string.Empty;
//                string note = string.Empty;
//                string session = string.Empty;
//                string method = string.Empty;
//                string flow = string.Empty;
//                string source = string.Empty;
//                int id = 0;
//                DateTime date = DateTime.Now;
//                switch (choice)
//                {
//                    case "1":
//                        AddSpendingRecord(out flow, out id, out source, out amount, out method, out category, out note, out date, out session);
//                        string spendingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spending.csv");
//                        string SpendingFileName = "Spending.csv";
//                        string SpendingFilePath = Path.Combine(spendingFilePath, SpendingFileName);
//                        WriteSpendingToCsv(SpendingFilePath, flow, id, source, amount, method, note, date);
//                        Console.ReadKey();
//                        break;
//                    case "2":
//                        AddIncomeRecord(out id, out session, out category, out source, out flow, out method, out date, out amount, out note);
//                        string incomeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Income.csv");
//                        string IncomeFileName = "Income.csv";
//                        string IncomeFilePath = Path.Combine(incomeFilePath, IncomeFileName);
//                        WriteIncomeToCsv(IncomeFilePath, id, source, flow, method, amount, note, date);
//                        Console.ReadKey();
//                        break;
//                    case "3":
//                        AddLoanRecord(out session, out id, out source, out flow, out method, out amount, out borrower, out note, out date);
//                        string loanFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Loan.csv");
//                        string LoanFileName = "Loan.csv";
//                        string LoanFilePath = Path.Combine(loanFilePath, LoanFileName);
//                        WriteLoanToCsv(LoanFilePath, method, id, source, flow, amount, borrower, note, date);
//                        Console.ReadKey();
//                        break;
//                    case "4":
//                        AddDebitRecord(out session, out id, out source, out flow, out method, out amount, out lender, out note, out date);
//                        string debitFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Debit.csv");
//                        string DebitFileName = "Debit.csv";
//                        string DebitFilePath = Path.Combine(debitFilePath, DebitFileName);
//                        WriteDebitToCsv(debitFilePath, method, id, source, flow, amount, lender, note, date);
//                        Console.ReadKey();
//                        break;
//                    case "0":
//                        return;
//                    default:
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine("Invalid choice. Returning to the main menu.");
//                        Console.ResetColor();
//                        break;
//                }
//                Console.WriteLine("\nPress any key to return to the Add Record menu...");
//                Console.ReadKey();
//            }
//        }

//        static void StoreTransaction(int id, string source, string flow, string method, DateTime date, string category, decimal amount, string note)
//        {
//            // Add a new transaction to the static list
//            Transactions.Add(new Transaction(id, source, flow, method, date, category, amount, note));
//            SaveTransactionsToFile();
//        }

//        static void AddSpendingRecord(out string flow, out int id, out string source, out decimal amount, out string method, out string category, out string note, out DateTime date, out string session)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Blue;
//                Console.WriteLine("=== Add Spending Record ===");
//                Console.ResetColor();
//                source = "Spending";
//                flow = "OUT";
//                id = ++currentId;
//                // Step 1: Read amount
//                amount = ReadDecimalInput();

//                // Step 2: Get payment method
//                method = GetMethod();

//                // Step 3: Get spending category
//                category = GetSpendingCategory();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                note = Console.ReadLine();

//                // Step 5: Get the date (adjusted for DD/MM format and default year)
//                date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                session = GetSessionOfDay();

//                // Step 7: Display result
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nSpending Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Category: {category}");
//                Console.WriteLine($"  - Note: {note}");

//                // Step 8: Confirm save
//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    StoreTransaction(id, source, flow, method, date, amount.ToString(), Convert.ToDecimal(note), "Spending");
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine("Record saved successfully!");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                    Console.WriteLine("Record not saved.");
//                    Console.ResetColor();
//                }

//                // Step 9: Ask to add another record
//                Console.WriteLine("\nDo you want to add another record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 0)
//                {
//                    break; // Exit to AddRecord menu
//                }
//            }
//        }
//        static void WriteSpendingToCsv(string SpendingFilePath, string flow, int id, string source, decimal amount, string method, string note, DateTime date)
//        {

//            try
//            {
//                if (!File.Exists(SpendingFilePath))
//                {
//                    using (File.Create(SpendingFilePath)) { }
//                }


//                List<Spending> spendings = new List<Spending>
//                {
//                    // Thêm thông tin vào danh sách chi tiêu
//                    new Spending
//                    {
//                        Amount = amount,
//                        Category = category,
//                        Description = description,
//                        Date = date
//                    }
//                };


//                var configSpendings = new CsvConfiguration(CultureInfo.InvariantCulture)
//                {
//                    HasHeaderRecord = false
//                };

//                using (StreamWriter streamWriter = new StreamWriter(SpendingFilePath, true))
//                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configSpendings))
//                {
//                    csvWriter.WriteRecords(spendings);
//                }

//                Console.WriteLine("Data written to CSV successfully.");
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        public class Spending
//        {
//            public decimal Amount { get; set; }
//            public string Category { get; set; }
//            public string Note { get; set; }
//            public string Method { get; set; }
//            public string Session { get; set; }
//            public DateTime Date { get; set; }
//            public string Source { get; set; }
//            public string Flow { get; set; }
//            public int Id { get; set; }
//        }

//        static void AddIncomeRecord(out int id, out string session, out string category, out string source, out string flow, out string method, out DateTime date, out decimal amount, out string note)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("=== Add Income Record ===");
//                Console.ResetColor();
//                id = ++currentId;
//                source = "Income";
//                flow = "IN";
//                amount = ReadDecimalInput();

//                category = GetIncomeCategory();

//                method = GetMethod();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                note = Console.ReadLine();

//                // Step 5: Get the date (adjusted for DD/MM format and default year)
//                date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                session = GetSessionOfDay();

//                // Step 7: Display result
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nIncome Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Category: {category}");
//                Console.WriteLine($"  - Note: {note}");

//                // Step 8: Confirm save
//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    StoreTransaction(id, source, flow, method, date, amount.ToString(), Convert.ToDecimal(note), "Income");
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine("Record saved successfully!");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                    Console.WriteLine("Record not saved.");
//                    Console.ResetColor();
//                }

//                // Step 9: Ask to add another record
//                Console.WriteLine("\nDo you want to add another record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 0)
//                {
//                    break; // Exit the loop to return to AddRecord()
//                }
//            }
//        }
//        static void WriteIncomeToCsv(string IncomeFilePath, int id, string source, string flow, string method, decimal amount, string note, DateTime date)
//        {

//            try
//            {
//                if (!File.Exists(IncomeFilePath))
//                {
//                    using (File.Create(IncomeFilePath)) { }
//                }


//                List<Income> incomes = new List<Income>
//                {
//                    // Thêm thông tin vào danh sách chi tiêu
//                    new Income
//                    {
//                        Id = id,
//                        Source = source,
//                        Flow = flow,
//                        Method = method,
//                        Date = date,
//                        Amount = amount,
//                        Note = note,
//                    }
//                };


//                var configIncomes = new CsvConfiguration(CultureInfo.InvariantCulture)
//                {
//                    HasHeaderRecord = false
//                };

//                using (StreamWriter streamWriter = new StreamWriter(IncomeFilePath, true))
//                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configIncomes))
//                {
//                    csvWriter.WriteRecords(incomes);
//                }

//                Console.WriteLine("Data written to CSV successfully.");
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        public class Income
//        {
//            public decimal Amount { get; set; }
//            public string Note { get; set; }
//            public string FLow { get; set; }
//            public string Method { get; set; }
//            public string Source { get; set; }
//            public string Flow { get; set; }
//            public int Id { get; set; }
//            public DateTime Date { get; set; }
//        }
//        static void AddLoanRecord(out string session, out int id, out string source, out string flow, out string method, out decimal amount, out string borrower, out string note, out DateTime date)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("=== Add Loan Record ===");
//                Console.ResetColor();
//                id = ++currentId;
//                source = "Loan";
//                flow = "OUT";
//                amount = ReadDecimalInput();

//                Console.Write("Enter who you lend to: ");
//                borrower = GetValidName();

//                method = GetMethod();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                note = Console.ReadLine();

//                // Step 5: Get the date (adjusted for DD/MM format and default year)
//                date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                session = GetSessionOfDay();

//                // Step 7: Display result
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nLoan Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Borrower: {borrower}");
//                Console.WriteLine($"  - Note: {note}");

//                // Step 8: Confirm save
//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    StoreTransaction(id, source, flow, method, date, amount.ToString(), Convert.ToDecimal(note), "Loan");
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine("Record saved successfully!");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                    Console.WriteLine("Record not saved.");
//                    Console.ResetColor();
//                }

//                // Step 9: Ask to add another record
//                Console.WriteLine("\nDo you want to add another record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 0)
//                {
//                    break; // Exit the loop to return to AddRecord()
//                }
//            }
//        }
//        static void WriteLoanToCsv(string LoanFilePath, string method, int id, string source, string flow, decimal amount, string borrower, string note, DateTime date)
//        {

//            try
//            {
//                if (!File.Exists(LoanFilePath))
//                {
//                    using (File.Create(LoanFilePath)) { }
//                }


//                List<Loan> loans = new List<Loan>
//                {
//                    // Thêm thông tin vào danh sách chi tiêu
//                    new Loan
//                    {
//                        Id = id,
//                        Source = source,
//                        Flow = flow,
//                        Method = method,
//                        Date = date,
//                        Amount = amount,
//                        Note = note,
//                    }
//                };
//                ///


//                var configIncomes = new CsvConfiguration(CultureInfo.InvariantCulture)
//                {
//                    HasHeaderRecord = false
//                };

//                using (StreamWriter streamWriter = new StreamWriter(LoanFilePath, true))
//                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configIncomes))
//                {
//                    csvWriter.WriteRecords(loans);
//                }

//                Console.WriteLine("Data written to CSV successfully.");
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        public class Loan
//        {
//            public decimal Amount { get; set; }
//            public string borrower { get; set; }
//            public string Note { get; set; }
//            public DateTime Date { get; set; }
//            public string Flow {  get; set; }
//            public string Method { get; set; }
//            public int Id { get; set; }
//            public string Source { get; set; }
//        }
//        static void AddDebitRecord(string source, string session, string method, int id, string flow, out decimal amount, out string lender, out string note, out DateTime date)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("=== Add Debit Record ===");
//                Console.ResetColor();
//                id = currentId++;
//                source = "Debit";
//                flow = "IN";
//                amount = ReadDecimalInput();

//                Console.Write("Enter who you borrow from: ");
//                lender = GetValidName();

//                method = GetMethod();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                note = Console.ReadLine();

//                // Step 5: Get the date (adjusted for DD/MM format and default year)
//                date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                session = GetSessionOfDay();

//                // Step 7: Display result
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nDebit Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Borrower: {lender}");
//                Console.WriteLine($"  - Note: {note}");

//                // Step 8: Confirm save
//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    StoreTransaction(id, source, flow, method, date, amount.ToString(), Convert.ToDecimal(note), "Debit");
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine("Record saved successfully!");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                    Console.WriteLine("Record not saved.");
//                    Console.ResetColor();
//                }

//                // Step 9: Ask to add another record
//                Console.WriteLine("\nDo you want to add another record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 0)
//                {
//                    break; // Exit the loop to return to AddRecord()
//                }
//            }
//        }
//        static void WriteDebitToCsv(string DebitFilePath, int id, string source, string flow, string method, decimal amount, string lender, string note, DateTime date)
//        {

//            try
//            {
//                if (!File.Exists(DebitFilePath))
//                {
//                    using (File.Create(DebitFilePath)) { }
//                }


//                List<Debit> debits = new List<Debit>
//                {
//                    // Thêm thông tin vào danh sách chi tiêu
//                    new Debit
//                    {
//                        Id = id,
//                        Source = source,
//                        Flow = flow,
//                        Method = method,
//                        Date = date,
//                        Amount = amount,
//                        Note = note,
//                    }
//                };


//                var configIncomes = new CsvConfiguration(CultureInfo.InvariantCulture)
//                {
//                    HasHeaderRecord = false
//                };

//                using (StreamWriter streamWriter = new StreamWriter(DebitFilePath, true))
//                using (CsvWriter csvWriter = new CsvWriter(streamWriter, configIncomes))
//                {
//                    csvWriter.WriteRecords(debits);
//                }

//                Console.WriteLine("Data written to CSV successfully.");
//            }

//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        public class Debit
//        {
//            public decimal Amount { get; set; }
//            public string lender { get; set; }
//            public string Note { get; set; }
//            public DateTime Date { get; set; }
//            public string Flow { get; set; }
//            public string Method { get; set; }
//            public int Id { get; set; }
//            public string Source { get; set; }
//        }

//        static decimal ReadDecimalInput()
//        {
//            while (true)
//            {
//                string input = Console.ReadLine().Trim().ToLower();
//                try
//                {
//                    decimal multiplier = 1;
//                    if (input.EndsWith("k")) // Handle 'k' for thousands
//                    {
//                        input = input.Substring(0, input.Length - 1);
//                        multiplier = 1000;
//                    }
//                    else if (input.EndsWith("m")) // Handle 'm' for millions
//                    {
//                        input = input.Substring(0, input.Length - 1);
//                        multiplier = 1_000_000;
//                    }
//                    else if (input.EndsWith("b")) // Handle 'b' for billions
//                    {
//                        input = input.Substring(0, input.Length - 1);
//                        multiplier = 1_000_000_000;
//                    }

//                    // Remove non-numeric characters (e.g., "vnd", ",", ".")
//                    string sanitizedInput = input.Replace(".", "").Replace(",", "").Replace("vnd", "").Trim();

//                    if (decimal.TryParse(sanitizedInput, out decimal amount))
//                    {
//                        return amount * multiplier;
//                    }

//                    throw new FormatException("Invalid numeric format.");
//                }
//                catch (FormatException)
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid amount format. Please try again (e.g., 30k, 3m, 3b, 30.000).");
//                    Console.ResetColor();
//                }
//            }
//        }

//        static string FormatCurrency(decimal amount)
//        {
//            // Format as Vietnamese currency with dot separator and add ₫ symbol
//            return $"{amount:N0}".Replace(",", ".");
//        }

//        static DateTime GetDateInput(string prompt)
//        {
//            Console.Write(prompt);
//            string input = Console.ReadLine();

//            if (string.IsNullOrEmpty(input))
//            {
//                return DateTime.Now;
//            }

//            if (DateTime.TryParse(input, out DateTime date))
//            {
//                return date;
//            }

//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine("Invalid date. Using today's date instead.");
//            Console.ResetColor();
//            return DateTime.Now;
//        }
//        static int GetNextId(string fileName)
//        {
//            if (!File.Exists(fileName))
//            {
//                return 1; // Start ID from 1 if file doesn't exist
//            }

//            // Read all lines from the file
//            var lines = File.ReadAllLines(fileName);

//            // Extract the last ID (skip the header)
//            if (lines.Length <= 1)
//            {
//                return 1;
//            }

//            // Get the ID of the last record
//            var lastLine = lines[^1];
//            var lastId = int.Parse(lastLine.Split(',')[0]);
//            return lastId + 1;
//        }
//        static int GetYorN_Selection()
//        {
//            while (true)
//            {
//                Console.Write("Your selection: ");
//                if (int.TryParse(Console.ReadLine(), out int selection))
//                {
//                    if (selection == 1 || selection == 0)
//                        return selection;

//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid selection. Please choose 0 or 1.");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Make sure to enter an integer value!");
//                    Console.ResetColor();
//                }
//                Console.Clear();
//            }
//        }

//        static void ShowBudget()
//        {
//            Console.Clear();
//            Console.WriteLine("=== Budget ===");
//            Console.WriteLine("Feature not implemented yet.");
//            Console.WriteLine("Press any key to return to the menu...");
//            Console.ReadKey();
//        }

//        static void ShowSaving()
//        {
//            Console.Clear();
//            Console.WriteLine("=== Saving ===");
//            Console.WriteLine("Feature not implemented yet.");
//            Console.WriteLine("Press any key to return to the menu...");
//            Console.ReadKey();
//        }

//        static void SaveToFile(string transaction)
//        {
//            try
//            {
//                File.AppendAllText("transactions.txt", transaction + Environment.NewLine);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error saving transaction: {ex.Message}");
//            }
//        }
//        static string GetMethod()
//        {
//            string method = "";
//            Console.WriteLine("Spend by (IN three following types: Banking, Cash or E-Wallet)");
//            Console.WriteLine("1. Banking");
//            Console.WriteLine("2. Cash");
//            Console.WriteLine("3. E-Wallet");

//            do
//            {
//                Console.Write("Your selection: ");
//                if (int.TryParse(Console.ReadLine(), out int selection))
//                {
//                    if (selection == 1)
//                    {
//                        method = "Banking";
//                        break;
//                    }
//                    else if (selection == 2)
//                    {
//                        method = "Cash";
//                        break;
//                    }
//                    else if (selection == 3)
//                    {
//                        method = "E-Wallet";
//                        break;
//                    }
//                    else
//                    {
//                        Console.WriteLine("Invalid selection. Please choose 1, 2, or 3.");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Make sure to enter an integer value!");
//                }
//            } while (method == "");
//            return method;
//        }

//        static string GetSpendingCategory()
//        {
//            string category = "";
//            Console.WriteLine("Enter category of spending (adjusted for typical spending habits):");
//            Console.WriteLine("1. Food & Groceries"); // Essential
//            Console.WriteLine("2. Housing & Utilities"); // Essential
//            Console.WriteLine("3. Transportation"); // Essential for many
//            Console.WriteLine("4. Healthcare"); // Important
//            Console.WriteLine("5. Education"); // Investment
//            Console.WriteLine("6. Personal & Family Care"); // Broad category
//            Console.WriteLine("7. Social & Entertainment"); // Leisure
//            Console.WriteLine("8. Savings & Investments"); // Future-oriented
//            Console.WriteLine("9. Debt Payments"); // Financial responsibility

//            do
//            {
//                Console.Write("Your selection: ");
//                if (int.TryParse(Console.ReadLine(), out int selection))
//                {
//                    switch (selection)
//                    {
//                        case 1:
//                            category = "Food & Groceries";
//                            break;
//                        case 2:
//                            category = "Housing & Utilities";
//                            break;
//                        case 3:
//                            category = "Transportation";
//                            break;
//                        case 4:
//                            category = "Healthcare";
//                            break;
//                        case 5:
//                            category = "Education";
//                            break;
//                        case 6:
//                            category = "Personal & Family Care";
//                            break;
//                        case 7:
//                            category = "Social & Entertainment";
//                            break;
//                        case 8:
//                            category = "Savings & Investments";
//                            break;
//                        case 9:
//                            category = "Debt Payments";
//                            break;
//                        default:
//                            Console.WriteLine("Invalid selection. Please choose from 1 to 9.");
//                            break;
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Make sure to enter an integer value!");
//                }
//            } while (category == "");
//            return category;
//        }

//        static string GetIncomeCategory()
//        {
//            string category = "";
//            Console.WriteLine("Enter income category:");
//            Console.WriteLine("1. Family Support");
//            Console.WriteLine("2. Main Job (Full-time/Part-time)");
//            Console.WriteLine("3. Freelancing/Gig Work");
//            Console.WriteLine("4. Scholarship/Grant");
//            Console.WriteLine("5. Investments/Interest");
//            Console.WriteLine("6. Other");

//            do
//            {
//                Console.Write("Your selection: ");
//                if (int.TryParse(Console.ReadLine(), out int selection))
//                {
//                    switch (selection)
//                    {
//                        case 1:
//                            category = "Family Support";
//                            break;
//                        case 2:
//                            category = "Main Job";
//                            break;
//                        case 3:
//                            category = "Freelancing/Gig Work";
//                            break;
//                        case 4:
//                            category = "Scholarship/Grant";
//                            break;
//                        case 5:
//                            category = "Investments/Interest";
//                            break;
//                        case 6:
//                            category = "Other";
//                            break;
//                        default:
//                            Console.WriteLine("Invalid selection. Please choose from 1 to 6.");
//                            break;
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Make sure to enter an integer value!");
//                }
//            } while (category == "");

//            // Optionally, get more details for "Other" category:
//            if (category == "Other")
//            {
//                Console.Write("Please specify the 'Other' income source: ");
//                category = Console.ReadLine(); // Overwrite "Other" with the user's input
//            }


//            return category;
//        }

//        static string GetValidName()
//        {
//            string name = "";

//            do
//            {
//                name = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(name)) // Check for empty or whitespace-only input
//                {
//                    Console.WriteLine("Name cannot be empty. Please enter a valid name.");
//                    Console.Write("Retry: ");
//                }
//                else if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$")) // Use regex for validation
//                {
//                    Console.WriteLine("Name contains invalid characters. Please use letters and spaces only.");
//                    Console.Write("Retry: ");
//                }

//            } while (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[a-zA-Z\s]+$"));

//            return name;
//        }

//        static string FormatCurrency(decimal amount)
//        {
//            // Format as Vietnamese currency with dot separator and add ₫ symbol
//            return $"{amount:N0}".Replace(",", ".");
//        }

//        static string GetSessionOfDay()
//        {
//            Console.WriteLine("\nPlease choose a session of the day:");
//            Console.WriteLine("  1. Morning (6:00-10:59)");
//            Console.WriteLine("  2. Midday (11:00-12:59)");
//            Console.WriteLine("  3. Afternoon (13:00-17:59)");
//            Console.WriteLine("  4. Evening (18:00-23:59)");
//            Console.WriteLine("  5. Night (0:00-5:59)");
//            Console.WriteLine("  0. Null (no specific session)");

//            while (true)
//            {
//                Console.Write("Your selection: ");
//                string input = Console.ReadLine();

//                switch (input)
//                {
//                    case "1": return "Morning (6:00-10:59)";
//                    case "2": return "Midday (11:00-12:59)";
//                    case "3": return "Afternoon (13:00-17:59)";
//                    case "4": return "Evening (18:00-23:59)";
//                    case "5": return "Night (0:00-5:59)";
//                    case "0": return "No specific session";
//                    default:
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine("Invalid selection. Please choose a valid option.");
//                        Console.ResetColor();
//                        break;
//                }
//            }
//        }

//        static DateTime GetDateInput(string prompt)
//        {
//            Console.Write(prompt);
//            string input = Console.ReadLine();

//            if (string.IsNullOrWhiteSpace(input))
//            {
//                return DateTime.Now;
//            }

//            // Try parsing input as DD/MM with the current year
//            if (DateTime.TryParseExact(input + $"/{DateTime.Now.Year}", "dd/MM/yyyy",
//                                       null, System.Globalization.DateTimeStyles.None, out DateTime date))
//            {
//                return date;
//            }

//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine("Invalid date. Using today's date instead.");
//            Console.ResetColor();
//            return DateTime.Now;
//        }
//        static void ShowHome()
//        {
//            Console.Clear();
//            Console.WriteLine("    /\\_/\\  ");
//            Console.WriteLine("   ( o.o ) ");
//            Console.WriteLine("    > ^ <  ");
//            Console.WriteLine("~Meow, meow~");
//            Console.WriteLine("Press any key to continue");
//            Console.ReadKey();
//            while (true)
//            {
//                Console.Clear();
//                Console.WriteLine("===== Financial system =====");
//                Console.WriteLine("1. Transaction record");
//                Console.WriteLine("2. Check level up and unlocked badges");
//                Console.WriteLine("3. Create challenge");
//                Console.WriteLine("4. Join challenge");
//                Console.WriteLine("5. Compare progress");
//                Console.WriteLine("6. Main menu");
//                Console.Write("Select an option: ");
//                string choice = Console.ReadLine();

//                switch (choice)
//                {
//                    case "1":
//                        // Ghi chép giao dịch và cộng điểm XP
//                        gamificationManager.EarnXP("Transaction record", 100);
//                        gamificationManager.CheckLevelUp();
//                        break;
//                    case "2":
//                        // Hiển thị cấp độ và huy chương của người dùng
//                        gamificationManager.ShowUserInfo();
//                        Console.WriteLine("Press any key to return...");
//                        Console.ReadKey();
//                        break;
//                    case "3":
//                        // Tạo thử thách tài chính
//                        Console.Write("Enter challenge name: ");
//                        string challengeName = Console.ReadLine();
//                        Console.Write("Enter goal (amount): ");
//                        if (int.TryParse(Console.ReadLine(), out int targetAmount))
//                        {
//                            challengeManager.CreateChallenge(challengeName, targetAmount);
//                        }
//                        else
//                        {
//                            Console.WriteLine("Invalid amount. Press any key to retry...");
//                            Console.ReadKey();
//                        }
//                        break;
//                    case "4":
//                        // Tham gia thử thách tài chính
//                        Console.Write("Enter your challenge name: ");
//                        string challengeToJoin = Console.ReadLine();
//                        challengeManager.JoinChallenge(challengeToJoin);
//                        break;
//                    case "5":
//                        // So sánh tiến độ giữa 2 người dùng
//                        challengeManager.ViewAllChallenges();
//                        Console.WriteLine("Press any key to return...");
//                        Console.ReadKey();
//                        break;
//                    case "6":
//                        // Thoát chương trình
//                        return;
//                    default:
//                        Console.WriteLine("Invalid choice. Press any key to retry...");
//                        Console.ReadKey();
//                        break;
//                }
//            }
//        }
//        public class GamificationManager
//        {
//            private int userXP = 0;
//            private int userLevel = 1;
//            private List<string> badges = new List<string>();

//            // Earn XP for completing tasks
//            public void EarnXP(string task, int xp)
//            {
//                Console.WriteLine($"Mission complete: {task}. Earned {xp} XP.");
//                userXP += xp;
//            }

//            // Check and level up the user
//            public void CheckLevelUp()
//            {
//                int requiredXPForLevelUp = userLevel * 500; // 500 XP per level
//                if (userXP >= requiredXPForLevelUp)
//                {
//                    userLevel++;
//                    Console.WriteLine($"Congratulations! You reached level {userLevel}.");
//                    UnlockBadge($"Level {userLevel} Badge");
//                }
//                else
//                {
//                    Console.WriteLine($"Current XP: {userXP}. You need {requiredXPForLevelUp - userXP} XP to level up.");
//                }
//            }

//            // Unlock badges when goals are achieved
//            public void UnlockBadge(string badge)
//            {
//                badges.Add(badge);
//                Console.WriteLine($"Badge unlocked: {badge}");
//            }

//            // Show user information (level, XP, badges)
//            public void ShowUserInfo()
//            {
//                Console.WriteLine($"Level: {userLevel}, XP: {userXP}");
//                Console.WriteLine("Unlocked Badges:");
//                foreach (var badge in badges)
//                {
//                    Console.WriteLine($"- {badge}");
//                }
//            }
//        }

//        public class ChallengeManager
//        {
//            private List<FinancialChallenge> challenges = new List<FinancialChallenge>();

//            // Create a financial challenge
//            public void CreateChallenge(string challengeName, int targetAmount)
//            {
//                var challenge = new FinancialChallenge(challengeName, targetAmount);
//                challenges.Add(challenge);
//                Console.WriteLine($"Challenge '{challengeName}' has been created with a target of {targetAmount}!");
//            }

//            // Join a financial challenge
//            public void JoinChallenge(string challengeName)
//            {
//                var challenge = challenges.Find(ch => ch.Name == challengeName);
//                if (challenge != null)
//                {
//                    challenge.Join();
//                    Console.WriteLine($"You have joined the challenge: {challengeName}");
//                }
//                else
//                {
//                    Console.WriteLine($"Challenge '{challengeName}' not found.");
//                }
//            }

//            // View all challenges
//            public void ViewAllChallenges()
//            {
//                Console.WriteLine("Active Challenges:");
//                foreach (var challenge in challenges)
//                {
//                    challenge.DisplayChallengeStatus();
//                }
//            }
//        }

//        public class FinancialChallenge
//        {
//            public string Name { get; }
//            public int TargetAmount { get; }
//            private bool isJoined = false;

//            public FinancialChallenge(string name, int targetAmount)
//            {
//                Name = name;
//                TargetAmount = targetAmount;
//            }

//            public void Join()
//            {
//                isJoined = true;
//            }

//            public void DisplayChallengeStatus()
//            {
//                Console.WriteLine($"Challenge: {Name}, Target: {TargetAmount}, Joined: {isJoined}");
//            }
//        }

//        static void LoadTransactionsFromFile(string type)
//        {
//            string filepath = transactionFilePath + type;
//            if (!File.Exists(filepath))
//            {
//                Console.WriteLine("Transaction file not found.");
//                return;
//            }

//            string[] rows = File.ReadAllLines(filepath);


//            for (int i = 1; i < rows.Length; i++) // Assuming first row is header
//            {
//                string[] columns = rows[i].Split(',');

//                if (columns.Length < 5)
//                {
//                    Console.WriteLine($"Invalid data on row {i + 1}. Skipping...");
//                    continue;
//                }

//                try
//                {
//                    string types = columns[0].Trim();
//                    decimal amount = decimal.Parse(columns[1].Trim());
//                    string category = columns[2].Trim();
//                    string description = columns[3].Trim();
//                    DateTime date = DateTime.Parse(columns[4].Trim());

//                    Transactions.Add(new Transaction(amount, category, description, date, types));
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error parsing row {i + 1}: {ex.Message}. Skipping...");
//                }
//            }
//        }

//        static void SaveTransactionsToFile()
//        {
//            if (Transactions == null || Transactions.Count == 0)
//            {
//                Console.WriteLine("No transactions to save.");
//                return;
//            }

//            List<Transaction> spendingTrans = new List<Transaction>();
//            List<Transaction> incomeTrans = new List<Transaction>();
//            List<Transaction> loanTrans = new List<Transaction>();
//            List<Transaction> debitTrans = new List<Transaction>();
//            // Add each transaction as a row
//            foreach (var transaction in Transactions)
//            {
//                switch (transaction.Type)
//                {
//                    case "Spending":
//                        spendingTrans.Add(transaction);
//                        break;
//                    case "Income":
//                        incomeTrans.Add(transaction);
//                        break;
//                    case "Loan":
//                        loanTrans.Add(transaction);
//                        break;
//                    case "Debit":
//                        debitTrans.Add(transaction);
//                        break;

//                }
//            }
//            SaveFileByType("Spending", spendingTrans);
//            SaveFileByType("Income", incomeTrans);
//            SaveFileByType("Loan", loanTrans);
//            SaveFileByType("Debit", debitTrans);
//        }

//        static void SaveFileByType(string type, List<Transaction> transactions)
//        {
//            string fileName = type + ".csv";


//            string filePath = transactionFilePath + fileName;
//            List<string> rows = new List<string>();

//            // Add header row
//            rows.Add("Type,Amount,Category,Description,Date");

//            // Add each transaction as a row
//            foreach (var transaction in transactions)
//            {
//                rows.Add($"{transaction.Type},{transaction.Amount},{transaction.Category},{transaction.Description},{transaction.Date:yyyy-MM-dd}");
//            }

//            // Write to file, overwriting any existing content
//            File.WriteAllLines(filePath, rows);

//            Console.WriteLine(type + " transactions saved to file successfully.");

//        }

//        static void ShowTransactions()
//        {
//            Console.Clear();
//            Console.WriteLine("=== View Transactions ===");
//            Console.WriteLine("Select filter by time:");
//            Console.WriteLine("[1] This Week");
//            Console.WriteLine("[2] This Month");
//            Console.WriteLine("[3] This Year");
//            Console.WriteLine("[4] Custom Date Range");
//            Console.WriteLine("[5] Show all");
//            string filterChoice = Console.ReadLine();

//            DateTime startDate = DateTime.MinValue;
//            DateTime endDate = DateTime.MaxValue;

//            switch (filterChoice)
//            {
//                case "1":
//                    startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
//                    endDate = DateTime.Now;
//                    break;
//                case "2":
//                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
//                    endDate = DateTime.Now;
//                    break;
//                case "3":
//                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
//                    endDate = DateTime.Now;
//                    break;
//                case "4":
//                    Console.Write("Enter start date (yyyy-mm-dd): ");
//                    while (!DateTime.TryParse(Console.ReadLine(), out startDate))
//                    {
//                        Console.Write("Invalid start date format. Please enter in yyyy-mm-dd format: ");
//                    }

//                    Console.Write("Enter end date (yyyy-mm-dd): ");
//                    while (!DateTime.TryParse(Console.ReadLine(), out endDate))
//                    {
//                        Console.Write("Invalid end date format. Please enter in yyyy-mm-dd format: ");
//                    }
//                    break;
//                case "5":
//                    // No filtering needed
//                    break;
//                default:
//                    Console.WriteLine("Invalid choice. Showing all records.");
//                    break;
//            }

//            // Filter the transactions by date range
//            var filteredTransactions = Transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();

//            Console.WriteLine("Select sort order:");
//            Console.WriteLine("[1] Sort by Amount (High to Low)");
//            Console.WriteLine("[2] Sort by Amount (Low to High)");
//            Console.WriteLine("[3] Sort by Date (Newest first)");
//            Console.WriteLine("[4] Sort by Date (Oldest first)");
//            string sortChoice = Console.ReadLine();

//            switch (sortChoice)
//            {
//                case "1":
//                    filteredTransactions = filteredTransactions.OrderByDescending(t => t.Amount).ToList();
//                    break;
//                case "2":
//                    filteredTransactions = filteredTransactions.OrderBy(t => t.Amount).ToList();
//                    break;
//                case "3":
//                    filteredTransactions = filteredTransactions.OrderByDescending(t => t.Date).ToList();
//                    break;
//                case "4":
//                    filteredTransactions = filteredTransactions.OrderBy(t => t.Date).ToList();
//                    break;
//                default:
//                    Console.WriteLine("Invalid sort choice. Showing unsorted transactions.");
//                    break;
//            }

//            Console.WriteLine("\nFiltered and Sorted Transactions:");
//            foreach (var transaction in filteredTransactions)
//            {
//                Console.WriteLine(transaction);
//            }

//            Console.WriteLine("\nPress any key to return...");
//            Console.ReadKey();
//        }

//        private static int currentId = 0; // Static field to track the current max ID

//        public class Transaction
//        {
//            public string Type { get; set; } // "Income" or "Expense"
//            public decimal Amount { get; set; }
//            public string Category { get; set; }
//            public string Note { get; set; }
//            public DateTime Date { get; set; }
//            public string Method { get; set; }
//            public string Session { get; set; }
//            public int Id { get; private set; } // Unique ID for each transaction        public string Source { get; set; }
//            public string Flow { get; set; }
//            public string Source { get; set; }

//            //ID	Source	Flow	Method	Date	Category	Amount
//            public Transaction(int id, string source, string flow, string method, DateTime date, string category, decimal amount, string note)
//            {
//                Id = id;
//                Source = source;
//                Flow = flow;
//                Method = method;
//                Date = date;
//                Category = category;
//                Amount = amount;
//                Note = note;
//            }

//            public override string ToString()
//            {
//                return $"{Date.ToShortDateString()} - {Type} - {Amount:C} - {Category} - {Note}";
//            }
//        }
//    }
//}
