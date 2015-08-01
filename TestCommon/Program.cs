using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtility.Common;
using CommonUtility.Command;
using CommonUtility.Excel;
using System.Data;

namespace TestCommon
{
    class Program
    {
        static void Main(string[] args)
        {
            var exHelper = new ClosedXMLExtension(1,1);

            var dt = exHelper.Import(@"W://test.xlsx");

            foreach (DataRow item in dt.Rows)
            {
                Console.WriteLine(item[0] + ":" + item[1]);
            }

            //var cmd = new CmdHelper(false);
            //Console.WriteLine(cmd.ExecCMD("Ping http://www.google.com.tw"));

            //Console.WriteLine("F12639459".MaskStr(2, startIndex: 3));
            Console.ReadLine();
        }
    }
}
