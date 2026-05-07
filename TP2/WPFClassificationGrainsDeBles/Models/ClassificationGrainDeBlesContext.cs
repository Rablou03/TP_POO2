using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WPFClassificationGrainsDeBles.Models
{
    internal class ClassificationGrainDeBlesContext : DbContext
    {
        public DbSet<Models.Donnee> donnees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string connection_string = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            string database_name = "GrainsDB" +
                ""; 
            dbContextOptionsBuilder.UseSqlServer($"{connection_string}; " +
                $"Database ={database_name};"); }

    }
}
