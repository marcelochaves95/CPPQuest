namespace Sesamo.Operators.Comparisons
{
    public class BiggerOrEqual : Comparison, IOperator
    {
        #region IOperador Members

        private string _cadeia = ">=";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public BiggerOrEqual()
        {
        }

        public BiggerOrEqual(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}