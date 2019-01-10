using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelper.Tool
{
    public class FileHelper
    {
        /// <summary>
        /// MP3合并文件
        /// </summary>
        /// <param name="fullName">文件名</param>
        /// <param name="files">文件路径集合</param>
        public static void Mp3Combine(string fullName, List<string> files)
        {
            using (FileStream output = new FileStream(fullName, FileMode.Create))
            {
                foreach (string file in files)
                {
                    using (Mp3FileReader reader = new Mp3FileReader(file))
                    {
                        if ((output.Position == 0) && (reader.Id3v2Tag != null))
                        {
                            output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                        }
                        Mp3Frame frame;
                        while ((frame = reader.ReadNextFrame()) != null)
                        {
                            output.Write(frame.RawData, 0, frame.RawData.Length);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="fullName">文件名</param>
        /// <param name="files">文件集合</param>
        public static void Combine(string fullName, List<string> files)
        {
            byte[] buffer = new byte[1024 * 100];
            using (FileStream outStream = new FileStream(fullName, FileMode.Create))
            {
                int readedLen = 0;
                FileStream srcStream = null;
                for (int i = 0; i < files.Count; i++)
                {
                    srcStream = new FileStream(files[i], FileMode.Open);
                    while ((readedLen = srcStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, readedLen);
                    }
                    srcStream.Close();
                }
            }
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="index">开始索引</param>
        /// <param name="str">字符串</param>
        /// <param name="TakeNumber">跳过数量</param>
        /// <returns></returns>
        public static string GetMyStr(int index, string str, int TakeNumber)
        {
            string allstr = string.Empty;
            var strs = str.Skip(index * TakeNumber).Take(TakeNumber);
            foreach (var item in strs)
            {
                allstr += item;
            }
            return allstr;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(List<string> lstpath)
        {
            foreach (var item in lstpath)
            {
                FileAttributes attr = File.GetAttributes(item);
                if (attr == FileAttributes.Directory)
                {
                    Directory.Delete(item, true);
                }
                else
                {
                    File.Delete(item);
                }
            }
        }

        #region 把流转换成缓存流
        public static MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }
        #endregion

        #region 把缓存流转换成字节组
        public static byte[] StreamTobyte(MemoryStream memoryStream)
        {
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        #endregion
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String Md5(string s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }
    }
}
