using dotNetCore5WebAPI_0323.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetCore5WebAPI_0323
{
    public class JsonSerialize: ISerialize
    {
        public string Serialize(object o)
            => Utils.ToJson(o);
    }
}
