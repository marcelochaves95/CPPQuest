namespace Sesamo.Operators
{
    public class Chain
    {
        private string valor_retorno;

        public Chain(string valor)
        {
            valor_retorno = valor;
        }

        public string Valor
        {
            get { return valor_retorno; }
            set { valor_retorno = value; }
        }
    }
}