using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Model.DataAccessLayer
{
    /// <summary>
    /// 取得成員
    /// </summary>
    public class UserData
    {
        public string UID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserOrgID { get; set; }
        public string UserAddrZipCode { get; set; }
        public string UserAddr { get; set; }
        public string CityID { get; set; }
        public string TownID { get; set; }
        public string User_status { get; set; }

    }

}
