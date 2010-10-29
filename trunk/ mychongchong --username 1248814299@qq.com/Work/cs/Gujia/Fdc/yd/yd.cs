using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZBPM.yd
{
    public class yddj : iyd, iyddj
    {
        private double fjqpj;
        private double jglxsz;
        private double cxsz;
        private double llsz;
        private double lnqksz;
        private double rjlsz;
        private double jtsz;
        #region iyd Members

        public double Fjqpj
        {
            get
            {
                return fjqpj;
            }
            set
            {
                fjqpj = value;
            }
        }

        public double Jglxsz
        {
            get
            {
                return jglxsz;
            }
            set
            {
                jglxsz = value;
            }
        }

        public double Cxsz
        {
            get
            {
                return cxsz;
            }
            set
            {
                cxsz = value;
            }
        }

        public double Llsz
        {
            get
            {
                return llsz;
            }
            set
            {
                llsz = value;
            }
        }

        public double Lnqksz
        {
            get
            {
                return lnqksz;
            }
            set
            {
                lnqksz = value;
            }
        }

        public virtual double Calu()
        {
            return fjqpj * (1 + jglxsz / 100 + cxsz / 100 + llsz / 100 + lnqksz / 100 + jtsz / 100) * rjlsz / 100;
        }

        #endregion

        #region iyddj Members

        public double Jtsz
        {
            get
            {
                return jtsz;
            }
            set
            {
                jtsz = value;
            }
        }

        public double Rjlsz
        {
            get
            {
                return rjlsz;
            }
            set
            {
                rjlsz = value;
            }
        }

        #endregion

    }

    public class ydfdj : iyd, iydfdj
    {
        private double fjqpj;
        private double jglxsz;
        private double cxsz;
        private double llsz;
        private double lnqksz;
        private double jzmjsz;
        private double lxsz;
        private double lcsz;
        private double wyglsz;
        private double fssz;
        private double gtsz;
        private double dtsz;
        #region iyd Members

        public double Fjqpj
        {
            get
            {
                return fjqpj;
            }
            set
            {
                fjqpj = value;
            }
        }

        public double Jglxsz
        {
            get
            {
                return jglxsz;
            }
            set
            {
                jglxsz = value;
            }
        }

        public double Cxsz
        {
            get
            {
                return cxsz;
            }
            set
            {
                cxsz = value;
            }
        }

        public double Llsz
        {
            get
            {
                return llsz;
            }
            set
            {
                llsz = value;
            }
        }

        public double Lnqksz
        {
            get
            {
                return lnqksz;
            }
            set
            {
                lnqksz = value;
            }
        }

        public virtual double Calu()
        {
            return fjqpj * (1 + jglxsz / 100 + cxsz / 100 + llsz / 100 + lnqksz / 100 + jzmjsz / 100 + lxsz / 100 + lcsz / 100 + wyglsz / 100 + fssz / 100 + gtsz / 100 + dtsz / 100);
        }

        #endregion

        #region iydfdj Members

        public double Jzmjsz
        {
            get
            {
                return jzmjsz;
            }
            set
            {
                jzmjsz = value;
            }
        }

        public double Lxsz
        {
            get
            {
                return lxsz;
            }
            set
            {
                lxsz = value;
            }
        }

        public double Lcsz
        {
            get
            {
                return lcsz;
            }
            set
            {
                lcsz = value;
            }
        }

        public double Wyglsz
        {
            get
            {
                return wyglsz;
            }
            set
            {
                wyglsz = value;
            }
        }

        public double Fssz
        {
            get
            {
                return fssz;
            }
            set
            {
                fssz = value;
            }
        }

        public double Gtsz
        {
            get
            {
                return gtsz;
            }
            set
            {
                gtsz = value;
            }
        }

        public double Dtsz
        {
            get { return dtsz; }
            set { dtsz = value; }
        }
        #endregion
    }

    #region 加入逆算功能 2010年10月31日
    public class yddjls : yddj
    {
        public override double Calu()
        {
            return  base.Fjqpj / (1 + base.Jglxsz / 100 + base.Cxsz / 100 + base.Llsz / 100 + base.Lnqksz / 100 + base.Jtsz / 100) * base.Rjlsz / 100;
        }
    }
    public class ydfdjls : ydfdj
    {
        public override double Calu()
        {
            return base.Fjqpj * (1 + base.Jglxsz / 100 + base.Cxsz / 100 + base.Llsz / 100 + base.Lnqksz / 100 + base.Jzmjsz / 100 + base.Lxsz / 100 + base.Lcsz / 100 + base.Wyglsz / 100 + base.Fssz / 100 + base.Gtsz / 100 + base.Dtsz / 100);
        }
    }
    #endregion

    class ydksearch : iydsearch
    {
        private string qz;
        private string dz;

        public string Qz
        {
            set { qz = value; }
            get { return qz; }
        }

        public string Dz
        {
            set { dz = value; }
            get { return dz; }
        }

        #region iydsearch Members

        string iydsearch.Where()
        {
            StringBuilder tmp = new StringBuilder();
            ArrayList al = new ArrayList();
            if (!string.IsNullOrEmpty(qz))
            {
                al.Add(string.Format(" 区镇 like '%{0}%'", qz));
            }
            if (!string.IsNullOrEmpty(dz))
            {
                al.Add(string.Format(" 座落 like '%{0}%'", dz));
            }
            if (al.Count > 1)
            {
                for (int i = 0; i < al.Count - 1; i++)
                {
                    tmp.Append(al[i].ToString());
                    tmp.Append(" and ");
                }
                tmp.Append(al[al.Count - 1].ToString());
                return tmp.ToString();

            }
            else if (al.Count == 1)
            {
                return al[0].ToString();
            }
            else
            {
                return "1!=1";
            }
        }

        #endregion
    }
}
