using System;
using System.Collections.Generic;
using System.Text;

namespace C__.UniversalModels
{
    class Table
    {
        public List<Expresion> expresiones = new List<Expresion>();
        public Expresion[] grammar = new Expresion[44];
        List<Expresion> momentExpresiones = new List<Expresion>();
        //cuidado
        List<char> terminales = new List<char>();
        List<char> noTerminales = new List<char>();
        List<char> simbols = new List<char>();
        public List<List<char>> siguientes = new List<List<char>>();
        public List<List<char>> primeros = new List<List<char>>();
        List<char> momen;
        public List<List<char>> productionsExtended; 
        //siguientes.Add({'$','+','('});


        List<List<string>> table = new List<List<string>>();
        List<List<Expresion>> tableExpresiones = new List<List<Expresion>>();
        
        Dictionary<string, List<Expresion>> dictionary = new Dictionary<string, List<Expresion>>();

        int t;
        public List<string[]> simbolsTable;
        string[,] expresionesStr = new string[6,2];
        string[] sim ;
        public Table(List<char> terminales, List<char> noTerminales, List<List<char>> productionsExtended)
        {
            this.terminales = terminales;
            this.terminales.Add('$');
            this.noTerminales = noTerminales;
            this.terminales.Remove('-');
            this.terminales.Remove('>');
            this.terminales.Remove('~');

            terminales.ForEach(a => simbols.Add(a));
            noTerminales.ForEach(a => simbols.Add(a));
            simbols.Add('S');
            this.productionsExtended = productionsExtended;
            Console.Write(productionsExtended.Count);

        }
        public void initTable()
        {
            this.initSimbolsTable();

            this.contrucExpresion();
            expresiones.CopyTo(grammar);

            this.createListExpresion();
            this.printTable();

        }
        private void initSimbolsTable()
        {
            t = simbols.Count;
            
            simbolsTable = new List<string[]>();
            simbolsTable.Add(newString());
        }
        private void contrucExpresion()
        {
            this.primeros[1] = this.primeros[0];
            this.primeros[2] = this.primeros[0];

            Console.WriteLine(productionsExtended[0].ToString());
            int n = productionsExtended.Count;
            string s="";
            char head;
            for (int i = 1; i < n; i++)
            {
                head = productionsExtended[i][0];
                productionsExtended[i].Remove('~');
                productionsExtended[i].RemoveAt(0);
                for(int j=0; j<productionsExtended[i].Count;j++ )
                {
                    s += productionsExtended[i][j].ToString();
                }
                Expresion es = new Expresion(head, s, i);
                s = "";
                expresiones.Add(es);
            }
        }

        private void createListExpresion()
        {
            
            int tablaIndexGenerate = 0;
            int tablaIndexCopy = 0;
            tableExpresiones.Add(expresiones);
            char toConsum = ' ' ;
            int indexF;
            int indexI;
            int nextState; 

            while (tablaIndexGenerate >= tablaIndexCopy)
            {
                momentExpresiones = tableExpresiones[tablaIndexCopy];
                while (momentExpresiones.Count > 0)
                {
                    Char head;
                    List<Expresion> exp = new List<Expresion>();
                    List<Expresion> toRemove = new List<Expresion>();
                    nextState = tablaIndexGenerate;
                    nextState++;
                    if (momentExpresiones[0].firstSimbol() != '/')
                    {
                        simbolsTable.Add(newString());
                        toConsum = momentExpresiones[0].firstSimbol();
                        if(_isNoTerminal(toConsum))
                        {   
                            indexF = simbols.FindIndex(a => a == toConsum);
                            simbolsTable[tablaIndexCopy][indexF] = "g"+Convert.ToString( nextState);
                            
                            for (int i = indexF+1; i < simbols.Count; i++)
                            {
                                simbolsTable[tablaIndexCopy][i] = this.searchLast(tablaIndexGenerate, i);
                            }
                          
                        }
                        else
                        {
                            indexF = simbols.FindIndex(a => a == toConsum);
                            simbolsTable[tablaIndexCopy][indexF] = "s" + Convert.ToString(nextState);
                            
                        }
                        Console.WriteLine(toConsum);
                        foreach (Expresion x in momentExpresiones)
                        {
                            if (toConsum == x.firstSimbol())
                            {
                                toRemove.Add(x);
                                x.consumNextSimbol();
                                exp.Add(x);
                            }
                        }
                        foreach (Expresion x in toRemove)
                        {
                            momentExpresiones.Remove(x);
                            if (this._isNoTerminal(x.firstSimbol()))
                            {
                                indexI = noTerminales.FindIndex(a => a == x.firstSimbol());
                                momen = primeros[0];
                                foreach (char c in momen)
                                {
                                    indexF = simbols.FindIndex(a => a == c);

                                    simbolsTable[tablaIndexGenerate+1][indexF] = this.searchLast(tablaIndexGenerate, indexF);

                                }
                                
                            }
                        }
                        tablaIndexGenerate++;
                        tableExpresiones.Add(exp);
                    }
                    else
                    {

                        head = momentExpresiones[0].Head;
                        indexI = noTerminales.FindIndex(a => a == head);
                        foreach(char s in siguientes[indexI])
                        {
                            indexF =  simbols.FindIndex(a => a == s);
                            simbolsTable[tablaIndexCopy][indexF] = "r" + momentExpresiones[0].Number;
                            
                        }
                        
                        momentExpresiones.RemoveAt(0);
                    }
                     
                    
                }
                Console.WriteLine("index finished"+tablaIndexCopy);

                tablaIndexCopy++;
            }
            indexF = simbols.FindIndex(a => a == '$');
            simbolsTable[1][indexF] = "ACC";
        }
 
        public string accion(int estado, char caracter)
        {
            string response;

            int indexF = simbols.FindIndex(a => a == caracter);
            response = simbolsTable[estado][indexF];
            return response;
        }
        private bool _isNoTerminal(char c)
        {
            string terminales = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return terminales.Contains(c);
        }
        private string[] newString()
        {
            sim = new string[t];
            for (int i = 0; i < t; i++)
            {
                sim[i] = "..";
            }
            return sim;
        }
        private void printTable()
        {
            
            for (int i = 0; i < simbols.Count; i++)
            {
                Console.Write(simbols[i] + "  | ");
            }
            Console.WriteLine();
            for (int i = 0; i < simbolsTable.Count; i++)
            {
                for (int j = 0; j < t; j++)
                {
                    Console.Write(simbolsTable[i][j] + " | ");
                }
                Console.WriteLine();
            }
        }

        private string searchLast(int len, int col)
        {
            string respnse="..";
       

            for(int i=0; i< len; i++)
            {
                if (simbolsTable[i][col] != ".." && simbolsTable[i][col][0] != 'r')
                {
                    respnse = simbolsTable[i][col];
                }
            }
            return respnse;

        }
    }
}

