using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace STEM_ROBOT.DAL.Models;

public partial class StemdbContext : DbContext
{
    public StemdbContext()
    {
    }

    public StemdbContext(DbContextOptions<StemdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<Contestant> Contestants { get; set; }

    public virtual DbSet<ContestantCompetition> ContestantCompetitions { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchHalf> MatchHalves { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PakageAccount> PakageAccounts { get; set; }

    public virtual DbSet<Referee> Referees { get; set; }

    public virtual DbSet<RefereeCompetition> RefereeCompetitions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<ScoreCategory> ScoreCategories { get; set; }

    public virtual DbSet<Stage> Stages { get; set; }

    public virtual DbSet<TableGroup> TableGroups { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamMatch> TeamMatches { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<TournamentFormat> TournamentFormats { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F0BE27616");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.MaxTournatment).HasDefaultValueSql("((3))");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.UsedTournament).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__RoleId__5EBF139D");
        });

        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83FDA61AAC8");

            entity.ToTable("Action");

            entity.Property(e => e.EventTime).HasColumnType("datetime");

            entity.HasOne(d => d.MatchHalf).WithMany(p => p.Actions)
                .HasForeignKey(d => d.MatchHalfId)
                .HasConstraintName("FK__Action__MatchHal__1DB06A4F");

            entity.HasOne(d => d.ScoreCategory).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ScoreCategoryId)
                .HasConstraintName("FK_Action_ScoreCategory");

            entity.HasOne(d => d.TeamMatch).WithMany(p => p.Actions)
                .HasForeignKey(d => d.TeamMatchId)
                .HasConstraintName("FK_Action_TeamMatch");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3213E83F125E57BB");

            entity.ToTable("Competition");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Regulation).HasColumnType("text");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Genre).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Competiti__Genre__75A278F5");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK__Competiti__Tourn__74AE54BC");
        });

        modelBuilder.Entity<Contestant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contesta__3213E83F0FA78220");

            entity.ToTable("Contestant");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.School).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Contestant_School");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK_Contestant_Tournament");
        });

        modelBuilder.Entity<ContestantCompetition>(entity =>
        {
            entity.ToTable("ContestantCompetition");

            entity.HasOne(d => d.Competition).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK_ContestantCompetition_Competition");

            entity.HasOne(d => d.Contestant).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.ContestantId)
                .HasConstraintName("FK_ContestantCompetition_Contestant");

            entity.HasOne(d => d.Team).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_ContestantCompetition_Team");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3213E83F32DCA6A9");

            entity.ToTable("Genre");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3213E83FE55DE9DD");

            entity.ToTable("Location");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.ContactPerson).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__Location__Compet__7F2BE32F");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Match__3213E83F7297D577");

            entity.ToTable("Match");

            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Round).WithMany(p => p.Matches)
                .HasForeignKey(d => d.RoundId)
                .HasConstraintName("FK__Match__RoundId__0B91BA14");

            entity.HasOne(d => d.Table).WithMany(p => p.Matches)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__Match__TableId__0C85DE4D");
        });

        modelBuilder.Entity<MatchHalf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchHal__3213E83FC7A15D6B");

            entity.ToTable("MatchHalf");

            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchHalves)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__MatchHalf__Match__1332DBDC");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<PakageAccount>(entity =>
        {
            entity.ToTable("PakageAccount");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Account).WithMany(p => p.PakageAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_PakageAccount_Account");

            entity.HasOne(d => d.Package).WithMany(p => p.PakageAccounts)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PakageAccount_Package");
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Referee__3213E83FF479F0DE");

            entity.ToTable("Referee");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Referees)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK__Referee__Tournam__787EE5A0");
        });

        modelBuilder.Entity<RefereeCompetition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefereeC__3213E83F929750B0");

            entity.ToTable("RefereeCompetition");

            entity.HasOne(d => d.Competition).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__RefereeCo__Compe__7C4F7684");

            entity.HasOne(d => d.Referee).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.RefereeId)
                .HasConstraintName("FK__RefereeCo__Refer__7B5B524B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F6E4C5509");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3213E83F7F87B3F2");

            entity.ToTable("Schedule");

            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Schedule_Location");

            entity.HasOne(d => d.Match).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__Schedule__MatchI__19DFD96B");

            entity.HasOne(d => d.Referee).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RefereeId)
                .HasConstraintName("FK_Schedule_RefereeCompetition");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__School__3213E83F7A25C3FB");

            entity.ToTable("School");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Area).HasMaxLength(200);
            entity.Property(e => e.District).HasMaxLength(200);
            entity.Property(e => e.DistrictCode).HasMaxLength(200);
            entity.Property(e => e.Province).HasMaxLength(200);
            entity.Property(e => e.ProvinceCode).HasMaxLength(200);
            entity.Property(e => e.SchoolCode).HasMaxLength(250);
            entity.Property(e => e.SchoolName).HasMaxLength(500);
        });

        modelBuilder.Entity<ScoreCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ScoreCat__3213E83F49DEC7F9");

            entity.ToTable("ScoreCategory");

            entity.Property(e => e.Description).HasColumnType("ntext");

            entity.HasOne(d => d.Genre).WithMany(p => p.ScoreCategories)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__ScoreCate__Genre__160F4887");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stage__3213E83F527E4245");

            entity.ToTable("Stage");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Stages)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK_Stage_Competition");
        });

        modelBuilder.Entity<TableGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TableGro__3213E83FAFB8010C");

            entity.ToTable("TableGroup");

            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Stage).WithMany(p => p.TableGroups)
                .HasForeignKey(d => d.StageId)
                .HasConstraintName("FK__TableGrou__Round__08B54D69");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3213E83F60DC024A");

            entity.ToTable("Team");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Competition).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK_Team_Competition");

            entity.HasOne(d => d.Table).WithMany(p => p.Teams)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK_Team_TableGroup");
        });

        modelBuilder.Entity<TeamMatch>(entity =>
        {
            entity.ToTable("TeamMatch");

            entity.HasOne(d => d.Match).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__TeamMatch__Match__0F624AF8");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__TeamMatch__TeamI__10566F31");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3213E83FB6A35A7E");

            entity.ToTable("Tournament");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.Mode).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TournamentLevel).HasMaxLength(300);

            entity.HasOne(d => d.Account).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Tournamen__Accou__656C112C");

            entity.HasOne(d => d.Format).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.FormatId)
                .HasConstraintName("FK__Tournamen__Forma__66603565");
        });

        modelBuilder.Entity<TournamentFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3213E83FD32182EA");

            entity.ToTable("TournamentFormat");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
