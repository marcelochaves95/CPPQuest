using System.Collections.Generic;

namespace Linguagem
{
    //*** Se a expressao é simples será a da propriedade Expressao
    //*** Se a expressao é sob uma condição, para a propriedade Expressao 
    // ser executada, precisa passar pela expressão da propriedade Condicao caso contrário
    // a expressão da propriedade ExpressaoCondicaoNaoAtendida será executada 
    // caso exista expressão nela

    public class ExpressaoCodigoIntermediario
    {
        private List<Token> _expressao = new List<Token>();
        public List<Token> Expressao
        {
            get
            {
                return _expressao;
            }
        }

        private List<Token> _condicao = new List<Token>();
        public List<Token> Condicao
        {
            get
            {
                return _condicao;
            }
            set
            {
                _condicao = value;
            }
        }

        private List<Token> _expressaocondicaonaoatendida = new List<Token>();
        public List<Token> ExpressaoCondicaoNaoAtendida
        {
            get
            {
                return _expressaocondicaonaoatendida;
            }
        }

        public bool ExpressaoSobCondicao
        {
            get
            {
                bool retorno;
                if (_condicao.Count > 0)
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }

                return retorno;
            }
        }

        public ExpressaoCodigoIntermediario()
        {

        }

        public ExpressaoCodigoIntermediario(List<Token> expressao, List<Token> expressaocondicaonaoatendida, List<Token>condicao)
        {
            this._expressao = expressao;
            this._expressaocondicaonaoatendida = expressaocondicaonaoatendida;
            this._condicao = condicao;
        }

        public void AdicionarTokenEmExpressao(Token tk)
        {
            _expressao.Add(tk);
        }

        public void AdicionarTokenEmCondicao(Token tk)
        {
            _condicao.Add(tk);
        }

        public void AdicionarTokenEmExpressaoCondicaoNaoAtendida(Token tk)
        {
            _expressaocondicaonaoatendida.Add(tk);
        }

        public List<Token> getCopiaCondicao()
        {
            List<Token> retorno = new List<Token>();
            foreach(Token tk in Condicao)
            {
                retorno.Add(tk);
            }
            return retorno;
        }
    }
}