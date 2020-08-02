namespace Sesamo.Operators.Mathematical
{
    public class Division : Mathematics, IOperator
    {
        #region IOperador Members

        private string _cadeia = @"/";
        public override Chain Chain
        {
            get { return new Chain(_cadeia); }
        }

        #endregion

        public Division()
        {
        }

        public Division(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}