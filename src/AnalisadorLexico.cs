using System;
using System.Collections.Generic;

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
            //*** SUBSTITUI + de 1 ESPAÇOS POR 1 ESPAÇO SOMENTE FORA DE STRINGS IDENTIFICADO POR COMEÇAR COM (") E TERMINAR COM (")
            //*** SUBSTITUI ESPAÇO PELA VARIAVEL Espaco PARA NÃO DAR PROBLEMA DE IDENTIFICAÇÃO DE TOKENS STRINGS COM SPAÇO QUANDO FIZER SPLIT
            string codigoRemontado = "";
            bool dentroDeString = false;
            for (int pos1 = 0; pos1 < Codigo.Length; pos1++)
            {
                char letra = Codigo[pos1];
                char letraAnterior = new char();
                char proximaLetra = new char();

                if (pos1 > 0)
                {
                    letraAnterior = Codigo[pos1 - 1];
                }

                if (pos1 < Codigo.Length - 1)
                {
                    proximaLetra = Codigo[pos1 + 1];
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

                for (int pos2 = 0; pos2 < Tokens.Length; pos2++)
                {
                    string valor = Tokens[pos2] != "\n" ? Tokens[pos2].Trim() : Tokens[pos2];
                    
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
                        CodigoFonte.Add(new Valor(valor, Tipos.Txt, Linha));
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
                    else if (new OSe().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OSe(Linha));
                    }
                    //*** SE É UM THEN
                    else if (new OEntao().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OEntao(Linha));
                    }
                    //*** SE É UM ELSE
                    else if (new OSenao().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OSenao(Linha));
                    }
                    //*** SE É UM ENDIF
                    else if (new OFimSe().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OFimSe(Linha));
                    }
                    
                    //*** SE É UM IGUAL
                    else if (new OIgual().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OIgual(Linha));
                    }
                    //*** SE É UM DIFERENTE
                    else if (new ODiferente().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new ODiferente(Linha));
                    }
                    //*** SE É UM MAIOR
                    else if (new OMaior().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OMaior(Linha));
                    }
                    //*** SE É UM MENOR
                    else if (new OMenor().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OMenor(Linha));
                    }
                    //*** SE É UM MAIOR OU IGUAL >=
                    else if (new OMaiorIgual().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OMaiorIgual(Linha));
                    }
                    //*** SE É UM MENOR OU IGUAL <=
                    else if (new OMenorIgual().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OMenorIgual(Linha));
                    }

                    //*** SE É UM SOMA
                    else if (new OSoma().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OSoma(Linha));
                    }
                    //*** SE É UM SUBTRAÇÃO
                    else if (new OSubtracao().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OSubtracao(Linha));
                    }
                    //*** SE É UM MULTIPLICAÇÃO
                    else if (new OMultiplicacao().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OMultiplicacao(Linha));
                    }
                    //*** SE É UM DIVISÃO
                    else if (new ODivisao().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new ODivisao(Linha));
                    }

                    //*** SE É UM OR
                    else if (new OOr().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OOr(Linha));
                    }
                    //*** SE É UM AND
                    else if (new OAnd().Cadeia.Valor == valor)
                    {
                        CodigoFonte.Add(new OAnd(Linha));
                    }
                    //*** SE É VAZIO PULA
                    else if (valor == "")
                    {
                        continue;
                    }

                    //*** SE NÃO ENTROU EM NENHUM DOS CASOS ANTERIORES, O SÍMBOLO NÃO É RECONHECIDO
                    else
                    {
                        this._mensagemerro = "Símbolo " + valor + " não reconhecido na linha " + Linha + ".";
                    }
                }
            }
            
            return true;
        }
    }
}