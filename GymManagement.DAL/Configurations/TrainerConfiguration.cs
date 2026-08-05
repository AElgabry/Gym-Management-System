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
	public class TrainerConfiguration : UserConfiguration<Trainer> , IEntityTypeConfiguration<Trainer>
	{
		public new void Configure(EntityTypeBuilder<Trainer> builder)
		{
			builder.Property(p => p.CreatedAt).HasColumnName("HireDate").HasDefaultValueSql("GETDATE()");
			base.Configure(builder);
		}
	}
}
