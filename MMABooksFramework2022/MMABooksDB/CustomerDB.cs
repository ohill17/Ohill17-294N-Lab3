using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using MMABooksProps;

using System.Data;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB 
{
    public class CustomerDB : DBBase, IReadDB, IWriteDB
    {
        /*public IBaseProps Create(IBaseProps props)
        {
            throw new NotImplementedException();
        }
        */
        public bool Delete(IBaseProps p)
        {
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            command.Parameters["custId"].Value = props.CustomerID;
            command.Parameters["name_p"].Value = props.Name;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public IBaseProps Create(IBaseProps p)
{
   int rowsAffected = 0;
   CustomerProps props = (CustomerProps)p;

   DBCommand command = new DBCommand();
   command.CommandText = "usp_CustomerCreate";
   command.CommandType = CommandType.StoredProcedure;
   command.Parameters.Add("custId", DBDbType.Int32);
   command.Parameters.Add("name_p", DBDbType.VarChar);
   command.Parameters[0].Direction = ParameterDirection.Output;
   command.Parameters["name_p"].Value = props.Name;
   try
   {
       rowsAffected = RunNonQueryProcedure(command);
       if (rowsAffected == 1)
       {
           props.CustomerID = (int)command.Parameters[0].Value;
           props.ConcurrencyID = 1;
           return props;
       }
       else
           throw new Exception("Unable to insert record. " + props.ToString());
   }
   catch (Exception e)
   {
       // log this error
       throw;
   }
   finally
   {
       if (mConnection.State == ConnectionState.Open)
           mConnection.Close();
   }
}

        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            CustomerProps props = new CustomerProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_CustomerSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("CustID", DBDbType.UInt32);
            command.Parameters["CustID"].Value = (int)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }
        public object RetrieveAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("custID", DBDbType.UInt32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["custID"].Value = props.CustomerID;
            command.Parameters["name_p"].Value = props.Name;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}
