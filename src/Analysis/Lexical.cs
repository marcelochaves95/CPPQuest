using System;
using System.Collections.Generic;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Lexical
    {
        private string Espaco = "º";
        private string _mensagemerro;
        public string MensagemErro
        {
            get { return _mensagemerro; }
            set { _mensagemerro = value; }
        }

        private Variable var = null;
        public Variable Variable
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

        public bool Validar(string Codigo, List<Value> ListaVariaveis)
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

                    var = new Variable(ListaVariaveis);
                    Int64 numeroConvertido = 0;
                    
                    //*** SE FOR UMA STRING
                    if (valor[0] == '"')
                    {
                        CodigoFonte.Add(new Value(valor, Types.Txt, Linha));
                    }
                    //*** SE FOR NÚMERO
                    else if (Int64.TryParse(valor, out numeroConvertido))
                    {
                        CodigoFonte.Add(new Value(numeroConvertido.ToString(), Types.Dec, Linha));
                    }
                    //*** SE É UM NOME DE VARIÁVEL
                    else if (var.ExisteVariavel(valor))
                    {
                        Value variavel = var.getVariavel(valor).Copia();
                        variavel.Linha = Linha;
                        CodigoFonte.Add(variavel);
                    }
                    
                    //*** SE É UM IF
                    else if (new If().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new If(Linha));
                    }
                    //*** SE É UM THEN
                    else if (new Then().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Then(Linha));
                    }
                    //*** SE É UM ELSE
                    else if (new Else().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Else(Linha));
                    }
                    //*** SE É UM ENDIF
                    else if (new EndIf().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new EndIf(Linha));
                    }
                    
                    //*** SE É UM IGUAL
                    else if (new Equal().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Equal(Linha));
                    }
                    //*** SE É UM DIFERENTE
                    else if (new Different().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Different(Linha));
                    }
                    //*** SE É UM MAIOR
                    else if (new Bigger().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Bigger(Linha));
                    }
                    //*** SE É UM MENOR
                    else if (new Less().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Less(Linha));
                    }
                    //*** SE É UM MAIOR OU IGUAL >=
                    else if (new BiggerOrEqual().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new BiggerOrEqual(Linha));
                    }
                    //*** SE É UM MENOR OU IGUAL <=
                    else if (new LessOrEqual().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new LessOrEqual(Linha));
                    }

                    //*** SE É UM SOMA
                    else if (new Addition().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Addition(Linha));
                    }
                    //*** SE É UM SUBTRAÇÃO
                    else if (new Subtraction().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Subtraction(Linha));
                    }
                    //*** SE É UM MULTIPLICAÇÃO
                    else if (new Multiplication().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Multiplication(Linha));
                    }
                    //*** SE É UM DIVISÃO
                    else if (new Division().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Division(Linha));
                    }

                    //*** SE É UM OR
                    else if (new Or().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new Or(Linha));
                    }
                    //*** SE É UM AND
                    else if (new And().Chain.Valor == valor)
                    {
                        CodigoFonte.Add(new And(Linha));
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