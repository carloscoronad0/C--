using System;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorSemantico
{
    public enum TipoDeNodo
    {
        ret, def
    }
    class NodoDeAnalisis
    {
        public string Value { get; set; }
        public NodoDeAnalisis Padre { get; set; }
        public List<NodoDeAnalisis> Hijos { get; set; }

        public TipoDeNodo Tipo { get; set; }

        public int _ValInt { get; set; }
        public bool _ValBool { get; set; }
        public float _ValFloat { get; set; }
        public char _ValChar { get; set; }
        public string _ValStr { get; set; }

        public string _DType { get; set; }

        public NodoDeAnalisis()
        {
            Padre = null;
            Hijos = null;
            _DType = "";
        }

    }
}
