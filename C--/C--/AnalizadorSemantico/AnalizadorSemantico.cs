using System;
using System.Collections.Generic;
using System.Text;
using C__.UniversalModels;

namespace C__.AnalizadorSemantico
{
    class AnalizadorSemantico
    {
        private List<Token> _simbolList;
        private List<Stack<string>> _slrStackList;
        private ArbolDeAnalisisGramatical _grammarAnalysisTree;

        public AnalizadorSemantico(List<Token> sl, List<Stack<string>> slrSL)
        {
            _simbolList = sl;
            _slrStackList = slrSL;
            //_grammarAnalysisTree = new ArbolDeAnalisisGramatical();
        }

        
    }
}
