using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Taramon.Exceller;
using System.Collections;

namespace AppraiseMethod.Excel
{
    [DefaultSheet("商住待估用地修正因素条件说明及修正系数表")]
    public class LandSz:ILand
    {
        private string _qph;
        [UseSheet("基准地价修正法参数设置")]
        [ToCell("A2")]
        public String Qph
        {
            get { return _qph; }
            set { _qph = value; }
        }

        private ArrayList _range_xz; //修正
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("A2", "A24")]
        public ArrayList Range_Xz
        {
            get { return _range_xz; }
            set { _range_xz = value; }
        }

        private ArrayList _range_xzsz; //修正选择
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H1", "L1")]
        public ArrayList Range_Xzsz
        {
            get { return _range_xzsz; }
            set { _range_xzsz = value; }
        }

        #region 具体条件和修正系数 根据【修正选择】的不同而不同,商住需要修正23项;

        #region 修正选择1
        private ArrayList _range_jttj_1; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H2", "L2")]
        public ArrayList Range_Jttj_1
        {
            get { return _range_jttj_1; }
            set { _range_jttj_1 = value; }
        }

        public ArrayList _range_xzxs_1; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P2", "T2")]
        public ArrayList Range_Xzxs_1
        {
            get { return _range_xzxs_1; }
            set { _range_xzxs_1 = value; }
        }
        #endregion
        #region 修正选择2
        private ArrayList _range_jttj_2; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H3", "L3")]
        public ArrayList Range_Jttj_2
        {
            get { return _range_jttj_2; }
            set { _range_jttj_2 = value; }
        }

        public ArrayList _range_xzxs_2; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P3", "T3")]
        public ArrayList Range_Xzxs_2
        {
            get { return _range_xzxs_2; }
            set { _range_xzxs_2 = value; }
        }
        #endregion
        #region 修正选择3
        private ArrayList _range_jttj_3; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H4", "L4")]
        public ArrayList Range_Jttj_3
        {
            get { return _range_jttj_3; }
            set { _range_jttj_3 = value; }
        }

        public ArrayList _range_xzxs_3; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P4", "T4")]
        public ArrayList Range_Xzxs_3
        {
            get { return _range_xzxs_3; }
            set { _range_xzxs_3 = value; }
        }
        #endregion
        #region 修正选择4
        private ArrayList _range_jttj_4; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H5", "L5")]
        public ArrayList Range_Jttj_4
        {
            get { return _range_jttj_4; }
            set { _range_jttj_4 = value; }
        }

        public ArrayList _range_xzxs_4; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P5", "T5")]
        public ArrayList Range_Xzxs_4
        {
            get { return _range_xzxs_4; }
            set { _range_xzxs_4 = value; }
        }
        #endregion
        #region 修正选择5
        private ArrayList _range_jttj_5; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H6", "L6")]
        public ArrayList Range_Jttj_5
        {
            get { return _range_jttj_5; }
            set { _range_jttj_5 = value; }
        }

        public ArrayList _range_xzxs_5; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P6", "T6")]
        public ArrayList Range_Xzxs_5
        {
            get { return _range_xzxs_5; }
            set { _range_xzxs_5 = value; }
        }
        #endregion
        #region 修正选择6
        private ArrayList _range_jttj_6; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H7", "L7")]
        public ArrayList Range_Jttj_6
        {
            get { return _range_jttj_6; }
            set { _range_jttj_6 = value; }
        }

        public ArrayList _range_xzxs_6; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P7", "T7")]
        public ArrayList Range_Xzxs_6
        {
            get { return _range_xzxs_6; }
            set { _range_xzxs_6 = value; }
        }
        #endregion
        #region 修正选择7
        private ArrayList _range_jttj_7; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H8", "L8")]
        public ArrayList Range_Jttj_7
        {
            get { return _range_jttj_7; }
            set { _range_jttj_7 = value; }
        }

        public ArrayList _range_xzxs_7; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P8", "T8")]
        public ArrayList Range_Xzxs_7
        {
            get { return _range_xzxs_7; }
            set { _range_xzxs_7 = value; }
        }
        #endregion
        #region 修正选择8
        private ArrayList _range_jttj_8; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H9", "L9")]
        public ArrayList Range_Jttj_8
        {
            get { return _range_jttj_8; }
            set { _range_jttj_8 = value; }
        }

        public ArrayList _range_xzxs_8; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P9", "T9")]
        public ArrayList Range_Xzxs_8
        {
            get { return _range_xzxs_8; }
            set { _range_xzxs_8 = value; }
        }
        #endregion
        #region 修正选择9
        private ArrayList _range_jttj_9; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H10", "L10")]
        public ArrayList Range_Jttj_9
        {
            get { return _range_jttj_9; }
            set { _range_jttj_9 = value; }
        }

        public ArrayList _range_xzxs_9; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P10", "T10")]
        public ArrayList Range_Xzxs_9
        {
            get { return _range_xzxs_9; }
            set { _range_xzxs_9 = value; }
        }
        #endregion
        #region 修正选择10
        private ArrayList _range_jttj_10; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H11", "L11")]
        public ArrayList Range_Jttj_10
        {
            get { return _range_jttj_10; }
            set { _range_jttj_10 = value; }
        }

        public ArrayList _range_xzxs_10; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P11", "T11")]
        public ArrayList Range_Xzxs_10
        {
            get { return _range_xzxs_10; }
            set { _range_xzxs_10 = value; }
        }
        #endregion
        #region 修正选择11
        private ArrayList _range_jttj_11; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H12", "L12")]
        public ArrayList Range_Jttj_11
        {
            get { return _range_jttj_11; }
            set { _range_jttj_11 = value; }
        }

        public ArrayList _range_xzxs_11; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P12", "T12")]
        public ArrayList Range_Xzxs_11
        {
            get { return _range_xzxs_11; }
            set { _range_xzxs_11 = value; }
        }
        #endregion
        #region 修正选择12
        private ArrayList _range_jttj_12; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H13", "L13")]
        public ArrayList Range_Jttj_12
        {
            get { return _range_jttj_12; }
            set { _range_jttj_12 = value; }
        }

        public ArrayList _range_xzxs_12; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P13", "T13")]
        public ArrayList Range_Xzxs_12
        {
            get { return _range_xzxs_12; }
            set { _range_xzxs_12 = value; }
        }
        #endregion
        #region 修正选择13
        private ArrayList _range_jttj_13; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H14", "L14")]
        public ArrayList Range_Jttj_13
        {
            get { return _range_jttj_13; }
            set { _range_jttj_13 = value; }
        }

        public ArrayList _range_xzxs_13; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P14", "T14")]
        public ArrayList Range_Xzxs_13
        {
            get { return _range_xzxs_13; }
            set { _range_xzxs_13 = value; }
        }
        #endregion
        #region 修正选择14
        private ArrayList _range_jttj_14; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H15", "L15")]
        public ArrayList Range_Jttj_14
        {
            get { return _range_jttj_14; }
            set { _range_jttj_14 = value; }
        }

        public ArrayList _range_xzxs_14; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P15", "T15")]
        public ArrayList Range_Xzxs_14
        {
            get { return _range_xzxs_14; }
            set { _range_xzxs_14 = value; }
        }
        #endregion
        #region 修正选择15
        private ArrayList _range_jttj_15; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H16", "L16")]
        public ArrayList Range_Jttj_15
        {
            get { return _range_jttj_15; }
            set { _range_jttj_15 = value; }
        }

        public ArrayList _range_xzxs_15; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P16", "T16")]
        public ArrayList Range_Xzxs_15
        {
            get { return _range_xzxs_15; }
            set { _range_xzxs_15 = value; }
        }
        #endregion
        #region 修正选择16
        private ArrayList _range_jttj_16; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H17", "L17")]
        public ArrayList Range_Jttj_16
        {
            get { return _range_jttj_16; }
            set { _range_jttj_16 = value; }
        }

        public ArrayList _range_xzxs_16; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P17", "T17")]
        public ArrayList Range_Xzxs_16
        {
            get { return _range_xzxs_16; }
            set { _range_xzxs_16 = value; }
        }
        #endregion
        #region 修正选择17
        private ArrayList _range_jttj_17; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H18", "L18")]
        public ArrayList Range_Jttj_17
        {
            get { return _range_jttj_17; }
            set { _range_jttj_17 = value; }
        }

        public ArrayList _range_xzxs_17; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P18", "T18")]
        public ArrayList Range_Xzxs_17
        {
            get { return _range_xzxs_17; }
            set { _range_xzxs_17 = value; }
        }
        #endregion
        #region 修正选择18
        private ArrayList _range_jttj_18; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H19", "L19")]
        public ArrayList Range_Jttj_18
        {
            get { return _range_jttj_18; }
            set { _range_jttj_18 = value; }
        }

        public ArrayList _range_xzxs_18; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P19", "T19")]
        public ArrayList Range_Xzxs_18
        {
            get { return _range_xzxs_18; }
            set { _range_xzxs_18 = value; }
        }
        #endregion
        #region 修正选择19
        private ArrayList _range_jttj_19; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H20", "L20")]
        public ArrayList Range_Jttj_19
        {
            get { return _range_jttj_19; }
            set { _range_jttj_19 = value; }
        }

        public ArrayList _range_xzxs_19; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P20", "T20")]
        public ArrayList Range_Xzxs_19
        {
            get { return _range_xzxs_19; }
            set { _range_xzxs_19 = value; }
        }
        #endregion
        #region 修正选择20
        private ArrayList _range_jttj_20; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H21", "L21")]
        public ArrayList Range_Jttj_20
        {
            get { return _range_jttj_20; }
            set { _range_jttj_20 = value; }
        }

        public ArrayList _range_xzxs_20; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P21", "T21")]
        public ArrayList Range_Xzxs_20
        {
            get { return _range_xzxs_20; }
            set { _range_xzxs_20 = value; }
        }
        #endregion
        #region 修正选择21
        private ArrayList _range_jttj_21; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H22", "L22")]
        public ArrayList Range_Jttj_21
        {
            get { return _range_jttj_21; }
            set { _range_jttj_21 = value; }
        }

        public ArrayList _range_xzxs_21; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P22", "T22")]
        public ArrayList Range_Xzxs_21
        {
            get { return _range_xzxs_21; }
            set { _range_xzxs_21 = value; }
        }
        #endregion
        #region 修正选择22
        private ArrayList _range_jttj_22; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H23", "L23")]
        public ArrayList Range_Jttj_22
        {
            get { return _range_jttj_22; }
            set { _range_jttj_22 = value; }
        }

        public ArrayList _range_xzxs_22; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P23", "T23")]
        public ArrayList Range_Xzxs_22
        {
            get { return _range_xzxs_22; }
            set { _range_xzxs_22 = value; }
        }
        #endregion
        #region 修正选择23
        private ArrayList _range_jttj_23; //具体条件
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("H24", "L24")]
        public ArrayList Range_Jttj_23
        {
            get { return _range_jttj_23; }
            set { _range_jttj_23 = value; }
        }

        public ArrayList _range_xzxs_23; //修正系数
        [UseSheet("商住待估用地修正因素条件说明及修正系数表")]
        [ToRange("P24", "T24")]
        public ArrayList Range_Xzxs_23
        {
            get { return _range_xzxs_23; }
            set { _range_xzxs_23 = value; }
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
