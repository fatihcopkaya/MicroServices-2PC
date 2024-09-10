﻿// <auto-generated />
using System;
using Cordinator.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cordinator.Migrations
{
    [DbContext(typeof(TwoPhaseCommitContext))]
    [Migration("20240910201929_seedData")]
    partial class seedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cordinator.Models.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Nodes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("37b30f38-38af-493d-90e1-089df554c587"),
                            Name = "Order.API"
                        },
                        new
                        {
                            Id = new Guid("ada5fe82-3a65-4824-acd7-474e0859992a"),
                            Name = "Stock.API"
                        },
                        new
                        {
                            Id = new Guid("67ad39fe-3dc2-45dd-a365-2df4476b63e8"),
                            Name = "Payment.API"
                        });
                });

            modelBuilder.Entity("Cordinator.Models.NodeState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IsReady")
                        .HasColumnType("int");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TransactionState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("NodeStates");
                });

            modelBuilder.Entity("Cordinator.Models.NodeState", b =>
                {
                    b.HasOne("Cordinator.Models.Node", "Node")
                        .WithMany("NodeStates")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("Cordinator.Models.Node", b =>
                {
                    b.Navigation("NodeStates");
                });
#pragma warning restore 612, 618
        }
    }
}
