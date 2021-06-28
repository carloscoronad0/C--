using C__.UniversalModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace C__.AnalizadorLexico
{
    public class AnalizadorLexico
    {

        // ===== Class that contains the scanner  ===== //
        DefinicionesLexicas dl;
        public AnalizadorLexico()  //Constructor
        {
            dl = new DefinicionesLexicas();
        }
        // Words
        List<List<char>> words = new List<List<char>>();
        int wordPosi = 0;

        // Numbers
        List<List<char>> numbers = new List<List<char>>();
        int numberPosi = 0;

        // Relational Operators
        List<List<char>> relationalOperators = new List<List<char>>();
        int relationalOperatorPosi = 0;

        // Operators
        List<List<char>> operators = new List<List<char>>();
        int operatorPosi = 0;

        // Delimiters
        List<List<char>> delimiters = new List<List<char>>();
        int delimiterPosi = 0;

        bool isValid;
        string errorMessage = "";
        // /////////////////////////////////////////////

        // Funcion that obtains all the tokens in the source code
        public Stack<Token> getTokens(string file)
        {
            clearStructures(); // Every time we call this function clear all the structures for new use
            addList(); // Initialize the first list of the structures that use List<List<char>>

            Stack<Token> tokenStack = new Stack<Token>(); // Create the stack of token tha will be passed to the sintactic analyzer

            char[] sourceCode = file.ToCharArray(); //Char array to work with
            int line = 1;   // Number of current line
            int column = 1; //Number of current column
            isValid = true;

            for (int i = 0; i < sourceCode.Length; i++)
            {   //Loop to cycle around the source code (Working char by char)
                char ch = sourceCode[i];
                char nextch;

                if (i == sourceCode.Length - 1)
                {
                    nextch = '\n';  // If the next char is null make it a new line
                }
                else
                {
                    nextch = sourceCode[i + 1]; //Get the next char
                }

                //  ----------------------- //
                // Analysis of the character //
                if (ch == '\n')
                {
                    line++;
                    column = 0;
                }
                else if (ch == ' ' || ch == '\t' || ch == '\r') { }

                // WORDS
                else if (Char.IsLetter(ch) || dl.isAllowedChar(ch.ToString()))
                {
                    words[wordPosi].Add(ch);   // Store the char into the list
                    if (!Char.IsLetter(nextch) && !dl.isAllowedChar(nextch.ToString())) // Word formation completed, update index and store number of line
                    {
                        // Once the word is formed we generate the token and add it to the token stack
                        tokenStack.Push(keywordOrIdentifierTokenGenerator(words[wordPosi], line, column));
                        wordPosi++;
                        words.Add(new List<char>());
                    }
                }

                // NUMBERS
                else if (Char.IsNumber(ch))
                {
                    numbers[numberPosi].Add(ch); // Store the char into the list
                    if (nextch == '.')
                    {
                        numbers[numberPosi].Add(nextch); // If the next char is a point store it into the numbers list
                        i++;
                        column++;
                    }
                    else if (Char.IsLetter(nextch))
                    {
                        // error on the formation of number
                        errorMessage += "Bad formation of a number\n";
                        errorMessage += $" ==>> bad number in position (Row, Column):\t ({line}, {column})\n\n";
                        isValid = false;
                    }
                    else if (!Char.IsNumber(nextch))    // Number formation completed, update index and store number of line
                    {
                        // Once the number is formed we generate the token and add it to the token stack
                        tokenStack.Push(numberTokenGenerator(numbers[numberPosi], line, column));
                        numberPosi++;
                        numbers.Add(new List<char>());
                    }
                }

                // OPERATORS
                else if (dl.isOperator(ch.ToString()))
                {
                    if (ch == '*')
                    {
                        operators[operatorPosi].Add(ch);
                        if (nextch == '*')
                        {
                            operators[operatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(operatorTokenGenerator(operators[operatorPosi], line, column));
                        operatorPosi++;
                        operators.Add(new List<char>());
                    }
                    else if (ch == '/')
                    {
                        operators[operatorPosi].Add(ch);
                        if (nextch == '/')
                        {
                            operators[operatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(operatorTokenGenerator(operators[operatorPosi], line, column));
                        operatorPosi++;
                        operators.Add(new List<char>());
                    }
                    else if (ch == '+')
                    {
                        operators[operatorPosi].Add(ch);
                        if (nextch == '+')
                        {
                            operators[operatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(operatorTokenGenerator(operators[operatorPosi], line, column));
                        operatorPosi++;
                        operators.Add(new List<char>());
                    }
                    else if (ch == '-')
                    {
                        operators[operatorPosi].Add(ch);
                        if (nextch == '-')
                        {
                            operators[operatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(operatorTokenGenerator(operators[operatorPosi], line, column));
                        operatorPosi++;
                        operators.Add(new List<char>());
                    }
                    else
                    {
                        tokenStack.Push(operatorTokenGenerator(operators[operatorPosi], line, column));
                        operatorPosi++;
                        operators.Add(new List<char>());
                    }
                }

                // DELIMITERS
                else if (dl.isDelimiter(ch.ToString()))
                {
                    delimiters[delimiterPosi].Add(ch);
                    tokenStack.Push(DelimiterTokenGenerator(delimiters[delimiterPosi], line, column));
                    delimiterPosi++;
                    delimiters.Add(new List<char>());
                }

                else if (dl.startRelOperator(ch))
                {
                    if (ch == '>')
                    {
                        relationalOperators[relationalOperatorPosi].Add(ch);
                        if (nextch == '=')
                        {
                            relationalOperators[relationalOperatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(relationalOperatorTokenGenerator(relationalOperators[relationalOperatorPosi], line, column));
                        relationalOperatorPosi++;
                        relationalOperators.Add(new List<char>());  
                    }
                    else if (ch == '<')
                    {
                        relationalOperators[relationalOperatorPosi].Add(ch);
                        if (nextch == '=')
                        {
                            relationalOperators[relationalOperatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(relationalOperatorTokenGenerator(relationalOperators[relationalOperatorPosi], line, column));
                        relationalOperatorPosi++;
                        relationalOperators.Add(new List<char>());
                    }
                    else if (ch == '=')
                    {
                        relationalOperators[relationalOperatorPosi].Add(ch);
                        if (nextch == '=' || nextch == '!')
                        {
                            relationalOperators[relationalOperatorPosi].Add(nextch);
                            i++;
                            column++;
                        }
                        tokenStack.Push(relationalOperatorTokenGenerator(relationalOperators[relationalOperatorPosi], line, column));
                        relationalOperatorPosi++;
                        relationalOperators.Add(new List<char>());
                    }
                    else if (ch == '|')
                    {
                        if (nextch == '|')
                        {
                            relationalOperators[relationalOperatorPosi].Add(ch);
                            relationalOperators[relationalOperatorPosi].Add(nextch);
                            i++;
                            column++;

                            tokenStack.Push(relationalOperatorTokenGenerator(relationalOperators[relationalOperatorPosi], line, column));
                            relationalOperatorPosi++;
                            relationalOperators.Add(new List<char>());
                        }
                        else {
                            errorMessage += $" ==>> bad formation of relational operator in position (Row, Column):\t ({line}, {column})\n\n";
                        }
                    }
                    else if (ch == '&')
                    {
                        if (nextch == '&')
                        {
                            relationalOperators[relationalOperatorPosi].Add(ch);
                            relationalOperators[relationalOperatorPosi].Add(nextch);
                            i++;
                            column++;

                            tokenStack.Push(relationalOperatorTokenGenerator(relationalOperators[relationalOperatorPosi], line, column));
                            relationalOperatorPosi++;
                            relationalOperators.Add(new List<char>());
                        }
                        else
                        {
                            errorMessage += $" ==>> bad formation of relational operator in position (Row, Column):\t ({line}, {column})\n\n";
                        }
                    }
                }
                else
                {
                    errorMessage += $" ==>> Invalid character in source code:\t{ch}\t\t Position(Row, Column):\t ({line}, {column})";
                    isValid = false;
                    break;
                }
                column++;
            } //End for
            cleanUnusableLists();

            if (isValid)
            {
                return tokenStack;
            }
            else
            {
                return null;
                // Coordinate with team
            }
        }



        //  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // FUNCTIONS
        // Generatation of Tokens //

        private Token keywordOrIdentifierTokenGenerator( List<char> word, int line, int column ) //Function to separte and store identifiers from keywords
        {
            string w = "";
            Token token = new Token();

            foreach (char ch in word)
            {
                w = w + ch.ToString();
            }
            // Once we get the complete word we verify if is keyword or not

            if (dl.isKeyword(w))
            {
                token.token = dl.getSimplifiedGrammar_Keyword(w);
                token.tipoToken = "Keyword";
                token.lexema = w;
                token.linea = line;
                token.columna = column;
            }
            else
            {
                if (dl.containsUpper(w))
                {
                    errorMessage += $"\nError detected in the formation of identifier";
                    errorMessage += $"An identifier can not have one or more upper case characters, review your code\n\n";
                    errorMessage += $"\n ==>>\t {w} \t\t Position(Row, Column):\t ({line}, {column})";
                    isValid = false;
                }
                else
                {
                    token.token = "i";
                    token.tipoToken = "Identificador";
                    token.lexema = w;
                    token.linea = line;
                    token.columna = column;
                }
            }
            return token;
        }

        private Token numberTokenGenerator(List<Char> number, int line, int column)
        {
            string w = "";
            Token token = new Token();

            foreach (char ch in number)
            {
                w = w + ch.ToString();
            }

            if (w.Contains("."))
            {
                token.token = "r";
                token.tipoToken = "Numero Real";
                token.lexema = w;
                token.linea = line;
                token.columna = column;
            }
            else
            {
                token.token = "e";
                token.tipoToken = "Numero Entero";
                token.lexema = w;
                token.linea = line;
                token.columna = column;
            }
            return token;
        }

        private Token operatorTokenGenerator(List<char> op, int line, int column)
        {
            string w = "";
            Token token = new Token();

            foreach (char ch in op)
            {
                w = w + ch.ToString();
            }

            if (w == "++" || w == "--")
            {
                token.token = "m";
            }
            else
            {
                token.token = "o";
            }
            token.tipoToken = "Operador";
            token.lexema = w;
            token.linea = line;
            token.columna = column;
            return token;
        }

        private Token DelimiterTokenGenerator(List<char> delimiter, int line, int column)
        {
            string w = "";
            Token token = new Token();

            foreach (char ch in delimiter)
            {
                w = w + ch.ToString();
            }

            if(w == "{")
            {
                token.token = "p";
            }else if (w == "}")
            {
                token.token = "q";
            }else if (w == "(")
            {
                token.token = "u";
            }
            else if (w == ")")
            {
                token.token = "v";
            }
            else if (w == ",")
            {
                token.token = "x";
            }
            else if (w == ";")
            {
                token.token = "y";
            }
            token.tipoToken = "Delimitador";
            token.lexema = w;
            token.linea = line;
            token.columna = column;
            return token;
        }

        private Token relationalOperatorTokenGenerator(List<char> relOp, int line, int column)
        {
            string w = "";
            Token token = new Token();

            foreach (char ch in relOp)
            {
                w = w + ch.ToString();
            }

            if(w == "=")
            {
                token.token = "k";
            }else if (w == "||" || w == "&&")
            {
                token.token = "c";
            }
            else
            {
                token.token = "d";
            }
            token.tipoToken = "Operador Relacional";
            token.lexema = w;
            token.linea = line;
            token.columna = column;
            return token;
        }


        // Manipulation of lists //
        private void clearStructures()
        {
            // Words 
            words.Clear(); wordPosi = 0;
            // Numbers 
            numbers.Clear(); numberPosi = 0;
            // Relational Operators 
            relationalOperators.Clear(); relationalOperatorPosi = 0;
            // Operators 
            operators.Clear(); operatorPosi = 0;
            // Delimiters
            delimiters.Clear(); delimiterPosi = 0;
        }
        private void addList()
        {
            words.Add(new List<char>());
            numbers.Add(new List<char>());
            relationalOperators.Add(new List<char>());
            operators.Add(new List<char>());
            delimiters.Add(new List<char>());
        }

        private void deleteLastElement(List<List<char>> list)
        {
            int numL = list.Count;
            list.RemoveAt(numL - 1);
        }

        private void cleanUnusableLists()
        {
            deleteLastElement(words);
            deleteLastElement(numbers);
            deleteLastElement(operators);
            deleteLastElement(relationalOperators);
            deleteLastElement(delimiters);
        }

    }
}
    