﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PassionLib.DAL;

#nullable disable

namespace PassionLib.Migrations
{
    [DbContext(typeof(RunsContext))]
    [Migration("20211126155415_Borrowed")]
    partial class Borrowed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PassionLib.Models.CraftEssence", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<short[]>("AttackScaling")
                        .HasColumnType("smallint[]");

                    b.Property<short?>("BaseMaxAttack")
                        .HasColumnType("smallint");

                    b.Property<string>("CnName")
                        .HasColumnType("text");

                    b.Property<int>("CollectionNo")
                        .HasColumnType("integer");

                    b.Property<string>("JpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KrName")
                        .HasColumnType("text");

                    b.Property<string>("NaName")
                        .HasColumnType("text");

                    b.Property<short?>("Rarity")
                        .HasColumnType("smallint");

                    b.Property<string>("TwName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CollectionNo")
                        .IsUnique();

                    b.ToTable("CraftEssences");
                });

            modelBuilder.Entity("PassionLib.Models.CraftEssenceAlias", b =>
                {
                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<int>("CraftEssenceId")
                        .HasColumnType("integer");

                    b.Property<int>("SubmitterId")
                        .HasColumnType("integer");

                    b.HasKey("Nickname");

                    b.HasIndex("CraftEssenceId");

                    b.HasIndex("SubmitterId");

                    b.ToTable("CraftEssenceAliases");
                });

            modelBuilder.Entity("PassionLib.Models.MysticCode", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("CnName")
                        .HasColumnType("text");

                    b.Property<string>("JpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KrName")
                        .HasColumnType("text");

                    b.Property<string>("NaName")
                        .HasColumnType("text");

                    b.Property<string>("TwName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MysticCodes");
                });

            modelBuilder.Entity("PassionLib.Models.MysticCodeAlias", b =>
                {
                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<int>("MysticCodeId")
                        .HasColumnType("integer");

                    b.Property<int>("SubmitterId")
                        .HasColumnType("integer");

                    b.HasKey("Nickname");

                    b.HasIndex("MysticCodeId");

                    b.HasIndex("SubmitterId");

                    b.ToTable("MysticCodeAliases");
                });

            modelBuilder.Entity("PassionLib.Models.Pong", b =>
                {
                    b.Property<string>("UserMention")
                        .HasColumnType("text");

                    b.HasKey("UserMention");

                    b.ToTable("Pongs");
                });

            modelBuilder.Entity("PassionLib.Models.Quest", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("CnName")
                        .HasColumnType("text");

                    b.Property<string>("JpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KrName")
                        .HasColumnType("text");

                    b.Property<string>("NaName")
                        .HasColumnType("text");

                    b.Property<string>("TwName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("PassionLib.Models.QuestAlias", b =>
                {
                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<int>("SubmitterId")
                        .HasColumnType("integer");

                    b.HasKey("Nickname");

                    b.HasIndex("QuestId");

                    b.HasIndex("SubmitterId");

                    b.ToTable("QuestAliases");
                });

            modelBuilder.Entity("PassionLib.Models.Run", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<short?>("Cost")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("CsUsed")
                        .HasColumnType("integer");

                    b.Property<int>("DpsId")
                        .HasColumnType("integer");

                    b.Property<bool?>("Failure")
                        .HasColumnType("boolean");

                    b.Property<string>("Misc")
                        .HasColumnType("text");

                    b.Property<int?>("MysticCodeId")
                        .HasColumnType("integer");

                    b.Property<bool?>("NoCe")
                        .HasColumnType("boolean");

                    b.Property<bool?>("NoCeDps")
                        .HasColumnType("boolean");

                    b.Property<bool?>("NoDupe")
                        .HasColumnType("boolean");

                    b.Property<bool?>("NoEventCeDps")
                        .HasColumnType("boolean");

                    b.Property<short?>("Phase")
                        .HasColumnType("smallint");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<int?>("RevivesUsed")
                        .HasColumnType("integer");

                    b.Property<bool?>("Rta")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("RunDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RunUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short?>("ServantCount")
                        .HasColumnType("smallint");

                    b.Property<int>("SubmitterId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DpsId");

                    b.HasIndex("MysticCodeId");

                    b.HasIndex("QuestId");

                    b.HasIndex("RunUrl")
                        .IsUnique();

                    b.HasIndex("SubmitterId");

                    b.ToTable("Runs");
                });

            modelBuilder.Entity("PassionLib.Models.Servant", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<short[]>("AttackScaling")
                        .HasColumnType("smallint[]");

                    b.Property<short?>("BaseMaxAttack")
                        .HasColumnType("smallint");

                    b.Property<string>("Class")
                        .HasColumnType("text");

                    b.Property<string>("CnName")
                        .HasColumnType("text");

                    b.Property<int>("CollectionNo")
                        .HasColumnType("integer");

                    b.Property<string>("JpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KrName")
                        .HasColumnType("text");

                    b.Property<string>("NaName")
                        .HasColumnType("text");

                    b.Property<short?>("Rarity")
                        .HasColumnType("smallint");

                    b.Property<string>("TwName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CollectionNo")
                        .IsUnique();

                    b.ToTable("Servants");
                });

            modelBuilder.Entity("PassionLib.Models.ServantAlias", b =>
                {
                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<int>("ServantId")
                        .HasColumnType("integer");

                    b.Property<int>("SubmitterId")
                        .HasColumnType("integer");

                    b.HasKey("Nickname");

                    b.HasIndex("ServantId");

                    b.HasIndex("SubmitterId");

                    b.ToTable("ServantAliases");
                });

            modelBuilder.Entity("PassionLib.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DiscordDiscriminator")
                        .HasColumnType("text");

                    b.Property<long?>("DiscordSnowflake")
                        .HasColumnType("bigint");

                    b.Property<string>("DiscordUsername")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PassionLib.Models.CraftEssenceAlias", b =>
                {
                    b.HasOne("PassionLib.Models.CraftEssence", "CraftEssence")
                        .WithMany()
                        .HasForeignKey("CraftEssenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.User", "Submitter")
                        .WithMany()
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CraftEssence");

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("PassionLib.Models.MysticCodeAlias", b =>
                {
                    b.HasOne("PassionLib.Models.MysticCode", "MysticCode")
                        .WithMany()
                        .HasForeignKey("MysticCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.User", "Submitter")
                        .WithMany()
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MysticCode");

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("PassionLib.Models.QuestAlias", b =>
                {
                    b.HasOne("PassionLib.Models.Quest", "Quest")
                        .WithMany()
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.User", "Submitter")
                        .WithMany()
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quest");

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("PassionLib.Models.Run", b =>
                {
                    b.HasOne("PassionLib.Models.Servant", "Dps")
                        .WithMany()
                        .HasForeignKey("DpsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.MysticCode", "MysticCode")
                        .WithMany()
                        .HasForeignKey("MysticCodeId");

                    b.HasOne("PassionLib.Models.Quest", "Quest")
                        .WithMany()
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.User", "Submitter")
                        .WithMany()
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("PassionLib.Models.PartySlot", "Party", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<bool?>("Borrowed")
                                .HasColumnType("boolean");

                            b1.Property<int?>("CraftEssenceId")
                                .HasColumnType("integer");

                            b1.Property<short?>("CraftEssenceLevel")
                                .HasColumnType("smallint");

                            b1.Property<bool?>("CraftEssenceMlb")
                                .HasColumnType("boolean");

                            b1.Property<int>("RunId")
                                .HasColumnType("integer");

                            b1.Property<short?>("ServantFou")
                                .HasColumnType("smallint");

                            b1.Property<int?>("ServantId")
                                .HasColumnType("integer");

                            b1.Property<short?>("ServantLevel")
                                .HasColumnType("smallint");

                            b1.Property<short?>("TotalAttack")
                                .HasColumnType("smallint");

                            b1.HasKey("Id");

                            b1.HasIndex("CraftEssenceId");

                            b1.HasIndex("RunId");

                            b1.HasIndex("ServantId");

                            b1.ToTable("PartySlot");

                            b1.HasOne("PassionLib.Models.CraftEssence", "CraftEssence")
                                .WithMany()
                                .HasForeignKey("CraftEssenceId");

                            b1.WithOwner()
                                .HasForeignKey("RunId");

                            b1.HasOne("PassionLib.Models.Servant", "Servant")
                                .WithMany()
                                .HasForeignKey("ServantId");

                            b1.Navigation("CraftEssence");

                            b1.Navigation("Servant");
                        });

                    b.Navigation("Dps");

                    b.Navigation("MysticCode");

                    b.Navigation("Party");

                    b.Navigation("Quest");

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("PassionLib.Models.ServantAlias", b =>
                {
                    b.HasOne("PassionLib.Models.Servant", "Servant")
                        .WithMany()
                        .HasForeignKey("ServantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PassionLib.Models.User", "Submitter")
                        .WithMany()
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Servant");

                    b.Navigation("Submitter");
                });
#pragma warning restore 612, 618
        }
    }
}
