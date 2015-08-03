using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using System.IO;
using System.Reflection;


/*
    2015.8.3 Create (尚未完成）
 */
namespace CommonUtility.DB
{
    public class OracelHelper : IDisposable
    {
        private OracleConnection conn = null;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="configText">Config ConnectionString區段標記名稱</param>
        public OracelHelper(string configText)
        {
            var connStr = ConfigurationManager.ConnectionStrings[configText];

            if (connStr == null)
                throw new Exception("無連線字串");
            else
                conn = new OracleConnection(connStr.ConnectionString);
        }

        /// <summary>
        /// 依參數執行query並產生被影響的資料列列數 (使用現有Connection)
        /// </summary>
        /// <param name="conn">連線字串</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <param name="bindByName">參數是否靠參數名稱綁定（True 是，False 否）</param>
        /// <param name="cmdTimeout">command timeout</param>
        /// <returns>回傳查詢後被影響的資料列列數(return int)</returns>
        public static int ExecuteNonQuery(OracleConnection conn, CommandType cmdType, string cmdText,
                            List<OracleParameter> cmdParms, bool bindByName = true, int? cmdTimeout = null)
        {
            OracleCommand cmd = new OracleCommand();

            GetCommand(cmd, conn, null, cmdType, cmdText, cmdParms.ToArray(), bindByName, cmdTimeout);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection
        /// </summary>
        /// <param name="conn">連線字串</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <param name="bindByName">參數是否靠參數名稱綁定（True 是，False 否）</param>
        /// <param name="cmdTimeout">command timeout</param>
        /// <returns>回傳查詢後被影響的資料列列數(return int)</returns>
        public static object ExecuteNonQuery(OracleConnection conn, CommandType cmdType, string cmdText,
                            List<OracleParameter> cmdParms, bool bindByName = true, int? cmdTimeout = null)
        {
            OracleCommand cmd = new OracleCommand();

            GetCommand(cmd, conn, null, cmdType, cmdText, cmdParms.ToArray(), bindByName, cmdTimeout);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection
        /// </summary>
        /// <param name="conn">連線字串</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <param name="bindByName">參數是否靠參數名稱綁定（True 是，False 否）</param>
        /// <param name="cmdTimeout">command timeout</param>
        /// <returns>回傳查詢後被影響的資料列列數(return int)</returns>
        public static DataSet ExecuteDataSet(OracleConnection conn, CommandType cmdType, string cmdText,
                    List<OracleParameter> cmdParms, bool bindByName = true, int? cmdTimeout = null)
        {
            OracleCommand cmd = new OracleCommand();
            GetCommand(cmd, conn, null, cmdType, cmdText, cmdParms.ToArray(), bindByName, cmdTimeout);
            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds;
        }

        #region 取得Command物件
        /// <summary>
        /// 取得Command物件
        /// </summary>
        /// <param name="cmd">OracleCommand 物件</param>
        /// <param name="conn">連線字串</param>
        /// <param name="trans"></param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">OracleParameter to use in the command</param>
        /// <param name="bindByName">參數是否靠參數名稱綁定（True 是，False 否）</param>
        /// <param name="cmdTimeout">command timeout</param>
        private static void GetCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans,
                            CommandType cmdType, string cmdText, OracleParameter[] cmdParms, bool bindByName = true, int? cmdTimeout = null)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;
            cmd.BindByName = bindByName;

            if (cmdTimeout != null)
                cmd.CommandTimeout = (int)cmdTimeout;

            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                {
                    if (parm != null)
                        cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #region 關閉連線
        /// <summary>
        /// 關閉連線
        /// </summary>
        private void CloseConn()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// 解構
        /// </summary>
        public void Dispose()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Dispose();
            }
        }
        #endregion
    }
}
