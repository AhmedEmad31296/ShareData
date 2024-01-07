using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ShareData.Authorization.Roles;
using ShareData.Authorization.Users;
using ShareData.MultiTenancy;
using ShareData.DataForm;
using ShareData.WorkFlowManagemet;

namespace ShareData.EntityFrameworkCore
{
    public class ShareDataDbContext : AbpZeroDbContext<Tenant, Role, User, ShareDataDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public ShareDataDbContext(DbContextOptions<ShareDataDbContext> options)
            : base(options)
        {
        }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormStage> FormStages { get; set; }
        public DbSet<FormStageAttachment> FormStageAttachments { get; set; }
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<WorkFlowStage> WorkFlowStages { get; set; }
        public DbSet<WorkFlowStageStatus> WorkFlowStageStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkFlowStageStatus>()
                .HasOne(status => status.WorkFlowStage)
                .WithMany(stage => stage.WorkFlowStageStatus)
                .HasForeignKey(status => status.WorkFlowStageId);
        }

    }
}
