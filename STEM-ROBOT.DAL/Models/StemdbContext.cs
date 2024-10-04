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

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchHalf> MatchHalves { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Referee> Referees { get; set; }

    public virtual DbSet<RefereeCompetition> RefereeCompetitions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC074CCB1FAC");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Image).HasColumnType("ntext");
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
            entity.HasKey(e => e.Id).HasName("PK__Action__3214EC07698E8892");

            entity.ToTable("Action");

            entity.Property(e => e.EventTime).HasColumnType("datetime");

            entity.HasOne(d => d.MatchHalf).WithMany(p => p.Actions)
                .HasForeignKey(d => d.MatchHalfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__MatchHal__6FE99F9F");

            entity.HasOne(d => d.Score).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ScoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__ScoreId__70DDC3D8");

            entity.HasOne(d => d.Team).WithMany(p => p.Actions)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Action__TeamId__71D1E811");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC07B790D334");

            entity.ToTable("Competition");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Genre).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Genre__46E78A0C");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Tourn__45F365D3");
        });

        modelBuilder.Entity<Contestant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contesta__3214EC071FC110F3");

            entity.ToTable("Contestant");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(100);
            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(11);
            entity.Property(e => e.Role).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.School).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Schoo__49C3F6B7");

            entity.HasOne(d => d.Tournament).WithMany(p => p.Contestants)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Tourn__4AB81AF0");
        });

        modelBuilder.Entity<ContestantCompetition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contesta__3214EC074BFCC84E");

            entity.ToTable("ContestantCompetition");

            entity.HasOne(d => d.Competition).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Compe__5EBF139D");

            entity.HasOne(d => d.Contestant).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.ContestantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__Conte__5DCAEF64");

            entity.HasOne(d => d.Team).WithMany(p => p.ContestantCompetitions)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contestan__TeamI__5FB337D6");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC078AC1B512");

            entity.ToTable("Genre");

            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Rules).HasColumnType("ntext");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Match__3214EC07868F2426");

            entity.ToTable("Match");

            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Stage).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Match__StageId__628FA481");

            entity.HasOne(d => d.Table).WithMany(p => p.Matches)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__Match__TableId__6383C8BA");
        });

        modelBuilder.Entity<MatchHalf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchHal__3214EC0736FB9A23");

            entity.ToTable("MatchHalf");

            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchHalves)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchHalf__Match__6A30C649");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC07AB77005F");

            entity.ToTable("Position");

            entity.Property(e => e.Address).HasMaxLength(500);

            entity.HasOne(d => d.Competition).WithMany(p => p.Positions)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Position__Compet__74AE54BC");
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Referee__3214EC075781AF27");

            entity.ToTable("Referee");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Referees)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Referee__Tournam__4D94879B");
        });

        modelBuilder.Entity<RefereeCompetition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefereeC__3214EC07693046FA");

            entity.ToTable("RefereeCompetition");

            entity.HasOne(d => d.Competition).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefereeCo__Compe__5AEE82B9");

            entity.HasOne(d => d.Referee).WithMany(p => p.RefereeCompetitions)
                .HasForeignKey(d => d.RefereeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefereeCo__Refer__59FA5E80");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC079C3A5B20");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC07E5BF91BE");

            entity.ToTable("Schedule");

            entity.HasOne(d => d.Match).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__MatchI__787EE5A0");

            entity.HasOne(d => d.Position).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Positi__797309D9");

            entity.HasOne(d => d.Referee).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RefereeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Refere__778AC167");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__School__3214EC07B065BD7E");

            entity.ToTable("School");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Area).HasMaxLength(200);
            entity.Property(e => e.AreaCode).HasMaxLength(200);
            entity.Property(e => e.District).HasMaxLength(200);
            entity.Property(e => e.DistrictCode).HasMaxLength(200);
            entity.Property(e => e.Province).HasMaxLength(200);
            entity.Property(e => e.ProvinceCode).HasMaxLength(200);
            entity.Property(e => e.SchoolCode).HasMaxLength(250);
            entity.Property(e => e.SchoolName).HasMaxLength(500);
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Score__3214EC07E6FD9293");

            entity.ToTable("Score");

            entity.Property(e => e.Description).HasColumnType("ntext");

            entity.HasOne(d => d.Genre).WithMany(p => p.Scores)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Score__GenreId__6D0D32F4");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stage__3214EC0796A66032");

            entity.ToTable("Stage");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Stages)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stage__Competiti__5070F446");
        });

        modelBuilder.Entity<TableGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TableGro__3214EC07E5727E60");

            entity.ToTable("TableGroup");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Stage).WithMany(p => p.TableGroups)
                .HasForeignKey(d => d.StageId)
                .HasConstraintName("FK__TableGrou__Stage__534D60F1");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC07EFD5A221");

            entity.ToTable("Team");

            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Competition).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Team__Competitio__5629CD9C");

            entity.HasOne(d => d.TableGroup).WithMany(p => p.Teams)
                .HasForeignKey(d => d.TableGroupId)
                .HasConstraintName("FK__Team__TableGroup__571DF1D5");
        });

        modelBuilder.Entity<TeamMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamMatc__3214EC073F6770F3");

            entity.ToTable("TeamMatch");

            entity.HasOne(d => d.Match).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamMatch__Match__66603565");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamMatches)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamMatch__TeamI__6754599E");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC07F8DE7A9A");

            entity.ToTable("Tournament");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.Mode).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TournamentLevel).HasMaxLength(300);

            entity.HasOne(d => d.Account).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tournamen__Accou__403A8C7D");

            entity.HasOne(d => d.TournamentFormat).WithMany(p => p.Tournaments)
                .HasForeignKey(d => d.TournamentFormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tournamen__Tourn__412EB0B6");
        });

        modelBuilder.Entity<TournamentFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tourname__3214EC077203E53A");

            entity.ToTable("TournamentFormat");

            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
