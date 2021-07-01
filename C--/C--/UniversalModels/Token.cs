using System;
using System.Collections.Generic;
using System.Text;

namespace C__.UniversalModels
{
    public class Token
    {
        public string token { get; set; }
        public string tokenType { get; set; }
        public string lexeme { get; set; }
        public int line { get; set; }
        public int column { get; set; }
        public Token()
        {
            tokenType = "";
        }
    }
}
