using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTSHelper.EnumType;

namespace TTSHelper.Model
{
    /// <summary>
    /// 讯飞语音转文本实体信息
    /// </summary>
    public class XFPostModel
    {
        /// <summary>
        /// 需要转语音的文本
        /// </summary>
        public string TextContext { get; set; }
        /// <summary>
        /// 保存路径(绝对路径+文件名)
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 讯飞APPID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 讯飞APIKey
        /// </summary>
        public string APIKey { get; set; }
        /// <summary>
        /// 语音类型
        /// </summary>
        public AueType AueType { get; set; }
    }
}
