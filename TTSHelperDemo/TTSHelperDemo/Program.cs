using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTSHelper;
using TTSHelper.EnumType;
using TTSHelper.Model;
using TTSHelper.Tool;

namespace TTSHelperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirPath = FileHelper.GetAbsolutePath("/UpLoad/Voice");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string relativePath = string.Format("/UpLoad/Voice/{0}.mp3", $"{Guid.NewGuid().ToString("N")}{DateTime.Now.ToString("yyyyMMddHHmmss")}");
            XFPostModel model = new XFPostModel()
            {
                AppID = "your AppID",
                APIKey = "your APIKey",
                AueType = AueType.Lame, //若保存此类型，文件后缀名请传mp3,若希望保存wav文件，AueType枚举类型为Raw，后缀名请传wav
                FilePath = FileHelper.GetAbsolutePath(relativePath),
                TextContext = "测试文本转语音"
            };
            var result = XFSpeechHelper.Post(model);
            Console.WriteLine(result.Message);
            Console.ReadKey();
        }
    }
}
