using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotNetCore5WebAPI_0323.Configuration
{    public class Utils
    {
        public static dynamic Configuration
        {
            get;
            internal set;
        }
        /// <summary>
        /// Get需求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static WebResponse GetResponse(string url)
        {
            var req = WebRequest.Create(url);
            req.Timeout = -1;
            return req.GetResponse();
        }
        /// <summary>
        /// 根據檔名取得MIME
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string MIME(string file)
            => MimeMapping.MimeUtility.GetMimeMapping(file);
        /// <summary>
        /// 讀取檔案內容
        /// </summary>
        /// <param name="path"></param>
        /// <returns>回傳二進位內容</returns>
        public static byte[] ReadFile(string path)
            => ReadFile(new FileInfo(path));
        /// <summary>
        /// 讀取檔案內容
        /// </summary>
        /// <param name="file"></param>
        /// <returns>回傳二進位內容，若為null表示檔案不存在</returns>
        public static byte[] ReadFile(FileInfo file)
        {
            if (file.Exists)
            {
                using (var fs = file.OpenRead())
                {
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 讀取JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ReadJson<T>(string json)
            => JsonConvert.DeserializeObject<T>(json);
        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static dynamic ReadValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Boolean:
                    return (bool)token;
                case JTokenType.Date:
                    return (DateTime)token;
                case JTokenType.Float:
                    return (double)token;
                case JTokenType.Integer:
                    return (int)token;
                case JTokenType.String:
                    return (string)token;
            }
            return token;
        }
        /// <summary>
        /// 轉成JSON格式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(object o)
            => JsonConvert.SerializeObject(o, new data.spatial.GeometryJsonConverter());
        /// <summary>
        /// 二進位內容轉成文字
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToString(byte[] bytes)
            => Encoding.GetString(bytes);
        /// <summary>
        /// 寫入二進位檔
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        public static void WriteFile(string path, byte[] bytes)
            => WriteFile(new FileInfo(path), bytes);
        /// <summary>
        /// 寫入二進位檔
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bytes"></param>
        public static void WriteFile(FileInfo file, byte[] bytes)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();
            using (var fs = file.Create())
            {
                fs.SetLength(0);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 將內容輸出至串流
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bytes"></param>
        public static void WriteTo(Stream s, byte[] bytes)
            => s.Write(bytes, 0, bytes.Length);
        /// <summary>
        /// 輸出到client
        /// </summary>
        /// <param name="res"></param>
        /// <param name="obj"></param>
        /// <param name="ext">副檔名，.json、.xml、.txt，預設為.txt</param>
        public static void WriteToClient(HttpResponse res, object obj, string ext)
        {
            res.ContentType = MIME(ext) + "; charset=" + Encoding.WebName;
            res.Headers.Add("Content-Encoding", new Microsoft.Extensions.Primitives.StringValues("Gzip"));
            var serialize = Serializes[ext == ".xml" ? 1 : ext == ".json" ? 0 : 2];
            using (var gzip = new GZipStream(res.Body, CompressionMode.Compress))
            {
                var bytes = Encoding.GetBytes(serialize.Serialize(obj));
                gzip.Write(bytes, 0, bytes.Length);
            }
            //res.WriteAsync(serialize.Serialize(obj));
        }
        /// <summary>
        /// 輸出紀錄
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", $"{DateTime.Today:yyyy-MM-dd}.log"));
            if (!file.Directory.Exists)
                file.Directory.Create();
            using (var writer = file.AppendText())
                writer.WriteLine(log);
        }
        /// <summary>
        /// 輸出或讀取文字的編碼
        /// </summary>
        public static Encoding Encoding = new UTF8Encoding();

        private static readonly ISerialize[] Serializes = { new JsonSerialize(), new XmlSerialize(), new TxtSerialize() };

    }
}
