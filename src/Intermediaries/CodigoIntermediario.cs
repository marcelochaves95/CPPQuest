using System.Collections.Generic;

namespace Linguagem
{
    public class CodigoIntermediario
    {
        List<ExpressaoCodigoIntermediario> _codigo = new List<ExpressaoCodigoIntermediario>();
        public List<ExpressaoCodigoIntermediario> Codigo
        {
            get
            {
                return _codigo;
            }
        }

        public CodigoIntermediario()
        {
        }

        public CodigoIntermediario(List<ExpressaoCodigoIntermediario> codigo)
        {
            _codigo = codigo;
        }

        public void AdicionarExpressao(ExpressaoCodigoIntermediario Expressao)
        {
            _codigo.Add(Expressao);
        }
    }
}