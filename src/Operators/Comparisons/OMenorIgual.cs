namespace Linguagem
{
    public class OMenorIgual : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "<=";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OMenorIgual()
        {
        }

        public OMenorIgual(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}