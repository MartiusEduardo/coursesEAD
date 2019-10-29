using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simulacao.Models;

namespace Simulacao.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

            public DbSet<Questao> Questao { get; set; }
            public DbSet<Resposta> Resposta { get; set; }
            public DbSet<Area> Area { get; set; }
            public DbSet<Subarea> Subarea { get; set; }
            public DbSet<ApplicationUser> ApplicationUser { get; set; }
            public DbSet<MaterialEstudo> MaterialEstudo { get; set; }
            public DbSet<Posting> Posting { get; set; }
            public DbSet<Topic> Topic { get; set; }
            public DbSet<Category> Category { get; set; }
            public DbSet<Group> Group { get; set; }
            public DbSet<Course> Course { get; set; }
            public DbSet<Pagamento> Pagamento { get; set; }
            public DbSet<Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Tables
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            builder.Entity<Questao>().ToTable("questao");
            builder.Entity<Resposta>().ToTable("resposta");
            builder.Entity<Area>().ToTable("area");
            builder.Entity<Subarea>().ToTable("subarea");
            builder.Entity<MaterialEstudo>().ToTable("MaterialEstudo");
            builder.Entity<Posting>().ToTable("Posting");
            builder.Entity<Topic>().ToTable("Topic");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Group>().ToTable("Group");
            builder.Entity<Course>().ToTable("Course");
            builder.Entity<Pagamento>().ToTable("Pagamento");
            builder.Entity<Enrollment>().ToTable("Enrollment");
            
            //Keys
            builder.Entity<ApplicationUser>().HasKey(a => a.Id);
            builder.Entity<Questao>().HasKey(q => q.idQuestao);
            builder.Entity<Resposta>().HasKey(r => new {r.idResposta, r.idQuestao});
            builder.Entity<Area>().HasKey(a => a.idArea);
            builder.Entity<Subarea>().HasKey(s => new {s.idSubarea, s.idArea});
            builder.Entity<MaterialEstudo>().HasKey(m => m.idMaterialEstudo);
            builder.Entity<Posting>().HasKey(p => new {p.idPosting, p.idTopic, p.idUser});
            builder.Entity<Topic>().HasKey(t => new {t.idTopic, t.idCategory, t.idUser});
            builder.Entity<Category>().HasKey(c => new {c.idCategory, c.idGroup});
            builder.Entity<Group>().HasKey(g => g.idGroup);
            builder.Entity<Course>().HasKey(c => c.idCourse);
            builder.Entity<Pagamento>().HasKey(p => p.idPagamento);
            builder.Entity<Enrollment>().HasKey(e => e.idEnrollment);
        }
    }
    class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("server=mysql08-farm56.kinghost.net;port=3306;database=managementci01;userid=managementci01;pwd=Mci0123;sslmode=none;charset=utf8mb4;");
            //optionsBuilder.UseNpgsql("User ID=managementci;Password=Mci0123;Host=pgsql.managementci.com;Port=5432;Database=managementci;Pooling=true;");
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }
    }
}
