using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;

namespace Tweakers.Models
{
    public static class DatabaseManager
    {
        /// <summary>
        /// Gets a new OracleConnection with it's connection string set (using 'OracleConnectionString' from the web config).
        /// </summary>
        private static OracleConnection Connection
        {
            get
            {
                return
                    new OracleConnection(
                        ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString);
            }
        }

        internal static List<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an OracleCommand for the given query using the static OracleConnection field, and sets the CommandType to CommandType.Text (used for plain text queries).
        /// Used prior to adding parameters and executing the query.
        /// </summary>
        /// <param name="connection">The connection information, which should be made using the Connection property.</param>
        /// <param name="sql">Query string to run</param>
        /// <returns>OracleCommand with the query and Connection information set</returns>
        private static OracleCommand CreateOracleCommand(OracleConnection connection, string sql)
        {
            OracleCommand command = new OracleCommand(sql, connection);
            command.CommandType = System.Data.CommandType.Text;
            return command;
        }

        internal static bool DisableAccount(Account account)
        {
            throw new NotImplementedException();
        }

        internal static bool UpdateAccount(Account account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs the query of an OracleCommand, and returns an unread OracleDataReader with the results of the query.
        /// </summary>
        /// <param name="command">OracleCommand containing the data for the query</param>
        /// <returns>OracleDataReader with the result of the query</returns>
        private static OracleDataReader ExecuteQuery(OracleCommand command)
        {
            try
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        command.Connection.Open();
                    }
                    catch (OracleException exc)
                    {
                        Debug.WriteLine("Database Connection failed!\n" + exc.Message);
                        throw;
                    }
                }

                OracleDataReader reader = command.ExecuteReader();

                return reader;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Runs the command in the given OracleCommand with ExecuteNonQuery; which is used for queries where no data is returned (in an OracleDataReader).
        /// Return value indicates if any rows are updated.
        /// </summary>
        /// <param name="command">OracleCommand containing the data for the query.</param>
        /// <returns>True when at least one row is updated.</returns>
        private static bool ExecuteNonQuery(OracleCommand command)
        {
            if (command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }

            return command.ExecuteNonQuery() != 0;
        }

        internal static bool CreateAccount(Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandCreate = CreateOracleCommand(connection,
                        "INSERT INTO ACCOUNT(Gebruikersnaam, Wachtwoord, Email, Voornaam) VALUES (:userName, :password, :email, :firstName)");
                    commandCreate.Parameters.Add(":userName", account.UserName);
                    commandCreate.Parameters.Add(":password", account.Password);
                    commandCreate.Parameters.Add(":Email", account.Email);
                    commandCreate.Parameters.Add(":firstName", account.FirstName);

                    return ExecuteNonQuery(commandCreate);
                }
                catch (OracleException)
                {
                    throw;
                }
            }
        }

        public static List<PriceWatchCategory> GetPriceWatch()
        {
            List<PriceWatchCategory> returnCategories = new List<PriceWatchCategory>();
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandSelect = CreateOracleCommand(connection,
                        "Select * from PRICEWATCH_CATEGORIE");

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    while (MainReader.Read())
                    {
                        int id = Convert.ToInt32(MainReader["ID"].ToString());
                        string name = MainReader["Naam"].ToString();
                        int subid = MainReader.IsDBNull(2) ? 0 : MainReader.GetInt32(2);
                        returnCategories.Add(new PriceWatchCategory(id, name, subid, null));

                    }
                    List<PriceWatchCategory> parentCategories = returnCategories;
                    foreach (PriceWatchCategory c in returnCategories)
                    {
                        foreach (PriceWatchCategory p in parentCategories)
                        {
                            if (c.SubID == p.ID)
                            {
                                c.ParentCat = p;
                            }
                        }
                    }
                    return returnCategories;
                    ;
                }
                catch (OracleException)
                {
                    throw;
                }
            }
        }

        public static List<Product> GetCatProducts(int id)
        {
            List<Product> returnProducts = new List<Product>();
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandSelect = CreateOracleCommand(connection,
                        "Select * from Product WHERE CAT_ID = (Select id from PRICEWATCH_CATEGORIE where id = :id)");
                    commandSelect.Parameters.Add(":id", id);
                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    //if (MainReader.IsDBNull(0))
                    //{
                    //    returnProducts = null;
                    //}
                    while (MainReader.Read())
                    {
                        int pid = Convert.ToInt32(MainReader["ID"].ToString());
                        string name = MainReader["Naam"].ToString();
                        string specifications = MainReader["Specificaties"].ToString();
                        double price = Convert.ToDouble(MainReader["Prijs"].ToString());
                        returnProducts.Add(new Product(pid, name, specifications, price));

                    }
                    return returnProducts;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }
        }
    }
}