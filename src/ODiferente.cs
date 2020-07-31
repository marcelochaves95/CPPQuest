namespace Linguagem
{
    public class ODiferente : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "<>";
        public override Cadeia Cadeia
        {
            get
            {
                return new Cadeia(_cadeia);
            }
        }

        #endregion

        public ODiferente()
        {
        }

        public ODiferente(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}