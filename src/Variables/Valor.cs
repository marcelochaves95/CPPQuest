using Sesamo.Tokens;

namespace Sesamo.Variables
{
    public class Valor : Token
    {
        public Valor(string Nome, string Valor, string TipoValor)
        {
            this._nomeVariavel = Nome;
            this._valorVariavel = Valor;
            this._tipo = TipoValor;
        }

        public Valor(string Valor, string TipoValor, int NumeroLinha)
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

        public Valor Copia()
        {
            Valor vCopia = new Valor(this.ValorVariavel, this.Tipo, this.Linha);
            vCopia.NomeVariavel = this.NomeVariavel;
            return vCopia;
        }
    }
}