namespace Sesamo.Operators.Conditional
{
    public class If : Operator, IOperator
    {
        #region IOperador Members

        private string _cadeia = "if";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public If()
        {
        }

        public If(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}