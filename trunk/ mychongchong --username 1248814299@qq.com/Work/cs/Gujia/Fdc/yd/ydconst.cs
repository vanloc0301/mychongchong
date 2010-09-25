using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZBPM.yd
{
    /// <summary>
    /// 样点常量
    /// </summary>
    public class ydconst
    {
        private string name;
        private List<string> al;
        private List<int> ls;

        public ydconst()
        {
            al = new List<string>();
            ls = new List<int>();
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public List<string> Al
        {
            get { return al; }
            set { al = value; }
        }
        public List<int> Ls
        {
            get { return ls; }
            set { ls = value; }
        }

    }

    public interface iyddata
    {

    }

    public class yddjdata : iyddata
    {
        string jglxsz;
        string cxsz;
        string lnqksz;
        double rjlsz;
        string jtsz;
        double jzmj;
        double llsz;

        public string Jglxsz
        {
            get { return jglxsz; }
            set { jglxsz = value; }
        }

        public string Cxsz
        {
            get { return cxsz; }
            set { cxsz = value; }
        }

        public string Lnqksz
        {
            get { return lnqksz; }
            set { lnqksz = value; }
        }

        public double Rjlsz
        {
            get { return rjlsz; }
            set { rjlsz = value; }
        }

        public string Jtsz
        {
            get { return jtsz; }
            set { jtsz = value; }
        }

        public double Jzmj
        {
            get { return jzmj; }
            set { jzmj = value; }
        }

        public double Llsz
        {
            get { return llsz; }
            set { llsz = value; }
        }
    }

    public class ydfdjdata : iyddata
    {
        string jglxsz;
        string cxsz;
        string lnqksz;
        double llsz;
        //建筑面积修正系数  楼型修正系数 楼层修正 总楼层 物业管理修正 复式修正 公摊修正 有无电梯
        double jzmj;
        string lxsz;
        double lcsz;
        double zlc;
        string wyglsz;
        string fssz;
        string gtsz;
        string ywdt;

        public string Jglxsz
        {
            get { return jglxsz; }
            set { jglxsz = value; }
        }

        public string Cxsz
        {
            get { return cxsz; }
            set { cxsz = value; }
        }

        public string Lnqksz
        {
            get { return lnqksz; }
            set { lnqksz = value; }
        }

        public double Llsz
        {
            get { return llsz; }
            set { llsz = value; }
        }

        public double Jzmj
        {
            get { return jzmj; }
            set { jzmj = value; }
        }

        public string Lxsz
        {
            get { return lxsz; }
            set { lxsz = value; }
        }

        public double Lcsz
        {
            get { return lcsz; }
            set { lcsz = value; }
        }

        public double Zlc
        {
            get { return zlc; }
            set { zlc = value; }
        }

        public string Wyglsz
        {
            get { return wyglsz; }
            set { wyglsz = value; }
        }

        public string Fssz
        {
            get { return fssz; }
            set { fssz = value; }
        }

        public string Gtsz
        {
            get { return gtsz; }
            set { gtsz = value; }
        }

        public string Ywdt
        {
            get { return ywdt; }
            set { ywdt = value; }
        }
    }

    public class ydcollection
    {
        private List<double> jglx;
        private List<double> cx;
        private List<double> ll;
        private List<double> lnqk;
        private List<double> jt;
        private List<double> jzmj;
        private List<double> lx;
        private List<double> gt;
        private List<double> wy;
        private List<double> fs;
        private List<double> dt;
        private List<double> fdt;
        private List<double> rjl;
        private List<double> dtsz;

        public ydcollection()
        {
            jglx = new List<double>();
            cx = new List<double>();
            ll = new List<double>();
            lnqk = new List<double>();
            jt = new List<double>();
            jzmj = new List<double>();
            lx = new List<double>();
            gt = new List<double>();
            wy = new List<double>();
            fs = new List<double>();
            dt = new List<double>();
            fdt = new List<double>();
            rjl = new List<double>();
            dtsz = new List<double>();
        }

        public List<double> Jglx
        {
            get { return jglx; }
            set { jglx = value; }
        }

        public List<double> Cx
        {
            get
            {
                return cx;
            }
            set
            {
                cx = value;
            }
        }

        public List<double> Ll
        {
            get
            {
                return ll;
            }
            set
            {
                ll = value;
            }
        }

        public List<double> Lnqk
        {
            get
            {
                return lnqk;
            }
            set
            {
                lnqk = value;
            }
        }

        public List<double> Jt
        {
            get
            {
                return jt;
            }
            set
            {
                jt = value;
            }
        }

        public List<double> Jzmj
        {
            get
            {
                return jzmj;
            }
            set
            {
                jzmj = value;
            }
        }

        public List<double> Lx
        {
            get { return lx; }
            set { lx = value; }
        }

        public List<double> Gt
        {
            get { return gt; }
            set { gt = value; }
        }

        public List<double> Wy
        {
            get { return wy; }
            set { wy = value; }
        }

        public List<double> Fs
        {
            get
            {
                return fs;
            }
            set
            {
                fs = value;
            }
        }

        public List<double> Dt
        {
            get { return dt; }
            set { dt = value; }
        }

        public List<double> Fdt
        {
            get
            {
                return fdt;
            }
            set
            {
                fdt = value;
            }
        }

        public List<double> Rjl
        {
            get { return rjl; }
            set { rjl = value; }
        }

        public List<double> Dtsz
        {
            get { return dtsz; }
            set { dtsz = value; }
        }

    }

}
