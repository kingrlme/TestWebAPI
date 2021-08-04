using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotNetCore5WebAPI_0323
{
    public class TxtSerialize : ISerialize
    {
        public string Serialize(object o)
        {
            if (o is IEnumerable<dynamic>)
            {
                var enu = o as IEnumerable<dynamic>;
                return string.Join("\r\n", enu.Select(e => ToString(e)));
            }
            return ToString(o);
        }
        /// <summary>
        /// 回傳以TAB分隔的字串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private string ToString(object o)
        {
            if (o.GetType().Name.IndexOf("Dictionary") >= 0)
            {
                var d = o as Dictionary<string, dynamic>;
                return string.Join("\t", d.Values);
            }
            else
            {
                var type = o.GetType();
                List<dynamic> list = new List<dynamic>();
                foreach (var m in type.GetMembers())
                {
                    switch (m.MemberType)
                    {
                        case MemberTypes.Property:
                            list.Add((m as PropertyInfo).GetValue(o));
                            break;
                        case MemberTypes.Field:
                            list.Add((m as FieldInfo).GetValue(o));
                            break;
                    }
                }
                return string.Join("\t", list);
            }
        }
    }
}
