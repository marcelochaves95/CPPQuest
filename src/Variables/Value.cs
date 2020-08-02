using Sesamo.Tokens;

namespace Sesamo.Variables
{
    public class Value : Token
    {
        public Value(string Nome, string Valor, string TipoValor)
        {
            this._nomeVariavel = Nome;
            this._valorVariavel = Valor;
            this._tipo = TipoValor;
        }

        public Value(string Valor, string TipoValor, int NumeroLinha)
        {
            this._valorVariavel = Valor;
            this._tipo = TipoValor;
            this.Linha = NumeroLinha;
        }

        private string _nomeVariavel;
        public string NomeVariavel
        {
            get { return _nomeVariavel; }
            set { _nomeVariavel = value; }
        }

        private string _valorVariavel;
        public string ValorVariavel
        {
            get { return _valorVariavel; }
            set { _valorVariavel = value; }
        }

        private string _tipo;
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public Value Copia()
        {
            Value vCopia = new Value(this.ValorVariavel, this.Tipo, this.Linha);
            vCopia.NomeVariavel = this.NomeVariavel;
            return vCopia;
        }
    }
}