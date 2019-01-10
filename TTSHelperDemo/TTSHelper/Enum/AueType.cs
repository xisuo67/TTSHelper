using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelper.Enum
{
    /// <summary>
    /// Post接口生成语音文件类型
    /// </summary>
    public enum AueType
    {
        //音频文件保存类型为 wav
        Raw=0,
        //音频文件保存类型为 mp3
        Lame = 1,
    }
}
