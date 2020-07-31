namespace Linguagem
{
    public class OEntao : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "then";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OEntao()
        {
        }

        public OEntao(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}