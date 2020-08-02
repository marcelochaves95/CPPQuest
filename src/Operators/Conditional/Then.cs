namespace Sesamo.Operators.Conditional
{
    public class Then : Operator, IOperator
    {
        #region IOperador Members

        private string _cadeia = "then";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Then()
        {
        }

        public Then(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}