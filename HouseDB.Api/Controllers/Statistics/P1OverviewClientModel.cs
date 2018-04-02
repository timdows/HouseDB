using HouseDB.Api.Controllers.SevenSegment;
using HouseDB.Api.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace HouseDB.Api.Controllers.Statistics
{
	public class P1OverviewClientModel : BaseClientModel
	{
		public List<CurrentP1Item> Items { get; set; } = new List<CurrentP1Item>();

		public void Load(DataContext dataContext, IMemoryCache memoryCache)
		{
			var helperClientModel = new SevenSegmentClientModel(dataContext, memoryCache);
			helperClientModel.Load();

			Items.Add(new CurrentP1Item
			{
				Name = nameof(helperClientModel.Watt),
				Value = helperClientModel.Watt
			});
			Items.Add(new CurrentP1Item
			{
				Name = nameof(helperClientModel.Today),
				Value = helperClientModel.Today
			});
			Items.Add(new CurrentP1Item
			{
				Name = nameof(helperClientModel.ThisWeek),
				Value = helperClientModel.ThisWeek
			});
			Items.Add(new CurrentP1Item
			{
				Name = nameof(helperClientModel.ThisMonth),
				Value = helperClientModel.ThisMonth
			});
			Items.Add(new CurrentP1Item
			{
				Name = nameof(helperClientModel.LastMonth),
				Value = helperClientModel.LastMonth
			});
		}

	}

	public class CurrentP1Item
	{
		public string Name { get; set; }
		public decimal Value { get; set; }

	}
}
