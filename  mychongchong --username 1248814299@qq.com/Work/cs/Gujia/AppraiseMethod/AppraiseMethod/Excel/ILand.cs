using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AppraiseMethod.Excel
{
    public interface ILand:IDisposable
    {
        string Qph { get; set; }

        ArrayList Range_Xz { get; set; }


    }
}
