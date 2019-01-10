using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelper
{
    /// <summary>
    /// 语音返回结果
    /// </summary>
    public class SpeechResult
    {
        /// <summary>
        /// 语音合成返回结果
        /// </summary>
        public ResultCode ResultCode { get; set; }

        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Message { get; set; }
    }
}
