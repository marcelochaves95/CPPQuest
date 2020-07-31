namespace Linguagem
{
    public class Cadeia
    {
        private string valor_retorno;

        public Cadeia(string valor)
        {
            valor_retorno = valor;
        }

        public string Valor
        {
            get
            {
                return valor_retorno;
            }
            set
            {
                valor_retorno = value;
            }
        }
    }
}