using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BBL.EntityFrameworkCore
{
    public static class BBLDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<BblDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<BblDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
