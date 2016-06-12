using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;
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

        public static List<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        public static List<Article> GetArticles()
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    List<Article> returnArticles = new List<Article>();
                    OracleCommand commandSelect = CreateOracleCommand(connection, "Select * from Artikel");

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    if (MainReader.HasRows == false)
                    {
                        return null;
                    }
                    while (MainReader.Read())
                    {
                        int id = Convert.ToInt32(MainReader["ID"].ToString());
                        string title = MainReader["Titel"].ToString();
                        DateTime date = Convert.ToDateTime(MainReader["Datum"].ToString());
                        string context = MainReader["Inhoud"].ToString();

                        returnArticles.Add(new Article(id, title, null, date, context));
                    }
                    foreach (Article a in returnArticles)
                    {
                        OracleCommand commandSubSelect = CreateOracleCommand(connection,
                            "SELECT * FROM ACCOUNT WHERE GEBRUIKERSNAAM = (SELECT AUTEUR FROM ARTIKEL WHERE ID = :id)");
                        commandSubSelect.Parameters.Add(":id", a.ID);
                        OracleDataReader SubReader = ExecuteQuery(commandSubSelect);
                        Account account = new Account();
                        while (SubReader.Read())
                        {
                            account.UserName = SubReader["Gebruikersnaam"].ToString();
                            account.FirstName = SubReader["Voornaam"].ToString();
                            account.LastName = SubReader["Achternaam"].ToString();
                            account.Education = SubReader["Opleidingsniveau"].ToString();
                            account.Function = SubReader["Functie"].ToString();
                        }
                        a.Author = account;

                    }
                    return returnArticles;
                }
                catch (OracleException)
                {
                    return null;
                    throw;
                }
                
            }
        }

        public static bool AddReview(Review review)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "INSERT INTO REVIEW(ID, PRODUCT_ID, AUTEUR, TITEL, REVIEW) VALUES (REVIEW_FCSEQ.NEXTVAl, :productID, :userName, :title, :context");
                    commandInsert.Parameters.Add(":productID", review.Product.ProductID);
                    commandInsert.Parameters.Add(":userName", review.Reviewer.UserName);
                    commandInsert.Parameters.Add(":title", review.Title);
                    commandInsert.Parameters.Add(":context", review.Context);

                    return ExecuteNonQuery(commandInsert);
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public static bool UpdateReview(Review review)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandUpdate = CreateOracleCommand(connection,
                        "UPDATE REVIEW SET TITEL = :title, REVIEW = :context WHERE ID = :id");
                    commandUpdate.Parameters.Add(":title", review.Title);
                    commandUpdate.Parameters.Add(":context", review.Context);
                    commandUpdate.Parameters.Add(":id", review.ID);

                    return ExecuteNonQuery(commandUpdate);
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public static bool UpdateProduct(Product product)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "UPDATE PRODUCT SET Naam = :name, Prijs = :price, Specificaties = :specifications WHERE ID = :id");
                    commandInsert.Parameters.Add(":name", product.Name);
                    commandInsert.Parameters.Add(":price", product.Price);
                    commandInsert.Parameters.Add(":specifications", product.Specifications);
                    commandInsert.Parameters.Add(":id", product.ProductID);

                    return ExecuteNonQuery(commandInsert);

                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public static bool AddProduct(Product product, int catID)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "INSERT INTO PRODUCT(ID, NAAM, PRIJS, SPECIFICATIES, CAT_ID) VALUES(Product_FCSEQ.NextVal, :name, :price, :specifications, :catID)");
                    commandInsert.Parameters.Add(":name", product.Name);
                    commandInsert.Parameters.Add(":price", product.Price);
                    commandInsert.Parameters.Add(":specifications", product.Specifications);
                    commandInsert.Parameters.Add(":catID", catID);

                    return ExecuteNonQuery(commandInsert);
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public static bool AddReviewReaction(Reaction reaction, Review review, Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "INSERT INTO REACTIE (ID, ACCOUNT, DATUM, BERICHT) VALUES (Reactie_FCSEQ.NextVal, :account, :datum, :bericht)");
                    commandInsert.Parameters.Add(":account", account.UserName);
                    commandInsert.Parameters.Add(":datum", reaction.PostTime);
                    commandInsert.Parameters.Add(":bericht", reaction.Context);
                    commandInsert.BindByName = true;

                    if (ExecuteNonQuery(commandInsert))
                    {
                        OracleCommand commandSelect = CreateOracleCommand(connection, "SELECT MAX(ID) FROM REACTIE");
                        OracleDataReader MainReader = ExecuteQuery(commandSelect);
                        int id = 0;
                        while (MainReader.Read())
                        {
                            id = Convert.ToInt32(MainReader["MAX(ID)"].ToString());
                        }
                        commandInsert = CreateOracleCommand(connection,
                            "INSERT INTO REACTIE_REVIEW (REVIEW_ID, REACTIE_ID) VALUES(:reviewID, :reactionID)");
                        commandInsert.Parameters.Add(":review", review.ID);
                        commandInsert.Parameters.Add(":reactionID", id);
                        return ExecuteNonQuery(commandInsert);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static bool AddArticleReaction(Reaction reaction, Article article, Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "INSERT INTO REACTIE (ID, ACCOUNT, DATUM, BERICHT) VALUES (Reactie_FCSEQ.NextVal, :account, :datum, :bericht)");
                    commandInsert.Parameters.Add(":account", account.UserName);
                    commandInsert.Parameters.Add(":datum", reaction.PostTime);
                    commandInsert.Parameters.Add(":bericht", reaction.Context);
                    commandInsert.BindByName = true;

                    if (ExecuteNonQuery(commandInsert))
                    {
                        OracleCommand commandSelect = CreateOracleCommand(connection, "SELECT MAX(ID) FROM REACTIE");
                        OracleDataReader MainReader = ExecuteQuery(commandSelect);
                        int id = 0;
                        while (MainReader.Read())
                        {
                            id = Convert.ToInt32(MainReader["MAX(ID)"].ToString());
                        }
                        commandInsert = CreateOracleCommand(connection,
                            "INSERT INTO REACTIE_ARTIKEL (ARTIKEL_ID, REACTIE_ID) VALUES(:articleID, :reactionID)");
                        commandInsert.Parameters.Add(":articleID", article.ID);
                        commandInsert.Parameters.Add(":reactionID", id);
                        return ExecuteNonQuery(commandInsert);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }
        public static bool AddSubArticleReaction(Reaction reaction,int parentid, Article article, Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandInsert = CreateOracleCommand(connection,
                        "INSERT INTO REACTIE (ID, ACCOUNT, DATUM, BERICHT) VALUES (Reactie_FCSEQ.NextVal, :account, :datum, :bericht)");
                    commandInsert.Parameters.Add(":account", account.UserName);
                    commandInsert.Parameters.Add(":datum", reaction.PostTime);
                    commandInsert.Parameters.Add(":bericht", reaction.Context);
                    commandInsert.BindByName = true;

                    if (ExecuteNonQuery(commandInsert))
                    {
                        OracleCommand commandSelect = CreateOracleCommand(connection, "SELECT MAX(ID) FROM REACTIE");
                        OracleDataReader MainReader = ExecuteQuery(commandSelect);
                        int id = 0;
                        while (MainReader.Read())
                        {
                            id = Convert.ToInt32(MainReader["MAX(ID)"].ToString());
                        }
                        commandInsert = CreateOracleCommand(connection,
                            "INSERT INTO REACTIE_ARTIKEL (ARTIKEL_ID, REACTIE_ID) VALUES(:articleID, :reactionID)");
                        commandInsert.Parameters.Add(":articleID", article.ID);
                        commandInsert.Parameters.Add(":reactionID", id);
                        if(ExecuteNonQuery(commandInsert))
                        {
                            commandInsert = CreateOracleCommand(connection,
                                "INSERT INTO SUBREACTIE(REACTIE_ID, SUBREACTIE_ID) VALUES(:parentid, :childid)");
                            commandInsert.Parameters.Add(":parentid", parentid);
                            commandInsert.Parameters.Add(":childid", id);
                            return ExecuteNonQuery(commandInsert);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static List<Reaction> GetReactions(Article article)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    List<Reaction> returnReactions = new List<Reaction>();
                    OracleCommand commandSelect = CreateOracleCommand(connection, "select * from REACTIE LEFT JOIN SUBREACTIE ON REACTIE.ID = SUBREACTIE_ID LEFT JOIN ACCOUNT ON REACTIE.ACCOUNT = ACCOUNT.GEBRUIKERSNAAM WHERE ID in (select reactie_id from REACTIE_ARTIKEL where ARTIKEL_ID = (select id from artikel where titel = :title))");
                    commandSelect.Parameters.Add(":title", article.Title);

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    while (MainReader.Read())
                    {
                        if (MainReader["Reactie_ID"].ToString() == string.Empty)
                        {
                            int id = Convert.ToInt32(MainReader["ID"].ToString());
                            Account account = new Account();
                            account.UserName = MainReader["Gebruikersnaam"].ToString();
                            account.FirstName = MainReader["Voornaam"].ToString();
                            account.LastName = MainReader["Achternaam"].ToString();
                            account.Education = MainReader["Opleidingsniveau"].ToString();
                            account.Function = MainReader["Functie"].ToString();
                            DateTime datetime = Convert.ToDateTime(MainReader["DATUM"].ToString());
                            string context = MainReader["Bericht"].ToString();
                            returnReactions.Add(new Reaction(id, account, datetime, context));
                        }
                    }
                    foreach(Reaction r in returnReactions)
                    {
                        r.SubReactions = new List<Reaction>();
                        MainReader = ExecuteQuery(commandSelect);
                        while (MainReader.Read())
                        {
                            if (MainReader["Reactie_ID"].ToString() != string.Empty && Convert.ToInt32(MainReader["Reactie_ID"].ToString()) == r.Id)
                            {
                                int id = Convert.ToInt32(MainReader["ID"].ToString());
                                Account account = new Account();
                                account.UserName = MainReader["Gebruikersnaam"].ToString();
                                account.FirstName = MainReader["Voornaam"].ToString();
                                account.LastName = MainReader["Achternaam"].ToString();
                                account.Education = MainReader["Opleidingsniveau"].ToString();
                                account.Function = MainReader["Functie"].ToString();
                                DateTime datetime = Convert.ToDateTime(MainReader["DATUM"].ToString());
                                string context = MainReader["Bericht"].ToString();
                                r.SubReactions.Add(new Reaction(id, account, datetime, context));
                            }
                        }
                        r.SubReactions.Sort((x, y) => y.PostTime.CompareTo(x.PostTime));
                    }
                    return returnReactions;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public static List<Reaction> GetReactions(Review review)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    List<Reaction> returnReactions = new List<Reaction>();
                    OracleCommand commandSelect = CreateOracleCommand(connection, "select * from REACTIE LEFT JOIN SUBREACTIE ON REACTIE.ID = SUBREACTIE_ID LEFT JOIN ACCOUNT ON REACTIE.ACCOUNT = ACCOUNT.GEBRUIKERSNAAM WHERE ID in (select reactie_id from REACTIE_REVIEW where REVIEW_ID = (select id from review where titel = :title))");
                    commandSelect.Parameters.Add(":title", review.Title);

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    if (!MainReader.HasRows)
                    {
                        return null;
                    }
                    while (MainReader.Read())
                    {
                        if (MainReader["Reactie_ID"].ToString() == string.Empty)
                        {
                            int id = Convert.ToInt32(MainReader["ID"].ToString());
                            Account account = new Account();
                            account.UserName = MainReader["Gebruikersnaam"].ToString();
                            account.FirstName = MainReader["Voornaam"].ToString();
                            account.LastName = MainReader["Achternaam"].ToString();
                            account.Education = MainReader["Opleidingsniveau"].ToString();
                            account.Function = MainReader["Functie"].ToString();
                            DateTime datetime = Convert.ToDateTime(MainReader["DATUM"].ToString());
                            string context = MainReader["Bericht"].ToString();
                            returnReactions.Add(new Reaction(id, account, datetime, context));
                        }
                    }
                    foreach (Reaction r in returnReactions)
                    {
                        r.SubReactions = new List<Reaction>();
                        MainReader = ExecuteQuery(commandSelect);
                        while (MainReader.Read())
                        {
                            if (MainReader["Reactie_ID"].ToString() != string.Empty && Convert.ToInt32(MainReader["Reactie_ID"].ToString()) == r.Id)
                            {
                                int id = Convert.ToInt32(MainReader["ID"].ToString());
                                Account account = new Account();
                                account.UserName = MainReader["Gebruikersnaam"].ToString();
                                account.FirstName = MainReader["Voornaam"].ToString();
                                account.LastName = MainReader["Achternaam"].ToString();
                                account.Education = MainReader["Opleidingsniveau"].ToString();
                                account.Function = MainReader["Functie"].ToString();
                                DateTime datetime = Convert.ToDateTime(MainReader["DATUM"].ToString());
                                string context = MainReader["Bericht"].ToString();
                                r.SubReactions.Add(new Reaction(id, account, datetime, context));
                            }
                        }
                    }
                    return returnReactions;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static Article GetArticle(string id)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    Article returnArticle = new Article();
                    OracleCommand commandSelect = CreateOracleCommand(connection, "Select * from Artikel LEFT JOIN ACCOUNT ON Artikel.Auteur = Account.Gebruikersnaam WHERE TITEL = :title");
                    commandSelect.Parameters.Add(":title", id);
                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    if (MainReader.HasRows == false)
                    {
                        return null;
                    }
                    while (MainReader.Read())
                    {
                        Account account = new Account();
                        int articleId = Convert.ToInt32(MainReader["ID"].ToString());
                        string title = MainReader["Titel"].ToString();
                        DateTime date = Convert.ToDateTime(MainReader["Datum"].ToString());
                        string context = MainReader["Inhoud"].ToString();
                        account.UserName = MainReader["Gebruikersnaam"].ToString();
                        account.FirstName = MainReader["Voornaam"].ToString();
                        account.LastName = MainReader["Achternaam"].ToString();
                        account.Education = MainReader["Opleidingsniveau"].ToString();
                        account.Function = MainReader["Functie"].ToString();
                        returnArticle = new Article(articleId, title, account, date, context);
                    }
                    return returnArticle;
                }
                catch (OracleException)
                {
                    return null;
                    throw;
                }

            }
        }

        public static Review GetReview(int reviewid)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    Review returnReview = new Review();
                    OracleCommand selectCommand =
                        CreateOracleCommand(connection,
                            "SELECT * FROM REVIEW WHERE PRODUCT_ID =(SELECT ID FROM PRODUCT WHERE ID = :id");
                    OracleDataReader MainReader = ExecuteQuery(selectCommand);
                    if (MainReader.HasRows == false)
                    {
                        return null;
                    }
                    while (MainReader.Read())
                    {
                        int id = Convert.ToInt32(MainReader["ID"].ToString());
                        string title = MainReader["Titel"].ToString();
                        string context = MainReader["Review"].ToString();

                        returnReview =new Review(id, null, null, title, context);
                    }
                    OracleCommand selectSubCommand = CreateOracleCommand(connection,
                            "SELECT * FROM ACCOUNT WHERE GEBRUIKERSNAAM = (SELECT AUTEUR FROM REVIEW WHERE ID =:id");
                        selectSubCommand.Parameters.Add(":id", returnReview.ID);

                        OracleDataReader SubReader = ExecuteQuery(selectSubCommand);
                        Account account = new Account();
                        while (SubReader.Read())
                        {
                            account.UserName = MainReader["Gebruikersnaam"].ToString();
                            account.FirstName = MainReader["Voornaam"].ToString();
                            account.LastName = MainReader["Achternaam"].ToString();
                            account.Education = MainReader["Opleidingsniveau"].ToString();
                            account.Function = MainReader["Functie"].ToString();
                        }
                    return returnReview;
                }
                catch (OracleException)
                {
                    throw;
                }
            }
        }

        public static Product GetProduct(int id)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    Product returnProduct = new Product();
                    OracleCommand commandSelect = CreateOracleCommand(connection,
                        "Select * from Product WHERE ID = :id");
                    commandSelect.Parameters.Add(":id", id);
                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    if (MainReader.HasRows == false)
                    {
                        return null;
                        throw new Exception("Could not find product");
                    }
                    while (MainReader.Read())
                    {
                        int pid = Convert.ToInt32(MainReader["ID"].ToString());
                        string name = MainReader["Naam"].ToString();
                        string specifications = MainReader["Specificaties"].ToString();
                        double price = Convert.ToDouble(MainReader["Prijs"].ToString());
                        returnProduct = new Product(pid, name, specifications, price);

                    }
                    return returnProduct;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }
        }

        public static Review GetReview(string title)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    Review returnReview = new Review();
                    OracleCommand commandSelect = CreateOracleCommand(connection,
                        "SELECT * FROM REVIEW LEFT JOIN ACCOUNT ON REVIEW.AUTEUR = ACCOUNT.GEBRUIKERSNAAM LEFT JOIN PRODUCT ON PRODUCT.ID = REVIEW.PRODUCT_ID WHERE TITEL = :title");
                    commandSelect.Parameters.Add(":title", title);

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);
                    if (!MainReader.HasRows)
                    {
                        return null;
                    }
                    while (MainReader.Read())
                    {
                        Account account = new Account();
                        Product product = new Product();
                        int id = Convert.ToInt32(MainReader["ID"].ToString());
                        string Rtitle = MainReader["Titel"].ToString();
                        string context = MainReader["Review"].ToString();
                        account.UserName = MainReader["Gebruikersnaam"].ToString();
                        account.FirstName = MainReader["Voornaam"].ToString();
                        account.LastName = MainReader["Achternaam"].ToString();
                        account.Education = MainReader["Opleidingsniveau"].ToString();
                        account.Function = MainReader["Functie"].ToString();
                        product.ProductID = Convert.ToInt32(MainReader["PRODUCT_ID"].ToString());
                        product.Name = MainReader["Naam"].ToString();
                        product.Specifications = MainReader["Specificaties"].ToString();
                        product.Price = Convert.ToDouble(MainReader["Prijs"].ToString());
                        returnReview = new Review(id, account, product, Rtitle, context);
                    }
                    return returnReview;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }
        public static List<Review> GetReviews(Product product)
        {
            using (OracleConnection connection = Connection)
            {
                List<Review> returnReviews = new List<Review>();
                try
                {
                    OracleCommand selectCommand =
                        CreateOracleCommand(connection,
                            "SELECT * FROM REVIEW WHERE PRODUCT_ID =(SELECT ID FROM PRODUCT WHERE ID = :id)");
                    selectCommand.Parameters.Add(":id", product.ProductID);
                    OracleDataReader MainReader = ExecuteQuery(selectCommand);
                    if (MainReader.HasRows == false)
                    {
                        return null;
                        throw new Exception("Could not find reviews for this product");
                    }
                    while (MainReader.Read())
                    {
                        int id = Convert.ToInt32(MainReader["ID"].ToString());
                        string title = MainReader["Titel"].ToString();
                        string context = MainReader["Review"].ToString();

                        returnReviews.Add(new Review(id, null, product,title , context));
                    }

                    foreach (Review r in returnReviews)
                    {
                        OracleCommand selectSubCommand = CreateOracleCommand(connection,
                            "SELECT * FROM ACCOUNT WHERE GEBRUIKERSNAAM = (SELECT AUTEUR FROM REVIEW WHERE ID =:id)");
                        selectSubCommand.Parameters.Add(":id", r.ID);

                        OracleDataReader SubReader = ExecuteQuery(selectSubCommand);
                        Account account = new Account();
                        while (SubReader.Read())
                        {
                            account.UserName = SubReader["Gebruikersnaam"].ToString();
                            account.FirstName = SubReader["Voornaam"].ToString();
                            account.LastName = SubReader["Achternaam"].ToString();
                            account.Education = SubReader["Opleidingsniveau"].ToString();
                            account.Function = SubReader["Functie"].ToString();
                        }

                    }
                    return returnReviews;
                }
                catch (OracleException)
                {
                    throw;
                }
            }
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

        public static Account GetAccount(Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandSelect = CreateOracleCommand(connection,
                        "Select * from Account where Email = :email and Wachtwoord = :password");
                    commandSelect.Parameters.Add(":email", account.Email);
                    commandSelect.Parameters.Add(":password", account.Password);

                    OracleDataReader MainReader = ExecuteQuery(commandSelect);

                    if (MainReader.HasRows == false)
                    {
                        return null;
                        throw new Exception("Could not find an account using this email or password");
                    }
                    while (MainReader.Read())
                    {
                        account.UserName = MainReader["Gebruikersnaam"].ToString();
                        account.FirstName = MainReader["Voornaam"].ToString();
                        account.LastName = MainReader["Achternaam"].ToString();
                        account.Education = MainReader["Opleidingsniveau"].ToString();
                        account.Function = MainReader["Functie"].ToString();
                        account.Type = MainReader["Soort"].ToString();
                    }
                    return account;
                }
                catch (OracleException)
                {
                    throw;
                }
            }
        }

        public static
            bool DisableAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public static bool UpdateAccount(Account account)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    OracleCommand commandUpdate = CreateOracleCommand(connection,
                        "UPDATE ACCOUNT SET Voornaam= :firstName, Achternaam = :lastName, FUNCTIE = :function, OPLEIDINGSNIVEAU = :education, WACHTWOORD = :password WHERE Gebruikersnaam = :userName");
                    commandUpdate.Parameters.Add(":firstName", account.FirstName);
                    commandUpdate.Parameters.Add(":lastName", account.LastName);
                    commandUpdate.Parameters.Add(":function", account.Function);
                    commandUpdate.Parameters.Add(":education", account.Education);
                    commandUpdate.Parameters.Add(":password", account.Password);
                    commandUpdate.Parameters.Add(":userName", account.UserName);

                    return ExecuteNonQuery(commandUpdate);
                }
                catch (Exception)
                {
                    throw;
                }
            }
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

        private static bool CheckReader(OracleDataReader reader)
        {
            return reader.HasRows;
        }

        public static bool CreateAccount(Account account)
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
                    if (MainReader.HasRows == false)
                    {
                        return null;
                    }
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
                    if (MainReader.HasRows == false)
                    {
                        return null;
                        throw new Exception("Could not find products in the database.");
                    }
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

        internal static List<Article> GetCatArticles(int id)
        {
            throw new NotImplementedException();
        }
    }
}