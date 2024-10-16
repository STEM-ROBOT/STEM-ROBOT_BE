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
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F7832DF6A");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__RoleId__398D8EEE");
        });

        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Action__3213E83FB22BEC98");

            entity.ToTable("Action");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventTime).HasColumnType("datetime");

            entity.HasOne(d => d.MatchHalf).WithMany(p => p.Actions)
                .HasForeignKey(d => d.MatchHalfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__MatchHal__787EE5A0");

            entity.HasOne(d => d.ScoreCategory).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ScoreCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__ScoreCat__797309D9");

            entity.HasOne(d => d.Team).WithMany(p => p.Actions)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__TeamId__7A672E12");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3213E83F00E5DB86");

            entity.ToTable("Competition");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Genre).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Genre__5070F446");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Tourn__4F7CD00D");
        });

        modelBuilder.Entity<Contestant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contesta__3213E83FAE06A761");

            entity.ToTable("Contestant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.School).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Schoo__440B1D61");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Tourn__44FF419A");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3213E83F88F0723A");

            entity.ToTable("Genre");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<GenreRegulation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GenreReg__3213E83F03952676");

            entity.ToTable("GenreRegulation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreRegulations)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GenreRegu__Genre__4BAC3F29");

            entity.HasOne(d => d.Role).WithMany(p => p.GenreRegulations)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GenreRegu__RoleI__4CA06362");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3213E83F6DDE6ED4");

            entity.ToTable("Location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.ContactPerson).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Location__Compet__59FA5E80");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Match__3213E83F46782D03");

            entity.ToTable("Match");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Round).WithMany(p => p.Matches)
                .HasForeignKey(d => d.RoundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Match__RoundId__66603565");

            entity.HasOne(d => d.Table).WithMany(p => p.Matches)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Match__TableId__6754599E");
        });

        modelBuilder.Entity<MatchHalf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchHal__3213E83F57D2665B");

            entity.ToTable("MatchHalf");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchHalves)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchHalf__Match__6E01572D");
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Referee__3213E83F89B25665");

            entity.ToTable("Referee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Referees)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Referee__Tournam__534D60F1");
        });

        modelBuilder.Entity<RefereeCompetition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefereeC__3213E83FED5D4D91");

            entity.ToTable("RefereeCompetition");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Competition).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefereeCo__Compe__571DF1D5");

            entity.HasOne(d => d.Referee).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.RefereeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefereeCo__Refer__5629CD9C");
        });

        modelBuilder.Entity<Regulation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Regulati__3214EC07ADF2C363");

            entity.ToTable("Regulation");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F5E8A8402");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3213E83FB649C7B8");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Locati__75A278F5");

            entity.HasOne(d => d.Match).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__MatchI__74AE54BC");

            entity.HasOne(d => d.Referee).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RefereeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Refere__73BA3083");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__School__3213E83F1D1D6FB1");

            entity.ToTable("School");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Area).HasMaxLength(200);
            entity.Property(e => e.District).HasMaxLength(200);
            entity.Property(e => e.Province).HasMaxLength(200);
            entity.Property(e => e.SchoolCode).HasMaxLength(250);
            entity.Property(e => e.SchoolName).HasMaxLength(500);
            entity.Property(e => e.SchoolType).HasMaxLength(200);
        });

        modelBuilder.Entity<ScoreCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ScoreCat__3213E83FCD74C8EB");

            entity.ToTable("ScoreCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne(d => d.Genre).WithMany(p => p.ScoreCategories)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ScoreCate__Genre__70DDC3D8");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stage__3213E83FB9ADFEB3");

            entity.ToTable("Stage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Stages)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stage__Competiti__5CD6CB2B");
        });

        modelBuilder.Entity<TableGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TableGro__3213E83FCBBEC8D9");

            entity.ToTable("TableGroup");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Round).WithMany(p => p.TableGroups)
                .HasForeignKey(d => d.RoundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TableGrou__Round__6383C8BA");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3213E83F8F6C5C8E");

            entity.ToTable("Team");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Competition).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Team__Competitio__60A75C0F");
        });

        modelBuilder.Entity<TeamMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamMatc__3213E83FF4A1D0BC");

            entity.ToTable("TeamMatch");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Match).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamMatch__Match__6A30C649");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamMatch__TeamI__6B24EA82");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3213E83F92843823");

            entity.ToTable("Tournament");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Level).HasMaxLength(300);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.Mode).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Account).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tournamen__Accou__403A8C7D");

            entity.HasOne(d => d.Format).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tournamen__Forma__412EB0B6");
        });

        modelBuilder.Entity<TournamentFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3213E83FCF2D0897");

            entity.ToTable("TournamentFormat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
