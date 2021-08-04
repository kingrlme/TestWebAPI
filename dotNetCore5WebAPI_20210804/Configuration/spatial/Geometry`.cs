using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;
using Geometry = OSGeo.OGR.Geometry;
using OSGeo.OSR;
using Newtonsoft.Json;

namespace dotNetCore5WebAPI_0323.data.spatial
{
    
    public class Geometry_
    {
        
        static Geometry_()
        {
            //Gdal.SetConfigOption("GDAL_DATA", @"C:\OSGWO4\share\gdal");
            Gdal.AllRegister();
            Ogr.RegisterAll();            
        }

        #region 屬性
        /// <summary>
        /// 設定或取得坐標系統(預設：4326)
        /// </summary>
        public int Epsg
        {
            get => _epsg;
            set => _epsg = value;
        }
        /// <summary>
        /// 取得執行個體
        /// </summary>
        [JsonIgnore]
        protected internal Geometry Instance => GetInstance();        
        /// <summary>
        /// 設定或取得wkt文字
        /// </summary>
        public string Wkt
        {
            get => _wkt;
            set
            {
                _wkt = EnsurePrecision(value);
                _instance?.Dispose();
                _instance = null;
            }
        }

        public string Json
        {
            set
            {
                _instance = Ogr.CreateGeometryFromJson(value);
                EnsurePrecision(_instance);
                _instance.ExportToWkt(out string wkt);
                Wkt = wkt;
            }
            get => ToJSON();
        }

        #endregion

        #region 公開方法
        /// <summary>
        /// 取得緩衝區
        /// </summary>
        /// <param name="radius">半徑(公尺)</param>
        public Geometry_ Buffer(double radius)
        {
            var g = Instance;
            g.TransformTo(GetEPSG(3826));
            g = g.Buffer(radius, 23);
            g.TransformTo(GetEPSG(_epsg));
            g.ExportToWkt(out string wkt);
            g.Dispose();
            g = null;
            return new Geometry_() { Epsg = this.Epsg, Wkt = wkt };
        }
        /// <summary>
        /// 是否交集
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(Geometry_ other)
            => Instance.Intersects(other.Instance);
        /// <summary>
        /// 回傳GML格式
        /// </summary>
        /// <returns></returns>
        public string ToGML()
            => Instance.ExportToGML();
        /// <summary>
        /// 回傳JSON格式
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
            => Instance.ExportToJson(new string[0]);
        /// <summary>
        /// 回傳KML格式
        /// </summary>
        /// <returns></returns>
        public string ToKML()
            => Instance.ExportToKML("");
        /// <summary>
        /// 回傳WKT
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => _wkt;
        /// <summary>
        /// 轉換坐標系統
        /// </summary>
        /// <param name="epsg"></param>
        public void TransformTo(int epsg)
        {
            Instance.TransformTo(GetEPSG(epsg));
            Instance.ExportToWkt(out string wkt);
            Epsg = epsg;
            Wkt = wkt;
        }
        #endregion

        #region 私有方法        
        /// <summary>
        /// 確保小數位
        /// </summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        private static string EnsurePrecision(string wkt)
        {
            var sb = new System.Text.StringBuilder();
            int sindex = 0, eindex = wkt.IndexOf('.', sindex);
            while (eindex > 0)
            {
                sb.Append(wkt.Substring(sindex, eindex - sindex + 1));
                //eindex++;
                for (int i = 0; i < Precision; i++)
                {    //抓小數點後的數字
                    eindex++;
                    if (Array.IndexOf(_StopChars, wkt[eindex]) >= 0)  //遇到停止字元，就跳出
                        break;
                    sb.Append(wkt[eindex]);
                }
                for (int i = 0; true; i++)
                {
                    if (Array.IndexOf(_StopChars, wkt[eindex]) >= 0)
                        break;
                    eindex++;
                }
                sindex = eindex;
                eindex = wkt.IndexOf('.', sindex);
            }
            sb.Append(wkt.Substring(sindex));
            return sb.ToString();
        }
        /// <summary>
        /// 確保小數位
        /// </summary>
        /// <param name="g"></param>
        private static void EnsurePrecision(Geometry g)
        {
            for (int i = 0, l = g.GetGeometryCount(); i < l; i++)
                EnsurePrecision(g.GetGeometryRef(i));
            for(int i = 0, l = g.GetPointCount(); i < l; i++)
            {
                g.SetPoint(i, Math.Round(g.GetX(i), Precision), Math.Round(g.GetY(i), Precision), Math.Round(g.GetZ(i), Precision));
            }
        }
        /// <summary>
        /// 取得執行個體
        /// </summary>
        /// <returns></returns>
        private Geometry GetInstance()
        {
            if (null == _instance)
            {
                _instance = Geometry.CreateFromWkt(_wkt);
                _instance.AssignSpatialReference(GetEPSG(_epsg));
            }
            return _instance;
        }
        /// <summary>
        /// 取得坐標系統參數
        /// </summary>
        /// <param name="epsg"></param>
        /// <returns></returns>
        private static SpatialReference GetEPSG(int epsg)
        {
            if (!_epsgs.TryGetValue(epsg, out SpatialReference value))
            {
                value = new SpatialReference("");
               if (_projs.TryGetValue(epsg, out string p))
                    value.ImportFromProj4(p);
                _epsgs.Add(epsg, value);
            }
            return value;
        }
        #endregion

        #region  變數宣告
        private int _epsg = 4326; 
        private string _wkt;        
        private Geometry _instance;
        public static int Precision = 6;
        private static readonly Dictionary<int, SpatialReference> _epsgs = new Dictionary<int, SpatialReference>();
        private static readonly char[] _StopChars = { ',', ')', '(', ' ' };

        private static readonly Dictionary<int, string> _projs = new Dictionary<int, string>()
        {
            {3825, "+proj=tmerc +lat_0=0 +lon_0=121 +k=0.9999 +x_0=250000 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs" },
            {3826, "+proj=tmerc +lat_0=0 +lon_0=119 +k=0.9999 +x_0=250000 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs" },
            {4326, "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs" }
        };
        #endregion
    }
}
