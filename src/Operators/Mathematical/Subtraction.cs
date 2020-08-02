namespace Sesamo.Operators.Mathematical
{
    public class Subtraction : Mathematics, IOperator
    {
        #region IOperador Members

        private string _cadeia = "-";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Subtraction()
        {
        }

        public Subtraction(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}