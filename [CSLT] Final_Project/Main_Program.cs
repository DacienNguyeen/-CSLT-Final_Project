using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _CSLT__Final_Project
{
    internal class Main_Program
    {
        static void Main(string[] args)
        {
            DisplayMenu();
            double income = GetIncome();
            Console.WriteLine(income);
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("Welcome to CashLand (example app name)!");
            Console.WriteLine("==========Menu of actions==========");
        }

        static double GetIncome()
        {
            Console.WriteLine("Input your income: ");
            double income = double.Parse(Console.ReadLine());
            //constraint: income is not negative, no charracter -> tryparse until correct

            return income;
        }
    }
}
