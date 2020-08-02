namespace Sesamo.Operators.Comparisons
{
    public class OMenor : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = "<";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OMenor()
        {
        }

        public OMenor(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}