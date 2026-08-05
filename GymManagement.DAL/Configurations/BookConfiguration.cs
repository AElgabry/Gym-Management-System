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
	internal class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.Ignore(x => x.ID);
			builder.HasOne(m => m.Member).WithMany(s => s.Sessions).HasForeignKey(f => f.MemberID);
			builder.HasOne(s => s.Session).WithMany(s => s.SessionMembers).HasForeignKey(f => f.SeesionID);
			builder.HasKey(x => new{x.MemberID, x.SeesionID});
		}
	}
}
