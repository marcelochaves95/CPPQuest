using System.Collections.Generic;

namespace Sesamo.Intermediaries
{
    public class Intermediate
    {
        private readonly List<IntermediateExpression> _code = new List<IntermediateExpression>();
        public List<IntermediateExpression> Code => _code;

        public Intermediate()
        {
        }

        public Intermediate(List<IntermediateExpression> code)
        {
            _code = code;
        }

        public void AddExpression(IntermediateExpression Expressao)
        {
            _code.Add(Expressao);
        }
    }
}