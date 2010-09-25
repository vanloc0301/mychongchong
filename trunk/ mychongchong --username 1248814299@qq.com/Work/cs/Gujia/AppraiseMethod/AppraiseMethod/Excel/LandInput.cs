using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Taramon.Exceller;
using System.Collections;

namespace AppraiseMethod.Excel
{
    
    public class LandInput
    {
        private string _qph_to;
        private string _type;
        private double _tdjb;
        private double _jzdj;

        public String Qph
        {
            get { return _qph_to; }
            set { _qph_to = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public double Tdjb
        {
            get
            {
                return _tdjb;
            }
            set
            {
                _tdjb = value;
            }
        }

        public double Jzdj
        {
            get { return _jzdj; }
            set { _jzdj = value; }
        }

    }
}
