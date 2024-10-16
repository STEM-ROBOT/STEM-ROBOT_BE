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

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GenreRegulation> GenreRegulations { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchHalf> MatchHalves { get; set; }

    public virtual DbSet<Referee> Referees { get; set; }

    public virtual DbSet<RefereeCompetition> RefereeCompetitions { get; set; }

    public virtual DbSet<Regulation> Regulations { get; set; }

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
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(250);

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

            entity.HasOne(d => d.Team).WithMany(p => p.Actions)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__Action__TeamId__1F98B2C1");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3213E83F125E57BB");

            entity.ToTable("Competition");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(250);
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
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3213E83F32DCA6A9");

            entity.ToTable("Genre");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<GenreRegulation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GenreReg__3213E83F2C66F70C");

            entity.ToTable("GenreRegulation");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreRegulations)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__GenreRegu__Genre__70DDC3D8");

            entity.HasOne(d => d.Role).WithMany(p => p.GenreRegulations)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__GenreRegu__RoleI__71D1E811");
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

        modelBuilder.Entity<Regulation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Regulati__3214EC07BDAEE66E");

            entity.ToTable("Regulation");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
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

            entity.HasOne(d => d.Match).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__Schedule__MatchI__19DFD96B");
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

            entity.Property(e => e.Description).HasColumnType("text");

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
        });

        modelBuilder.Entity<TableGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TableGro__3213E83FAFB8010C");

            entity.ToTable("TableGroup");

            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Round).WithMany(p => p.TableGroups)
                .HasForeignKey(d => d.RoundId)
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
        });

        modelBuilder.Entity<TeamMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamMatc__3213E83F4B2662C8");

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

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
