using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using System.IO;
using System.Net;
using System.Data;
using Windows.UI.Xaml.Shapes;
using System.Threading;
using Path = System.IO.Path;
using Windows.Media.Protection.PlayReady;
using System.Runtime.CompilerServices;

namespace Hello_World
{
    public static class DataAccess
    {
        public static string Dmail;
        public static string Dpassword;
        public static DateTime Dtime;
        public static int antiForceBrute = 10;
        public static int longueur = 15;
        public static int tentativesEchoue = 0;
        public static int tentativep = 5;
        public static class Clients
        {
            public static void Initialize()
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String tableCommand = "CREATE TABLE IF NOT EXISTS Clients (Name varchar(255), typeDeClient varchar(255)); ";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                    SqliteCommand selectAdmin = new SqliteCommand("SELECT * FROM Clients", db);

                    createTable.ExecuteReader();
                    SqliteDataReader query = selectAdmin.ExecuteReader();
                    query.Read();
                    try
                    {
                        query.GetString(0);
                    }
                    catch
                    {
                        SqliteCommand createcl1 = new SqliteCommand("INSERT INTO Clients VALUES ('Paul','affaire');", db);
                        SqliteCommand createcl2 = new SqliteCommand("INSERT INTO Clients VALUES ('Jules','affaire');", db);
                        SqliteCommand createcl3 = new SqliteCommand("INSERT INTO Clients VALUES ('Pierre','resid');", db);
                        SqliteCommand createcl4 = new SqliteCommand("INSERT INTO Clients VALUES ('Alex','resid');", db);

                        createcl1.ExecuteReader();
                        createcl2.ExecuteReader();
                        createcl3.ExecuteReader();
                        createcl4.ExecuteReader();
                    }

                }
            }

            public static void AddData(string Nom, string typeDeClient, string mail, string password)
            {
                List<string> tmp = DataAccess.Users.connexion(mail, password);
                if (tmp[0] == "bonMotDePass" & (tmp[1] == typeDeClient | tmp[1] == "admin"))
                {
                    string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                    using (SqliteConnection db =
                      new SqliteConnection($"Filename={dbpath}"))
                    {
                        db.Open();

                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = db;

                        // Use parameterized query to prevent SQL injection attacks
                        insertCommand.CommandText = "INSERT INTO Clients VALUES ('" + Nom + "','" + typeDeClient + "');";

                        insertCommand.ExecuteReader();
                    }
                }
            }

            public static List<String> GetData(string typeOfClient)
            {
                List<String> entries = new List<string>();

                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT Name from Clients WHERE typeDeClient='" + typeOfClient + "';", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();


                    while (query.Read())
                    {
                        entries.Add(query.GetString(0));
                    }
                }

                return entries;
            }

            public static void DeleteData(string nom ,string typeDeClient, string mail, string password)
            {
                List<string> tmp = DataAccess.Users.connexion(mail, password);
                if (tmp[0] == "bonMotDePass" & (tmp[1]==typeDeClient | tmp[1]=="admin"))
                {
                    string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                    using (SqliteConnection db =
                      new SqliteConnection($"Filename={dbpath}"))
                    {
                        db.Open();

                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = db;

                        // Use parameterized query to prevent SQL injection attacks
                        insertCommand.CommandText = "DELETE FROM Clients WHERE Name='" + nom + "' AND typeDeClient='"+ typeDeClient+"'; ";

                        insertCommand.ExecuteReader();
                    }
                }
            }
        }

        public static class Users
        {
            public static void Initialize()
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String tableCommand = "CREATE TABLE IF NOT EXISTS User (Mail varchar(255), Password varchar(255), role varchar(255), salt INT); ";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                    SqliteCommand selectAdmin = new SqliteCommand("SELECT * FROM User", db);

                    createTable.ExecuteReader();
                    SqliteDataReader query = selectAdmin.ExecuteReader();
                    query.Read();
                    try{ 
                        query.GetString(0);
                    }
                    catch
                    {
                        Random rnd = new Random();
                        int salt = rnd.Next();
                        SqliteCommand createAdmin = new SqliteCommand("INSERT INTO User VALUES ('admin','" + Chiffrement.Hash("admin", salt, 20) + "','" + Chiffrement.Hash("admin", salt, 20) + "','"+salt+"');", db);
                        salt = rnd.Next();
                        SqliteCommand creatUser1 = new SqliteCommand("INSERT INTO User VALUES ('user1','" + Chiffrement.Hash("password", salt, 20) + "','" + Chiffrement.Hash("resid", salt, 20) + "','"+salt+"');", db);
                        salt = rnd.Next();
                        SqliteCommand creatUser2 = new SqliteCommand("INSERT INTO User VALUES ('user2','" + Chiffrement.Hash("password", salt, 20) + "','" + Chiffrement.Hash("affaire", salt, 20) + "','"+ salt+"');", db);

                        createAdmin.ExecuteReader();
                        creatUser1.ExecuteReader();
                        creatUser2.ExecuteReader();
                    }
                }
            }

            public static List<string> connexion(string username, string password)
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                string role = null;
                string statut = null;

                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectSalt = new SqliteCommand("SELECT salt FROM User WHERE Mail='"+username+"';", db);
                    SqliteDataReader querySalt = selectSalt.ExecuteReader();
                    querySalt.Read();
                    int salt;
                    try
                    {
                        salt = int.Parse(querySalt.GetString(0));
                    }
                    catch
                    {
                        return new List<string> { "existPas", null };
                    }

                    SqliteCommand selectPassword = new SqliteCommand
                        ("SELECT Password from User WHERE Mail='" + username + "';", db);

                    SqliteDataReader query = selectPassword.ExecuteReader();

                    try
                    {
                        query.Read();
                        string passwordDB = query.GetString(0);

                        if (passwordDB == Chiffrement.Hash(password,salt,20))
                        {
                            SqliteCommand selectRole = new SqliteCommand
                            ("SELECT role from User WHERE Mail='" + username + "';", db);

                            SqliteDataReader query2 = selectRole.ExecuteReader();

                            query2.Read();
                            role = query2.GetString(0);

                            statut = "bonMotDePass";
                        }
                        else
                        {
                            statut = "mauvaisMotDePass";
                        }
                        if (role == Chiffrement.Hash("admin", salt, 20))
                        {
                            role = "admin";
                        }
                        if (role == Chiffrement.Hash("resid", salt, 20))
                        {
                            role = "resid";
                        }
                        if (role == Chiffrement.Hash("affaire", salt, 20))
                        {
                            role = "affaire";
                        }
                    }
                    catch
                    {
                        statut = "existPas";
                    }
                }
                return new List<string> { statut, role };

            }

            public static void changePassword(string mail, string password, string newPassword)
            {
                DateTime tmp = DateTime.Now;
                TimeSpan ts = tmp - DataAccess.Dtime;
                if (ts.TotalSeconds < DataAccess.longueur)
                {
                    List<string> tmp2 = DataAccess.Users.connexion(mail,password);
                    if (tmp2[0] == "bonMotDePass")
                    {
                        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                        {
                            db.Open();

                            SqliteCommand selectSalt = new SqliteCommand("SELECT salt FROM User WHERE Mail='" + mail + "';", db);
                            SqliteDataReader querySalt = selectSalt.ExecuteReader();
                            querySalt.Read();
                            int salt;
                            salt = int.Parse(querySalt.GetString(0));

                            SqliteCommand change = new SqliteCommand("UPDATE User SET Password ='"+Chiffrement.Hash(newPassword, salt, 20)+"' WHERE Mail='"+ mail+"';",db);

                            change.ExecuteReader();
                        }
                    }
                }
            }

            public static void addUser(string mail, string password, string role)
            {
                DateTime tmp = DateTime.Now;
                TimeSpan ts = tmp - DataAccess.Dtime;
                if (ts.TotalSeconds < DataAccess.longueur)
                {
                    string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                    using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                    {
                        db.Open();

                        SqliteCommand verif = new SqliteCommand("SELECT * FROM User WHERE Mail='"+mail+"' ;", db);
                        SqliteDataReader query = verif.ExecuteReader();

                        try
                        {
                            query.Read();
                            query.GetString(0);
                        }
                        catch
                        {
                            Random rnd = new Random();
                            int salt = rnd.Next();
                            SqliteCommand createUser = new SqliteCommand("INSERT INTO User VALUES ('" + mail + "','" + Chiffrement.Hash(password, salt, 20) + "','" + Chiffrement.Hash(role, salt, 20) + "','" + salt + "');", db);
                            DataAccess.Logs.AddLog("utilisateur ajouter");

                            createUser.ExecuteReader();
                        }
                    }
                }
            }

            public static void delUser(string mail)
            {
                DateTime tmp = DateTime.Now;
                TimeSpan ts = tmp - DataAccess.Dtime;
                if (ts.TotalSeconds < DataAccess.longueur)
                {
                    string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                    using (SqliteConnection db =
                      new SqliteConnection($"Filename={dbpath}"))
                    {
                        db.Open();

                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = db;

                        // Use parameterized query to prevent SQL injection attacks
                        insertCommand.CommandText = "DELETE FROM User WHERE Mail='" + mail + "'; ";

                        insertCommand.ExecuteReader();

                        DataAccess.Logs.AddLog("suprimer user");
                    }
                }
            }


            public static List<string> getUser()
            {
                List<string> result = new List<string>();

                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand select = new SqliteCommand("SELECT Mail, role, salt FROM User ;", db);
                    SqliteDataReader query = select.ExecuteReader();

                    while (query.Read())
                    {
                        if(Chiffrement.Hash("admin",int.Parse(query.GetString(2)),20) == query.GetString(1))
                        {
                            result.Add(query.GetString(0) + ": admin");
                        }
                        else if (Chiffrement.Hash("affaire", int.Parse(query.GetString(2)), 20) == query.GetString(1))
                        {
                            result.Add(query.GetString(0) + ": affaire");
                        }
                        if (Chiffrement.Hash("resid", int.Parse(query.GetString(2)), 20) == query.GetString(1))
                        {
                            result.Add(query.GetString(0) + ": résidentiel");
                        }
                    }
                }

                return result;

            }
        }

        public static class Logs
        {
            public static void Initialize()
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String tableCommand = "CREATE TABLE IF NOT EXISTS Logs (log varchar(255)); ";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                    SqliteCommand selectAdmin = new SqliteCommand("SELECT * FROM Logs", db);

                    createTable.ExecuteReader();
                    SqliteDataReader query = selectAdmin.ExecuteReader();
                    query.Read();
                    try
                    {
                        query.GetString(0);
                    }
                    catch
                    {
                        SqliteCommand firstLog = new SqliteCommand("INSERT INTO Logs VALUES ('"+DateTime.Now.ToString()+": premiere lancement des Logs');", db);

                        firstLog.ExecuteReader();
                    }
                }
            }

            public static void AddLog(string message)
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Logs VALUES ('" + DateTime.Now.ToString()+": " + message + "');";

                    insertCommand.ExecuteReader();
                }
            }

            public static List<String> GetLogs()
            {
                List<String> entries = new List<string>();

                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT Log from Logs;", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();


                    while (query.Read())
                    {
                        entries.Add(query.GetString(0));
                    }
                }

                return entries;
            }

            public static void savedata()
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("data.txt", CreationCollisionOption.OpenIfExists);
                string txtpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "data.txt");

                StreamWriter sw = new StreamWriter(txtpath);
                sw.WriteLine(DataAccess.longueur);
                sw.WriteLine(DataAccess.antiForceBrute);
                sw.WriteLine(DataAccess.tentativep);
                sw.Close();
            }

            public static void getData()
            {
                ApplicationData.Current.LocalFolder.CreateFileAsync("data.txt", CreationCollisionOption.OpenIfExists);
                string txtpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "data.txt");


                try
                {
                    StreamReader sw = new StreamReader(txtpath);
                    DataAccess.longueur = int.Parse(sw.ReadLine());
                    DataAccess.antiForceBrute = int.Parse(sw.ReadLine());
                    DataAccess.tentativep = int.Parse(sw.ReadLine());
                    sw.Close();
                }
                catch
                {

                }
            }
        }
    }
}
