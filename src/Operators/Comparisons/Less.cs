namespace Sesamo.Operators.Comparisons
{
    public class Less : Comparison, IOperator
    {
        #region IOperador Members

        private string _cadeia = "<";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Less()
        {
        }

        public Less(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}