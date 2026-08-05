using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
	public class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
	{
		public void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);
			builder.Property(p => p.Email).HasColumnType("varchar").HasMaxLength(100);
			builder.HasIndex(i => i.Name).IsUnique();
			builder.HasIndex(i => i.Phone).IsUnique();
			builder.OwnsOne<Address>(a => a.Address, address =>
			{
				address.Property(p => p.Street).HasColumnType("varchar").HasMaxLength(30);
				address.Property(p => p.City).HasColumnType("varchar").HasMaxLength(30);
			});

		}
	}
}
