﻿// <auto-generated />
using System;
using BillingManagement.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BillingManagement.Migrations
{
    [DbContext(typeof(DataAccessLayer))]
    partial class DataAccessLayerModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("BillingManagement.Model.BillInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("BillName")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("BillInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
