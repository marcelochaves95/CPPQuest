namespace Sesamo.Operators.Mathematical
{
    public class Addition : Mathematics, IOperator
    {
        #region IOperador Members

        private string _cadeia = "+";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Addition()
        {
        }

        public Addition(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}