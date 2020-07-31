namespace Linguagem
{
    public class OSenao : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "else";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OSenao()
        {
        }

        public OSenao(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}