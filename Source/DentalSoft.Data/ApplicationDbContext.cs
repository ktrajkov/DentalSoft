namespace DentalSoft.Data
{
    using DentalSoft.Data.Migrations;
    using DentalSoft.Data.Models;
    using DentalSoft.Data.Models.Diagnoses;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using DentalSoft.Data.Models.Status;
    using DentalSoft.Data.Models.Teeths;
    using DentalSoft.Data.Models.Treatments;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DentalSoft.Data.Models.Images;
    using EntityFramework.DynamicFilters;
    using DentalSoft.Data.Models.Diseases;
    using DentalSoft.Data.Models.DailyPlannings;
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Models.FinancialPlan;

    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual IDbSet<PersonalData> PersonalData { get; set; }

        public virtual IDbSet<Patient> Patients { get; set; }

        public virtual IDbSet<Dentist> Dentists { get; set; }

        public virtual IDbSet<Region> Regions { get; set; }

        public virtual IDbSet<Municipality> Municipalityes { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

        public virtual IDbSet<Address> Addresses { get; set; }

        public virtual IDbSet<Telephone> Telephones { get; set; }
    
        public virtual IDbSet<Diagnosis> Diagnoses { get; set; }

        public virtual IDbSet<Treatment> Treatments { get; set; }

        public virtual IDbSet<Tooth> Teeth{ get; set; }

        public virtual IDbSet<Operation> Operations { get; set; }

        public virtual IDbSet<Image> Images { get; set; }

        public virtual IDbSet<Desease> Deseases { get; set; }

        public virtual IDbSet<Status> Statuses { get; set; }

        public virtual IDbSet<DeseaseCategory> DeseaseCategories { get; set; }

        public virtual IDbSet<PlanningItem> PlanningItems { get; set; }

        public virtual IDbSet<Contact> Contacts { get; set; }

        public virtual IDbSet<FinancialPlan> FinancialPlan { get; set; }

      
        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        #region Protected Members

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Filter("IsDeleted", (IDeletableEntity d) => d.IsDeleted, false); 
            modelBuilder.Entity<PersonalData>()
                .HasOptional(x => x.Contact).WithMany()
                .HasForeignKey(x => x.ContactId);

            modelBuilder.Entity<Contact>()
                .HasOptional(x => x.PersonalData).WithMany()
                .HasForeignKey(x => x.PersonalDataId);
        }

        #endregion

        #region Private Members

        private void ApplyAuditInfoRules()
        {
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        } 

        #endregion
    }
}
