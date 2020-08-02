namespace Sesamo.Operators.Comparisons
{
    public class LessOrEqual : Comparison, IOperator
    {
        #region IOperador Members

        private string _cadeia = "<=";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public LessOrEqual()
        {
        }

        public LessOrEqual(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}