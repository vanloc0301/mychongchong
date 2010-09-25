using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ZBPM.yd
{
    class yddj : iyd, iyddj
    {
        private double fjqpj;
        private double jglxsz;
        private double cxsz;
        private double llsz;
        private double lnqksz;
        private double rjlsz;
        private double jtsz;
        #region iyd Members

        double iyd.Fjqpj
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

        double iyd.Jglxsz
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

        double iyd.Cxsz
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

        double iyd.Llsz
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

        double iyd.Lnqksz
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

        double iyd.Calu()
        {
            return fjqpj * (1 + jglxsz/100 + cxsz/100 + llsz/100 + lnqksz/100  + jtsz/100) * rjlsz/100;
        }

        #endregion

        #region iyddj Members

        double iyddj.Jtsz
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

        double iyddj.Rjlsz
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

    class ydfdj : iyd, iydfdj
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

        double iyd.Fjqpj
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

        double iyd.Jglxsz
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

        double iyd.Cxsz
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

        double iyd.Llsz
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

        double iyd.Lnqksz
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

        double iyd.Calu()
        {
            return fjqpj * (1 + jglxsz/100 + cxsz/100 + llsz/100 + lnqksz/100 + jzmjsz/100 + lxsz/100 + lcsz/100 + wyglsz/100 + fssz/100 + gtsz/100 + dtsz/100);
        }

        #endregion

        #region iydfdj Members

        double iydfdj.Jzmjsz
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

        double iydfdj.Lxsz
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

        double iydfdj.Lcsz
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

        double iydfdj.Wyglsz
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

        double iydfdj.Fssz
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

        double iydfdj.Gtsz
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

        double iydfdj.Dtsz 
        {
            get { return dtsz; }
            set { dtsz = value; }
        }
        #endregion
    }

    class ydksearch:iydsearch
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
