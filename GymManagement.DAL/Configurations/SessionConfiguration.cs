using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
	public class SessionConfiguration : IEntityTypeConfiguration<Session>
	{
		public void Configure(EntityTypeBuilder<Session> builder)
		{
			builder.ToTable(t =>
			{
				t.HasCheckConstraint("CpacityCheck", "Capacity Between 1 and 25");
				t.HasCheckConstraint("EndDateCheck", "EndDate > StartDate");
			});
				builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");

			;
		}
	}
}
