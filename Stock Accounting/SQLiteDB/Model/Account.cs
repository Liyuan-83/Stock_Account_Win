﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;

namespace MySQLiteDB.Model
{
    public class Account : _DefaultModel
    {
        public static new string TABLE_NAME = "account";

        public string Name { get; set; }
        public int Assets { get; set; }

        public int Cash { get; set; }
        
        public double Fee { get; set; }

        public int StockValue { get; set; }

        public override String TableName() => TABLE_NAME;

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, assets INTEGER, cash INTEGER, fee REAL, stock_value INTEGER)";
        }

        public override string InsertValue()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES (null, '" + Name + "','" + Assets + "','" + Cash + "','" + Fee + "','" + StockValue + "');";
        }

        public override string EditValue()
        {
            return "UPDATE " + TABLE_NAME + " SET name = '" + Name + "', assets = " + Assets + ",cash = " + Cash + ",fee = " + Fee + ",stock_value = " + StockValue + " WHERE id = " + ID;
        }

        public override void GetValue(SQLiteDataReader reader)
        {
            ID = Int32.Parse(reader["id"].ToString());
            Name = reader["name"].ToString();
            Assets = Int32.Parse(reader["assets"].ToString());
            Cash = Int32.Parse(reader["cash"].ToString());
            Fee = Double.Parse(reader["fee"].ToString());
            StockValue = Int32.Parse(reader["stock_value"].ToString());
        }
    }
}
