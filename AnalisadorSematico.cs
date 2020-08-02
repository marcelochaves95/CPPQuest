using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Linguagem
{
    public class AnalisadorSematico
    {
        private string _mensagemerro;
        public string MensagemErro
        {
            get => _mensagemerro;
            set => _mensagemerro = value;
        }

        private AnalisadorSintatico _analise;
        public AnalisadorSintatico AnaliseSintatica
        {
            get => _analise;
        }
        
        CodigoIntermediario _codigoIntermediario = new CodigoIntermediario();
        public CodigoIntermediario Codigo
        {
            get => _codigoIntermediario;
        }

        public DataTable getCodigoIntermediario()
        {
            DataTable retorno = new DataTable();
            retorno.Columns.Add("Codicao");
            retorno.Columns.Add("Expressao");
            retorno.Columns.Add("ExpCondicaoNaoAtendida");

            foreach (ExpressaoCodigoIntermediario expressao in _codigoIntermediario.Codigo)
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

        public bool Validar(AnalisadorSintatico Analise)
        {
            bool retorno = true;

            _analise = Analise;

            bool dentrodeIF = false;
            bool dentrodeTHEN = false;
            bool dentrodeELSE = false;
            ExpressaoCodigoIntermediario expressao = new ExpressaoCodigoIntermediario();

            for (int pos = 0; pos < Analise.AnalisadorLexica.CodigoFonte.Count; pos++)
            {
                Token tk = Analise.AnalisadorLexica.CodigoFonte[pos];
                Token tkAnterior = null;
                Token tkProximo = null;

                int linha = 0;
                linha = tk.Linha;

                if (pos > 0)
                {
                    tkAnterior = Analise.AnalisadorLexica.CodigoFonte[pos - 1];
                }

                if (pos < Analise.AnalisadorLexica.CodigoFonte.Count - 1)
                {
                    tkProximo = Analise.AnalisadorLexica.CodigoFonte[pos + 1];
                }

                // TODO: Conferir esse primeiro if
                // Comparação original tk is OMatematico && tk is OComparacao
                if (tkAnterior is OMatematico && tkProximo is OComparacao)
                {
                    if (tk is OMaior || tk is OMenor || tk is OMaiorIgual || tk is OMenorIgual)
                    {
                        if (((Valor) tkAnterior).Tipo != Tipos.Dec && ((Valor) tkAnterior).Tipo != Tipos.Hex && ((Valor) tkAnterior).Tipo != Tipos.Bin)
                        {
                            string NomeValor = ((Valor) tkAnterior).NomeVariavel == "" ? ((Valor) tkAnterior).ValorVariavel.ToString() : ((Valor) tkAnterior).NomeVariavel;
                            this._mensagemerro = "Os tipo do valor " + NomeValor + " não é de possível comparação na linha: " + linha + ".";
                            retorno = false;
                            break;
                        }
                    }
                    else
                    {
                        this._mensagemerro = "Não é possível efetuar operação aritmética com valores do tipo " + ((Valor) tk).Tipo + ". Erro na linha: " + linha + ".";
                        retorno = false;
                        break;
                    }
                }

                if (tk is OComparacao)
                {
                    if (((Valor) tkAnterior).Tipo != Tipos.Dec || ((Valor) tkAnterior).Tipo != Tipos.Hex || ((Valor) tkAnterior).Tipo != Tipos.Bin)
                    {
                        if (((Valor) tkAnterior).Tipo != ((Valor) tkProximo).Tipo)
                        {
                            this._mensagemerro = "Os tipos de valores devem ser iguais na comparação da linha " + linha + ".";
                            retorno = false;
                            break;
                        }
                        else
                        {
                            if (!(tk is OIgual))
                            {
                                this._mensagemerro = "Não é possível efetuar comparação numérica com valores do tipo " + ((Valor) tkAnterior).Tipo + " e " + ((Valor) tkProximo).Tipo
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
                            _codigoIntermediario.AdicionarExpressao(expressao);

                            ExpressaoCodigoIntermediario expressaoTemp = new ExpressaoCodigoIntermediario();
                            if ((dentrodeTHEN || dentrodeELSE) && !(tk is OFimSe))
                            {
                                expressaoTemp.Condicao = expressao.getCopiaCondicao();
                            }

                            expressao = expressaoTemp;
                        }

                        if (tk is OFimSe && expressao.Condicao.Count > 0)
                        {
                            expressao.Condicao = new List<Token>();
                        }
                    }
                }

                if (tk is OSe)
                {
                    dentrodeIF = true;
                    dentrodeTHEN = false;
                    dentrodeELSE = false;

                    continue;
                }

                if (tk is OEntao)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = true;
                    dentrodeELSE = false;

                    continue;
                }

                if (tk is OSenao)
                {
                    dentrodeIF = false;
                    dentrodeTHEN = false;
                    dentrodeELSE = true;

                    continue;
                }

                if (tk is OFimSe)
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
                _codigoIntermediario.AdicionarExpressao(expressao);
            }

            return retorno;
        }
    }
}