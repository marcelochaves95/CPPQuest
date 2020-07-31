namespace Linguagem
{
    public class AnalisadorLexico
    {
        private string Espaco = "º";
        private string _mensagemerro;
        public string MensagemErro
        {
            get { return _mensagemerro; }
            set { _mensagemerro = value; }
        }

        private Variaveis var = null;
        public Variaveis Variaveis
        {
            get { return var; }
        }

        private List<Token> _codigofonte;
        public List<Token> CodigoFonte
        {
            get
            {
                if (_codigofonte == null)
                {
                    _codigofonte = new List<Token>();
                }
                return _codigofonte;
            }
        }

        public bool Validar(string Codigo, List<Valor> ListaVariaveis)
        {
            bool retorno = true;

            //*** SUBSTITUI + de 1 ESPAÇOS POR 1 ESPAÇO SOMENTE FORA DE STRINGS IDENTIFICADO POR COMEÇAR COM (") E TERMINAR COM (")
            //*** SUBSTITUI ESPAÇO PELA VARIAVEL Espaco PARA NÃO DAR PROBLEMA DE IDENTIFICAÇÃO DE TOKENS STRINGS COM SPAÇO QUANDO FIZER SPLIT
            string codigoRemontado = "";
            bool dentroDeString = false;
            for (int pos = 0; pos < Codigo.Length; pos++)
            {
                char letra = Codigo[pos];
                char letraAnterior = new char();
                char proximaLetra = new char();

                if (pos > 0)
                {
                    letraAnterior = Codigo[pos - 1];
                }

                if (pos < Codigo.Length - 1)
                {
                    proximaLetra = Codigo[pos + 1];
                }

                if (letra == '"')
                {
                    if (!dentroDeString)
                    {
                        dentroDeString = true;
                    }
                    else
                    {
                        dentroDeString = false;
                    }
                }

                if (letra == '\n' && dentroDeString)
                {
                    break;
                }
                else if (dentroDeString)
                {
                    codigoRemontado += letra.ToString();
                }
                else if (letra == ' ')
                {
                    if (letraAnterior != ' ' && letraAnterior != '\n' && proximaLetra != '\n')
                    {
                        codigoRemontado += Espaco;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (dentroDeString)
                {
                    this._mensagemerro = "Sequencia String de valores não fechada.";
                    return false;
                }

                Codigo = codigoRemontado;
                
                //*** SEPARA ENTER DOS OUTROS CARACTERES COM ESPAÇO
                Codigo = Codigo.Replace("\n", Espaco + "\n" + Espaco);

                string[] Tokens = Codigo.Split(Convert.ToChar(Espaco));
                int Linha = 1;

                for (int pos = 0; pos < Tokens.Length; pos++)
                {
                    string valor = Tokens[pos] != "\n" ? Tokens[pos].Trim() : Tokens[pos];
                    
                    //*** IDENTIFICA AS LINHAS POR ENTER
                    if (valor == "\n")
                    {
                        Linha++;
                        continue;
                    }
                    else if (valor == "")
                    {
                        continue;
                    }

                    var = new Variaveis(ListaVariaveis);
                    Int64 numeroConvertido = 0;
                    
                    //*** SE FOR UMA STRING
                    if (valor[0] == '"')
                    {
                        CodigoFonte.Add(new Valor(valor, Tipos.txt, Linha));
                    }
                    //*** SE FOR NÚMERO
                    else if (Int64.TryParse(valor, out numeroConvertido))
                    {
                        CodigoFonte.Add(new Valor(numeroConvertido.ToString(), Tipos.Dec, Linha));
                    }
                    //*** SE É UM NOME DE VARIÁVEL
                    else if (var.ExisteVariavel(valor))
                    {
                        Valor variavel = var.getVariavel(valor).Copia();
                        variavel.Linha = Linha;
                        CodigoFonte.Add(variavel);
                    }
                    
                    //*** SE É UM IF
                    else if (new OSe().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OSe(Linha));
                    }
                    //*** SE É UM THEN
                    else if (new OEntao().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OEntao(Linha));
                    }
                    //*** SE É UM ELSE
                    else if (new OSenao().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OSenao(Linha));
                    }
                    //*** SE É UM ENDIF
                    else if (new OFimSe().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OFimSe(Linha));
                    }
                    
                    //*** SE É UM IGUAL
                    else if (new OIgual().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OIgual(Linha));
                    }
                    //*** SE É UM DIFERENTE
                    else if (new ODiferente().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new ODiferente(Linha));
                    }
                    //*** SE É UM MAIOR
                    else if (new OMaior().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OMaior(Linha));
                    }
                    //*** SE É UM MENOR
                    else if (new OMenor().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OMenor(Linha));
                    }
                    //*** SE É UM MAIOR OU IGUAL >=
                    else if (new OMaiorIgual().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OMaiorIgual(Linha));
                    }
                    //*** SE É UM MENOR OU IGUAL <=
                    else if (new OMenorIgual().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OMenorIgual(Linha));
                    }

                    //*** SE É UM SOMA
                    else if (new OSoma().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OSoma(Linha));
                    }
                    //*** SE É UM SUBTRAÇÃO
                    else if (new OSubtracao().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OSubtracao(Linha));
                    }
                    //*** SE É UM MULTIPLICAÇÃO
                    else if (new OMultiplicacao().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OMultiplicacao(Linha));
                    }
                    //*** SE É UM DIVISÃO
                    else if (new ODivisao().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new ODivisao(Linha));
                    }

                    //*** SE É UM OR
                    else if (new OOr().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OOr(Linha));
                    }
                    //*** SE É UM AND
                    else if (new OAnd().Cadeia.Valor == Valor)
                    {
                        CodigoFonte.Add(new OAnd(Linha));
                    }
                    //*** SE É VAZIO PULA
                    else if (Valor == "")
                    {
                        continue;
                    }

                    //*** SE NÃO ENTROU EM NENHUM DOS CASOS ANTERIORES, O SÍMBOLO NÃO É RECONHECIDO
                    else
                    {
                        this._mensagemerro = "Símbolo " + valor + " não reconhecido na linha " + Linha + ".";
                    }
                }

                return retorno
            }
        }
    }
}