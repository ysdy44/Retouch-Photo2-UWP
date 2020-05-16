using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo2.Elements
{
    public enum LoadingState
    {
        None,

        Loading,
        LoadFailed,

        FileCorrupt,
        FileNull,

        Saving,
        SaveSuccess,
        SaveFailed,
    }
}
