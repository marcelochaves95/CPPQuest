namespace Sesamo.Operators
{
    public class Chain
    {
        private string _value;
        public string Value
        {
            get => _value;
            set => _value = value;
        }

        public Chain(string valor)
        {
            _value = valor;
        }
    }
}