using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PM_EOS.Models
{
    public partial class PM_EOSContext : DbContext
    {
        public PM_EOSContext()
        {
        }

        public PM_EOSContext(DbContextOptions<PM_EOSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AnswerDeThi> AnswerDeThis { get; set; }
        public virtual DbSet<CauHoi> CauHois { get; set; }
        public virtual DbSet<DapAn> DapAns { get; set; }
        public virtual DbSet<DeThi> DeThis { get; set; }
        public virtual DbSet<DetailDeThi> DetailDeThis { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<MonHocDeThi> MonHocDeThis { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=21AK22-COM;Initial Catalog=PM_EOS;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Idacc);

                entity.ToTable("Account");

                entity.Property(e => e.Idacc).HasColumnName("IDAcc");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AnswerDeThi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AnswerDeThi");

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.DapAnId).HasColumnName("DapAnID");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.HasOne(d => d.CauHoi)
                    .WithMany()
                    .HasForeignKey(d => d.CauHoiId)
                    .HasConstraintName("FK_AnswerDeThi_CauHoi");

                entity.HasOne(d => d.DapAn)
                    .WithMany()
                    .HasForeignKey(d => d.DapAnId)
                    .HasConstraintName("FK_AnswerDeThi_DapAn");

                entity.HasOne(d => d.DeThi)
                    .WithMany()
                    .HasForeignKey(d => d.DeThiId)
                    .HasConstraintName("FK_AnswerDeThi_DeThi");
            });

            modelBuilder.Entity<CauHoi>(entity =>
            {
                entity.HasKey(e => e.IdcauHoi);

                entity.ToTable("CauHoi");

                entity.Property(e => e.IdcauHoi).HasColumnName("IDCauHoi");

                entity.Property(e => e.NoiDungCauHoi).HasMaxLength(200);

                entity.Property(e => e.TenCauHoi).HasMaxLength(50);
            });

            modelBuilder.Entity<DapAn>(entity =>
            {
                entity.HasKey(e => e.IddapAn);

                entity.ToTable("DapAn");

                entity.Property(e => e.IddapAn).HasColumnName("IDDapAn");
            });

            modelBuilder.Entity<DeThi>(entity =>
            {
                entity.HasKey(e => e.IddeThi);

                entity.ToTable("DeThi");

                entity.Property(e => e.IddeThi).HasColumnName("IDDeThi");

                entity.Property(e => e.TenDeThi).HasMaxLength(100);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetailDeThi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DetailDeThi");

                entity.Property(e => e.CauHoiId).HasColumnName("CauHoiID");

                entity.Property(e => e.DapAnId).HasColumnName("DapAnID");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.HasOne(d => d.CauHoi)
                    .WithMany()
                    .HasForeignKey(d => d.CauHoiId)
                    .HasConstraintName("FK_DetailDeThi_CauHoi");

                entity.HasOne(d => d.DapAn)
                    .WithMany()
                    .HasForeignKey(d => d.DapAnId)
                    .HasConstraintName("FK_DetailDeThi_DapAn");

                entity.HasOne(d => d.DeThi)
                    .WithMany()
                    .HasForeignKey(d => d.DeThiId)
                    .HasConstraintName("FK_DetailDeThi_DeThi");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(e => e.Iddiem);

                entity.ToTable("Mark");

                entity.Property(e => e.Iddiem).HasColumnName("IDDiem");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.HocSinhId).HasColumnName("HocSinhID");

                entity.Property(e => e.MonHocId).HasColumnName("MonHocID");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.DeThi)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.DeThiId)
                    .HasConstraintName("FK_Mark_DeThi");

                entity.HasOne(d => d.HocSinh)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.HocSinhId)
                    .HasConstraintName("FK_Mark_Account");

                entity.HasOne(d => d.MonHoc)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.MonHocId)
                    .HasConstraintName("FK_Mark_MonHoc");
            });

            modelBuilder.Entity<MonHoc>(entity =>
            {
                entity.HasKey(e => e.IdmonHoc);

                entity.ToTable("MonHoc");

                entity.Property(e => e.IdmonHoc).HasColumnName("IDMonHoc");

                entity.Property(e => e.TenMonHoc).HasMaxLength(100);
            });

            modelBuilder.Entity<MonHocDeThi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MonHoc_DeThi");

                entity.Property(e => e.DeThiId).HasColumnName("DeThiID");

                entity.Property(e => e.MonHocId).HasColumnName("MonHocID");

                entity.HasOne(d => d.DeThi)
                    .WithMany()
                    .HasForeignKey(d => d.DeThiId)
                    .HasConstraintName("FK_MonHoc_DeThi_DeThi");

                entity.HasOne(d => d.MonHoc)
                    .WithMany()
                    .HasForeignKey(d => d.MonHocId)
                    .HasConstraintName("FK_MonHoc_DeThi_MonHoc");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
