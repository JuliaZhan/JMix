using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 概述：webRequests
    /// 文件：Common.WebRequestsHelper
    /// 作者：詹詹    创建时间：2018/4/28 11:28:56
    /// 描述：
    ///    > add description for WebRequestsHelper
    ///    修改历史：
    ///
    /// <summary>
    public class WebRequestsHelper
    {
        /// <summary>
        /// 表单post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="authorization">头部认证信息</param>
        public static string sendPost(string url, Dictionary<string, string> param, string authorization)
        {
            //组合参数
            string parameters = "";
            if (param != null)
            {
                foreach (KeyValuePair<string, string> kv in param)
                {
                    parameters += parameters == "" ? "" : "&";
                    parameters += kv.Key + "=" + kv.Value;
                }
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //设置头部认证信息
            if (!string.IsNullOrEmpty(authorization))
            {
                request.Headers["Authorization"] = authorization;
            }
            request.ContentType = "application/x-www-form-urlencoded";
            //传入post参数
            request.ContentLength = parameters.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(parameters);
            writer.Flush();
            //请求接口并获得返回值
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                //返回错误信息
                response = (HttpWebResponse)ex.Response;
            }
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8";
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            return reader.ReadToEnd();
        }
        /// <summary>
        /// 表达get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="authorization">头部认证信息</param>
        public static string sendGet(string url, Dictionary<string, string> param, string authorization)
        {
            //组合参数
            string parameters = "";
            if (param != null)
            {
                foreach (KeyValuePair<string, string> kv in param)
                {
                    parameters += parameters == "" ? "" : "&";
                    parameters += kv.Key + "=" + kv.Value;
                }
            }
            //拼接get参数
            url += parameters == "" ? "" : "?" + parameters;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            //设置头部认证信息
            request.Headers["Authorization"] = authorization;
            request.ContentType = "application/x-www-form-urlencoded";
            //请求接口并获得返回值
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                //返回错误信息
                response = (HttpWebResponse)ex.Response;
            }
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8";
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            return reader.ReadToEnd();
        }
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
