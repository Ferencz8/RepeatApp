using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;

namespace Repeat.DAL
{
    public class Db
    {
        private string dbConn = "Data Source= D:\\Db_Data\\sqlLiteDb.s3db";
        public DataTable SelectQuery(string query)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbConn))
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();  //Initiate connection to the db
                    cmd.CommandText = query;  //set the passed query


                    //DataTable dt = new DataTable();
                    SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
                    ad.Fill(dt); //fill the datasource
                    return dt;
                }
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
                return null;
            }
        }

        public int InsertQuery(string query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbConn))
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                conn.Open();  //Initiate connection to the db
                cmd.CommandText = query;  //set the passed query
                int result = cmd.ExecuteNonQuery();
                return result;
            }
        }


        public static void Main()
        {
            var dataObj = new DataClass();
            //dataObj.InsertQuery("INSERT INTO Notebooks VALUES(3, 'third')");
            var dt = dataObj.SelectQuery("SELECT * FROM Notebooks");

            foreach (DataRow myRow in dt.Rows)
            {
                myRow.ItemArray.ToList().ForEach(x =>
                    Console.WriteLine(x.ToString())
                );
            }
            Console.ReadKey();
        }
    }
}
