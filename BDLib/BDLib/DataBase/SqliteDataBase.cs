using System;
using System.IO;
using BDLib.Text;
using System.Data.SQLite;
using BDLib.BDLibInfo;
namespace BDLib.DataBase
{
    public class SQLite
    {
        public static bool Create(string Path)
        {
            if (!Info.Moduls.Contains("DataBase/SqliteDataBase.cs"))
                Info.Moduls.Add("DataBase/SqliteDataBase.cs");

            if (StringHelpers.IsWhiteSpaceOrNull(Path))
            {
                throw new ArgumentNullException("$Path can't be white space or null");
            }
            if(!Path.Contains(".db"))
            {
                Path += ".db";
            }

            if(File.Exists(Path))
            {
                return true;
            }


            try
            {
                SQLiteConnection.CreateFile(Path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //external
        private string filename = "";
        private string ConnectionString = "";

        //internal
        private SQLiteConnection CON;
        private SQLiteCommand CMD;
        private SQLiteDataReader Reader;
        private bool Connected;
        

        public SQLiteDataReader ReadReturn
        {
            get { if (Connected) return Reader; else throw new InvalidOperationException("there must be a connection to read from first"); } }


        public SQLite(string Path)
        {
            if (!Info.Moduls.Contains("DataBase/SqliteDataBase.cs"))
                Info.Moduls.Add("DataBase/SqliteDataBase.cs");

            if (StringHelpers.IsWhiteSpaceOrNull(Path))
            {
                throw new ArgumentNullException("$Path can't be white space or null");
            }
            if (!Path.Contains(".db"))
            {
                Path += ".db";
            }
            if (Create(Path))//looks if file exists if not create new
            {
                filename = Path;
            }
            else throw new Exception("i dont know what happed but something fucked up badly");

            ConnectionString = $"Filename={filename};Version=3;New=False;Compress=True;";
            Connected = false;
        }


        public void ExecuteNoReturn(string Query)
        {
            if (!Connected)
            {
                using (SQLiteConnection con = new SQLiteConnection(ConnectionString))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(Query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            else throw new InvalidOperationException("connection must be closed via 'CloseReturn()'");
        }
        public void ExecuteWithReturn(string Query)
        {
            CloseReturn();
            CON = new SQLiteConnection(ConnectionString);
            CMD = new SQLiteCommand(Query, CON);
            Reader = CMD.ExecuteReader();
            Connected = true;
        }

        public void CloseReturn()
        {
            if(Connected)
            {
                Reader.Dispose();//close reader
                Reader = null;
                
                CMD.Dispose();//close command
                CMD = null;

                CON.Close();
                CON.Dispose();//close Connection
                CON = null;

                Connected = false;
            }
        }

        #region Get/Set

        public string FilePath
        {
            get { return filename; }
        }
        public bool IsConnected
        {
            get { return Connected; }
        }

        #endregion
    }
}