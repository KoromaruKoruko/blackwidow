using System;
using System.IO;
using BDLib.Text;
using System.Data.SQLite;
using BDLib.BDLibInfo;
using System.Collections.Generic;

namespace BDLib.DataBase
{
    public class SQLite
    {
        /// <summary>
        /// creates a new DataBase At
        /// </summary>
        /// <param name="Path">File path</param>
        /// <returns>if it was Created or not</returns>
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

        /// <summary>
        /// the Return from ExecuteWithReturn(string)
        /// </summary>
        public SQLiteDataReader ReadReturn
        {
            get { if (Connected) return Reader; else throw new InvalidOperationException("there must be a connection to read from first"); }
        }

        /// <summary>
        /// constructer
        /// </summary>
        /// <param name="Path">path to DataBase File</param>
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

            if (File.Exists(Environment.CurrentDirectory + filename)) filename = Environment.CurrentDirectory + filename;

            ConnectionString = $"DataSource={filename};";
            Connected = false;
        }

        /// <summary>
        /// executes a Query without a Return
        /// </summary>
        /// <param name="Query">the SQL Query</param>
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
        /// <summary>
        /// executes a Query with a Return
        /// </summary>
        /// <param name="Query">the SQL Query</param>
        public void ExecuteWithReturn(string Query)
        {
            CloseReturn();
            CON = new SQLiteConnection(ConnectionString);
            CON.Open();//open connection
            CMD = new SQLiteCommand(Query, CON);
            Reader = CMD.ExecuteReader();
            Connected = true;
        }

        /// <summary>
        /// closes the connection
        /// </summary>
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

        /// <summary>
        /// returns a value from
        /// </summary>
        /// <typeparam name="T">the value type</typeparam>
        /// <param name="Table">table to read form</param>
        /// <param name="row">row to read from</param>
        /// <param name="ValueName">the colum name</param>
        /// <returns> a value in table at row in colum</returns>
        public T GetValueWhere<T>(string Table, string row, string ValueName)
        {
            ExecuteWithReturn($"SELECT * FROM {Table} WHERE {row}");
            T outp = (T)ReadReturn[ValueName];
            CloseReturn();
            return outp;
        }
        /// <summary>
        /// returns all the Data in a colum
        /// </summary>
        /// <param name="tabel">table to get the data from</param>
        /// <param name="colname">colum name</param>
        /// <returns>all the data in colum from table</returns>
        public string[] GetAllColomData(string tabel, string colname)
        {
            ExecuteWithReturn($"SELECT {colname} FROM {tabel}");
            List<string> rows = new List<string>();
            while(Reader.Read())
            {
                rows.Add(Reader[0].ToString());
            }
            CloseReturn();
            return rows.ToArray();
        }

        #region Get/Set

        /// <summary>
        /// path to DataBase File
        /// </summary>
        public string FilePath
        {
            get { return filename; }
        }
        /// <summary>
        /// if i am Connected to The DataBase
        /// Ie there is Data inside ReadReturn
        /// </summary>
        public bool IsConnected
        {
            get { return Connected; }
        }

        #endregion
    }
}