using System;
using System.IO;
using System.Security.Cryptography;

namespace ShadowViewer.Core.Helpers
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class EncryptingHelper
    {
        /// <summary>
        /// 同时加密MD5与Sha1
        /// </summary> 
        /// <returns>MD5,SHA1</returns>
        public static (string, string) CreateMd5AndSha1(string path)
        {
            using FileStream stream = File.OpenRead(path);
            var sha1Hash = CreateSha1(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var md5Hash = CreateMd5(stream);
            return (md5Hash, sha1Hash);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateSha1(string path)
        {
            using FileStream stream = File.OpenRead(path);
            return CreateSha1(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateMd5(string path)
        {
            using FileStream stream = File.OpenRead(path);
            return CreateMd5(stream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CreateSha1(Stream stream)
        {
            using SHA1 sha1 = SHA1.Create();
            return Bytes2String(sha1.ComputeHash(stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CreateMd5(Stream stream)
        {
            using MD5 md5 = MD5.Create();
            return Bytes2String(md5.ComputeHash(stream));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string CreateMd5(byte[] bytes)
        {
            using MD5 md5 = MD5.Create();
            return Bytes2String(md5.ComputeHash(bytes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string CreateSha1(byte[] bytes)
        {
            using var sha1 = SHA1.Create();
            return Bytes2String(sha1.ComputeHash(bytes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Bytes2String(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
