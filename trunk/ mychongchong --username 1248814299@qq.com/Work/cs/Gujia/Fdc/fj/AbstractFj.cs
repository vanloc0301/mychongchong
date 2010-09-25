using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Taramon.Exceller;

namespace ZBPM.fj
{
    [DefaultSheet("Sheet1")]
    public abstract class AbstractFj : ifjext
    {

        #region ifjext Members
        private double jzlx;
        [UseSheet("Sheet1")]
        [ToCell("D18")]
        public double Jzlx
        {
            set { jzlx = value; }
            get { return jzlx; }
        }

        #endregion

        #region ifj Members
        private double fjqpj;
        [UseSheet("Sheet1")]
        [ToCell("D15")]
        public double Fjqpj
        {
            set { fjqpj = value; }
            get { return fjqpj; }
        }

        private double tdqpj;
        [UseSheet("Sheet1")]
        [ToCell("D16")]
        public double Tdqpj
        {
            set { tdqpj = value; }
            get { return tdqpj; }
        }

        private double rjl;
        [UseSheet("Sheet1")]
        [ToCell("D17")]
        public double Rjl
        {
            set { rjl = value; }
            get { return rjl; }
        }

        #endregion
        public abstract ArrayList Result();

        public abstract ArrayList Name();
    }
}
