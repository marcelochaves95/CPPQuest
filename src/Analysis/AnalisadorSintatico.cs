using System.Text;
using System.Text.RegularExpressions;

namespace Linguagem
{
    public class AnalisadorSintatico
    {
        private string _mensagemerro;
        public string MensagemErro
        {
            get { return _mensagemerro; }
            set { _mensagemerro = value; }
        }

        private AnalisadorLexico _analise;
        public AnalisadorLexico AnalisadorLexica
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
            sb.Append(new OIgual().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new ODiferente().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMaior().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMenor().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMaiorIgual().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMenorIgual().Cadeia.Valor);

            sb.Append(@")");

            return sb.ToString();
        }

        private string ExpressaoRegularOperadoresLogicos()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"(");

            sb.Append(new OAnd().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(new OOr().Cadeia.Valor);

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
            sb.Append(new OSoma().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OSubtracao().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMultiplicacao().Cadeia.Valor);

            sb.Append(@"|");

            sb.Append(@"\");
            sb.Append(new OMenor().Cadeia.Valor);

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

        public bool Validar(AnalisadorLexico Analise)
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
                    if (tk is Operador && !(tk is OSe) && !(tk is OSenao) && !(tk is OFimSe))
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
                        if (tk is OComparacao || tk is OMatematico || tk is OSe)
                        {
                            this._mensagemerro = "Erro de sintaxe: Uso incorreto do operador no final da expressão. Linha " + tk.Linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                }

                if (tk is Valor)
                {
                    if (lido == "" || lido == "O")
                    {
                        lido = "V";
                    }
                    else
                    {
                        this._mensagemerro = "Erro de sintaxe: Símbolo " + ((Valor) tk).NomeVariavel + " utilizado de forma incorreta na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                }
                else if (tk is Operador)
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
                if (tk is OSe)
                {
                    dentrodeIF = true;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is OEntao)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = true;
                    dentrodeELSE = false;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is OSenao)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = true;

                    ocorreuMudancaEstruturaIf = true;
                }

                if (tk is OFimSe)
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

                if (tk is OLogico  && !dentrodeIF)
                {
                    this._mensagemerro = "Erro de sintaxe: Operador Lógico " + ((OLogico) tk).Cadeia.Valor + " fora de operador condicional na linha " + linha + ".";
                    retorno = false;
                    break;
                }

                if (dentrodeIF)
                {
                    if (tk is OSe)
                    {
                        ConteudoIf += ((OSe) tk).Cadeia.Valor;
                    }
                    else if (tk is OComparacao)
                    {
                        ConteudoIf += ((OComparacao) tk).Cadeia.Valor;
                    }
                    else if (tk is OMatematico)
                    {
                        ConteudoIf += ((OMatematico) tk).Cadeia.Valor;
                    }
                    else if (tk is OLogico)
                    {
                        ConteudoIf += ((OLogico) tk).Cadeia.Valor;
                    }
                    else if (tk is Valor)
                    {
                        if (((Valor) tk).NomeVariavel != null)
                        {
                            ConteudoIf += ((Valor) tk).NomeVariavel;
                        }
                        else
                        {
                            ConteudoIf += ((Valor) tk).ValorVariavel;
                        }
                    }
                    else
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OSe().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }

                    ConteudoIf += " ";
                }
                else if (dentrodeTHEN)
                {
                    if (ConteudoIf != "")
                    {
                        ConteudoIf += new OSenao().Cadeia.Valor;
                        string ER = ExpressaoRegularSeEntao();
                        Match match = Regex.Match(ConteudoIf, ER);

                        if (match.Success)
                        {
                            ConteudoIf = "";
                        }
                        else
                        {
                            this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OSe().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                    else
                    {
                        if (tk is OComparacao)
                        {
                            ConteudoThen_PorLinha += ((OComparacao) tk).Cadeia.Valor;
                        }
                        else if (tk is OMatematico)
                        {
                            ConteudoThen_PorLinha += ((OMatematico) tk).Cadeia.Valor;
                        }
                        else if (tk is OLogico)
                        {
                            ConteudoThen_PorLinha += ((OLogico) tk).Cadeia.Valor;
                        }
                        else if (tk is Valor)
                        {
                            if (((Valor) tk).NomeVariavel != null)
                            {
                                ConteudoThen_PorLinha += ((Valor) tk).NomeVariavel;
                            }
                            else
                            {
                                ConteudoThen_PorLinha += ((Valor) tk).ValorVariavel;
                            }
                        }
                        else
                        {
                            this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OEntao().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }

                    if (tkAnterior != null)
                    {
                        if ((!(tk is OEntao) && tk.Linha != tkProximo.Linha) || tkProximo is OSenao || tkProximo is OFimSe)
                        {
                            string ER = ExpressaoRegularExpressoes();
                            Match match = Regex.Match(ConteudoThen_PorLinha, ER);

                            if (match.Success)
                            {
                                ConteudoThen_PorLinha = "";
                            }
                            else
                            {
                                this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OEntao().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
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
                    if (tk is OComparacao)
                    {
                        ConteudoElse_PorLinha += ((OComparacao) tk).Cadeia.Valor;
                    }
                    else if (tk is OMatematico)
                    {
                        ConteudoElse_PorLinha += ((OMatematico) tk).Cadeia.Valor;
                    }
                    else if (tk is OLogico)
                    {
                        ConteudoElse_PorLinha += ((OLogico) tk).Cadeia.Valor;
                    }
                    else if (tk is Valor)
                    {
                        if (((Valor) tk).NomeVariavel != null)
                        {
                            ConteudoElse_PorLinha += ((Valor) tk).NomeVariavel;
                        }
                        else
                        {
                            ConteudoElse_PorLinha += ((Valor) tk).ValorVariavel;
                        }
                    }
                    else if (!(tk is OSenao))
                    {
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OSenao().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                    
                    if (tkAnterior != null)
                    {
                        if ((!(tk is OSenao) && tk.Linha != tkProximo.Linha) || tkProximo is OFimSe)
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
                                    this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OSenao().Cadeia.Valor + " com símbolo não reconhecido, identificado na linha " + linha + ".";
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
                        this._mensagemerro = "Erro de sintaxe: Operador Condicional " + new OSe().Cadeia.Valor + " sem fechamento do operador "
                                             + new OEntao().Cadeia.Valor + " identificado na linha " + linha + ".";
                        retorno = false;
                        break;
                    }
                }
            }

            return retorno;
        }
    }
}