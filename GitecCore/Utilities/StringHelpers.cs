namespace Gitec.Utilities;

public static class StringHelpers
{
    public static string RemoveNonIntegers(string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
    
    public static string RemoveNonLetters(string input)
    {
        return new string(input.Where(char.IsLetter).ToArray());
    }
    
    public static string RemoveNonLettersOrDigits(string input)
    {
        return new string(input.Where(c => char.IsLetterOrDigit(c)).ToArray());
    }
    
}