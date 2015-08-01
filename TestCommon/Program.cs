using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtility.Common;
using CommonUtility.Command;

namespace TestCommon
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new CmdHelper(false);
            Console.WriteLine(cmd.ExecCMD("Ping http://www.google.com.tw"));

            //Console.WriteLine("F12639459".MaskStr(2, startIndex: 3));
            Console.ReadLine();
        }
    }
}
