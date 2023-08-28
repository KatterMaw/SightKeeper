﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SightKeeper.Data;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4");

            modelBuilder.Entity("AssetWeights", b =>
                {
                    b.Property<int>("AssetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeightsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssetId", "WeightsId");

                    b.HasIndex("WeightsId");

                    b.ToTable("WeightsAssets", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Usage")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.ToTable("Assets", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProcessName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Image", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.ToTable("ItemClasses", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Resolution")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DataSets");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemClassId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("ItemClassId");

                    b.ToTable("DetectorItems", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LibraryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("Screenshots", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasAnyScreenshots")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("MaxQuantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ScreenshotsLibraries", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Weights", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("BoundingLoss")
                        .HasColumnType("REAL");

                    b.Property<float>("ClassificationLoss")
                        .HasColumnType("REAL");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<float>("DeformationLoss")
                        .HasColumnType("REAL");

                    b.Property<uint>("Epoch")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("WeightsLibraryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WeightsLibraryId");

                    b.ToTable("Weights");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("WeightsLibraries", (string)null);
                });

            modelBuilder.Entity("AssetWeights", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Asset", null)
                        .WithMany()
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Weights", null)
                        .WithMany()
                        .HasForeignKey("WeightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithMany("Assets")
                        .HasForeignKey("DataSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Screenshot", "Screenshot")
                        .WithOne("Asset")
                        .HasForeignKey("SightKeeper.Domain.Model.Common.Asset", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");

                    b.Navigation("Screenshot");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Image", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Screenshot", null)
                        .WithOne("Image")
                        .HasForeignKey("SightKeeper.Domain.Model.Common.Image", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithMany("ItemClasses")
                        .HasForeignKey("DataSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Asset", "Asset")
                        .WithMany("Items")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Common.ItemClass", "ItemClass")
                        .WithMany("Items")
                        .HasForeignKey("ItemClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SightKeeper.Domain.Model.Detector.Bounding", "Bounding", b1 =>
                        {
                            b1.Property<int>("DetectorItemId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Bottom")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingBottom");

                            b1.Property<double>("Left")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingLeft");

                            b1.Property<double>("Right")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingRight");

                            b1.Property<double>("Top")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingTop");

                            b1.HasKey("DetectorItemId");

                            b1.ToTable("DetectorItems");

                            b1.WithOwner()
                                .HasForeignKey("DetectorItemId");
                        });

                    b.Navigation("Asset");

                    b.Navigation("Bounding")
                        .IsRequired();

                    b.Navigation("ItemClass");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.ScreenshotsLibrary", "Library")
                        .WithMany("Screenshots")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithOne("ScreenshotsLibrary")
                        .HasForeignKey("SightKeeper.Domain.Model.ScreenshotsLibrary", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Weights", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.WeightsLibrary", null)
                        .WithMany("Weights")
                        .HasForeignKey("WeightsLibraryId");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithOne("WeightsLibrary")
                        .HasForeignKey("SightKeeper.Domain.Model.WeightsLibrary", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("ItemClasses");

                    b.Navigation("ScreenshotsLibrary")
                        .IsRequired();

                    b.Navigation("WeightsLibrary")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.Navigation("Asset");

                    b.Navigation("Image")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.Navigation("Screenshots");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.Navigation("Weights");
                });
#pragma warning restore 612, 618
        }
    }
}
