using System;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter a character (A-Z) to generate the diamond, or any other key to exit:");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                Console.WriteLine("Invalid input. Exiting diamond generator.");
                break;
            }

            char midChar = char.ToUpper(input[0]);

            if (midChar < 'A' || midChar > 'Z')
            {
                Console.WriteLine("Invalid input. Please enter a letter from A to Z. Exiting diamond generator.");
                break;
            }

            DiamondKata.GenerateDiamond(midChar, Console.Out);

            Console.WriteLine("\n---");
        }

        Console.WriteLine("Application finished. Press any key to close the console.");
        Console.ReadKey();
    }
}