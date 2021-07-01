using System;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorSintactico
{
    class PilaDescomposicion:IpilaResult
    {
        public List<Stack<string>> pasos { get; set; }
    }
}
