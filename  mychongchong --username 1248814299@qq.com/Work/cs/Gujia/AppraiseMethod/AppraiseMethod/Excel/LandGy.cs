using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Taramon.Exceller;
using System.Collections;

namespace AppraiseMethod.Excel
{
    [DefaultSheet("Sheet1")]
    class Student
    {
        private string _Name;
        [ToCell("A5")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Family;
        [ToCell("B2")]
        public string Family
        {
            get { return _Family; }
            set { _Family = value; }
        }

        private ArrayList _Numbers;
        [ToRange("B3", "E3")]
        public ArrayList Numbers
        {
            get { return _Numbers; }
            set { _Numbers = value; }
        }
    }


    [DefaultSheet("Sheet1")]
    class Map
    {
        private ArrayList _Range;

        [UseSheet("Sheet1")]
        [ToRange("A1", "A4")]
        public ArrayList Range
        {
            get { return _Range; }
            set { _Range = value; }
        }
    }

    [DefaultSheet("工业待估用地修正因素条件说明及修正系数表")]
    public class LandGy:ILand
    {
        private string _qph;
        [UseSheet("基准地价修正法参数设置")]
        [ToCell("A10")]
        public String Qph
        {
            get { return _qph; }
            set { _qph = value; }
        }

        private ArrayList _range_xz; //修正
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("A2", "A12")]
        public ArrayList Range_Xz
        {
            get { return _range_xz; }
            set { _range_xz = value; }
        }

        private ArrayList _range_xzsz; //修正选择
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H1", "L1")]
        public ArrayList Range_Xzsz
        {
            get { return _range_xzsz; }
            set { _range_xzsz = value; }
        }

        #region 具体条件和修正系数 根据【修正选择】的不同而不同,工业需要修正11项;

        #region 修正选择1
        private ArrayList _range_jttj_1; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H2", "L2")]
        public ArrayList Range_Jttj_1
        {
            get { return _range_jttj_1; }
            set { _range_jttj_1 = value; }
        }

        public ArrayList _range_xzxs_1; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P2", "T2")]
        public ArrayList Range_Xzxs_1
        {
            get { return _range_xzxs_1; }
            set { _range_xzxs_1 = value; }
        }
        #endregion
        #region 修正选择2
        private ArrayList _range_jttj_2; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H3", "L3")]
        public ArrayList Range_Jttj_2
        {
            get { return _range_jttj_2; }
            set { _range_jttj_2 = value; }
        }

        public ArrayList _range_xzxs_2; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P3", "T3")]
        public ArrayList Range_Xzxs_2
        {
            get { return _range_xzxs_2; }
            set { _range_xzxs_2 = value; }
        }
        #endregion
        #region 修正选择3
        private ArrayList _range_jttj_3; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H4", "L4")]
        public ArrayList Range_Jttj_3
        {
            get { return _range_jttj_3; }
            set { _range_jttj_3 = value; }
        }

        public ArrayList _range_xzxs_3; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P4", "T4")]
        public ArrayList Range_Xzxs_3
        {
            get { return _range_xzxs_3; }
            set { _range_xzxs_3 = value; }
        }
        #endregion
        #region 修正选择4
        private ArrayList _range_jttj_4; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H5", "L5")]
        public ArrayList Range_Jttj_4
        {
            get { return _range_jttj_4; }
            set { _range_jttj_4 = value; }
        }

        public ArrayList _range_xzxs_4; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P5", "T5")]
        public ArrayList Range_Xzxs_4
        {
            get { return _range_xzxs_4; }
            set { _range_xzxs_4 = value; }
        }
        #endregion
        #region 修正选择5
        private ArrayList _range_jttj_5; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H6", "L6")]
        public ArrayList Range_Jttj_5
        {
            get { return _range_jttj_5; }
            set { _range_jttj_5 = value; }
        }

        public ArrayList _range_xzxs_5; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P6", "T6")]
        public ArrayList Range_Xzxs_5
        {
            get { return _range_xzxs_5; }
            set { _range_xzxs_5 = value; }
        }
        #endregion
        #region 修正选择6
        private ArrayList _range_jttj_6; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H7", "L7")]
        public ArrayList Range_Jttj_6
        {
            get { return _range_jttj_6; }
            set { _range_jttj_6 = value; }
        }

        public ArrayList _range_xzxs_6; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P7", "T7")]
        public ArrayList Range_Xzxs_6
        {
            get { return _range_xzxs_6; }
            set { _range_xzxs_6 = value; }
        }
        #endregion
        #region 修正选择7
        private ArrayList _range_jttj_7; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H8", "L8")]
        public ArrayList Range_Jttj_7
        {
            get { return _range_jttj_7; }
            set { _range_jttj_7 = value; }
        }

        public ArrayList _range_xzxs_7; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P8", "T8")]
        public ArrayList Range_Xzxs_7
        {
            get { return _range_xzxs_7; }
            set { _range_xzxs_7 = value; }
        }
        #endregion
        #region 修正选择8
        private ArrayList _range_jttj_8; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H9", "L9")]
        public ArrayList Range_Jttj_8
        {
            get { return _range_jttj_8; }
            set { _range_jttj_8 = value; }
        }

        public ArrayList _range_xzxs_8; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P9", "T9")]
        public ArrayList Range_Xzxs_8
        {
            get { return _range_xzxs_8; }
            set { _range_xzxs_8 = value; }
        }
        #endregion
        #region 修正选择9
        private ArrayList _range_jttj_9; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H10", "L10")]
        public ArrayList Range_Jttj_9
        {
            get { return _range_jttj_9; }
            set { _range_jttj_9 = value; }
        }

        public ArrayList _range_xzxs_9; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P10", "T10")]
        public ArrayList Range_Xzxs_9
        {
            get { return _range_xzxs_9; }
            set { _range_xzxs_9 = value; }
        }
        #endregion
        #region 修正选择10
        private ArrayList _range_jttj_10; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H11", "L11")]
        public ArrayList Range_Jttj_10
        {
            get { return _range_jttj_10; }
            set { _range_jttj_10 = value; }
        }

        public ArrayList _range_xzxs_10; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P11", "T11")]
        public ArrayList Range_Xzxs_10
        {
            get { return _range_xzxs_10; }
            set { _range_xzxs_10 = value; }
        }
        #endregion
        #region 修正选择11
        private ArrayList _range_jttj_11; //具体条件
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("H12", "L12")]
        public ArrayList Range_Jttj_11
        {
            get { return _range_jttj_11; }
            set { _range_jttj_11 = value; }
        }

        public ArrayList _range_xzxs_11; //修正系数
        [UseSheet("工业待估用地修正因素条件说明及修正系数表")]
        [ToRange("P12", "T12")]
        public ArrayList Range_Xzxs_11
        {
            get { return _range_xzxs_11; }
            set { _range_xzxs_11 = value; }
        }
        #endregion
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
