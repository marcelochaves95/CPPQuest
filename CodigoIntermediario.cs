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

        private Mensagens _mensagens;
        public Mensagens Mensagens
        {
            get
            {
                return _mensagens;
            }
            set
            {
                _mensagens = value;
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