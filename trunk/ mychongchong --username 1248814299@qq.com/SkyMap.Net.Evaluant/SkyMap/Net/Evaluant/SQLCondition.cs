namespace SkyMap.Net.Evaluant
{
    using System;

    public class SQLCondition
    {
        public string EvalExpression;
        public string Message = "不符合后台定义的某个条件";
        public string NameSpace = "Default";
        public string SQL;
        public SQLType Type;
    }
}

