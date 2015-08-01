using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility.Command
{
    /// <summary>
    /// 命令提示字元Helper
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        /// 是否顯示命令提示視窗
        /// </summary>
        private bool _IsShowWindow { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="IsShowWindow">是否顯示命令提示視窗</param>
        public CmdHelper(bool IsShowWindow = false)
        {
            this._IsShowWindow = IsShowWindow;
        }

        /// <summary>
        /// 執行命令提示字元並回傳執行結果文字
        /// </summary>
        /// <param name="cmdText">指令</param>
        /// <returns>結果文字</returns>
        public string ExecCMD(string cmdText)
        {
            Process cmd = new Process();

            ProcessStartInfo startInfo = GetProStartInfo(cmdText, true);

            cmd.StartInfo = startInfo;
            cmd.Start();

            string strOutput = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();

            return strOutput;
        }

        /// <summary>
        /// 執行命令提示字元
        /// </summary>
        /// <param name="cmdText">指令</param>
        public void ExecNonCMD(string cmdText)
        {
            Process cmd = new Process();

            ProcessStartInfo startInfo = GetProStartInfo(cmdText, false);

            cmd.StartInfo = startInfo;
            cmd.Start();

            cmd.WaitForExit();
        }

        /// <summary>
        /// 取得Process 初始參數
        /// </summary>
        /// <param name="CmdText">命令字元</param>
        /// <param name="IsReturnOutput">是否要輸出結果</param>
        /// <returns></returns>
        private ProcessStartInfo GetProStartInfo(string CmdText, bool IsReturnOutput)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (!_IsShowWindow)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            if (IsReturnOutput)
                startInfo.RedirectStandardOutput = true;

            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c" + @CmdText;

            return startInfo;
        }
    }
}
