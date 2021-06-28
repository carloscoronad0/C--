using System;
using System.Collections.Generic;
using System.Text;

namespace C__.UniversalModels
{
    public class Token
    {
        public string token { get; set; }
        public string tipoToken { get; set; }
        public string lexema { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }
        public Token()
        {
        }
    }
}
