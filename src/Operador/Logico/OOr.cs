namespace Linguagem
{
    public class OOr : OLogico, IOperador
    {
        #region IOperador Members

        private string _cadeia = "or";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OOr()
        {
        }

        public OOr(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}