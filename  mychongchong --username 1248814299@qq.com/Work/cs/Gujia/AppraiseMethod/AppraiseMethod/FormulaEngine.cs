using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AppraiseMethod
{
    class FormulaEngine
    {
        public static Hashtable ht = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strExpression"></param>
        /// <param name="nResult"></param>
        /// <param name="dValue"></param>
        /// <param name="pErr"></param>
        /// <param name="ls">返回strExpression表达式中的所有变量</param>
        /// <param name="ls1">返回:=左边的变量(其实质是最后所得的结果)</param>
        public static void GetMasterVariables(string strExpression, ref byte nResult, ref double dValue, ref string pErr, ref Dictionary<string, double> ls, ref Dictionary<string, double> ls1)
        {
            string tmpexpression = "";
            string tmpreturn = "";
            tmpreturn = strExpression.Substring(0, strExpression.IndexOf(":="));
            tmpexpression = strExpression.Substring(strExpression.IndexOf(":=") + 2);
            EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(tmpexpression);
            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);
            string result = "";
            if (eval.Evaluate(out result, out pErr) == true)
            {
                if (!ls1.Keys.Contains(tmpreturn))
                {
                    ls1.Add(tmpreturn, 0);
                }
            }
            else
            {
                for (int i = 0; i < token.Variables.Count; i++)
                {
                    if (!ls.Keys.Contains(token.Variables[i].VariableName))
                    {
                        ls.Add(token.Variables[i].VariableName, 0);
                    }
                }
                if (!ls1.Keys.Contains(tmpreturn))
                {
                    ls1.Add(tmpreturn, 0);
                }
            }
        }

        public static bool CheckCanEvaluate(string strExpression, ref byte nResult, ref double dValue, ref string pErr)
        {
            string tmpexpression = "";
            string tmpreturn = "";

            tmpreturn = strExpression.Substring(0, strExpression.IndexOf(":="));
            tmpexpression = strExpression.Substring(strExpression.IndexOf(":=") + 2);
            EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(tmpexpression);
            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);
            string result = "";
            if (eval.Evaluate(out result, out pErr) == true)
            {
                dValue = Convert.ToDouble(result.ToString());
                if (!ht.ContainsKey(tmpreturn))
                {
                    ht.Add(tmpreturn, dValue);
                }
                nResult = 1;
                return true;
            }
            else
            {
                for (int i = 0; i < token.Variables.Count; i++)
                {
                    #region 如果返回false，表示所需要的变量没有准备好
                    if (!ht.ContainsKey(token.Variables[i].VariableName))
                    {
                        return false;
                    }
                    else
                    {
                        token.Variables[i].VariableValue = ht[token.Variables[i].VariableName].ToString();
                    }
                    #endregion
                }
                EvalExpression(strExpression, ref nResult, ref dValue, ref pErr);
                if (nResult == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
          
        }

        public static void EvalExpression(string strExpression, ref byte nResult, ref double dValue, ref string pErr)
        {
            string tmpexpression = "";
            string tmpreturn = "";
            tmpreturn = strExpression.Substring(0, strExpression.IndexOf(":="));
            tmpexpression = strExpression.Substring(strExpression.IndexOf(":=") + 2);
            EvaluationEngine.Parser.Token token = new EvaluationEngine.Parser.Token(tmpexpression);
            EvaluationEngine.Evaluate.Evaluator eval = new EvaluationEngine.Evaluate.Evaluator(token);
            string result = "";
            if (eval.Evaluate(out result, out pErr) == true)
            {
                dValue = Convert.ToDouble(result.ToString());
                if (!ht.ContainsKey(tmpreturn))
                {
                    ht.Add(tmpreturn, dValue);
                }
                nResult = 1;
            }
            else
            {
                for (int i = 0; i < token.Variables.Count; i++)
                {
                    token.Variables[i].VariableValue = ht[token.Variables[i].VariableName].ToString();
                }
                if (eval.Evaluate(out result, out pErr) == true)
                {
                    dValue = Convert.ToDouble(result.ToString());
                    if (!ht.ContainsKey(tmpreturn))
                    {
                        ht.Add(tmpreturn, dValue);
                    }
                    nResult = 1;
                }
                else
                {
                    nResult = 0;
                }
            }

        }

        public static void ClearVariableTable()
        {
            ht.Clear();
        }

        public static void GetVariables(ref string pStr, sbyte bOnlyName)
        {
            StringBuilder str = new StringBuilder();
            if (bOnlyName == 1)
            {
                foreach (DictionaryEntry each in ht)
                {
                    str.Append(each.Key.ToString());
                    str.Append(",");
                }
                pStr = str.ToString();
                pStr = pStr.Substring(0, pStr.Length - 1);
            }
            else
            {
            }
        }
    }
}
