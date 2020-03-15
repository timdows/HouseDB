using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Core.Entities
{
	public class SqlBase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime? DateTimeDeleted { get; set; }
		public int? DeletedByIdentityServerUserId { get; set; }
	}
}
