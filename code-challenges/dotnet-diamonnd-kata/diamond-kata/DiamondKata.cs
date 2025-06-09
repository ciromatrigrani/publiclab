using System;

public class DiamondKata
{
    public static void GenerateDiamond(char midChar, TextWriter writer = null)
    {
        int midCharIndex = midChar - 'A';
        int totalRows = (midCharIndex * 2) + 1;

        for (int i = 0; i <= midCharIndex; i++)
        {
            char currentChar = (char)('A' + i);
            PrintRow(currentChar, midCharIndex, i, writer);
        }

        for (int i = midCharIndex - 1; i >= 0; i--)
        {
            char currentChar = (char)('A' + i);
            PrintRow(currentChar, midCharIndex, i, writer);
        }
    }

    private static void PrintRow(char currentChar, int midCharIndex, int currentRowIndex, TextWriter writer)
    {
        int leadingSpaces = midCharIndex - currentRowIndex;
        writer.Write(new string(' ', leadingSpaces));
        writer.Write(currentChar);

        if (currentChar != 'A')
        {
            int innerSpaces = (currentRowIndex * 2) - 1;
            writer.Write(new string(' ', innerSpaces));
            writer.Write(currentChar);
        }
        writer.WriteLine();
    }
}