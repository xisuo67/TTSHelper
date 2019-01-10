using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelper.EnumType
{
    public enum AueType
    {
        [Description("音频文件保存类型应为wav")]
        Raw =0,
        [Description("音频文件保存类型应为mp3")]
        Lame =1,
    }
}
