using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.DataAccessLayer
{
    public class Organization
    {
        public int SID { get; set; }
        public string OrgID { get; set; }
        public string OrgName { get; set; }
        public string OrgStatus { get; set; }
        public string OrgCreateYear { get; set; }
        public string OrgCreateMonth { get; set; }
        public string CreateTime { get; }
        public decimal WGS84X { get; set; }
        public decimal WGS84Y { get; set; }
        public string Geom { get; }
        

    }
}
