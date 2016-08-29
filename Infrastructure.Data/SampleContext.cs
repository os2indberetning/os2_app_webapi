﻿using System.Data.Entity;
using Core.DomainModel;
using Core.DomainModel.Model;

namespace Infrastructure.Data
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class SampleContext : DbContext
    {
        public SampleContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<SampleContext>(new SampleSeedInitializer());
        }

        public IDbSet<DriveReport> DriveReports { get; set; }
        public IDbSet<Employment> Employments { get; set; }
        public IDbSet<GPSCoordinate> GPSCoordinates { get; set; }
        public IDbSet<OrgUnit> OrgUnits { get; set; }
        public IDbSet<Profile> Profiles { get; set; }
        public IDbSet<Rate> Rates { get; set; }
        public IDbSet<Route> Routes { get; set; }
        public IDbSet<Token> Tokens { get; set; }
        public IDbSet<UserAuth> UserAuths { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // use conventions when possible
            modelBuilder.Entity<DriveReport>().HasRequired(x => x.Route).WithRequiredPrincipal(x => x.DriveReport);
        }
    }
}
