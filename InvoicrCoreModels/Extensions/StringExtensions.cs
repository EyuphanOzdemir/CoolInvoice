using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreModels.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEmptyLines(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line));
            return string.Join(Environment.NewLine, nonEmptyLines);
        }
    }
}
