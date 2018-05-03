using Microsoft.EntityFrameworkCore;
using ReferatsDownloader.Models;
using System.Configuration;

namespace ReferatsDownloader.DAL
{
    public class ReferatsContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Referat> Referats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["ReferatsDatabase"].ConnectionString;
            //var connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Database=ReferatsDB;AttachDbFilename=D:\DATA\Alexzander\db\ReferatsDB.mdf;Integrated Security=True;Connect Timeout=30";
            var connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Database=ReferatsDB;AttachDbFilename=E:\PROJECTS\Seredin\ReferatsDownloader2\ReferatsDownloader\db\ReferatsDB.mdf;Integrated Security=True;Connect Timeout=30";
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
