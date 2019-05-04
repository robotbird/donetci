using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Utry.Framework.Utils
{
    /// <summary>
    /// 验证类
    /// </summary>
   public class ValidateHelper
    {


       /// <summary>
        /// 是否安全名称 中文 英文 数字 中划线
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        public static bool IsSafeText(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[a-zA-Z0-9\u4e00-\u9fa5\-]+$");
        }

       /// <summary>
        /// 是否编码格式 英文 数字 中划线
       /// </summary>
       /// <returns></returns>
        public static bool IsSafeCode(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[a-zA-Z0-9\-]+$");
        }
        /// <summary>
        /// 是否手机格式
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[1]+[1-9]+\d{9}")&&obj.ToString().Length==11;
        }
       /// <summary>
       /// 是否身份证
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        public bool IsIDcard(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"(^\d{18}$)|(^\d{15}$)");

        }
        /// <summary>
        /// 是否邮政编码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsPostalcode(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^\d{6}$");

        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }


        /// <summary>
        /// 是否数字类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumeric(object obj)
        {
            if (obj == null || obj==DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(),@"^[-]?\d+[.]?\d*$");
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return true;
            }
            return string.IsNullOrEmpty(obj.ToString().Trim());
        }

        /// <summary>
        /// 判断是否当前长度
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length">需要比较的长度</param>
        /// <returns></returns>
        public static bool IsThisLength(object obj,int length)
        {
            if (obj == null || obj == DBNull.Value)
            {
                if (length == 0)
                {
                    return true;
                }
                return false;
            }
            int len = 0;

            for (int i = 0; i < obj.ToString().Length; i++)
            {
                byte[] byteLen = Encoding.Default.GetBytes(obj.ToString().Substring(i, 1));
                if (byteLen.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return length >= len;
        }

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[0-9]*$");
        }
       /// <summary>
       /// 是否浮点型
       /// </summary>
        /// <param name="obj"></param>
       /// <returns></returns>
        public static bool IsFloat(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
           return Regex.IsMatch(obj.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
        }

        /// <summary>
        /// 判断用户输入是否为短日期格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSortDate(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})");
        }

        /// <summary>
        /// 判断用户输入是否为日期
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// 可判断格式如下（其中-可替换为/，不影响验证)
        /// YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF
        /// </remarks>
        public static bool IsDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }
            string strValue = obj.ToString();
            string regexDate = @"[1-2]{1}[0-9]{3}((-|\/){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";

            if (Regex.IsMatch(strValue, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性
                int _IndexY = -1;
                int _IndexM = -1;
                int _IndexD = -1;

                if (-1 != (_IndexY = strValue.IndexOf("-")))
                {
                    _IndexM = strValue.IndexOf("-", _IndexY + 1);
                    _IndexD = strValue.IndexOf(":");
                }
                else
                {
                    _IndexY = strValue.IndexOf("/");
                    _IndexM = strValue.IndexOf("/", _IndexY + 1);
                    _IndexD = strValue.IndexOf(":");
                }

                //不包含日期部分，直接返回true
                if (-1 == _IndexM)
                    return true;

                if (-1 == _IndexD)
                {
                    _IndexD = strValue.Length + 3;
                }

                int iYear = Convert.ToInt32(strValue.Substring(0, _IndexY));
                int iMonth = Convert.ToInt32(strValue.Substring(_IndexY + 1, _IndexM - _IndexY - 1));
                int iDate = Convert.ToInt32(strValue.Substring(_IndexM + 1, _IndexD - _IndexM - 4));

                //判断月份日期
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                        return true;
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                            return true;
                    }
                    else
                    {
                        //闰年
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                                return true;
                        }
                        else
                        {
                            if (iDate < 29)
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
                return false;

            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }
        /// <summary>
        ///  判断字符串是否合法的日期格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsData(string value)
        {
            try
            {
                System.DateTime.Parse(value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否为httpUrl地址
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static bool IsHttpUrl(string httpUrl)
        {
            //return Regex.IsMatch(WebUrl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            //    return Regex.IsMatch(WebUrl, @"http://");

            return httpUrl.IndexOf("http://") != -1;
        }
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\'|\+]");
        }
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0)
                            return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                        return false;

                    cOctets--;
                }
            }

            if (cOctets > 0)
                return false;

            if (bAllAscii)
                return false;

            return true;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
    }
}
