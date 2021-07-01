using System;
using System.Collections.Generic;
using System.Text;
using C__.UniversalModels;

namespace C__.AnalizadorSemantico
{
    class ArbolDeAnalisisGramatical
    {
        private List<string> _alphabet;
        private List<Token> _simbolList;
        private ReglasSemanticas _reglasSemanticas;

        public NodoDeAnalisis Root { get; set; }

        public ArbolDeAnalisisGramatical(List<string> alph, List<Token> sL)
        {
            _alphabet = alph;
            _simbolList = sL;
            _reglasSemanticas = new ReglasSemanticas();
        }

        public void creacionDelArbolDeAnalisis(List<Stack<string>> slrStackList)
        {
            List<NodoDeAnalisis> nl_act = new List<NodoDeAnalisis>();
            List<NodoDeAnalisis> nl_ant = new List<NodoDeAnalisis>();

            foreach (Stack<string> s in slrStackList)
            {
                nl_act = _obtenerObjetosContenidos(s);

                if ((nl_ant.Count > 0) && (nl_ant.Count >= nl_act.Count))
                {
                    int marker = nl_act.Count - 2;
                    _setSubListUptoFather(marker, nl_ant, nl_act[nl_act.Count - 1]);
                }

                nl_ant = nl_act;
            }
        }

        private void _setSubListUptoFather(int m, List<NodoDeAnalisis> nl, NodoDeAnalisis father)
        {
            int count = nl.Count - 1;
            List<NodoDeAnalisis> temp = new List<NodoDeAnalisis>();

            while(count > m)
            {
                nl[count].Padre = father;
                temp.Add(nl[count]);
            }

            father.Hijos = temp;

            _reglasSemanticas.calcularValor(father);
        }

        private List<NodoDeAnalisis> _obtenerObjetosContenidos(Stack<string> slrSL)
        {
            List<NodoDeAnalisis> temp = new List<NodoDeAnalisis>();

            for (int i = 0; i < slrSL.Count; i++)
            {
                string value = slrSL.Pop();
                if (_alphabet.Contains(value))
                {
                    NodoDeAnalisis aux = new NodoDeAnalisis();
                    aux.Value = value;
                    Token find = _simbolList.Find(a => a.token == value);
                    if (find.tokenType != "")
                    {
                        aux._DType = find.tokenType;

                        if (find.tokenType == "Operator")
                            aux.Tipo = TipoDeNodo.def;
                        else
                            _reglasSemanticas.obtenerValorEnFuncionDelTipo(aux, find.lexeme);
                    }
                    else
                        aux.Tipo = _reglasSemanticas.obtenerTipoNodo(value);

                    temp.Add(aux);
                }
            }

            return temp;
        }
    }
}
