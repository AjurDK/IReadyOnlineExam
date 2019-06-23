using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.DAL
{
   public class DbEntities : System.Data.Entity.DbContext
    {
        // If you wish to target a different database and/or database provider, modify the 'ModelCodFirst' 
        // connection string in the application configuration file.
        public DbEntities()
            : base("name=DefaultConnection")
        {
            this.Configuration.ProxyCreationEnabled = false;
            //this.Database.Connection.ConnectionString = DatabaseConfig.ConnectionString;
        }

        public DbEntities(string ConnectionString)
        {
            this.Database.Connection.ConnectionString = ConnectionString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.Entities.Quiz> Quizs { get; set; }
        public virtual DbSet<Model.Entities.Question> Questions { get; set; }
        public virtual DbSet<Model.Entities.Answer> Answers { get; set; }
        public virtual DbSet<Model.Entities.User> Users { get; set; }
        public virtual DbSet<Model.Entities.QuizLog> QuizLogs { get; set; }
    }
}
