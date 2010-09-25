using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taramon.Exceller;
using System.Collections;
using ZBPM.fj;

namespace ZBPM
{
    /// <summary>
    /// 钢混
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Jglxgh : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("E96", "E99")]
        public ArrayList _Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public override System.Collections.ArrayList Result()
        {
            return _result;
        }

        [UseSheet("Sheet1")]
        [FromRange("I96", "I99")]
        public ArrayList _Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public override System.Collections.ArrayList Name()
        {
            return _name;
        }
    }

    /// <summary>
    /// 混合
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Jglxhh : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("F96", "F99")]
        public ArrayList _Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public override System.Collections.ArrayList Result()
        {
            return _result;
        }

        [UseSheet("Sheet1")]
        [FromRange("I96", "I99")]
        public ArrayList _Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public override System.Collections.ArrayList Name()
        {
            return _name;
        }
    }

    /// <summary>
    /// 砖木
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Jglxzm : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("G96", "G99")]
        public ArrayList _Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public override System.Collections.ArrayList Result()
        {
            return _result;
        }

        [UseSheet("Sheet1")]
        [FromRange("I96", "I99")]
        public ArrayList _Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public override System.Collections.ArrayList Name()
        {
            return _name;
        }
    }

    /// <summary>
    /// 其它
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Jglxqt : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("H96", "H99")]
        public ArrayList _Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public override System.Collections.ArrayList Result()
        {
            return _result;
        }

        [UseSheet("Sheet1")]
        [FromRange("I96", "I99")]
        public ArrayList _Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public override System.Collections.ArrayList Name()
        {
            return _name;
        }
    }
}
