namespace Linguagem
{
    public class ODivisao : OMatematico, IOperador
    {
        #region IOperador Members

        private string _cadeia = @"/";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public ODivisao()
        {
        }

        public ODivisao(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}