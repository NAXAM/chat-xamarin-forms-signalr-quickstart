using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
namespace Inx.Data
{
    public class InxDbContextFactory : IDesignTimeDbContextFactory<InxDbContext>
    {
        public InxDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InxDbContext>();
            builder.UseSqlite("Data Source=inx.db;");
            //builder.UseSqlServer("Server=tcp:naxam-asia.database.windows.net,1433;Initial Catalog=Inx;Persist Security Info=False;User ID=nxadmin;Password=Shar#Nx2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new InxDbContext(builder.Options);
        }
    }
}
