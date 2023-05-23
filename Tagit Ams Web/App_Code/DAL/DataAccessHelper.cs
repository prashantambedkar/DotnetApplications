using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using ECommerce.DataAccess;

namespace ECommerce.DataAccess
{
    public class DataAccessHelper1 : DataAccessMain
    {
        private SqlParameter[] parameters;

        public SqlParameter[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        /// <summary>
        /// Initialize this class' object to use DataAccess layer's functionalities, by providing StoredProcedures enum for 
        /// Procedure name which is to be executed. 
        /// </summary>
        /// <param name="procedureName">The Procedure name enum from StoredProcedures enum collection. Which is to be used for execution against database.
        /// If the specific Procedure doesn't exist in enum collection then ProcedureName property of DataAccessHelper object can be changed later on.</param>
        /// <param name="parameters">The values added and all prepared array of Sql Parameters.</param>
        public DataAccessHelper1(StoredProcedures procedureName, SqlParameter[] Parameters)
        {
            ProcedureName = procedureName.ToString();
            parameters = Parameters;
        }
        public DataAccessHelper1(string procedureName, SqlParameter[] Parameters)
        {
            ProcedureName = procedureName;
            parameters = Parameters;
        }





        public object ExecuteScalar()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }






        /// <summary>
        /// This will attach the parameters to the method's created command, execute the NonQuery and returns affected no of rows.
        /// It will throw ArgumentNullException if connection argument passed is null.
        /// </summary>
        /// <param name="connection">The initialized connection object.</param>
        /// <param name="Parameters">The prepared parameters array to be added to the method created command</param>
        /// <returns>Return the no of rows affected.</returns>
       
        
        
        
        
        
        public int ExecuteNonQuery(SqlConnection connection)
        {
            if (connection == null)
                connection = new SqlConnection(ConnectionString);
            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }
        public int ExecuteNonQuery()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        /// <summary>
        /// This will attach supplied parameters execute transaction and return affected no of rows.
        /// It will throw ArgumentNullException if transaction argument passed is null.
        /// </summary>
        /// <param name="transaction">The initialized transaction object.</param>
        /// <param name="Parameters">The prepared parameters array to be added to the method's created command</param>
        /// <returns>Return the no of rows affected.</returns>
        public int ExecuteNonQuery(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("The parameter transaction was null.");
            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public DataSet ExecuteDataset(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("The parameter transaction was null.");
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public DataSet ExecuteDataset(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public DataSet ExecuteDataset()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public DataSet ExecuteDatasetText()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteDataset(connection, CommandType.Text, ProcedureName, Parameters);
        }

        public object ExecuteScalar(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("The parameter transaction was null.");

            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public object ExecuteScalar(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, ProcedureName, Parameters);

        }

        public SqlDataReader ExecuteReader(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public SqlDataReader ExecuteReader(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("The parameter connection was null.");
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public XmlReader ExecuteXmlReader(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((connection.ConnectionString == null) || (connection.ConnectionString == ""))
                connection.ConnectionString = ConnectionString;

            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, ProcedureName, Parameters);
        }

        public XmlReader ExecuteXmlReader(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException("The parameter connection was null.");
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, ProcedureName, Parameters);
        }
    }
}
