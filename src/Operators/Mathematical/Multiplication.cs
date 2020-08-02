namespace Sesamo.Operators.Mathematical
{
    public class Multiplication : Mathematics, IOperator
    {
        #region IOperador Members

        private string _cadeia = "*";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Multiplication()
        {
        }

        public Multiplication(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}