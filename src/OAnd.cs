namespace Linguagem
{
    public class OAnd : OLogico, IOperador
    {
        #region IOperador Members

        private string _cadeia = "and";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OAnd()
        {
        }

        public OAnd(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}