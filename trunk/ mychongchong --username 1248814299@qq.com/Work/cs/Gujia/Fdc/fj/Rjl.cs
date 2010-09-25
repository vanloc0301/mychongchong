using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Taramon.Exceller;

namespace ZBPM.fj
{
    /// <summary>
    /// 钢混
    /// </summary>
    [DefaultSheet("Sheet1")]
    public  class Rjlgh : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("I26", "BK26")]
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
        [FromRange("I27", "BK27")]
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
    public class Rjlhh : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I31", "BK31")]
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
        [FromRange("I27", "BK27")]
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
    public class Rjlzm : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I36", "BK36")]
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
        [FromRange("I27", "BK27")]
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
    public class Rjlqt : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I41", "BK41")]
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
        [FromRange("I27", "BK27")]
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
