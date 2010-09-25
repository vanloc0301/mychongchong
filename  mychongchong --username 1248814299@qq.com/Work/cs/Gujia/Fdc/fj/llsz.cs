using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taramon.Exceller;
using System.Collections;

namespace ZBPM.fj
{
    /// <summary>
    /// 自建房钢混
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszzjhgh : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I49", "AF49")]
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
        [FromRange("I50", "AF50")]
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
    /// 自建房砖混
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszzjhzh : AbstractFj
    {
        private ArrayList _result = null;
        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I55", "AD55")]
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
        [FromRange("I56", "AD56")]
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
    /// 自建房砖木
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszzjhzm : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;

        [UseSheet("Sheet1")]
        [FromRange("I61", "AB61")]
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
        [FromRange("I62", "AB62")]
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
    /// 自建房其它
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszzjhqt : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I67", "Q67")]
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
        [FromRange("I68", "Q68")]
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
    /// 商品房砖混
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszsphzh : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I73", "AF73")]
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
        [FromRange("I74", "AF74")]
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
    /// 商品房钢混1  注:钢混(7层以下)
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszsphgh1 : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I80", "AF80")]
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
        [FromRange("I81", "AF81")]
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
    /// 商品房钢混1  注:钢混(8-18层)
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszsphgh2 : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I87", "AF87")]
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
        [FromRange("I81", "AF81")]
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
    /// 商品房钢混1  注:钢混(19-25层)
    /// </summary>
    [DefaultSheet("Sheet1")]
    public class Llszsphgh3 : AbstractFj
    {
        private ArrayList _result = null;

        private ArrayList _name = null;
        [UseSheet("Sheet1")]
        [FromRange("I93", "AF93")]
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
        [FromRange("I81", "AF81")]
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
