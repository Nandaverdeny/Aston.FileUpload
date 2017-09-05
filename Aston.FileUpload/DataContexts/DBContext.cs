using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Aston.FileUpload.Models;

namespace Aston.FileUpload.DataContexts
{
    public class DBContext : DbContext
    {

        public DBContext()
        : base("DefaultConnection")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<FlatFile> FlatFiles { get; set; }
        //public DbSet<FlatFileWithData> FlatFilesWithData { get; set; }
        //public DbSet<UploadedData> UploadedDatas { get; set; }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<AssetLocation> AssetLocations { get; set; }



    }
}