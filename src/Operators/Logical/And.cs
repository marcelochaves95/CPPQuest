namespace Sesamo.Operators.Logical
{
    public class And : Logic, IOperator
    {
        #region IOperador Members

        private string _cadeia = "and";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public And()
        {
        }

        public And(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}