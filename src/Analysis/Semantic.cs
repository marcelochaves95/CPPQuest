using System.Collections.Generic;
using System.Data;
using System.Text;
using Sesamo.Intermediaries;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Semantic
    {
        private string _mensagemerro;
        public string MensagemErro
        {
            get => _mensagemerro;
            set => _mensagemerro = value;
        }

        private Parser _analise;
        public Parser AnaliseSintatica
        {
            get => _analise;
        }
        
        Intermediate _intermediate = new Intermediate();
        public Intermediate Codigo
        {
            get => _intermediate;
        }

        public DataTable getCodigoIntermediario()
        {
            DataTable retorno = new DataTable();
            retorno.Columns.Add("Codicao");
            retorno.Columns.Add("Expressao");
            retorno.Columns.Add("ExpCondicaoNaoAtendida");

            foreach (IntermediateExpression expressao in _intermediate.Codigo)
            {
                DataRow DR = retorno.NewRow();
                StringBuilder exp = new StringBuilder();

                foreach (Token tk in expressao.Condicao)
                {
                    exp.Append(tk.Texto);
                    exp.Append(" ");
                }
                DR["Condicao"] = exp.ToString();

                exp = new StringBuilder();

                foreach (Token tk in expressao.Expressao)
                {
                    exp.Append(tk.Texto);
                    exp.Append(" ");
                }
                DR["Expressao"] = exp.ToString();

                exp = new StringBuilder();

                foreach (Token tk in expressao.ExpressaoCondicaoNaoAtendida)
                {
                    exp.Append(tk.Texto);
                    exp.Append(" ");
                }
                DR["ExpCondicaoNaoAtendida"] = exp.ToString();
                retorno.Rows.Add(DR);
            }

            return retorno;
        }

        public bool Validar(Parser Analise)
        {
            bool retorno = true;

            _analise = Analise;

            bool dentrodeIF = false;
            bool dentrodeTHEN = false;
            bool dentrodeELSE = false;
            IntermediateExpression expressao = new IntermediateExpression();

            for (int pos = 0; pos < Analise.AnalisadorLexica.SourceCode.Count; pos++)
            {
                Token tk = Analise.AnalisadorLexica.SourceCode[pos];
                Token tkAnterior = null;
                Token tkProximo = null;

                int linha = 0;
                linha = tk.Linha;

                if (pos > 0)
                {
                    tkAnterior = Analise.AnalisadorLexica.SourceCode[pos - 1];
                }

                if (pos < Analise.AnalisadorLexica.SourceCode.Count - 1)
                {
                    tkProximo = Analise.AnalisadorLexica.SourceCode[pos + 1];
                }

                // TODO: Conferir esse primeiro if
                // Comparação original tk is OMatematico && tk is OComparacao
                if (tk is Mathematics && tk is Comparison)
                {
                    if (tk is Bigger || tk is Less || tk is BiggerOrEqual || tk is LessOrEqual)
                    {
                        if (((Value) tkAnterior).Tipo != Types.Dec && ((Value) tkAnterior).Tipo != Types.Hex && ((Value) tkAnterior).Tipo != Types.Bin)
                        {
                            string NomeValor = ((Value) tkAnterior).NomeVariavel == "" ? ((Value) tkAnterior).ValorVariavel.ToString() : ((Value) tkAnterior).NomeVariavel;
                            this._mensagemerro = "Os tipo do valor " + NomeValor + " não é de possível comparação na linha: " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                    else
                    {
                        this._mensagemerro = "Não é possível efetuar operação aritmética com valores do tipo " + ((Value) tk).Tipo + ". Erro na linha: " + linha + ".";
                        retorno = false;
                        break;
                    }
                }

                if (tk is Comparison)
                {
                    if (((Value) tkAnterior).Tipo != Types.Dec || ((Value) tkAnterior).Tipo != Types.Hex || ((Value) tkAnterior).Tipo != Types.Bin)
                    {
                        if (((Value) tkAnterior).Tipo != ((Value) tkProximo).Tipo)
                        {
                            this._mensagemerro = "Os tipos de valores devem ser iguais na comparação da linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                        else
                        {
                            if (!(tk is Equal))
                            {
                                this._mensagemerro = "Não é possível efetuar comparação numérica com valores do tipo " + ((Value) tkAnterior).Tipo + " e " + ((Value) tkProximo).Tipo
                                                     + ". Erro na linha: " + linha + ".";
                                retorno = false;
                                break;
                            }
                        }
                    }
                }

                if (tkAnterior != null)
                {
                    if (tkAnterior.Linha != tk.Linha)
                    {
                        if (expressao.Expressao.Count > 0 || expressao.ExpressaoCondicaoNaoAtendida.Count > 0)
                        {
                            _intermediate.AdicionarExpressao(expressao);

                            IntermediateExpression expressaoTemp = new IntermediateExpression();
                            if ((dentrodeTHEN || dentrodeELSE) && !(tk is EndIf))
                            {
                                expressaoTemp.Condicao = expressao.getCopiaCondicao();
                            }

                            expressao = expressaoTemp;
                        }

                        if (tk is EndIf && expressao.Condicao.Count > 0)
                        {
                            expressao.Condicao = new List<Token>();
                        }
                    }
                }

                if (tk is If)
                {
                    dentrodeIF = true;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    continue;
                }

                if (tk is Then)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = true;
                    dentrodeELSE = false;

                    continue;
                }

                if (tk is Else)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = true;

                    continue;
                }

                if (tk is EndIf)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    continue;
                }

                if (dentrodeIF)
                {
                    expressao.AdicionarTokenEmCondicao(tk);
                }
                else if (dentrodeTHEN)
                {
                    expressao.AdicionarTokenEmExpressao(tk);
                }
                else if (dentrodeELSE)
                {
                    expressao.AdicionarTokenEmExpressaoCondicaoNaoAtendida(tk);
                }
                else
                {
                    expressao.AdicionarTokenEmExpressao(tk);
                }
            }

            if (expressao.Expressao.Count > 0 || expressao.ExpressaoCondicaoNaoAtendida.Count > 0)
            {
                _intermediate.AdicionarExpressao(expressao);
            }

            return retorno;
        }
    }
}