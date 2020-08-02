namespace Sesamo.Operators.Conditional
{
    public class Else : Operator, IOperator
    {
        #region IOperador Members

        private string _cadeia = "else";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Else()
        {
        }

        public Else(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}