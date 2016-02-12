using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace Repeat
{
    public static class Storage
    {
        //static List<string> items = new List<string> { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };

        private static Db db = new Db();

        public static void AddItem(Note item)
        {
            if (item != null)
            {
                
                List<SqliteParameter> parameters = new List<SqliteParameter>() {
                    new SqliteParameter("@name", item.Name),
                    new SqliteParameter("@content", item.Content)
                };
                db.InsertQuery("insert into notes(Name, Content) values(@name, @content)", parameters);
                //items.Add(item);
            }
        }

        public static List<Note> GetItems()
        {
            return db.SelectQuery("select * from notes;");
        }
    }
}