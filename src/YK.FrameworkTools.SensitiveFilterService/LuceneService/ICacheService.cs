using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace YK.FrameworkTools.SensitiveFilterService
{
    internal interface ICacheService
    {
        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存是否存在</returns>
        bool IsContainsKey(string key);

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取的缓存对象类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象</returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        void Set(string key, object data);

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="timeSpan">缓存滑动过期时间</param>
        void Set(string key, object data, TimeSpan timeSpan);

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="dependency">缓存依赖项</param>
        void Set(string key, object data, CacheDependency dependency);

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="dependency">缓存依赖项</param>
        /// <param name="timeSpan">缓存滑动过期时间</param>
        void Set(string key, object data, CacheDependency dependency, TimeSpan timeSpan);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        void Remove(string key);
    }
}
