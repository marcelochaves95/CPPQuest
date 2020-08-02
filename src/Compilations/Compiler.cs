using System.Collections.Generic;
using System.Data;
using System.Text;
using Sesamo.Intermediaries;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Compilations
{
    public class Compiler
    {
        private List<string> _mensagemerro = new List<string>();
        public List<string> MensagemErro
        {
            get => _mensagemerro;
            set => _mensagemerro = value;
        }

        public void Executar(Intermediate Codigo, Variable listaVariable)
        {
            foreach (IntermediateExpression expressao in Codigo.Codigo)
            {
                ExecutarExpressao(expressao);
            }
        }

        private void ExecutarExpressao(IntermediateExpression expressao)
        {
            if (expressao.Condicao.Count > 0)
            {
                if (CondicaoExpressaoValida(expressao.Condicao))
                {
                    if (expressao.Expressao.Count > 0)
                    {
                        ExecutarInstrucao(expressao.Expressao);
                    }
                }
                else
                {
                    if (expressao.ExpressaoCondicaoNaoAtendida.Count > 0)
                    {
                        ExecutarInstrucao(expressao.ExpressaoCondicaoNaoAtendida);
                    }
                }
            }
            else
            {
                ExecutarInstrucao(expressao.Expressao);
            }
        }

        private bool CondicaoExpressaoValida(List<Token> Condicao)
        {
            bool retorno = true;

            StringBuilder sb = new StringBuilder();
            foreach (Token tk in Condicao)
            {
                if (tk is Value)
                {
                    sb.Append(((Value) tk).ValorVariavel);
                }

                if (tk is Mathematics)
                {
                    sb.Append(tk.Texto);
                }

                if (tk is Logic)
                {
                    sb.Append(tk.Texto);
                }

                if (tk is Comparison)
                {
                    sb.Append(tk.Texto);
                }

                sb.Append(" ");

                retorno = ValidarBooleano(sb.ToString());
            }
            
            return retorno;
        }

        private bool ExecutarInstrucao(List<Token> Instrucao)
        {
            bool retorno = true;

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTexto = new StringBuilder();

            foreach (Token tk in Instrucao)
            {
                if (tk is Value)
                {
                    sb.Append(((Value) tk).ValorVariavel);
                    sbTexto.Append(tk.Texto);
                }

                if (tk is Mathematics)
                {
                    sb.Append(tk.Texto);
                    sbTexto.Append(tk.Texto);
                }

                if (tk is Comparison)
                {
                    sb.Append(tk.Texto);
                    sbTexto.Append(tk.Texto);
                }

                sb.Append(" ");
                sbTexto.Append(" ");
            }

            retorno = ValidarBooleano(sb.ToString());

            if (!retorno)
            {
                _mensagemerro.Add("Regra violada: " + sbTexto.ToString() + " (" + sb.ToString() + ")");
            }

            return retorno;
        }

        private bool ValidarBooleano(string instrucao)
        {
            instrucao = instrucao.Replace('"'.ToString(), "'");

            DataTable table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), instrucao);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return bool.Parse((string) row["expression"]);
        }
    }
}