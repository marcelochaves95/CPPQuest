namespace Sesamo.Operators.Mathematical
{
    public class OSubtracao : OMatematico, IOperador
    {
        #region IOperador Members

        private string _cadeia = "-";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OSubtracao()
        {
        }

        public OSubtracao(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}