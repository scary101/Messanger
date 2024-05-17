using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp7.Model
{
    internal class Validation
    {
        public bool ValidateString(string input)
        {
            string pattern = @"^[а-яА-Яa-zA-Z0-9]+$";
            return Regex.IsMatch(input, pattern);
        }


    }
}
