using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorLexico
{
    public class DefinicionesLexicas
    {

        // ===== Class that contains the necessary definitions of elements for the scanner ===== //

        public DefinicionesLexicas() { }  //Constructor

        // ------------------------- //
        //Definition of keywords // 
        public String[] listKeywords = { "If", "Else", "Begin", "End", "For", "While", "True", "False", "Boolean", "Int", "Float", "Char", "String" };
        //Definition of relational operators //
        public String[] listRelOperators = { "==", ">=", "<=", "=", ">", "<", "=!", "||", "&&" };
        //Definition of operators //
        public String[] listOperators = { "+", "-", "*", "/", "**", "//", "++", "--"};
        //Definition of delimitators //
        public String[] listDelimiters = { "(", ")", "{", "}", ";", "," };
        //Definition of allowed chars
        public String[] allowedChars = { "_", "-" };

        // /------------------------- //


        // ------------------------- //
        // Extra funcionts //
        public bool startRelOperator(char c)
        {  // Function to know if a char is the start of a relational operator
            bool val = false;
            if (c == '=' || c == '<' || c == '>' || c == '&' || c == '|')
            {
                val = true;
            }
            return val;
        }
        public bool isKeyword(string s)
        {
            return contains(listKeywords, s);
        }

        public bool isDelimiter(string s)
        {
            return contains(listDelimiters, s);
        }

        public bool isOperator(string s)
        {
            return contains(listOperators, s);
        }

        public bool isAllowedChar(string s)
        {
            return contains(allowedChars, s);
        }

        private bool contains(string[] a, string s)
        {
            bool val = false;
            if (a.Contains(s))
            {
                val = true;
            }
            return val;
        }

        public bool containsUpper(string a)
        {
            bool val = false;
            char[] aux = a.ToCharArray();
            for (int i = 0; i < aux.Length; i++)
            {
                if (Char.IsUpper(aux[i]))
                {
                    val = true;
                }
            }
            return val;
        }
        // /------------------------- //


        public string getSimplifiedGrammar_Keyword(string lexema)
        {
            if (lexema == "Begin")
            {
                return "a";
            }
            else if (lexema == "Boolean" || lexema == "True" || lexema == "False")
            {
                return "b";
            }
            else if (lexema == "For")
            {
                return "f";
            }
            else if (lexema == "If")
            {
                return "g";
            }
            else if (lexema == "Else")
            {
                return "j";
            }
            else if (lexema == "Int" || lexema == "Float" || lexema == "Char" || lexema == "String")
            {
                return "t";
            }
            else if (lexema == "While")
            {
                return "w";
            }
            else if (lexema == "End")
            {
                return "z";
            }
            else {
                return "";
            }

        }

    }

}
