using System.Collections.Generic;

namespace Sesamo.Intermediaries
{
    public class Intermediate
    {
        List<IntermediateExpression> _codigo = new List<IntermediateExpression>();
        public List<IntermediateExpression> Codigo
        {
            get
            {
                return _codigo;
            }
        }

        public Intermediate()
        {
        }

        public Intermediate(List<IntermediateExpression> codigo)
        {
            _codigo = codigo;
        }

        public void AdicionarExpressao(IntermediateExpression Expressao)
        {
            _codigo.Add(Expressao);
        }
    }
}