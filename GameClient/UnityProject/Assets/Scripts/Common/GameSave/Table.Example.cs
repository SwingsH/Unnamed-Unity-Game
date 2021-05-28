﻿
using TIZSoft.Database.Attributes;
using Dapper.Contrib.Extensions;

namespace TIZSoft.Database
{
	[Table("table_example")]
	partial class TableExample
	{
		[PrimaryKey]
		public string 	owner 	{ get; set; }
		public string 	name 	{ get; set; }
		public long 	amount 	{ get; set; }
	}
}
