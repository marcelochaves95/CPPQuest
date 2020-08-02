namespace Sesamo.Operators.Comparisons
{
    public class Equal : Comparison, IOperator
    {
        #region IOperador Members

        private string _cadeia = "==";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Equal()
        {
        }

        public Equal(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}