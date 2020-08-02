namespace Sesamo.Operators.Conditional
{
    public class EndIf : Operator, IOperator
    {
        #region IOperador Members

        private string _cadeia = "endif";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public EndIf()
        {
        }

        public EndIf(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}