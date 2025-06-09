namespace diamondKataTest;


public class DiamondKataTests
{
    [Theory]
    [InlineData('A', "A\r\n")]
    [InlineData('B', " A\r\nB B\r\n A\r\n")]
    [InlineData('C', "  A\r\n B B\r\nC   C\r\n B B\r\n  A\r\n")]
    [InlineData('K',
        "          A\r\n" +
        "         B B\r\n" +
        "        C   C\r\n" +
        "       D     D\r\n" +
        "      E       E\r\n" +
        "     F         F\r\n" +
        "    G           G\r\n" +
        "   H             H\r\n" +
        "  I               I\r\n" +
        " J                 J\r\n" +
        "K                   K\r\n" +
        " J                 J\r\n" +
        "  I               I\r\n" +
        "   H             H\r\n" +
        "    G           G\r\n" +
        "     F         F\r\n" +
        "      E       E\r\n" +
        "       D     D\r\n" +
        "        C   C\r\n" +
        "         B B\r\n" +
        "          A\r\n")]
    [InlineData('M',
        "            A\r\n" +
        "           B B\r\n" +
        "          C   C\r\n" +
        "         D     D\r\n" +
        "        E       E\r\n" +
        "       F         F\r\n" +
        "      G           G\r\n" +
        "     H             H\r\n" +
        "    I               I\r\n" +
        "   J                 J\r\n" +
        "  K                   K\r\n" +
        " L                     L\r\n" +
        "M                       M\r\n" +
        " L                     L\r\n" +
        "  K                   K\r\n" +
        "   J                 J\r\n" +
        "    I               I\r\n" +
        "     H             H\r\n" +
        "      G           G\r\n" +
        "       F         F\r\n" +
        "        E       E\r\n" +
        "         D     D\r\n" +
        "          C   C\r\n" +
        "           B B\r\n" +
        "            A\r\n")]
    [InlineData('Z',
        "                         A\r\n" +
        "                        B B\r\n" +
        "                       C   C\r\n" +
        "                      D     D\r\n" +
        "                     E       E\r\n" +
        "                    F         F\r\n" +
        "                   G           G\r\n" +
        "                  H             H\r\n" +
        "                 I               I\r\n" +
        "                J                 J\r\n" +
        "               K                   K\r\n" +
        "              L                     L\r\n" +
        "             M                       M\r\n" +
        "            N                         N\r\n" +
        "           O                           O\r\n" +
        "          P                             P\r\n" +
        "         Q                               Q\r\n" +
        "        R                                 R\r\n" +
        "       S                                   S\r\n" +
        "      T                                     T\r\n" +
        "     U                                       U\r\n" +
        "    V                                         V\r\n" +
        "   W                                           W\r\n" +
        "  X                                             X\r\n" +
        " Y                                               Y\r\n" +
        "Z                                                 Z\r\n" +
        " Y                                               Y\r\n" +
        "  X                                             X\r\n" +
        "   W                                           W\r\n" +
        "    V                                         V\r\n" +
        "     U                                       U\r\n" +
        "      T                                     T\r\n" +
        "       S                                   S\r\n" +
        "        R                                 R\r\n" +
        "         Q                               Q\r\n" +
        "          P                             P\r\n" +
        "           O                           O\r\n" +
        "            N                         N\r\n" +
        "             M                       M\r\n" +
        "              L                     L\r\n" +
        "               K                   K\r\n" +
        "                J                 J\r\n" +
        "                 I               I\r\n" +
        "                  H             H\r\n" +
        "                   G           G\r\n" +
        "                    F         F\r\n" +
        "                     E       E\r\n" +
        "                      D     D\r\n" +
        "                       C   C\r\n" +
        "                        B B\r\n" +
        "                         A\r\n")]
    public void GenerateDiamond_ValidLetter_ProducesCorrectOutput(char inputChar, string expectedOutput)
    {
        // Arrange
        var stringWriter = new StringWriter();

        // Act
        DiamondKata.GenerateDiamond(inputChar, stringWriter);
        string actualOutput = stringWriter.ToString();

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
    }
}
