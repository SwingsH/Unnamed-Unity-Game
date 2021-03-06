﻿using System;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace TIZSoft.Database.MySQL
{
	
	public class MySQLTableMapper
	{
		public Type type;
		public string name;
		public TableRowMapper[] rows;
		
		protected string mySQLString			= string.Empty;
		protected string mySQLString_Prefixed 	= string.Empty;
		
		public MySQLTableMapper(Type _type, string _name, int rowCount)
		{
			type = _type;
			name = _name;
			rows = new TableRowMapper[rowCount];
		}

		public string RowsToMySQLInsertString
		{
			get
			{
				string tableParameters = string.Empty;
			
				foreach (TableRowMapper row in rows)
				{
					tableParameters += row.ToMySQLString;
					tableParameters += " NOT NULL";
				
					if (row != rows.Last())
						tableParameters += ",";
				}
				
				return tableParameters;
			}
		}
		
		public string RowsToMySQLString(string prefix= "")
		{
			string convertedString;
			
			if (!String.IsNullOrWhiteSpace(prefix))
				convertedString = mySQLString_Prefixed;
			else
				convertedString = mySQLString;
						
			if (String.IsNullOrWhiteSpace(convertedString))
			{
				foreach (TableRowMapper row in rows)
				{
					if (!String.IsNullOrWhiteSpace(prefix))
						convertedString += prefix + row.name;
					else
						convertedString += "`" + row.name + "`";
			
					if (row != rows.Last())
						convertedString += ",";
				}
				
				if (!String.IsNullOrWhiteSpace(prefix))
					mySQLString_Prefixed = convertedString;
				else
					mySQLString = convertedString;
			}
			return convertedString;
		}
		
		public MySqlParameter[] RowsToMySQLParameters
		{
			get
			{
				MySqlParameter[] parameters = new MySqlParameter[rows.Length];
				
				for (int i = 0; i < rows.Length; i++)
					parameters[i] = new MySqlParameter("@"+rows[i].name, rows[i].value);
				
				return parameters;
				
			}
		}
		
		public bool HasPrimaryKey
		{
			get
			{
				foreach (TableRowMapper row in rows)
					if (row.primary) return true;
				return false;
			}
		}
		
		public string GetPrimaryKey
		{
			get
			{
				foreach (TableRowMapper row in rows)
					if (row.primary) return row.name;
				return string.Empty;
			}
		}
		
		public void UpdateValue(string rowname, object obj)
		{
			foreach (TableRowMapper row in rows)
				if (row.name == rowname)
					row.value = obj;
		}
		
		public void UpdateValues(object obj)
		{
			PropertyInfo[] info;
			info = obj.GetType().GetProperties();
			
			for (int i = 0; i < info.Length; i++)
				rows[i].value = info[i].GetValue(obj);
		}
		
		public T ToType<T>()
		{
			T result = (T)Activator.CreateInstance(typeof(T));
		
			PropertyInfo[] pInfo0;
			pInfo0 = result.GetType().GetProperties();
			
			for (int i = 0; i < pInfo0.Length; i++)
				pInfo0[i].SetValue(result, rows[i].value);
				
			return result;
		}		
	}	
}