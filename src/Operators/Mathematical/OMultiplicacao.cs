namespace Sesamo.Operators.Mathematical
{
    public class OMultiplicacao : OMatematico, IOperador
    {
        #region IOperador Members

        private string _cadeia = "*";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OMultiplicacao()
        {
        }

        public OMultiplicacao(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}