using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TTSHelper.Model;
using TTSHelper.Tool;

namespace TTSHelper
{
    /// <summary>
    /// 讯飞语音识别
    /// </summary>
    public class XFSpeechHelper
    {
        public static SpeechResult Post(XFPostModel model)
        {
            SpeechResult result = new SpeechResult();
            String url = "http://api.xfyun.cn/v1/service/v1/tts";
            String bodys;
            //aue = raw, 音频文件保存类型为 wav
            //aue = lame, 音频文件保存类型为 mp3
            //获取文件拓展名
            var extension = Path.GetExtension(model.FilePath);
            string AUE = string.Empty;
            switch (model.AueType)
            {
                case EnumType.AueType.Raw:
                    if (extension!="wav")
                    {
                        result.ResultCode= ResultCode.
                        result.Message=
                    }
                    AUE = "raw";
                    break;
                case EnumType.AueType.Lame:
                    AUE = "lame";
                    break;
                default:
                    break;
            }
            string param = "{\"aue\":\"" + AUE + "\",\"auf\":\"audio/L16;rate=16000\",\"voice_name\":\"xiaoyan\",\"engine_type\":\"intp65\"}";
            //对要合成语音的文字先用utf-8然后进行URL加密
            //原文本长度
            var contextLength = model.TextContext.Length;
            byte[] textData = Encoding.UTF8.GetBytes(model.TextContext);
            var Contexts = HttpUtility.UrlEncode(textData);
            var encodeLength = Contexts.Length;
            var avg = encodeLength / (double)contextLength;//平均数
            var splitNum = 1000 / avg;//取字符串截取长度
            var val = model.TextContext.Length / splitNum;
            List<string> lst = new List<string>();
            var num = Math.Ceiling(decimal.Parse(val.ToString()));
            for (int i = 0; i < num; i++)
            {
                var file = Path.GetFileNameWithoutExtension(model.FilePath);
                var tempfileName = model.FilePath.Replace(file, $"{file}_{i.ToString()}");
                lst.Add(tempfileName);
                var body = FileHelper.GetMyStr(i, model.TextContext, (int)splitNum);//截取后的内容
                byte[] textDatas = Encoding.UTF8.GetBytes(body);//对要合成语音的文字先用utf-8然后进行URL加密
                var encodeContext = HttpUtility.UrlEncode(textDatas);
                bodys = string.Format("text={0}", encodeContext);
                //获取十位的时间戳
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string curTime = Convert.ToInt64(ts.TotalSeconds).ToString();
                //对参数先utf-8然后用base64编码
                byte[] paramData = Encoding.UTF8.GetBytes(param);
                string paraBase64 = Convert.ToBase64String(paramData);
                //形成签名
                string checkSum = FileHelper.Md5(model.APIKey + curTime + paraBase64);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("X-Param", paraBase64);
                request.Headers.Add("X-CurTime", curTime);
                request.Headers.Add("X-Appid", model.AppID);
                request.Headers.Add("X-CheckSum", checkSum);

                Stream requestStream = request.GetRequestStream();
                StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("gb2312"));
                streamWriter.Write(bodys);
                streamWriter.Close();

                String htmlStr = string.Empty;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                {
                    string header_type = response.Headers["Content-Type"];
                    if (header_type == "audio/mpeg")
                    {
                        Stream st = response.GetResponseStream();
                        MemoryStream memoryStream = FileHelper.StreamToMemoryStream(st);
                        File.Create(tempfileName).Dispose();
                        File.WriteAllBytes(tempfileName, FileHelper.StreamTobyte(memoryStream));
                        result.ResultCode = 0;
                        result.Message = "语音合成成功";
                    }
                    else
                    {
                        htmlStr = reader.ReadToEnd();
                        var resultBody = JsonConvert.DeserializeObject<Body>(htmlStr);
                        result.ResultCode = resultBody.code;
                        result.Message = resultBody.code.GetDescription();
                        return result;
                    }
                }
                responseStream.Close();
            }
            FileHelper.Mp3Combine(model.FilePath, lst);
            FileHelper.DeleteFile(lst);
            return result;
        }
    }
    public class Body
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 错误提醒
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 返回码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        success = 0,
        [Description("文件类型参数错误，请检查文件后缀名是否和需要生成的文件匹配")]
        ParameterError =-1,
        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("检查apiKey，ip，checkSum等授权参数是否正确")]
        illegalaccess = 10105,
        /// <summary>
        /// 无效参数
        /// </summary>
        [Description("上传必要的参数， 检查参数格式以及编码")]
        invalidparameter = 10106,
        /// <summary>
        /// 非法参数值	
        /// </summary>
        [Description("检查参数值是否超过范围或不符合要求")]
        illegalparameter = 10107,
        /// <summary>
        /// 文本/音频长度非法
        /// </summary>
        [Description("检查上传文本/音频长度是否超过限制")]
        illegaltextoraudiolength = 10109,
        /// <summary>
        /// 无授权许可
        /// </summary>
        [Description("提供请求的 appid、 auth_id 向服务商反馈")]
        nolicense = 10110,
        /// <summary>
        /// 超时
        /// </summary>
        [Description("检测网络连接或联系服务商")]
        timeout = 10114,
        /// <summary>
        /// 引擎错误
        /// </summary>
        [Description("提供接口返回值，向服务商反馈")]
        engineerror = 10700,
        /// <summary>
        /// 无授权
        /// </summary>
        [Description("确认是否使用了未授权的功能点，例如部分识别语种、合成发音人、高阶评测等等，如果需要申请购买相关授权请提交工单")]
        novcnauth = 11200,
        /// <summary>
        /// 服务单日调用次数超过限制
        /// </summary>
        [Description("确认服务单日调用次数是否超过限制")]
        appidauthorizenumbernotenough = 11201
    }
}
