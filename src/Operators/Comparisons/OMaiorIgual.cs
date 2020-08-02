namespace Sesamo.Operators.Comparisons
{
    public class OMaiorIgual : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = ">=";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OMaiorIgual()
        {
        }

        public OMaiorIgual(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}