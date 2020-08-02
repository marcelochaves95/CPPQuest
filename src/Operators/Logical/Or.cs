namespace Sesamo.Operators.Logical
{
    public class Or : Logic, IOperator
    {
        #region IOperador Members

        private string _cadeia = "or";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Or()
        {
        }

        public Or(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}