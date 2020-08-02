using System.Text;
using System.Text.RegularExpressions;
using Sesamo.Operators;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Parser
    {
        private string _mensagemerro;
        public string MensagemErro
        {
            get { return _mensagemerro; }
            set { _mensagemerro = value; }
        }

        private Lexical _analise;
        public Lexical AnalisadorLexica
        {
            get { return _analise; }
        }

        private string ExpressaoRegularCadeia()
        {
            return @"(\" + '"'.ToString() + @"(\w|\.|\:|\,|\-|\+|\*|\/|\&|\(|\)|\%|\$|\#|\@|\!|\?|\<|\>|\;|\ )*\" + '"'.ToString() + @")|\w+";
        }

        private string ExpressaoRegularPermitePontoEmVariavel()
        {
            return @"(\.\w+)*";
        }

        private string ExpressaoRegularOperadoresComparacao()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"(\");
            sb.Append(new Equal().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Different().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Bigger().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Less().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new BiggerOrEqual().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new LessOrEqual().Chain.Valor);

            sb.Append(@")");

            return sb.ToString();
        }

        private string ExpressaoRegularOperadoresLogicos()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"(");

            sb.Append(new And().Chain.Valor);

            sb.Append(@"|");

            sb.Append(new Or().Chain.Valor);

            sb.Append(@")");

            return sb.ToString();
        }
        
        private string ExpressaoRegularOperadoresMatematicos(bool ComEspacoFinal)
        {
            StringBuilder sb = new StringBuilder();

            if (ComEspacoFinal)
            {
                sb.Append(@"((");
            }
            else
            {
                sb.Append(@"(\s(");
            }

            sb.Append(@"\");
            sb.Append(new Addition().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Subtraction().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Multiplication().Chain.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new Less().Chain.Valor);

            if (ComEspacoFinal)
            {
                sb.Append(@")\s\w+" + ExpressaoRegularPermitePontoEmVariavel() + @"\s)*");
            }
            else
            {
                sb.Append(@")\s\w+" + ExpressaoRegularPermitePontoEmVariavel() + @"\)*");
            }

            return sb.ToString();
        }

        private string ExpressaoRegularExpressoes()
        {
            StringBuilder sb = new StringBuilder();
            string OperadoresMatematicos = ExpressaoRegularOperadoresMatematicos(true);
            string OperadoresMatematicosFinais = ExpressaoRegularOperadoresMatematicos(false);
            string OperadoresComparacao = ExpressaoRegularOperadoresComparacao();

            sb.Append(@"^" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"\s");
            sb.Append(OperadoresMatematicos);
            sb.Append(OperadoresComparacao);
            sb.Append(@"+\s" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"");
            sb.Append(OperadoresMatematicosFinais);
            sb.Append(@"$");

            return sb.ToString();
        }

        private string ExpressaoRegularSeEntao()
        {
            StringBuilder sb = new StringBuilder();
            string OperadoresComparacao = ExpressaoRegularOperadoresComparacao();
            string OperadoresLogicos = ExpressaoRegularOperadoresLogicos();
            string OperadoresMatematicos = ExpressaoRegularOperadoresMatematicos(true);

            sb.Append(@"if\s" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"\s");
            sb.Append(OperadoresMatematicos);
            sb.Append(OperadoresComparacao);
            sb.Append(@"+\s" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"\s(");
            sb.Append(OperadoresMatematicos);
            sb.Append(OperadoresLogicos);
            sb.Append(@"+\s" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"\s");
            sb.Append(OperadoresMatematicos);
            sb.Append(OperadoresComparacao);
            sb.Append(@"+\s" + ExpressaoRegularCadeia() + ExpressaoRegularPermitePontoEmVariavel() + @"\s");
            sb.Append(OperadoresMatematicos);
            sb.Append(@")*then");

            return sb.ToString();
        }

        public bool Validar(Lexical Analise)
        {
            bool retorno = true;

            _analise = Analise;

            string lido = "";
            int linha = 0;

            bool dentrodeIF = false;
            bool dentrodeTHEN = false;
            bool dentrodeELSE = false;
            bool ocorreuMudancaEstruturaIf = false;
            bool ocorreuOperadorMatematico = false;
            bool ocorreuOperadorComparacao = false;
            bool ocorreuOperadorLogico = false;

            string ConteudoIf = "";
            string ConteudoThen_PorLinha = "";
            string ConteudoElse_PorLinha = "";

            for (int Pos = 0; Pos < Analise.CodigoFonte.Count; Pos++)
            {
                Token tk = Analise.CodigoFonte[Pos];
                if (linha != tk.Linha)
                {
                    lido = "";
                    if (tk is Operator && !(tk is If) && !(tk is Else) && !(tk is EndIf))
                    {
                        this._mensagemerro = "Erro de sintaxe: Uso incorreto do operador no início da expressão. Linha " + tk.Linha + ".";
                        retorno = false;
                        break;
                    }
                }

                linha = tk.Linha;
                if (Pos < Analise.CodigoFonte.Count - 1)
                {
                    Token ProximoToken = Analise.CodigoFonte[Pos - 1];
                    if (ProximoToken.Linha != linha)
                    {
                        if (tk is Comparison || tk is Mathematics || tk is If)
                        {
                            this._mensagemerro = "Erro de sintaxe: Uso incorreto do operador no final da expressão. Linha " + tk.Linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                }

                if (tk is Value)
                {
                    if (lido == "" || lido == "O")
                    {
                        lido = "V";
                    }
                    else
                    {
                        this._mensagemerro = "Erro de sintaxe: Símbolo " + ((Value) tk).NomeVariavel + " utilizado de forma incorreta na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                }
                else if (tk is Operator)
                {
                    if (lido == "" || lido == "V")
                    {
                        lido = "O";
                    }
                    else
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador utilizado de forma incorreta na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                }

                ocorreuMudancaEstruturaIf = false;
                if (tk is If)
                {
                    dentrodeIF = true;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is Then)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = true;
                    dentrodeELSE = false;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is Else)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = true;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is EndIf)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    ocorreuMudancaEstruturaIf = true;
                }

                Token tkAnterior = null;
                Token tkProximo = null;

                if (Pos > 0)
                {
                    tkAnterior = Analise.CodigoFonte[Pos - 1];
                }

                if (Pos < Analise.CodigoFonte.Count - 1)
                {
                    tkProximo = Analise.CodigoFonte[Pos + 1];
                }

                if (tk is Logic  && !dentrodeIF)
                {
                    this._mensagemerro = "Erro de sintaxe: Operador Lógico " + ((Logic) tk).Chain.Valor + " fora de operador condicional na linha " + linha + ".";
                    retorno = false;
                    break;
                }

                if (dentrodeIF)
                {
                    if (tk is If)
                    {
                        ConteudoIf += ((If) tk).Chain.Valor;
                    }
                    else if (tk is Comparison)
                    {
                        ConteudoIf += ((Comparison) tk).Chain.Valor;
                    }
                    else if (tk is Mathematics)
                    {
                        ConteudoIf += ((Mathematics) tk).Chain.Valor;
                    }
                    else if (tk is Logic)
                    {
                        ConteudoIf += ((Logic) tk).Chain.Valor;
                    }
                    else if (tk is Value)
                    {
                        if (((Value) tk).NomeVariavel != null)
                        {
                            ConteudoIf += ((Value) tk).NomeVariavel;
                        }
                        else
                        {
                            ConteudoIf += ((Value) tk).ValorVariavel;
                        }
                    }
                    else
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new If().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }

                    ConteudoIf += " ";
                }
                else if (dentrodeTHEN)
                {
                    if (ConteudoIf != "")
                    {
                        ConteudoIf += new Else().Chain.Valor;
                        string ER = ExpressaoRegularSeEntao();
                        Match match = Regex.Match(ConteudoIf, ER);

                        if (match.Success)
                        {
                            ConteudoIf = "";
                        }
                        else
                        {
                            this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new If().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                    else
                    {
                        if (tk is Comparison)
                        {
                            ConteudoThen_PorLinha += ((Comparison) tk).Chain.Valor;
                        }
                        else if (tk is Mathematics)
                        {
                            ConteudoThen_PorLinha += ((Mathematics) tk).Chain.Valor;
                        }
                        else if (tk is Logic)
                        {
                            ConteudoThen_PorLinha += ((Logic) tk).Chain.Valor;
                        }
                        else if (tk is Value)
                        {
                            if (((Value) tk).NomeVariavel != null)
                            {
                                ConteudoThen_PorLinha += ((Value) tk).NomeVariavel;
                            }
                            else
                            {
                                ConteudoThen_PorLinha += ((Value) tk).ValorVariavel;
                            }
                        }
                        else
                        {
                            this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new Then().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }

                    if (tkAnterior != null)
                    {
                        if ((!(tk is Then) && tk.Linha != tkProximo.Linha) || tkProximo is Else || tkProximo is EndIf)
                        {
                            string ER = ExpressaoRegularExpressoes();
                            Match match = Regex.Match(ConteudoThen_PorLinha, ER);

                            if (match.Success)
                            {
                                ConteudoThen_PorLinha = "";
                            }
                            else
                            {
                                this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new Then().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                                retorno = false;
                                break;
                            }
                        }
                    }

                    if (ConteudoThen_PorLinha != "")
                    {
                        ConteudoThen_PorLinha += " ";
                    }
                }
                else if (dentrodeELSE)
                {
                    if (tk is Comparison)
                    {
                        ConteudoElse_PorLinha += ((Comparison) tk).Chain.Valor;
                    }
                    else if (tk is Mathematics)
                    {
                        ConteudoElse_PorLinha += ((Mathematics) tk).Chain.Valor;
                    }
                    else if (tk is Logic)
                    {
                        ConteudoElse_PorLinha += ((Logic) tk).Chain.Valor;
                    }
                    else if (tk is Value)
                    {
                        if (((Value) tk).NomeVariavel != null)
                        {
                            ConteudoElse_PorLinha += ((Value) tk).NomeVariavel;
                        }
                        else
                        {
                            ConteudoElse_PorLinha += ((Value) tk).ValorVariavel;
                        }
                    }
                    else if (!(tk is Else))
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new Else().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                    
                    if (tkAnterior != null)
                    {
                        if ((!(tk is Else) && tk.Linha != tkProximo.Linha) || tkProximo is EndIf)
                        {
                            if (ConteudoElse_PorLinha.Trim() != "")
                            {
                                string ER = ExpressaoRegularExpressoes();
                                Match match = Regex.Match(ConteudoElse_PorLinha, ER);

                                if (match.Success)
                                {
                                    ConteudoElse_PorLinha = "";
                                }
                                else
                                {
                                    this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new Else().Chain.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                                    retorno = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (ConteudoElse_PorLinha != "")
                    {
                        ConteudoElse_PorLinha += " ";
                    }
                }
                else
                {
                    if (ConteudoIf != "")
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new If().Chain.Valor + " sem fechamento do operador "
                                             + new Then().Chain.Valor + " identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                }
            }

            return retorno;
        }
    }
}