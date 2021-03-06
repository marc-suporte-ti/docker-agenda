// <auto-generated />
using System;
using DockerAgenda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DockerAgenda.Migrations
{
    [DbContext(typeof(DockerAgendaContext))]
    [Migration("20210812104422_AdicionandoDescricaoItemContato")]
    partial class AdicionandoDescricaoItemContato
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DockerAgenda.Entity.AgendaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomeResponsavel")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Agendas");
                });

            modelBuilder.Entity("DockerAgenda.Entity.ContatoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgendaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoContato")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgendaId");

                    b.ToTable("Contatos");
                });

            modelBuilder.Entity("DockerAgenda.Entity.ItemContatoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContatoEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Observacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Registro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoItemContato")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContatoEntityId");

                    b.ToTable("ItensContatos");
                });

            modelBuilder.Entity("DockerAgenda.Entity.ContatoEntity", b =>
                {
                    b.HasOne("DockerAgenda.Entity.AgendaEntity", "Agenda")
                        .WithMany("Contatos")
                        .HasForeignKey("AgendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agenda");
                });

            modelBuilder.Entity("DockerAgenda.Entity.ItemContatoEntity", b =>
                {
                    b.HasOne("DockerAgenda.Entity.ContatoEntity", "Contato")
                        .WithMany("ItensContato")
                        .HasForeignKey("ContatoEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contato");
                });

            modelBuilder.Entity("DockerAgenda.Entity.AgendaEntity", b =>
                {
                    b.Navigation("Contatos");
                });

            modelBuilder.Entity("DockerAgenda.Entity.ContatoEntity", b =>
                {
                    b.Navigation("ItensContato");
                });
#pragma warning restore 612, 618
        }
    }
}
