using C__.UniversalModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace C__.AnalizadorSintactico
{
    class AnalizadorSintactico
    {
        
        static public List<Stack<string>> LeerCadenaFull(string cadena, Table table)
        {
            Queue<Expresion> expresions = new Queue<Expresion>();
            Stack<int> pila = new Stack<int>();
            Stack<string> pilastr = new Stack<string>();
            List<Stack<string>> pasosArbol = new List<Stack<string>>();

            char s = head(ref cadena);

            int estado = 0;
            int sigEstado = 0;
            string accionEstat = "";
            char accion = 's';
            int numprod = 0;

            pila.Push(estado);
            pilastr.Push(estado.ToString()); // Anadimos 0 a las 2 pilas
            var clonPila = clonarPila(pilastr);
            pasosArbol.Add(clonPila); //Agregar a los pasos del arbol
            while (true)
            {
                estado = pila.Peek();
                accionEstat = table.accion(estado, s);  //accionEstado: s1,r4,g8
                //Si no hay accion para s 
                // pedir accion(estado,lambda)
                // Si no hay accionEstat = ".."
                // guardar en accionEstat
                accion = head(ref accionEstat);

                if (accion == 's')
                {
                    pila.Push(Convert.ToInt32(accionEstat));
                    //Agregamos primero el simbolo y luego el estado
                    pilastr.Push(s.ToString());
                    pilastr.Push(accionEstat);
                    clonPila = clonarPila(pilastr);
                    pasosArbol.Add(clonPila); //Agregar a los pasos del arbol

                    s = head(ref cadena);
                }
                else if (accion == 'r')
                {
                    numprod = Convert.ToInt32(accionEstat);//Accion estat no es estado sino numero de producion 
                    Expresion produccion = table.grammar[numprod - 1];
                    foreach (char c in produccion._RestBody) //Por cada elemento de la produccion sacamos estados de la pila
                    {
                        pila.Pop();
                        //Quitamos 2 elementos de la pila compuesta
                        pilastr.Pop();
                        pilastr.Pop();
                    }

                    estado = pila.Peek(); //estado en la parte superior de la pila

                    accionEstat = table.accion(estado, produccion.Head);//GOTO con el no terminal de la reduccion
                    accion = head(ref accionEstat);

                    pila.Push(Convert.ToInt32(accionEstat));//Meter el estado del goto a la pila

                    pilastr.Push(produccion.Head.ToString());
                    pilastr.Push(accionEstat);//Meter a la pila compuesta Primero el Noterminal de la red y luego el estado

                    clonPila = clonarPila(pilastr);
                    pasosArbol.Add(clonPila); //Agregar a los pasos del arbol

                    expresions.Enqueue(produccion);//Metemos la produccion de esta reduccion al historial de reducciones
                }
                else if (accion == 'A')
                {
                    Console.WriteLine("Cadena aceptada");
                    break;
                }
                else
                {
                    throw new Exception("La cadena no pertenece a la gramatica");
                }

            }
            return pasosArbol;
        }
        static public IpilaResult LeerTokens(Queue<Token> cadena, Table2 table)
        {
            var colaerr = new Queue<string>();
            Queue<Expresion> expresions = new Queue<Expresion>();
            Stack<int> pila = new Stack<int>();
            Stack<string> pilastr = new Stack<string>();
            List<Stack<string>> pasosArbol = new List<Stack<string>>();

            Token token= cadena.Dequeue();
            char s = token.token[0];

            int estado = 0;
            string accionEstat = "";
            char accion = 's';
            int numprod = 0;

            pila.Push(estado);
            pilastr.Push(estado.ToString()); // Anadimos 0 a las 2 pilas
            var clonPila = clonarPila(pilastr);
            pasosArbol.Add(clonPila); //Agregar a los pasos del arbol
            
            while (true)
            {
                estado = pila.Peek();
                accionEstat = table.accion(estado, s);  //accionEstado: s1,r4,g8
                accion = head(ref accionEstat);

                if (accion == 's')
                {
                    pila.Push(Convert.ToInt32(accionEstat));
                    //Agregamos primero el simbolo y luego el estado
                    pilastr.Push(s.ToString());
                    pilastr.Push(accionEstat);
                    clonPila = clonarPila(pilastr);
                    pasosArbol.Add(clonPila); //Agregar a los pasos del arbol

                    token = cadena.Dequeue();
                    s = token.token[0];
                }
                else if (accion == 'r')
                {
                    numprod = Convert.ToInt32(accionEstat);//Accion estat no es estado sino numero de producion 
                    Expresion produccion = table.grammar[numprod - 1];
                    foreach (char c in produccion._RestBody) //Por cada elemento de la produccion sacamos estados de la pila
                    {
                        pila.Pop();
                        //Quitamos 2 elementos de la pila compuesta
                        pilastr.Pop();
                        pilastr.Pop();
                    }

                    estado = pila.Peek(); //estado en la parte superior de la pila

                    accionEstat = table.accion(estado, produccion.Head);//GOTO con el no terminal de la reduccion
                    accion = head(ref accionEstat);

                    if (accion == '.')
                    {
                        colaerr.Enqueue($"Syntax Error: {token.token} | Line: {token.line} | column: {token.column}");
                        return new PilaErrores() {
                            errores = colaerr
                        };
                    }

                    pila.Push(Convert.ToInt32(accionEstat));//Meter el estado del goto a la pila

                    pilastr.Push(produccion.Head.ToString());
                    pilastr.Push(accionEstat);//Meter a la pila compuesta Primero el Noterminal de la red y luego el estado

                    clonPila = clonarPila(pilastr);
                    pasosArbol.Add(clonPila); //Agregar a los pasos del arbol

                    expresions.Enqueue(produccion);//Metemos la produccion de esta reduccion al historial de reducciones
                }
                else if (accion == 'A')
                {
                    Console.WriteLine("Cadena aceptada");
                    break;
                }
                else
                {
                    colaerr.Enqueue($"Syntax Error: {token.token} | Line: {token.line} | column: {token.column}");
                    return new PilaErrores()
                    {
                        errores = colaerr
                    };
                }

            }
            return new PilaDescomposicion
            {
                pasos = pasosArbol
            };
        }
        static public char head(ref string s)
        {
            try
            {
                char c = s[0];
                if (s.Length > 1)
                {
                    s = s.Substring(1);
                }
                else
                {
                    s = "";
                }
                return c;
            }
            catch
            {
                return '~';
            }
        }

        private static Stack<string> clonarPila(Stack<string> orig)
        {
            Stack<string> clon = new Stack<string>();
            foreach (var elem in orig)
            {
                clon.Push(elem);
            }
            return clon;
        }
    }
}
