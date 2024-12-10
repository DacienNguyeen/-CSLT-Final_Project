using System;

class CatLifeStagesProgram
{
    static void Main6(string[] args)
    {
        Console.Title = "Cat Life Stages";
        Console.Clear();

        DisplayCats();

        Console.ResetColor();
        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }

    static void DisplayCats()
    {
        string[] kittenArt = {
            @"     Curious Kitten     ",
            @"      /\_/\  * blink *   ",
            @"     ( o.o )             ",
            @"      > ^ <   Meow?      ",
            @"    /  ---  \            ",
            @"   ( (     ) )           ",
            @"    \  ===  /            ",
            @"     |_|_|_|               "
        };

        string[] teenCatArt = {
            @"     Confident Teen Cat  ",
            @"      /\_/\  *cool*      ",
            @"     ( ^_^ )             ",
            @"      ( Y )   Awesome!   ",
            @"    /   ~   \            ",
            @"   (  (   )  )           ",
            @"    \  ===  /            ",
            @"     |_|_|_|             "
        };

        string[] grownCatArt = {
            @"     Elegant Grown Cat   ",
            @"      /\_/\  *poise*     ",
            @"     ( -.- )             ",
            @"      > ~ <   Graceful   ",
            @"    /  ---  \            ",
            @"   (  (   )  )           ",
            @"    \  ===  /            ",
            @"     |_|_|_|             "
        };

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("🐱 Cat Life Stages Showcase 🐱\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        DisplayCatArt("1. Curious Kitten", kittenArt);

        Console.ForegroundColor = ConsoleColor.Green;
        DisplayCatArt("2. Confident Teen Cat", teenCatArt);

        Console.ForegroundColor = ConsoleColor.Yellow;
        DisplayCatArt("3. Elegant Grown Cat", grownCatArt);
    }

    static void DisplayCatArt(string title, string[] catArt)
    {
        Console.WriteLine(title);
        Console.WriteLine(new string('-', title.Length));

        foreach (string line in catArt)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }
}