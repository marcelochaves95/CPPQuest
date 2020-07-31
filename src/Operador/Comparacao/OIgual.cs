namespace Linguagem
{
    public class OIgual : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "==";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OIgual()
        {
        }

        public OIgual(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}