namespace LuzzedroCMS.Domain.Infrastructure.Abstract
{
    public interface ITextBuilder
    {
        string RemovePolishChars(string input);
        string RemoveSpecialChars(string input);
        string RemoveSpaces(string input);
        string RandomizeText(string input, int length = 2);
        string GetRandomString(int length = 20);
        string GetUrlTitle(string title);
        string GetHash(string text);
    }
}
