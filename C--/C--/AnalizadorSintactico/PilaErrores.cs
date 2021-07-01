using System;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorSintactico
{
    class PilaErrores:IpilaResult
    {
        public Queue<string> errores { get; set; }
    }
}
