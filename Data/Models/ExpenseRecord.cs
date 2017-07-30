using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Data.Models
{
	public class ExpenseRecord : SqlBase
    {
		public ExpenseType ExpenseType { get; set; }
		public DateTime Date { get; set; } = DateTime.MinValue;
		public decimal Amount { get; set; }

		[ForeignKey("ExpenseType")]
		public long ExpenseTypeID { get; set; }

		[NotMapped]
		public int Year
		{
			get
			{
				return Date.Year;
			}
			set
			{
				if (Date == DateTime.MinValue)
				{
					Date = new DateTime(value, DateTime.Today.Month, 1);
				}
				else
				{
					Date = new DateTime(value, Date.Month, 1);
				}
			}
		}

		[NotMapped]
		public int Month
		{
			get
			{
				return Date.Month;
			}
			set
			{
				if (Date == DateTime.MinValue)
				{
					Date = new DateTime(DateTime.Today.Year, value, 1);
				}
				else
				{
					Date = new DateTime(Date.Year, value, 1);
				}
			}
		}
	}
}
