﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Simulacao.Data;
using System;

namespace Simulacao.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180904003629_Pagamentos")]
    partial class Pagamentos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Simulacao.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address")
                        .HasMaxLength(200);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("DateOfBirth")
                        .HasMaxLength(10);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Identity")
                        .HasMaxLength(10);

                    b.Property<string>("Location")
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfessionalSummary");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<byte[]>("UserPhoto");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Simulacao.Models.Area", b =>
                {
                    b.Property<int>("idArea")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idarea")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("About")
                        .IsRequired()
                        .HasColumnName("about");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnName("image");

                    b.Property<string>("NomeArea")
                        .IsRequired()
                        .HasColumnName("area");

                    b.Property<string>("ProfessorName")
                        .IsRequired()
                        .HasColumnName("professorname")
                        .HasMaxLength(200);

                    b.HasKey("idArea");

                    b.ToTable("area");
                });

            modelBuilder.Entity("Simulacao.Models.Category", b =>
                {
                    b.Property<int>("idCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idcategory")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idGroup")
                        .HasColumnName("idgroup");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("idCategory", "idGroup");

                    b.HasAlternateKey("idCategory");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Simulacao.Models.Group", b =>
                {
                    b.Property<int>("idGroup")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idgroup")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("idGroup");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Simulacao.Models.MaterialEstudo", b =>
                {
                    b.Property<long>("idMaterialEstudo")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idmaterialestudo")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DescricaoConteudo")
                        .HasColumnName("descricaoconteudo");

                    b.Property<string>("Titulo")
                        .HasColumnName("titulo");

                    b.Property<int>("idArea")
                        .HasColumnName("idarea");

                    b.Property<int>("idSubarea")
                        .HasColumnName("idsubarea");

                    b.HasKey("idMaterialEstudo");

                    b.ToTable("MaterialEstudo");
                });

            modelBuilder.Entity("Simulacao.Models.Posting", b =>
                {
                    b.Property<int>("idPosting")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idposting")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idTopic")
                        .HasColumnName("idtopic");

                    b.Property<string>("idUser")
                        .HasColumnName("iduser");

                    b.Property<DateTime>("DatePublication")
                        .HasColumnName("datepublication");

                    b.Property<string>("Message")
                        .HasColumnName("message");

                    b.HasKey("idPosting", "idTopic", "idUser");

                    b.HasAlternateKey("idPosting");

                    b.ToTable("Posting");
                });

            modelBuilder.Entity("Simulacao.Models.Questao", b =>
                {
                    b.Property<long>("idQuestao")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idquestao");

                    b.Property<string>("Pergunta")
                        .IsRequired()
                        .HasColumnName("questao");

                    b.Property<int>("idArea")
                        .HasColumnName("idarea");

                    b.Property<int>("idSubarea")
                        .HasColumnName("idsubarea");

                    b.HasKey("idQuestao");

                    b.ToTable("questao");
                });

            modelBuilder.Entity("Simulacao.Models.Resposta", b =>
                {
                    b.Property<long>("idResposta")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idresposta")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("idQuestao")
                        .HasColumnName("idquestao");

                    b.Property<bool>("Correta")
                        .HasColumnName("correta");

                    b.Property<string>("Explicacao")
                        .HasColumnName("explicacao");

                    b.Property<string>("Respostas")
                        .IsRequired()
                        .HasColumnName("resposta");

                    b.HasKey("idResposta", "idQuestao");

                    b.HasAlternateKey("idResposta");

                    b.ToTable("resposta");
                });

            modelBuilder.Entity("Simulacao.Models.Subarea", b =>
                {
                    b.Property<int>("idSubarea")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idsubarea")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idArea")
                        .HasColumnName("idarea");

                    b.Property<string>("NomeSubarea")
                        .IsRequired()
                        .HasColumnName("subarea");

                    b.HasKey("idSubarea", "idArea");

                    b.HasAlternateKey("idSubarea");

                    b.ToTable("subarea");
                });

            modelBuilder.Entity("Simulacao.Models.Topic", b =>
                {
                    b.Property<int>("idTopic")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idtopic")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("idCategory")
                        .HasColumnName("idcategory");

                    b.Property<string>("idUser")
                        .HasColumnName("iduser");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnName("dateregister");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("idTopic", "idCategory", "idUser");

                    b.HasAlternateKey("idTopic");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Simulacao.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Simulacao.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Simulacao.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Simulacao.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
