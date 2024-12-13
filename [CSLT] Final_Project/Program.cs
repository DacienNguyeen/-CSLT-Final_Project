//using System;
//using System.Globalization;
//using System.Text.RegularExpressions;
//using CsvHelper.Configuration;
//using CsvHelper;
//using System.IO;
//using System.Linq;
//using static System.Net.Mime.MediaTypeNames;

//namespace PersonalFinanceApp
//{
//    class Program2
//    {
//        private static List<Transaction> transactions = new List<Transaction>();
//        private static List<Transaction> Transactions = new List<Transaction>();
//        public class Transaction
//        {
//            public int ID { get; set; }
//            public string Source { get; set; }
//            public string Flow { get; set; } // "IN" or "OUT"
//            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
//            public DateTime Date { get; set; }
//            public string Category { get; set; }
//            public double Amount { get; set; }
//            public string Id { get; set; }
//            public string Note { get; set; }  
//            public string Type { get; set; }
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

//        public class Spending
//        {
//            public int ID { get; set; }
//            public string Session { get; set; }
//            public DateTime Date { get; set; }
//            public double Amount { get; set; }
//            public string Method { get; set; }
//            public string Category { get; set; }
//            public string Note { get; set; }
//        }

//        public class Income
//        {
//            public int ID { get; set; }
//            public string Session { get; set; }
//            public DateTime Date { get; set; }
//            public double Amount { get; set; }
//            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
//            public string Category { get; set; }
//            public string Note { get; set; }
//        }

//        public class Loan
//        {
//            public int ID { get; set; }
//            public string Session { get; set; }
//            public DateTime Date { get; set; }
//            public double Amount { get; set; }
//            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
//            public string Borrower { get; set; }
//            public string Note { get; set; }
//        }
//        public class Debit
//        {
//            public int ID { get; set; }
//            public string Session { get; set; }
//            public DateTime Date { get; set; }
//            public double Amount { get; set; }
//            public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
//            public string Lender { get; set; }
//            public string Note { get; set; }

//        }

//        static void Main(string[] args)
//        {
//            NavigationBar();
//        }

//        static void NavigationBar()
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

//        static void ShowHome()
//        {
//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Cyan;
//            Console.WriteLine("=== Home ===");
//            Console.ResetColor();
//            Console.WriteLine("Gamify your expense tracking by maintaining a streak.");
//            Console.WriteLine("TODO: Add streak tracking logic using System.DateTime");
//        }

//        static void ShowTransactions()
//        {
//            Console.Clear();
//            Console.WriteLine("=== View Transactions ===");

//            // Select a filter by time
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
//                    Console.Write("Enter start date (dd/mm/yyyy): ");
//                    while (!DateTime.TryParse(Console.ReadLine(), out startDate))
//                    {
//                        Console.Write("Invalid start date format. Please enter in dd/mm/yyyy format: ");
//                    }

//                    Console.Write("Enter end date (dd/mm/yyyy): ");
//                    while (!DateTime.TryParse(Console.ReadLine(), out endDate))
//                    {
//                        Console.Write("Invalid end date format. Please enter in dd/mm/yyyy format: ");
//                    }
//                    break;
//                case "5":
//                    // No filtering needed
//                    break;
//                default:
//                    Console.WriteLine("Invalid choice. Showing all records.");
//                    break;
//            }

//            // Load transactions from all CSV files
//            var transactions = LoadTransactionsFromFiles();

//            // Apply the date filter and sort by date (latest to oldest)
//            var filteredTransactions = transactions
//                .Where(t => t.Date >= startDate && t.Date <= endDate)
//                .OrderByDescending(t => t.Date)
//                .ToList();

//            // Display the transactions in a tabular format
//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Cyan;
//            Console.WriteLine($"=== Transactions ({filteredTransactions.Count} records) ===");
//            Console.ResetColor();

//            Console.WriteLine("ID | Session  | Date       | Flow | Amount    | Source");
//            Console.WriteLine("---|----------|------------|------|-----------|--------");

//            int sequentialId = 1;
//            foreach (var transaction in filteredTransactions)
//            {
//                string session = GetSessionOfDay(transaction.Date);
//                Console.WriteLine($"{sequentialId++,3} | {session,-8} | {transaction.Date:dd/MM/yyyy} | {transaction.Flow,-4} | {transaction.Amount,9:N0} | {transaction.Source}");
//            }

//            Console.WriteLine("\nPress any key to return...");
//            Console.ReadKey();
//        }

//        static List<Transaction> LoadTransactionsFromFiles()
//        {
//            var transactions = new List<Transaction>();

//            // Load Spending.csv
//            if (File.Exists("Spending.csv"))
//            {
//                using (var reader = new StreamReader("Spending.csv"))
//                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
//                {
//                    var spendings = csv.GetRecords<Spending>().ToList();
//                    foreach (var spending in spendings)
//                    {
//                        transactions.Add(new Transaction
//                        {
//                            ID = spending.ID,
//                            Source = "Spending",
//                            Flow = "OUT",
//                            Method = spending.Method,
//                            Date = spending.Date,
//                            Amount = spending.Amount
//                        });
//                    }
//                }
//            }

//            // Load Income.csv
//            if (File.Exists("Income.csv"))
//            {
//                using (var reader = new StreamReader("Income.csv"))
//                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
//                {
//                    var incomes = csv.GetRecords<Income>().ToList();
//                    foreach (var income in incomes)
//                    {
//                        transactions.Add(new Transaction
//                        {
//                            ID = income.ID,
//                            Source = "Income",
//                            Flow = "IN",
//                            Method = income.Method,
//                            Date = income.Date,
//                            Amount = income.Amount
//                        });
//                    }
//                }
//            }

//            // Load Loan.csv
//            if (File.Exists("Loan.csv"))
//            {
//                using (var reader = new StreamReader("Loan.csv"))
//                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
//                {
//                    var loans = csv.GetRecords<Loan>().ToList();
//                    foreach (var loan in loans)
//                    {
//                        transactions.Add(new Transaction
//                        {
//                            ID = loan.ID,
//                            Source = "Loan",
//                            Flow = "OUT",
//                            Method = loan.Method,
//                            Date = loan.Date,
//                            Amount = loan.Amount
//                        });
//                    }
//                }
//            }

//            // Load Debit.csv
//            if (File.Exists("Debit.csv"))
//            {
//                using (var reader = new StreamReader("Debit.csv"))
//                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
//                {
//                    var debits = csv.GetRecords<Debit>().ToList();
//                    foreach (var debit in debits)
//                    {
//                        transactions.Add(new Transaction
//                        {
//                            ID = debit.ID,
//                            Source = "Debit",
//                            Flow = "IN",
//                            Method = debit.Method,
//                            Date = debit.Date,
//                            Amount = debit.Amount
//                        });
//                    }
//                }
//            }

//            return transactions;
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
//<<<<<<< HEAD:[CSLT] Final_Project/Program.cs
//                    // Generate a new ID for the record
//                    int id = GetNextId("Spending.csv");

//                    // Save the record
//                    StoreSpendingRecord(id, session, date, (double)amount, method, category, note);
//=======
//                    StoreTransaction(id, source, flow, method, date, amount.ToString(), Convert.ToDecimal(note), "Spending");
//>>>>>>> 0757664bbeec00cab58d2575361edcff6973c892:[CSLT] Final_Project/Demo.cs
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
//<<<<<<< HEAD:[CSLT] Final_Project/Program.cs

//        static void StoreSpendingRecord(int id, string session, DateTime date, double amount, string method, string category, string note)
//=======
//        static void WriteSpendingToCsv(string SpendingFilePath, string flow, int id, string source, decimal amount, string method, string note, DateTime date)
//        {

//<<<<<<< HEAD:[CSLT] Final_Project/Program.cs
//            // Check if the file exists
//            bool fileExists = File.Exists(fileName);

//            var spending = new Spending
//            {
//                ID = id,
//                Session = session,
//                Date = date,
//                Amount = amount,
//                Method = method,
//                Category = category,
//                Note = note
//            };

//            var configSpendings = new CsvConfiguration(CultureInfo.InvariantCulture)
//            {
//                HasHeaderRecord = !fileExists // Only add headers if the file is new
//            };

//            try
//            {
//                using (var writer = new StreamWriter(fileName, append: true))
//                using (var csvWriter = new CsvWriter(writer, configSpendings))
//                {
//                    if (!fileExists)
//                    {
//                        // Write header if it's a new file
//                        csvWriter.WriteHeader<Spending>();
//                        csvWriter.NextRecord();
//                    }

//                    // Write the spending record
//                    csvWriter.WriteRecord(spending);
//                    csvWriter.NextRecord();
//                }

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("Data written to CSV successfully.");
//                Console.ResetColor();
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"Error writing to CSV: {ex.Message}");
//                Console.ResetColor();
//=======
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
//                        Id = id,
//                        Source = source,
//                        Flow = flow,
//                        Method = method,
//                        Date = date,
//                        Amount = amount,
//                        Note = note
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
//>>>>>>> 0757664bbeec00cab58d2575361edcff6973c892:[CSLT] Final_Project/Demo.cs
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
//            public string Flow { get; set; }
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


//        static int GetNextId(string fileName)
//        {
//            if (!File.Exists(fileName))
//            {
//                return 1; // Start ID from 1 if the file doesn't exist
//            }

//            // Read all lines from the file
//            var lines = File.ReadAllLines(fileName);

//            // Skip header and parse the last ID
//            if (lines.Length <= 1) return 1;

//            var lastLine = lines[^1];
//            if (int.TryParse(lastLine.Split(',')[0], out int lastId))
//            {
//                return lastId + 1;
//            }

//            return 1; // Default to 1 if parsing fails
//        }

//        static void AddIncomeRecord()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Blue;
//                Console.WriteLine("=== Add Income Record ===");
//                Console.ResetColor();

//                // Step 1: Read amount
//                decimal amount = ReadDecimalInput();

//                // Step 2: Get payment method
//                string method = GetMethod();

//                // Step 3: Get spending category
//                string category = GetIncomeCategory();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                string note = Console.ReadLine();

//                // Step 5: Get the date (adjusted for DD/MM format and default year)
//                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                string session = GetSessionOfDay();

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
//                    // Generate a new ID for the record
//                    int id = GetNextId("Income.csv");

//                    // Save the record
//                    StoreIncomeRecord(id, session, date, (double)amount, method, category, note);
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

//        static void StoreIncomeRecord(int id, string session, DateTime date, double amount, string method, string category, string note)
//        {
//            const string fileName = "Income.csv";

//            // Check if the file exists
//            bool fileExists = File.Exists(fileName);

//            var income = new Income
//            {
//                ID = id,
//                Session = session,
//                Date = date,
//                Amount = amount,
//                Method = method,
//                Category = category,
//                Note = note
//            };

//            var configSpendings = new CsvConfiguration(CultureInfo.InvariantCulture)
//            {
//                HasHeaderRecord = !fileExists // Only add headers if the file is new
//            };

//            try
//            {
//                using (var writer = new StreamWriter(fileName, append: true))
//                using (var csvWriter = new CsvWriter(writer, configSpendings))
//                {
//                    if (!fileExists)
//                    {
//                        // Write header if it's a new file
//                        csvWriter.WriteHeader<Income>();
//                        csvWriter.NextRecord();
//                    }

//                    // Write the spending record
//                    csvWriter.WriteRecord(income);
//                    csvWriter.NextRecord();
//                }

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("Data written to CSV successfully.");
//                Console.ResetColor();
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"Error writing to CSV: {ex.Message}");
//                Console.ResetColor();
//            }
//        }
//        static void AddLoanRecord()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("=== Add Loan Record ===");
//                Console.ResetColor();

//                decimal amount = ReadDecimalInput();

//                Console.Write("Enter who you lend to: ");
//                string borrower = GetValidName();

//                string method = GetMethod();

//                Console.Write("Enter Note: ");
//                string note = Console.ReadLine();

//                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                string session = GetSessionOfDay();

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nLoan Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Borrower: {borrower}");
//                Console.WriteLine($"  - Note: {note}");

//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    int id = GetNextId("Loan.csv");
//                    StoreLoanRecord(id, session, date, (double)amount, method, borrower, note);
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

//                Console.WriteLine("\nDo you want to add another record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 0)
//                {
//                    break; // Exit to AddRecord menu
//                }
//            }
//        }

//        static void StoreLoanRecord(int id, string session, DateTime date, double amount, string method, string borrower, string note)
//        {
//            const string fileName = "Loan.csv";

//            bool fileExists = File.Exists(fileName);

//            var loan = new Loan
//            {
//                ID = id,
//                Session = session,
//                Date = date,
//                Amount = amount,
//                Method = method,
//                Borrower = borrower,
//                Note = note
//            };

//            var configLoans = new CsvConfiguration(CultureInfo.InvariantCulture)
//            {
//                HasHeaderRecord = !fileExists
//            };

//            try
//            {
//                using (var writer = new StreamWriter(fileName, append: true))
//                using (var csvWriter = new CsvWriter(writer, configLoans))
//                {
//                    if (!fileExists)
//                    {
//                        csvWriter.WriteHeader<Loan>();
//                        csvWriter.NextRecord();
//                    }

//                    csvWriter.WriteRecord(loan);
//                    csvWriter.NextRecord();
//                }

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("Data written to CSV successfully.");
//                Console.ResetColor();
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"Error writing to CSV: {ex.Message}");
//                Console.ResetColor();
//            }
//        }
//        static void AddDebitRecord()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("=== Add Debit Record ===");
//                Console.ResetColor();

//                // Step 1: Get amount
//                decimal amount = ReadDecimalInput();

//                // Step 2: Get lender's name
//                Console.Write("Enter who you borrow from: ");
//                string lender = GetValidName();

//                // Step 3: Get payment method
//                string method = GetMethod();

//                // Step 4: Enter a note
//                Console.Write("Enter Note: ");
//                string note = Console.ReadLine();

//                // Step 5: Get the date
//                DateTime date = GetDateInput("Enter date DD/MM (Default by current year, leave blank for today): ");

//                // Step 6: Choose session of the day
//                string session = GetSessionOfDay();

//                // Step 7: Display record summary
//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("\nDebit Record:");
//                Console.ResetColor();
//                Console.WriteLine($"  - Session: {session}");
//                Console.WriteLine($"  - Date: {date:dd/MM/yyyy}");
//                Console.WriteLine($"  - Amount: {FormatCurrency(amount)} VND");
//                Console.WriteLine($"  - Method: {method}");
//                Console.WriteLine($"  - Lender: {lender}");
//                Console.WriteLine($"  - Note: {note}");

//                // Step 8: Confirm save
//                Console.WriteLine("\nDo you want to save this record?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");

//                if (GetYorN_Selection() == 1)
//                {
//                    // Generate a new ID for the record
//                    int id = GetNextId("Debit.csv");

//                    // Save the record
//                    StoreDebitRecord(id, session, date, (double)amount, method, lender, note);
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
//        static void StoreDebitRecord(int id, string session, DateTime date, double amount, string method, string lender, string note)
//        {
//            const string fileName = "Debit.csv";

//            // Check if the file exists
//            bool fileExists = File.Exists(fileName);

//            // Create a Debit object
//            var debit = new Debit
//            {
//                ID = id,
//                Session = session,
//                Date = date,
//                Amount = amount,
//                Method = method,
//                Lender = lender,
//                Note = note
//            };

//            var configDebits = new CsvConfiguration(CultureInfo.InvariantCulture)
//            {
//                HasHeaderRecord = !fileExists // Write headers only if the file is new
//            };

//            try
//            {
//                using (var writer = new StreamWriter(fileName, append: true))
//                using (var csvWriter = new CsvWriter(writer, configDebits))
//                {
//                    if (!fileExists)
//                    {
//                        // Write header for a new file
//                        csvWriter.WriteHeader<Debit>();
//                        csvWriter.NextRecord();
//                    }

//                    // Write the debit record
//                    csvWriter.WriteRecord(debit);
//                    csvWriter.NextRecord();
//                }

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine("Data written to CSV successfully.");
//                Console.ResetColor();
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"Error writing to CSV: {ex.Message}");
//                Console.ResetColor();
//            }
//        }

//        static decimal ReadDecimalInput()
//        {
//            while (true)
//            {
//                Console.Write("Enter amount (e.g., 30k, 3m, 3B, 30.000): ");
//                string input = Console.ReadLine().Trim().ToLower();
//                try
//                {
//                    decimal multiplier = 1;

//                    // Handle suffix multipliers
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
//                        amount *= multiplier;

//                        // Reject values less than or equal to 0
//                        if (amount <= 0)
//                        {
//                            throw new ArgumentOutOfRangeException("Amount must be greater than 0.");
//                        }

//                        return amount;
//                    }

//                    throw new FormatException("Invalid numeric format.");
//                }
//                catch (ArgumentOutOfRangeException)
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid amount. The value must be greater than 0. Please try again.");
//                    Console.ResetColor();
//                }
//                catch (FormatException)
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid amount format. Please try again (e.g., 30k, 3m, 3b, 30.000).");
//                    Console.ResetColor();
//                }
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

//        static void ShowBudget()
//        {
//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Magenta;
//            Console.WriteLine("=== Budget ===");
//            Console.ResetColor();

//            // Get Input Data from transactions
//            List<Transaction> transactions = new List<Transaction>
//    {
//        new Transaction { ID = 1, Flow = "IN", Method = "Banking", Date = DateTime.Parse("1/12/2024 8:00"), Category = "Subsidy", Amount = 3000000 },
//        new Transaction { ID = 2, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 8:30"), Category = "Withdrawal", Amount = 500000 },
//        new Transaction { ID = 3, Flow = "IN", Method = "Cash", Date = DateTime.Parse("1/12/2024 8:30"), Category = "Withdrawal", Amount = 500000 },
//        new Transaction { ID = 4, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 9:00"), Category = "Food", Amount = 30000 },
//        new Transaction { ID = 5, Flow = "IN", Method = "E-Wallet", Date = DateTime.Parse("1/12/2024 10:00"), Category = "Loan", Amount = 28000 },
//        new Transaction { ID = 6, Flow = "OUT", Method = "Cash", Date = DateTime.Parse("1/12/2024 11:00"), Category = "Debit", Amount = 15000 },
//        new Transaction { ID = 7, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 12:00"), Category = "Food", Amount = 35000 },
//        new Transaction { ID = 8, Flow = "OUT", Method = "Cash", Date = DateTime.Parse("1/12/2024 13:00"), Category = "Snack", Amount = 20000 },
//        new Transaction { ID = 9, Flow = "OUT", Method = "Banking", Date = DateTime.Parse("1/12/2024 18:00"), Category = "Food", Amount = 35000 }
//    };

//            do
//            {
//                int month, year;

//                // Menu for date selection
//                Console.WriteLine("\nSelect date filter option:");
//                Console.WriteLine("1. Current month and year");
//                Console.WriteLine("2. Customize date");
//                Console.Write("Your selection: ");
//                string dateChoice = Console.ReadLine();

//                if (dateChoice == "1")
//                {
//                    // Use current month and year
//                    month = DateTime.Now.Month;
//                    year = DateTime.Now.Year;
//                }
//                else if (dateChoice == "2")
//                {
//                    // Customize date
//                    year = GetValidYear();
//                    month = GetValidMonth();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid choice. Please select 1 or 2.");
//                    Console.ResetColor();
//                    continue;
//                }

//                // Filter transactions
//                var filteredTransactions = FilterByMonthYear(transactions, month, year);

//                if (filteredTransactions.Count == 0)
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                    Console.WriteLine($"No transaction data in {month:D2}/{year}");
//                    Console.ResetColor();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine($"Using the current month: {month:D2}/{year}");
//                    Console.ResetColor();

//                    PerformNextAction(filteredTransactions, month, year);
//                }

//                Console.WriteLine("\nDo you want to refilter?");
//                Console.WriteLine("1. Yes");
//                Console.WriteLine("0. No");
//            } while (GetYorN_Selection() == 1);

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine("Thank you for using the transaction filter!");
//            Console.ResetColor();
//        }

//        static int GetValidYear()
//        {
//            while (true)
//            {
//                Console.Write("Enter the year (e.g., 2024): ");
//                if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
//                {
//                    return year;
//                }

//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("Invalid input. Please enter a valid year (e.g., 2024).");
//                Console.ResetColor();
//            }
//        }

//        static int GetValidMonth()
//        {
//            while (true)
//            {
//                Console.Write("Enter the month (1-12): ");
//                if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
//                {
//                    return month;
//                }

//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("Invalid input. Please enter a valid month (1-12).");
//                Console.ResetColor();
//            }
//        }

//        static void PerformNextAction(List<Transaction> filteredTransactions, int month, int year)
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.ForegroundColor = ConsoleColor.Magenta;
//                Console.WriteLine("=== Budget ===");
//                Console.ResetColor();

//                Console.ForegroundColor = ConsoleColor.Green;
//                Console.WriteLine($"Using the current month: {month:D2}/{year}");
//                Console.ResetColor();

//                Console.WriteLine("\nWhat would you like to do next?");
//                Console.WriteLine("1. View your budget (balances by method)");
//                Console.WriteLine("2. View your expenditure by category");
//                Console.WriteLine("3. View your income by category");
//                Console.WriteLine("0. Exit this menu");
//                Console.Write("Your selection: ");

//                if (int.TryParse(Console.ReadLine(), out int choice))
//                {
//                    switch (choice)
//                    {
//                        case 1:
//                            Console.Clear();
//                            Console.WriteLine("=== Budget (Balances by Method) ===");
//                            DisplayBalancesByMethod(filteredTransactions);
//                            Console.ForegroundColor = ConsoleColor.Green;
//                            Console.WriteLine($"Total Balance: {FormatCurrency(CalculateTotalBalance(filteredTransactions))}");
//                            Console.ResetColor();
//                            break;

//                        case 2:
//                            Console.Clear();
//                            Console.WriteLine("=== Expenditure by Category ===");
//                            DisplayOutFlowByCategory(filteredTransactions);
//                            break;

//                        case 3:
//                            Console.Clear();
//                            Console.WriteLine("=== Income by Category ===");
//                            DisplayInFlowByCategory(filteredTransactions);
//                            break;

//                        case 0:
//                            Console.Clear();
//                            Console.ForegroundColor = ConsoleColor.Green;
//                            Console.WriteLine("Exiting the current menu...");
//                            Console.ResetColor();
//                            return;

//                        default:
//                            Console.ForegroundColor = ConsoleColor.Red;
//                            Console.WriteLine("Invalid choice. Please select a valid option.");
//                            Console.ResetColor();
//                            break;
//                    }

//                    Console.WriteLine("\nPress any key to return to the menu...");
//                    Console.ReadKey();
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid input. Please enter a number corresponding to the options.");
//                    Console.ResetColor();
//                }
//            }
//        }

//        static void DisplayOutFlowByCategory(List<Transaction> transactions)
//        {
//            Console.WriteLine("\nOutflow by Category:");
//            var expenditure = transactions
//                .Where(t => t.Flow == "OUT")
//                .GroupBy(t => t.Category)
//                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

//            double totalExpenditure = 0;

//            foreach (var item in expenditure)
//            {
//                Console.WriteLine($"  {item.Category}: {FormatCurrency(item.Total)}");
//                totalExpenditure += item.Total;
//            }

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"Total Expenditure: {FormatCurrency(totalExpenditure)}");
//            Console.ResetColor();
//        }

//        static void DisplayInFlowByCategory(List<Transaction> transactions)
//        {
//            Console.WriteLine("\nInflow by Category:");
//            var income = transactions
//                .Where(t => t.Flow == "IN")
//                .GroupBy(t => t.Category)
//                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

//            double totalIncome = 0;

//            foreach (var item in income)
//            {
//                Console.WriteLine($"  {item.Category}: {FormatCurrency(item.Total)}");
//                totalIncome += item.Total;
//            }

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"Total Income: {FormatCurrency(totalIncome)}");
//            Console.ResetColor();
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
//        static bool TryParseMonthYear(string input, out int month, out int year)
//        {
//            month = 0;
//            year = 0;
//            if (DateTime.TryParseExact(input, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
//            {
//                month = result.Month;
//                year = result.Year;
//                return true;
//            }
//            return false;
//        }

//        static List<Transaction> FilterByMonthYear(List<Transaction> transactions, int month, int year)
//        {
//            return transactions.Where(t => t.Date.Month == month && t.Date.Year == year).ToList();
//        }

//        static void DisplayBalancesByMethod(List<Transaction> transactions)
//        {
//            var methods = transactions.Select(t => t.Method).Distinct();
//            foreach (var method in methods)
//            {
//                double inflow = transactions.Where(t => t.Method == method && t.Flow == "IN").Sum(t => t.Amount);
//                double outflow = transactions.Where(t => t.Method == method && t.Flow == "OUT").Sum(t => t.Amount);
//                Console.WriteLine($"{method} Balance: {FormatCurrency(inflow - outflow)}");
//            }
//        }

//        static double CalculateTotalBalance(List<Transaction> transactions)
//        {
//            return transactions.Where(t => t.Flow == "IN").Sum(t => t.Amount) -
//                   transactions.Where(t => t.Flow == "OUT").Sum(t => t.Amount);
//        }
//        static string FormatCurrency(double amount)
//        {
//            return $"{amount:N0}".Replace(",", ".") + " vnd";
//        }

//        static void DisplayTransactions(List<Transaction> transactions)
//        {
//            foreach (var t in transactions)
//            {
//                Console.WriteLine($"{t.ID} | {t.Flow} | {t.Method} | {t.Date} | {t.Category} | {FormatCurrency(t.Amount)}");
//            }
//        }
//        static void ShowSaving()
//        {
//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine("=== Saving ===");
//            Console.ResetColor();
//            Console.WriteLine("Please choose an action:");
//            Console.WriteLine("1. Set a daily spending constraint");
//            Console.WriteLine("2. Set up a financial plan (scenario)");
//            Console.WriteLine("3. Use spending forecast function");
//            Console.WriteLine("4. Use spending suggestions function");
//            Console.WriteLine("0. Return to Main Menu");

//            int choice = GetSavingMenuSelection();

//            // Stub for next steps; this can be expanded later to call specific functions
//            switch (choice)
//            {
//                case 1:
//                    Console.WriteLine("Set a daily spending constraint - functionality coming soon.");
//                    break;
//                case 2:
//                    Console.WriteLine("Set up a financial plan (scenario) - functionality coming soon.");
//                    break;
//                case 3:
//                    Console.WriteLine("Use spending forecast function - functionality coming soon.");
//                    break;
//                case 4:
//                    Console.WriteLine("Use spending suggestions function - functionality coming soon.");
//                    break;
//                case 0:
//                    Console.WriteLine("Returning to Main Menu...");
//                    break;
//                default:
//                    Console.WriteLine("Invalid selection.");
//                    break;
//            }
//        }
//        static void SetDailySpendingConstraint(double totalIncome)
//        {
//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Cyan;
//            Console.WriteLine("=== Set a Daily Spending Constraint ===");
//            Console.ResetColor();

//            // Default daily spending limit: 55% of income
//            double defaultLimit = totalIncome * 0.55 / 30; // Assuming 30 days in a month
//            double spendingLimit = defaultLimit;

//            // Step 1: Ask user to input spending limit
//            while (true)
//            {
//                Console.Write($"Enter the daily spending limit (default: {FormatCurrency(defaultLimit)}) or leave blank: ");
//                string input = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(input))
//                {
//                    spendingLimit = defaultLimit;
//                    Console.WriteLine($"Daily spending limit set to default: {FormatCurrency(spendingLimit)}");
//                    break;
//                }
//                else if (TryParseAmount(input, out double limit))
//                {
//                    spendingLimit = limit;
//                    Console.WriteLine($"Daily spending limit set to: {FormatCurrency(spendingLimit)}");
//                    break;
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid input. Please enter a valid number or leave blank.");
//                    Console.ResetColor();
//                }
//            }

//            // Step 2: Ask user for the number of reminders
//            int defaultReminderCount = 3;
//            int reminderCount = defaultReminderCount;

//            while (true)
//            {
//                Console.Write($"Enter the number of reminders for overspending (default: {defaultReminderCount}) or leave blank: ");
//                string input = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(input))
//                {
//                    reminderCount = defaultReminderCount;
//                    Console.WriteLine($"Reminder count set to default: {defaultReminderCount}");
//                    break;
//                }
//                else if (int.TryParse(input, out int count) && count > 0)
//                {
//                    reminderCount = count;
//                    Console.WriteLine($"Reminder count set to: {count}");
//                    break;
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine("Invalid input. Please enter a valid positive integer or leave blank.");
//                    Console.ResetColor();
//                }
//            }

//            // Final confirmation
//            Console.WriteLine("\nSummary:");
//            Console.WriteLine($"  Daily Spending Limit: {FormatCurrency(spendingLimit)}");
//            Console.WriteLine($"  Reminder Count: {reminderCount}");

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine("Your spending constraint settings have been saved!");
//            Console.ResetColor();
//        }
//        static bool TryParseAmount(string input, out double amount)
//        {
//            input = input.Trim().ToLower();
//            if (input.EndsWith("k")) input = input.Replace("k", "000");
//            else if (input.EndsWith("m")) input = input.Replace("m", "000000");
//            else if (input.EndsWith("b")) input = input.Replace("b", "000000000");
//            input = input.Replace(",", "").Replace("₫", "").Trim();

//            return double.TryParse(input, out amount);
//        }
//        static int GetSavingMenuSelection()
//        {
//            while (true)
//            {
//                Console.Write("Your selection: ");
//                if (int.TryParse(Console.ReadLine(), out int selection) && selection >= 0 && selection <= 4)
//                {
//                    return selection;
//                }
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("Invalid choice. Please enter a number between 0 and 4.");
//                Console.ResetColor();
//            }
//        }
//    }
//}
