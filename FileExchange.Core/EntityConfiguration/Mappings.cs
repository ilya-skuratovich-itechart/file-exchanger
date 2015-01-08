using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.EntityConfiguration
{
    internal partial class MembershipMapping : EntityTypeConfiguration<MemberShip>
    {
        public MembershipMapping()
        {
            this.HasKey(t => t.UserId);
            this.ToTable("webpages_Membership");
            this.Property(t => t.UserId).HasColumnName("UserId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ConfirmationToken).HasColumnName("ConfirmationToken").HasMaxLength(128);
            this.Property(t => t.IsConfirmed).HasColumnName("IsConfirmed");
            this.Property(t => t.LastPasswordFailureDate).HasColumnName("LastPasswordFailureDate");
            this.Property(t => t.PasswordFailuresSinceLastSuccess).HasColumnName("PasswordFailuresSinceLastSuccess");
            this.Property(t => t.Password).HasColumnName("Password").IsRequired().HasMaxLength(128);
            this.Property(t => t.PasswordChangedDate).HasColumnName("PasswordChangedDate");
            this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt").IsRequired().HasMaxLength(128);
            this.Property(t => t.PasswordVerificationToken).HasColumnName("PasswordVerificationToken").HasMaxLength(128);
            this.Property(t => t.PasswordVerificationTokenExpirationDate).HasColumnName("PasswordVerificationTokenExpirationDate");
        }
    }

    internal partial class RolesMapping : EntityTypeConfiguration<UserRoles>
    {
        public RolesMapping()
        {
            this.HasKey(t => t.RoleId);
            this.ToTable("webpages_Roles");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName").IsRequired().HasMaxLength(256);
        }
    }

    internal partial class UsersInRolesMapping : EntityTypeConfiguration<UserInRoles>
    {
        public UsersInRolesMapping()
        {
            this.HasKey(t => new { t.UserId, t.RoleId });
            this.ToTable("webpages_UsersInRoles");
            this.Property(t => t.UserId).HasColumnName("UserId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
            this.Property(t => t.RoleId).HasColumnName("RoleId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
            this.HasRequired(t => t.Role).WithMany(t => t.UserInRoles).HasForeignKey(d => d.RoleId);
        }
    }
}