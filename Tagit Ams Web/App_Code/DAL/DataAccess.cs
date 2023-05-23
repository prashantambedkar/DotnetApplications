using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.ApplicationBlocks.Data;


namespace ECommerce.DataAccess
{
    /// <summary>
    /// Summary description for DataAccess
    /// </summary>
    public class DataAccessHelper
    {
        SqlConnection conn=null;
        DataSet resultExecuteDataSet;
        string proc, detailedMessage;      
        int effectedRowCount;
        XmlReader resultExecuteXmlReader;
        SqlDataReader resultExecuteReader;
        Object resultExecuteScalar;

        public string DetailedMessage
        {
            get { return detailedMessage; }
            set { detailedMessage = value; }
        }
        //result of Execute scalar will be stored in this property
        public Object ResultExecuteScalar
        {
            get { return resultExecuteScalar; }
            set { resultExecuteScalar = value; }
        }
        //result of ExecuteReader will be stored in this property
        public SqlDataReader ResultExecuteReader
        {
            get { return resultExecuteReader; }
            set { resultExecuteReader = value; }
        }

        public XmlReader ResultExecuteXmlReader
        {
            get { return resultExecuteXmlReader; }
            set { resultExecuteXmlReader = value; }
        }

        public int EffectedRowCount
        {
            get { return effectedRowCount; }
            set { effectedRowCount = value; }
        }
        public int ResultExecuteNonQuery
        {
            get { return effectedRowCount; }
            set { effectedRowCount = value; }
        }

        public string Proc
        {
            get { return proc; }
            set { proc = value; }
        }
        public DataSet ResultExecuteDataSet
        {
            get { return resultExecuteDataSet; }
            set { resultExecuteDataSet = value; }
        }

        public DataAccessHelper()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public DataAccessHelper(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString was null or empty");
            conn = new SqlConnection(connectionString);
        }

        public DataAccessHelper(SqlConnection  connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            conn = connection;
        }
        //Execute non query overloads
        public ProcessingErrors ExecuteNonQuery(string query,SqlParameter[] param)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                effectedRowCount = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteNonQuery(string query,SqlParameter[] param,SqlTransaction tran)
        {
              try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                else if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }

            effectedRowCount = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, query, param);
                  return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteNonQuery( StoredProcedures sp , SqlParameter[] param)
        {
            try
            {
                effectedRowCount = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteNonQuery( StoredProcedures sp, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {               
                 if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                effectedRowCount = SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteNonQuery(StoredProcedures sp, SqlCommand cmd)
        {
            try
            {
                if (cmd == null)
                {
                    detailedMessage = "cmd(Command) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }                
                SqlParameter[] param = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(param, 0);
                cmd.Parameters.Clear();
                effectedRowCount = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
               
        }

        public ProcessingErrors ExecuteNonQuery(string sp, SqlCommand cmd)
        {
            try
            {
                if (cmd == null)
                {
                    detailedMessage = "cmd(Command) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                SqlParameter[] param = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(param, 0);
                cmd.Parameters.Clear();
                effectedRowCount = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sp, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }

        }

        public ProcessingErrors ExecuteNonQuery( SqlParameter[] param,string sp)
        {
            try
            {
                effectedRowCount = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, sp, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }
        
        //Execute Dataset overloads
        public ProcessingErrors ExecuteDataSet( string query, SqlParameter[] param)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteDataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteDataSet( string query, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                else if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }

                resultExecuteDataSet = SqlHelper.ExecuteDataset(tran, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteDataSet(StoredProcedures sp, SqlParameter[] param)
        {
            try
            {               
                resultExecuteDataSet = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteDataSet(StoredProcedures sp, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {               
               if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteDataSet = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteDataSet(StoredProcedures sp, SqlCommand cmd)
        {
            try
            {
                if (cmd == null)
                {
                    detailedMessage = "cmd(Command) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                SqlParameter[] param=new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(param, 0);
                cmd.Parameters.Clear();
                resultExecuteDataSet = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;                
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        //Execute Reader overloads
        public ProcessingErrors ExecuteReader( string query, SqlParameter[] param)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteReader( string query, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                else if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }

                resultExecuteReader = SqlHelper.ExecuteReader(tran, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteReader( StoredProcedures sp, SqlParameter[] param)
        {
            try
            {               
                resultExecuteReader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteReader( StoredProcedures sp, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {               
               if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteReader = SqlHelper.ExecuteReader(tran, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        //Execute XmlReader overloads
        public ProcessingErrors ExecuteXmlReader( string query, SqlParameter[] param)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteXmlReader = SqlHelper.ExecuteXmlReader(conn, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteXmlReader( string query, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                else if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }

                resultExecuteXmlReader = SqlHelper.ExecuteXmlReader(tran, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteXmlReader( StoredProcedures sp, SqlParameter[] param)
        {
            try
            {                
                resultExecuteXmlReader = SqlHelper.ExecuteXmlReader(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteXmlReader( StoredProcedures sp, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteXmlReader = SqlHelper.ExecuteXmlReader(tran, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        //Execute Scalar overloads
        public ProcessingErrors ExecuteScalar( string query, SqlParameter[] param)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteScalar = SqlHelper.ExecuteScalar(conn, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteScalar( string query, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    detailedMessage = "query parameter was null or blank.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                else if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }

                resultExecuteScalar = SqlHelper.ExecuteScalar(tran, CommandType.Text, query, param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteScalar( StoredProcedures sp, SqlParameter[] param)
        {
            try
            {
                resultExecuteScalar = SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteScalar( StoredProcedures sp, SqlParameter[] param, SqlTransaction tran)
        {
            try
            {
                if (tran == null)
                {
                    detailedMessage = "tran(transaction) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                resultExecuteScalar = SqlHelper.ExecuteScalar(tran, CommandType.StoredProcedure, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        public ProcessingErrors ExecuteScalar(StoredProcedures sp, SqlCommand cmd)
        {
            try
            {
                if (cmd == null)
                {
                    detailedMessage = "cmd(Command) parameter was null.";
                    return ProcessingErrors.MinimumDataNotProvided;
                }
                SqlParameter[] param = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(param, 0);
                cmd.Parameters.Clear();
                resultExecuteScalar = SqlHelper.ExecuteScalar(conn, sp.ToString(), param);
                return ProcessingErrors.ProcessedSuccessfully;
            }
            catch (SqlException e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.DataBaseExceptionCaught;
            }
            catch (Exception e)
            {
                detailedMessage = e.Message;
                return ProcessingErrors.ExceptionCaught;
            }
        }

        
    }
}
