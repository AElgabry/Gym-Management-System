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
	internal class MemberPlanConfiguration : IEntityTypeConfiguration<MemberPlan>
	{
		public void Configure(EntityTypeBuilder<MemberPlan> builder)
		{
			builder.HasOne(m => m.Member).WithMany(p => p.MemberPlan).HasForeignKey(f => f.MemberID);
			builder.HasOne(p => p.Plan).WithMany(p => p.PlanMember).HasForeignKey(f => f.PlanID);
			builder.HasKey(k => k.ID);
			builder.Property(p => p.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");
		}
	}
}
