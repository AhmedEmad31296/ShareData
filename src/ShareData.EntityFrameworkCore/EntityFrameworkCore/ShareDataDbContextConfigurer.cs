using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ShareData.EntityFrameworkCore
{
    public static class ShareDataDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ShareDataDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ShareDataDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
