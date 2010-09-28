using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taramon.Exceller;
using System.Collections;

namespace ZBPM.wk
{
    [DefaultSheet("tmp")]
    public class wcexcel
    {
        #region yw_wc
        private string _scdh;
        private string _ckh;
        private string _qlq;

        [UseSheet("yw_wcexcel")]
        [FromCell("预计齐料期")]
        //预计齐料期
        public string Qlq
        {
            get { return _qlq; }
            set { _qlq = value; }
        }

        //[UseSheet("yw_wcexcel")]
        //[FromCell("厂款号")]
        ////厂款号
        //public string Ckh
        //{
        //    get { return _ckh; }
        //    set { _ckh = value; }
        //}

        [UseSheet("yw_wcexcel")]
        [FromCell("生产单号")]
        //生产单号
        public string Scdh
        {
            get { return _scdh; }
            set { _scdh = value; }
        }

        #endregion

        #region yw_bom
        private ArrayList _xh;

        [UseSheet("yw_bomexcel")]
        [FromRange("序号","")]
        public ArrayList Xh
        {
            get { return _xh; }
            set { _xh = value; }
        }
        private ArrayList _wlmc;

        [UseSheet("yw_bomexcel")]
        [FromRange("物料名称", "")]
        public ArrayList Wlmc
        {
            get { return _wlmc; }
            set { _wlmc = value; }
        }
        private ArrayList _ys;

        [UseSheet("yw_bomexcel")]
        [FromRange("颜色", "")]
        public ArrayList Ys
        {
            get { return _ys; }
            set { _ys = value; }
        }
        private ArrayList _zrl;

        [UseSheet("yw_bomexcel")]
        [FromRange("总用量", "")]
        public ArrayList Zrl
        {
            get { return _zrl; }
            set { _zrl = value; }
        }

        private ArrayList _dw;

        [UseSheet("yw_bomexcel")]
        [FromRange("单位", "")]
        public ArrayList Dw
        {
            get { return _dw; }
            set { _dw = value; }
        }

        private ArrayList _gys;

        [UseSheet("yw_bomexcel")]
        [FromRange("供应商", "")]
        public ArrayList Gys
        {
            get { return _gys; }
            set { _gys = value; }
        }

        private ArrayList _dhsl;

        [UseSheet("yw_bomexcel")]
        [FromRange("来料数量", "")]
        public ArrayList Dhsl
        {
            get { return _dhsl; }
            set { _dhsl = value; }
        }

        private ArrayList _dhrq;

        [UseSheet("yw_bomexcel")]
        [FromRange("来料日期", "")]
        public ArrayList Dhrq
        {
            get { return _dhrq; }
            set { _dhrq = value; }
        }
        private ArrayList _cgfq;

        [UseSheet("yw_bomexcel")]
        [FromRange("采购复期", "")]
        public ArrayList Cgfq
        {
            get { return _cgfq; }
            set { _cgfq = value; }
        }

        //private ArrayList _cgbz;
        //[UseSheet("yw_bomexcel")]
        //[FromRange("采购备注", "")]
        //public ArrayList Cgbz
        //{
        //    get { return _cgbz; }
        //    set { _cgbz = value; }
        //}

        #endregion
    }


}
