namespace Sesamo.Operators.Comparisons
{
    public class OMaior : OComparacao, IOperador
    {
        #region IOperador Members

        private string _cadeia = ">";
        public override Cadeia Cadeia
        {
            get { return new Cadeia(_cadeia); }
        }

        #endregion

        public OMaior()
        {
        }

        public OMaior(int NumeroLinha)
        {
            this.Linha = NumeroLinha;
        }
    }
}