using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Management.Models
{
    public partial class wallet2Context : DbContext
    {
        public wallet2Context()
        {
        }
        public wallet2Context(DbContextOptions<wallet2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<BanksysBank> BanksysBank { get; set; }
        public virtual DbSet<BanksysBankActions> BanksysBankActions { get; set; }
        public virtual DbSet<BanksysBranch> BanksysBranch { get; set; }
        public virtual DbSet<BanksysUserBranchs> BanksysUserBranchs { get; set; }
        public virtual DbSet<BanksysUsers> BanksysUsers { get; set; }
        public virtual DbSet<CashIn> CashIn { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ClintInfo> ClintInfo { get; set; }
        public virtual DbSet<DealersUserGroup> DealersUserGroup { get; set; }
        public virtual DbSet<IdentityVerification> IdentityVerification { get; set; }
        public virtual DbSet<Merchants> Merchants { get; set; }
        public virtual DbSet<PersonalInfo> PersonalInfo { get; set; }
        public virtual DbSet<PersonalInfo1> PersonalInfo1 { get; set; }
        public virtual DbSet<RefugeeCashIn> RefugeeCashIn { get; set; }
        public virtual DbSet<Refugees> Refugees { get; set; }
        public virtual DbSet<ServiceCenter> ServiceCenter { get; set; }
        public virtual DbSet<Streets> Streets { get; set; }
        public virtual DbSet<Tokens> Tokens { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserGroupDetails> UserGroupDetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        // Unable to generate entity type for table 'dbo.Account'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.customers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.UserMpay'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Table_1'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"server=GUJ-DEVDB-T01;Database=wallet2;User Id=alm_nid;Password=alm_nid;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.ToTable("applications");

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("applicationId")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .HasColumnName("applicationName")
                    .HasMaxLength(50);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.TokenLength).HasColumnName("tokenLength");

                entity.Property(e => e.Ttl).HasColumnName("TTL");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("appointment");

                entity.Property(e => e.TransactionId).HasColumnName("transactionId");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnName("appointmentDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.BankId)
                    .HasColumnName("bankId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Msisdn)
                    .HasColumnName("MSISDN")
                    .HasColumnType("char(9)");

                entity.Property(e => e.Nid)
                    .HasColumnName("NID")
                    .HasColumnType("char(12)");

                entity.Property(e => e.ReferenceCode).HasColumnName("reference_code");

                entity.Property(e => e.ServiceCenterId)
                    .HasColumnName("serviceCenterId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transactionStatus")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TransactionType).HasColumnName("transactionType");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("bank");

                entity.Property(e => e.BankId).HasColumnName("bankId");

                entity.Property(e => e.BankName)
                    .HasColumnName("bankName")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<BanksysBank>(entity =>
            {
                entity.HasKey(e => e.BankId);

                entity.ToTable("banksys_Bank");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<BanksysBranch>(entity =>
            {
                entity.HasKey(e => e.BranchId);

                entity.ToTable("banksys_Branch");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BanksysBranch)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_Branch_Bank");
            });

            modelBuilder.Entity<BanksysUserBranchs>(entity =>
            {
                entity.HasKey(e => e.UserBranchId);

                entity.ToTable("banksys_UserBranchs");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.BanksysUserBranchs)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_UserBranchs_Branch");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BanksysUserBranchs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserBranchs_Users");
            });


            modelBuilder.Entity<BanksysBankActions>(entity =>
            {
                entity.HasKey(e => e.BankActionId);

                entity.ToTable("banksys_BankActions");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.HasOne(d => d.CashIn)
                    .WithMany(p => p.BanksysBankActions)
                    .HasForeignKey(d => d.CashInId)
                    .HasConstraintName("FK_banksys_BankActions_CashIn");

                entity.HasOne(d => d.PersonalInfo)
                    .WithMany(p => p.BanksysBankActions)
                    .HasForeignKey(d => d.PersonalInfoId)
                    .HasConstraintName("FK_banksys_BankActions_banksys_BankActions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BanksysBankActions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankActions_banksys_Users");
            });



            modelBuilder.Entity<BanksysUsers>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("banksys_Users");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.BanksysUsers)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Users_Branch");
            });

            modelBuilder.Entity<CashIn>(entity =>
            {
                entity.Property(e => e.AuthNumber).HasMaxLength(50);

                entity.Property(e => e.BanckAccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.Item).HasMaxLength(50);

                entity.Property(e => e.NumInvoiceDep).HasMaxLength(50);

                entity.Property(e => e.PersonalId).HasColumnName("personalId");

                entity.Property(e => e.RefrenceNumber).HasColumnName("refrenceNumber");

                entity.Property(e => e.ShipmentDuration).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Valueletters).HasMaxLength(50);

                entity.Property(e => e.Walletaccount).HasMaxLength(50);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.CashIn)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_CashIn_bank");

                entity.HasOne(d => d.Personal)
                    .WithMany(p => p.CashIn)
                    .HasForeignKey(d => d.PersonalId)
                    .HasConstraintName("FK_CashIn_personalInfo");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.SmallIcon).HasMaxLength(50);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.CityId).ValueGeneratedOnAdd();

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.HasOne(d => d.CityNavigation)
                    .WithOne(p => p.InverseCityNavigation)
                    .HasForeignKey<City>(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_city_city");
            });

            modelBuilder.Entity<ClintInfo>(entity =>
            {
                entity.ToTable("clintInfo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activity).HasMaxLength(50);

                entity.Property(e => e.ActivityCommissioner).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AdressOrgan).HasMaxLength(50);

                entity.Property(e => e.AppointmentDate)
                    .HasColumnName("appointmentDate")
                    .HasMaxLength(50);

                entity.Property(e => e.AppointmentTime)
                    .HasColumnName("appointmentTime")
                    .HasMaxLength(50);

                entity.Property(e => e.Bankaccountnumber).HasMaxLength(50);

                entity.Property(e => e.Bankbranch).HasMaxLength(50);

                entity.Property(e => e.Bankname).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailCommissioner).HasMaxLength(50);

                entity.Property(e => e.EstablishmentDate).HasColumnType("datetime");

                entity.Property(e => e.FamilyNumber).HasMaxLength(50);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.FatherNameEn).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.GrandName).HasMaxLength(50);

                entity.Property(e => e.GrandNameEn).HasMaxLength(50);

                entity.Property(e => e.Item)
                    .HasColumnName("item")
                    .HasMaxLength(50);

                entity.Property(e => e.LegalForm).HasMaxLength(50);

                entity.Property(e => e.Licensor).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEn).HasMaxLength(50);

                entity.Property(e => e.NameOrganization).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NearByPlace).HasMaxLength(50);

                entity.Property(e => e.Nid).HasMaxLength(20);

                entity.Property(e => e.NumInvoiceDep).HasMaxLength(50);

                entity.Property(e => e.NumberCommerceRoom).HasMaxLength(50);

                entity.Property(e => e.NumberInCommerceRecord).HasMaxLength(50);

                entity.Property(e => e.OrganType).HasMaxLength(50);

                entity.Property(e => e.PassportExportDate).HasColumnType("datetime");

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.PersonalCardExportDate).HasColumnType("datetime");

                entity.Property(e => e.PersonalCardNumber).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PhoneCommissioner).HasMaxLength(50);

                entity.Property(e => e.ReferenceCode)
                    .HasColumnName("referenceCode")
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationPlace).HasMaxLength(50);

                entity.Property(e => e.ServiceCenterName)
                    .HasColumnName("serviceCenterName")
                    .HasMaxLength(50);

                entity.Property(e => e.ShipmentDuration).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SurName).HasMaxLength(50);

                entity.Property(e => e.SurNameEn).HasMaxLength(50);

                entity.Property(e => e.Valuedigits).HasMaxLength(50);

                entity.Property(e => e.Valueletters).HasMaxLength(50);

                entity.Property(e => e.Walletaccount).HasMaxLength(50);

                entity.Property(e => e.Wallettype).HasMaxLength(50);
            });

            modelBuilder.Entity<DealersUserGroup>(entity =>
            {
                entity.Property(e => e.UesrGroupId).HasColumnName("uesrGroupId");
            });

            modelBuilder.Entity<IdentityVerification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreationOn)
                    .HasColumnName("creationOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modifiedOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.Msisdn)
                    .HasColumnName("msisdn")
                    .HasMaxLength(20);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Merchants>(entity =>
            {
                entity.HasKey(e => e.MerchantId);

                entity.Property(e => e.MerchantId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FacebookLink).HasMaxLength(50);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Logo)
                    .HasColumnName("logo")
                    .HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.MerchantAdress).HasMaxLength(50);

                entity.Property(e => e.MerchantName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.ReservationDate).HasColumnType("datetime");

                entity.Property(e => e.SadadWalletId).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkHours).HasMaxLength(50);

                entity.HasOne(d => d.Merchant)
                    .WithOne(p => p.InverseMerchant)
                    .HasForeignKey<Merchants>(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Merchants_Merchants");
            });

            modelBuilder.Entity<PersonalInfo>(entity =>
            {
                entity.ToTable("personalInfo");

                entity.HasIndex(e => e.GenratedNumber)
                    .HasName("uniqueGenratedNumber2")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("IX_personalInfo_12")
                    .IsUnique();

                entity.HasIndex(e => e.Nid)
                    .HasName("UQ__personal__C7D1D6CA87A93F76")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("UQ__personal__5C7E359EA281D1BC")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activity).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AppointmentDate)
                    .HasColumnName("appointmentDate")
                    .HasColumnType("date");

                entity.Property(e => e.Bankaccountnumber).HasMaxLength(50);

                entity.Property(e => e.Bankbranch).HasMaxLength(50);

                entity.Property(e => e.Bankname).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CityMpayId).HasDefaultValueSql("((1))");

                entity.Property(e => e.CrmFullName)
                    .HasColumnName("crmFullName")
                    .HasColumnType("nchar(200)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FamilyNumber).HasMaxLength(50);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.FatherNameEn).HasMaxLength(50);

                entity.Property(e => e.GenratedNumber)
                    .IsRequired()
                    .HasColumnName("genratedNumber")
                    .HasMaxLength(200);

                entity.Property(e => e.GrandName).HasMaxLength(50);

                entity.Property(e => e.GrandNameEn).HasMaxLength(50);

                entity.Property(e => e.Item).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEn).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NearByPlace).HasMaxLength(50);

                entity.Property(e => e.Nid).HasMaxLength(20);

                entity.Property(e => e.NumInvoiceDep).HasMaxLength(50);

                entity.Property(e => e.PassportExportDate).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.PersonalCardExportDate).HasMaxLength(50);

                entity.Property(e => e.PersonalCardNumber).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Reference).HasColumnName("reference");

                entity.Property(e => e.ShipmentDuration).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SurName).HasMaxLength(50);

                entity.Property(e => e.SurNameEn).HasMaxLength(50);

                entity.Property(e => e.Valuedigits).HasMaxLength(50);

                entity.Property(e => e.Valueletters).HasMaxLength(50);

                entity.Property(e => e.Walletaccount).HasMaxLength(50);

                entity.Property(e => e.Wallettype).HasMaxLength(50);
            });

            modelBuilder.Entity<PersonalInfo1>(entity =>
            {
                entity.ToTable("personalInfo1");

                entity.HasIndex(e => e.GenratedNumber)
                    .HasName("uniqueGenratedNumber")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("IX_personalInfo_1")
                    .IsUnique();

                entity.HasIndex(e => e.Nid)
                    .HasName("UQ__personal__C7D1D6CA412EB0B6")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("UQ__personal__5C7E359E3E52440B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activity).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AppointmentDate)
                    .HasColumnName("appointmentDate")
                    .HasColumnType("date");

                entity.Property(e => e.Bankaccountnumber).HasMaxLength(50);

                entity.Property(e => e.Bankbranch).HasMaxLength(50);

                entity.Property(e => e.Bankname).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CrmFullName)
                    .HasColumnName("crmFullName")
                    .HasColumnType("nchar(200)");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FamilyNumber).HasMaxLength(50);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.FatherNameEn).HasMaxLength(50);

                entity.Property(e => e.GenratedNumber)
                    .IsRequired()
                    .HasColumnName("genratedNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.GrandName).HasMaxLength(50);

                entity.Property(e => e.GrandNameEn).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEn).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NearByPlace).HasMaxLength(50);

                entity.Property(e => e.Nid).HasMaxLength(20);

                entity.Property(e => e.PassportExportDate).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.PersonalCardExportDate).HasMaxLength(50);

                entity.Property(e => e.PersonalCardNumber).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Reference).HasColumnName("reference");

                entity.Property(e => e.ShipmentDuration).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SurName).HasMaxLength(50);

                entity.Property(e => e.SurNameEn).HasMaxLength(50);

                entity.Property(e => e.Valuedigits).HasMaxLength(50);

                entity.Property(e => e.Valueletters).HasMaxLength(50);

                entity.Property(e => e.Walletaccount).HasMaxLength(50);

                entity.Property(e => e.Wallettype).HasMaxLength(50);
            });

            modelBuilder.Entity<RefugeeCashIn>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(250);

                entity.Property(e => e.Reference).HasColumnName("reference");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Refugee)
                    .WithMany(p => p.RefugeeCashIn)
                    .HasForeignKey(d => d.RefugeeId)
                    .HasConstraintName("FK_RefugeeCashIn_Refugees");
            });

            modelBuilder.Entity<Refugees>(entity =>
            {
                entity.HasKey(e => e.RefugeeId);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Bankaccountnumber).HasMaxLength(50);

                entity.Property(e => e.Bankbranch).HasMaxLength(50);

                entity.Property(e => e.Bankname).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FamilyNumber).HasMaxLength(50);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.FatherNameEn).HasMaxLength(50);

                entity.Property(e => e.GrandName).HasMaxLength(50);

                entity.Property(e => e.GrandNameEn).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEn).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.NearByPlace).HasMaxLength(50);

                entity.Property(e => e.Nid).HasMaxLength(20);

                entity.Property(e => e.PassportExportDate).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.PersonalCardExportDate).HasMaxLength(50);

                entity.Property(e => e.PersonalCardNumber).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Reason).HasMaxLength(250);

                entity.Property(e => e.Reference).HasColumnName("reference");

                entity.Property(e => e.Rid)
                    .HasColumnName("RID")
                    .HasMaxLength(20);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.SurName).HasMaxLength(50);

                entity.Property(e => e.SurNameEn).HasMaxLength(50);
            });

            modelBuilder.Entity<ServiceCenter>(entity =>
            {
                entity.ToTable("serviceCenter");

                entity.Property(e => e.ServiceCenterId).HasColumnName("serviceCenterId");

                entity.Property(e => e.EndOfWorkTime).HasColumnName("endOfWorkTime");

                entity.Property(e => e.PeriodOfOneCaseInMinutes)
                    .HasColumnName("periodOfOneCaseInMinutes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ServiceCenterName)
                    .HasColumnName("serviceCenterName")
                    .HasMaxLength(150);

                entity.Property(e => e.StartOfWorkTime).HasColumnName("startOfWorkTime");
            });

            modelBuilder.Entity<Streets>(entity =>
            {
                entity.HasKey(e => e.StreetId);

                entity.Property(e => e.StreetName).HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Streets)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Streets_city");
            });

            modelBuilder.Entity<Tokens>(entity =>
            {
                entity.HasKey(e => e.SecretCode);

                entity.ToTable("tokens");

                entity.Property(e => e.SecretCode)
                    .HasColumnName("secretCode")
                    .HasColumnType("char(55)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationId).HasColumnName("applicationId");

                entity.Property(e => e.BlackListed).HasColumnName("blackListed");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("lastUpdateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TokenCode)
                    .HasColumnName("tokenCode")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("userGroup");

                entity.Property(e => e.UserGroupId).HasColumnName("userGroupId");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Descrption).HasMaxLength(1000);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<UserGroupDetails>(entity =>
            {
                entity.ToTable("userGroupDetails");

                entity.Property(e => e.UserGroupDetailsId).HasColumnName("userGroupDetailsId");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.UserGroupId).HasColumnName("userGroupId");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });
        }
    }
}
