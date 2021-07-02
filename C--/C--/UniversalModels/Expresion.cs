using System;
using System.Collections.Generic;
using System.Text;

namespace C__.UniversalModels
{
    class Expresion
    {
        public Char Head { get; set; }
        public Queue<Char> _body = new Queue<Char>();

        public Queue<Char> _RestBody = new Queue<Char>();
        public int Number { get; set; }

        public Expresion(Char head, string body, int number)
        {
            Number = number;
            Head = head;
            foreach (char x in body)
            {
                _body.Enqueue(x);
            }
        }
        public void consumNextSimbol()
        {
            _RestBody.Enqueue(_body.Dequeue());
        }
        public char firstSimbol()
        {
            char c = '/';
            if (_body.Count > 0)
            {
                c = _body.Peek();
            }
            return c;
        }
    }
}
