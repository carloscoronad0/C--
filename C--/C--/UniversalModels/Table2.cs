using SRL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace SRL
{
    class Table2
    {
        List<char> terminales = new List<char>();
        List<char> noTerminales = new List<char>();
        List<char> simbols = new List<char>();

        public List<Expresion> expresiones = new List<Expresion>();
        public Expresion[] grammar = new Expresion[48];

        List<List<string>> table = new List<List<string>>();
        List<string[]> simbolsTable = new List<string[]>();
        public List<List<char>> productionsExtended;

        public Table2(List<char> terminales, List<char> noTerminales, List<List<char>> productionsExtended)
        {
            this.terminales = terminales;
            this.terminales.Add('$');
            this.noTerminales = noTerminales;
            this.terminales.Remove('-');
            this.terminales.Remove('>');
            this.terminales.Remove('~');

            terminales.ForEach(a => simbols.Add(a));
            noTerminales.ForEach(a => simbols.Add(a));
            this.productionsExtended = productionsExtended;

            this.initTable();

        }
        public void initTable()
        {
            this.jsonRead();
            this.contrucExpresion();
            expresiones.CopyTo(grammar);
            this.printTable();

        }
        private void contrucExpresion()
        {
            int n = productionsExtended.Count;
            string s = "";
            char head;
            for (int i = 1; i < n; i++)
            {
                head = productionsExtended[i][0];
                productionsExtended[i].Remove('~');
                productionsExtended[i].RemoveAt(0);
                for (int j = 0; j < productionsExtended[i].Count; j++)
                {
                    s += productionsExtended[i][j].ToString();
                }
                Expresion es = new Expresion(head, s, i);
                s = "";
                expresiones.Add(es);
            }
        }

        public string accion(int estado, char caracter)
        {
            string response;

            int indexF = simbols.FindIndex(a => a == caracter);
            response = simbolsTable[estado][indexF];
            return response;
        }
        private void printTable()
        {
            int t = simbols.Count;
            for (int i = 0; i < simbols.Count; i++)
            {
                Console.Write(simbols[i] + " |");
            }
            Console.WriteLine();
            for (int i = 0; i < simbolsTable.Count; i++)
            {
                for (int j = 0; j < t; j++)
                {
                    Console.Write(simbolsTable[i][j] + "|");
                }
                Console.WriteLine();
            }
        }
        public void jsonRead()
        {
            string json = "";
            try
            {
                string path = "../../../table.json";
                string jsonM;
                // Create an instance of StreamReader to read from a file.

                using (StreamReader sr = new StreamReader(path))
                {
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((jsonM = sr.ReadLine()) != null)
                    {

                        json += jsonM;
                    }
                    // Get the list of products
                    Console.WriteLine(json);
                    simbolsTable = JsonConvert.DeserializeObject<List<string[]>>(json);
                    Console.WriteLine(simbolsTable[0][0]);
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
            }

        }

    }
}

