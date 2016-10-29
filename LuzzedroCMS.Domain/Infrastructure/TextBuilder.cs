using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LuzzedroCMS.Domain.Infrastructure
{
    public class TextBuilder
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
               .Replace("?", "")
               .Replace("/", "")
               .Replace("|", "")
               .Replace("'\'", "")
               .Replace("+", "")
               .Replace("=", "")
               .Replace("<", "")
               .Replace(">", "")
               .Replace("\"", "")
               .Replace("\'", "")
               .Replace("[", "")
               .Replace("]", "")
               .Replace("{", "")
               .Replace("}", "")
               .Replace("(", "")
               .Replace(")", "")
               .Replace("%", "")
               .Replace("$", "")
               .Replace("#", "")
               .Replace("@", "")
               .Replace("!", "")
               .Replace("`", "")
               .Replace("~", "")
               .Replace("*", "")
               .Replace(":", "")
               .Replace(";", "")
               .Replace(",", "")
               .Replace(".", "");
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
    }
}