using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

class Program67
{
    // Transaction record structure
    public class Transaction
    {
        public int ID { get; set; }
        public string Flow { get; set; } // "IN" or "OUT"
        public string Method { get; set; } // "Banking", "Cash", or "E-Wallet"
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
    }

    static void Main8(string[] args)
    {
        // Sample data (based on the provided table)
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

        // Display results
        DisplayBalancesByMethod(transactions);
        Console.WriteLine($"Total Balance: {CalculateTotalBalance(transactions):N0} ₫");

        Console.WriteLine("\nFiltered Transactions (1/12/2024):");
        var filtered = FilterByDate(transactions, DateTime.Parse("1/12/2024"));
        DisplayTransactions(filtered);

        Console.WriteLine("\nTransactions Sorted by Amount (Descending):");
        var sortedByAmount = SortBy(transactions, "amount", descending: true);
        DisplayTransactions(sortedByAmount);

        Console.WriteLine("\nTransactions Sorted by Date:");
        var sortedByDate = SortBy(transactions, "date");
        DisplayTransactions(sortedByDate);
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

    static List<Transaction> FilterByDate(List<Transaction> transactions, DateTime date)
    {
        return transactions.Where(t => t.Date.Date == date.Date).ToList();
    }

    static List<Transaction> SortBy(List<Transaction> transactions, string criteria, bool descending = false)
    {
        return criteria.ToLower() switch
        {
            "amount" => descending
                ? transactions.OrderByDescending(t => t.Amount).ToList()
                : transactions.OrderBy(t => t.Amount).ToList(),
            "date" => descending
                ? transactions.OrderByDescending(t => t.Date).ToList()
                : transactions.OrderBy(t => t.Date).ToList(),
            _ => transactions
        };
    }

    static string FormatCurrency(double amount)
    {
        return $"{amount:N0}".Replace(",", ".") + " ₫";
    }

    static void DisplayTransactions(List<Transaction> transactions)
    {
        foreach (var t in transactions)
        {
            Console.WriteLine($"{t.ID} | {t.Flow} | {t.Method} | {t.Date} | {t.Category} | {FormatCurrency(t.Amount)}");
        }
    }
}
