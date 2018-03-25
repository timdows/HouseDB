using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HouseDB.Api.Controllers.Expenses
{
	public class ExprensesController : HouseDBController
	{
		public ExprensesController(DataContext dataContext) : base(dataContext)
		{
		}

		public JsonResult AddExpense([FromBody] ExpenseRecord expenseRecord)
		{
			// Check if expense type exists
			if (_dataContext.ExpenseTypes.Single(a_item => a_item.ID == expenseRecord.ExpenseTypeID) == null)
			{
				throw new ArgumentException($"ExpenseType {expenseRecord.ExpenseTypeID} not found");
			}

			_dataContext.ExpenseRecords.Add(expenseRecord);
			_dataContext.SaveChanges();

			return Json(expenseRecord);
		}
	}
}
