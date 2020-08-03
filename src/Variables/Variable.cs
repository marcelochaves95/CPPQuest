using System.Collections.Generic;

namespace Sesamo.Variables
{
    public class Variable
    {
        public Variable(List<Value> Variaveis)
        {
            //CARREGA A LISTA DE VARIAVEIS
            this._listaVariaveis = Variaveis;
        }

        public Value getVariavel(string Nome)
        {
            Value retorno = null;
            foreach (Value var in _listaVariaveis)
            {
                if (var.VariableName == Nome)
                {
                    retorno = var;
                    break;
                }
            }
            return retorno;
        }

        private List<Value> _listaVariaveis;
        public List<Value> ListaVariaveis
        {
            get
            {
                return _listaVariaveis;
            }
        }

        public void AdicionarVariavel(Value Variavel)
        {
            _listaVariaveis.Add(Variavel);
        }

        public bool ExisteVariavel(Value Variavel)
        {
            bool retorno = false;

            foreach (Value var in _listaVariaveis)
            {
                if (var.VariableName == Variavel.VariableName)
                {
                    retorno = true;
                    break;
                }
            }

            return retorno;
        }

        public bool ExisteVariavel(string Variavel)
        {
            bool retorno = false;

            Value vl = new Value(Variavel, "0", null);
            retorno = ExisteVariavel(vl);

            return retorno;
        }
    }
}