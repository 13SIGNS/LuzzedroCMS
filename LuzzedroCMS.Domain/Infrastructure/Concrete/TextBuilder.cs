using LuzzedroCMS.Domain.Infrastructure.Abstract;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LuzzedroCMS.Domain.Infrastructure.Concrete
{
    public class TextBuilder : ITextBuilder
    {
        public string RemovePolishChars(string input)
        {
            return input
                .Replace("ą", "a")
                .Replace("ś", "s")
                .Replace("ł", "l")
                .Replace("ę", "e")
                .Replace("ó", "o")
                .Replace("ż", "z")
                .Replace("ź", "z")
                .Replace("ń", "n");
        }

        public string RemoveSpecialChars(string input)
        {
            return input
               .Replace("?", string.Empty)
               .Replace("/", string.Empty)
               .Replace("|", string.Empty)
               .Replace("'\'", string.Empty)
               .Replace("+", string.Empty)
               .Replace("=", string.Empty)
               .Replace("<", string.Empty)
               .Replace(">", string.Empty)
               .Replace("\"", string.Empty)
               .Replace("\'", string.Empty)
               .Replace("[", string.Empty)
               .Replace("]", string.Empty)
               .Replace("{", string.Empty)
               .Replace("}", string.Empty)
               .Replace("(", string.Empty)
               .Replace(")", string.Empty)
               .Replace("%", string.Empty)
               .Replace("$", string.Empty)
               .Replace("#", string.Empty)
               .Replace("@", string.Empty)
               .Replace("!", string.Empty)
               .Replace("`", string.Empty)
               .Replace("~", string.Empty)
               .Replace("*", string.Empty)
               .Replace(":", string.Empty)
               .Replace(";", string.Empty)
               .Replace(",", string.Empty)
               .Replace(".", string.Empty);
        }

        public string RemoveSpaces(string input)
        {
            return input.Replace(" ", "-");
        }

        public string RandomizeText(string input, int length = 2)
        {
            return input + "-" + this.GetRandomString(length);
        }

        public string GetRandomString(int length = 20)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnoprstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetUrlTitle(string title)
        {
            return RemoveSpecialChars(RemovePolishChars(RemoveSpaces(title))).ToLower();
        }

        public string GetHash(string text)
        {
            if (text != null)
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                    return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
                }
            }
            else
            {
                return null;
            }
        }
    }
}