using System;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorSemantico
{
    class ReglasSemanticas
    {
        public string[] Productores = new string[]
        {
            "A", "K", "U", "L", "H", "F", "W", 
            "C", "P", "D", "E", "N", "J", "R", 
            "Q", "I", "V", "M", "X", "B", "T", 
            "O", "Z", "G"
        };

        public TipoDeNodo[] TiposDeProductores = new TipoDeNodo[]
        {
            TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, 
            TipoDeNodo.ret, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.ret, TipoDeNodo.ret, TipoDeNodo.ret,
            TipoDeNodo.ret, TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.ret, TipoDeNodo.ret, TipoDeNodo.ret, TipoDeNodo.def,
            TipoDeNodo.def, TipoDeNodo.def, TipoDeNodo.def
        };

        public Dictionary<string, TipoDeNodo> TipoReglas;

        public ReglasSemanticas()
        {
            TipoReglas = new Dictionary<string, TipoDeNodo>();
            agregarTiposParaLasReglas();
        }

        public void agregarTiposParaLasReglas()
        {
            for (int i = 0; i < Productores.Length; i++)
            {
                TipoReglas.Add(Productores[i], TiposDeProductores[i]);
            }
        }

        public TipoDeNodo obtenerTipoNodo(string key)
        {
            return TipoReglas.GetValueOrDefault(key);
        }

        public void obtenerValorEnFuncionDelTipo(NodoDeAnalisis nda, string lexeme)
        {
            switch (nda._DType)
            {
                case "Int":
                    nda._ValInt = Int32.Parse(lexeme);
                    break;
                case "Float":
                    nda._ValFloat = float.Parse(lexeme);
                    break;
                case "Char":
                    char[] aux = lexeme.ToCharArray();
                    nda._ValChar = aux[0];
                    break;
                case "String":
                    nda._ValStr = lexeme;
                    break;
            }
        }

        public void calcularValor(NodoDeAnalisis nda)
        {
            NodoDeAnalisis part1 = new NodoDeAnalisis();
            NodoDeAnalisis part2 = new NodoDeAnalisis();
            NodoDeAnalisis op;

            if (nda.Tipo == TipoDeNodo.ret)
            {
                if (nda.Hijos.Count > 1)
                {
                    
                }
                else
                {
                    nda._DType = nda.Hijos[0]._DType;
                    switch (nda.Hijos[0]._DType)
                    {
                        case "Int":
                            nda._ValInt = nda.Hijos[0]._ValInt;
                            break;
                        case "Float":
                            nda._ValFloat = nda.Hijos[0]._ValFloat;
                            break;
                        case "Char":
                            nda._ValChar = nda.Hijos[0]._ValChar;
                            break;
                        case "String":
                            nda._ValStr = nda.Hijos[0]._ValStr;
                            break;
                    }
                }
            }
        }

        private void _calcularValorU(NodoDeAnalisis n1, string operador)
        {
            switch (operador)
            {
                case "++":
                    break;
                case "--":
                    break;
            }
        }

        private void _calcularValorBinario(NodoDeAnalisis n1, NodoDeAnalisis n2, string operador)
        {
            switch (operador)
            {
                case "==":
                    if (n1._DType == n2._DType)
                    {
                        
                    }
                    break;
                case "<=":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") && 
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case ">=":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "!=":
                    if (n1._DType == n2._DType)
                    {

                    }
                    break;
                case "<":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case ">":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "&&":
                    if (n1._DType == "Bool" && n2._DType == "Bool")
                    {

                    }
                    break;
                case "||":
                    if (n1._DType == "Bool" && n2._DType == "Bool")
                    {

                    }
                    break;
                case "+":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    else if (n1._DType == "String" && n2._DType == "String")
                    {

                    }
                    break;
                case "-":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "*":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "/":
                    if ((n1._DType == "Int" || n1._DType == "Char" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Char" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "**":
                    if ((n1._DType == "Int" || n1._DType == "Float") &&
                        (n2._DType == "Int" || n2._DType == "Float"))
                    {

                    }
                    break;
                case "//":
                    break;
            }
        }
    }
}
