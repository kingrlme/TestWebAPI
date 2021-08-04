using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCore5WebAPI_0323
{
    interface ISerialize
    {
        string Serialize(object o);
    }
}
