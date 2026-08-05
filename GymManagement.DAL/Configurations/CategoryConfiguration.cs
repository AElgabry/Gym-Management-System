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
	public class CategoryConfiguration :IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.Property(p => p.CategoryName).HasColumnType("varchar").HasMaxLength(20);
			builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
			builder.HasData(
			   new Category { ID = 1, CategoryName = "Cardio" },
					new Category { ID = 2, CategoryName = "Strength" },
					new Category { ID = 3, CategoryName = "Yoga" },
					new Category { ID = 4, CategoryName = "Boxing" },
					new Category { ID = 5, CategoryName = "Nutrition" }
				   );
		}
	}
}
