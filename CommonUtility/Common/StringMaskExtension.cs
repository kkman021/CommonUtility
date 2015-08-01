using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility.Common
{
    /// <summary>
    /// 字串遮罩 Extension
    /// </summary>
    public static class StringMaskExtension
    {
        /// <summary>
        /// 遮罩字串
        /// </summary>
        /// <param name="oriText">原始字串</param>
        /// <param name="maskLength">遮罩長度</param>
        /// <param name="maskText">遮罩替換文字（預設O）</param>
        /// <param name="startIndex">遮罩開始位置（預設0）</param>
        /// <returns>遮罩後字串</returns>
        public static string MaskStr(this string oriText, int maskLength, string maskText = "O", int startIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(oriText) || maskLength == null)
                return oriText;

            if (oriText.Length <= startIndex)
                return oriText;

            return MaskStr(oriText, maskLength, startIndex, null, maskText);
        }

        /// <summary>
        /// 遮罩字串(指定Index)
        /// </summary>
        /// <param name="oriText">原始字串</param>
        /// <param name="maskIndexs">指定遮罩Index</param>
        /// <param name="maskText">遮罩替換文字</param>
        /// <returns>遮罩後字串</returns>
        public static string MaskStr(this string oriText, List<int> maskIndexs, string maskText = "O")
        {
            if (string.IsNullOrWhiteSpace(oriText) || maskIndexs == null)
                return oriText;

            return MaskStr(oriText, null, null, maskIndexs, maskText);
        }

        /// <summary>
        /// 遮罩字串
        /// </summary>
        /// <param name="oriText">原始字串</param>
        /// <param name="maskLength">遮罩長度</param>
        /// <param name="startIndex">遮罩開始位置</param>
        /// <param name="maskIndexs">指定遮罩Index(若有指定則只會替換指定位置)</param>
        /// <param name="maskText">遮罩替換文字</param>
        /// <returns>遮罩後字串</returns>
        private static string MaskStr(this string oriText, int? maskLength, int? startIndex, List<int> maskIndexs, string maskText)
        {
            var oriTextArry = oriText.ToArray();

            var result = "";

            if (maskIndexs != null && maskIndexs.Count > 0)
            {
                for (int i = 0; i < oriText.Length; i++)
                {
                    if (maskIndexs.Contains(i))
                    {
                        result += maskText;
                    }
                    else
                    {
                        result += oriTextArry[i].ToString();
                    }
                }
            }
            else
            {

                for (int i = 0; i < oriText.Length; i++)
                {
                    if (i >= startIndex && i < (startIndex + maskLength))
                    {
                        result += maskText;
                    }
                    else
                    {
                        result += oriTextArry[i].ToString();
                    }
                }
            }
            return result;
        }
    }
}
