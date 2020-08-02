namespace Sesamo.Operators.Comparisons
{
    public class Different : Comparison, IOperator
    {
        #region IOperador Members

        private string _cadeia = "!=";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Different()
        {
        }

        public Different(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}