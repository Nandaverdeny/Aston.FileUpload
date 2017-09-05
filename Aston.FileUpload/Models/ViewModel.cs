using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Aston.FileUpload.Models
{
    public class BaseClass
    {
        public string getDate() { return DateTime.Today.ToString("ddMMyyyy"); }
    }

    public class ViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusCD { get; set; }
        public int Quantity { get; set; }
        public string No { get; set; }

        //Asset
        public string Owner { get; set; }
        public string PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string DepreciationDuration { get; set; }
        public int CategoryCD { get; set; }
        public string ManufactureDate { get; set; }

        //Location
        public int LocationTypeCD { get; set; }
        public string Floor { get; set; }

        public string CompanyCode
        {
            get { return ConfigurationManager.AppSettings["companyCode"]; }
        }
        public string ApplicationCode
        {
            get { return ConfigurationManager.AppSettings["applicationCode"]; }
        }
        public virtual string MainCategory { get; }
        public virtual string SubCategory { get; set;}

        public string Number
        {
            get
            {
                switch (No.Length)
                {
                    case 0:
                        return "0000";
                    case 1:
                        return "000" + No;
                    case 2:
                        return "00" + No;
                    case 3:
                        return "0" + No;
                    case 4:
                        return No;
                    default:
                        return No;
                }
            }
        }

        public string GenerateCode() { return this.CompanyCode + this.ApplicationCode + this.MainCategory + this.SubCategory + this.Number; } // (2 + 2) + 4 + 4 + 4
    }

    [Table("Asset")]
    public class Asset : BaseClass
    {
        public Asset(AssetViewModel model) {
            this.Name = model.Name;
            this.Description = model.Description;
            this.StatusCD = model.StatusCD;
            this.IsMovable = model.IsMovable;
            this.Owner = model.Owner;
            this.PurchaseDate = model.PurchaseDate;
            this.PurchasePrice = model.PurchasePrice;
            this.DepreciationDuration = model.DepreciationDuration;
            this.CategoryCD = model.CategoryCD;
            this.StatusCD = model.StatusCD;
            this.ManufactureDate = model.ManufactureDate;

            this.CreatedDate = this.getDate();
        }

        public Asset()
        {

        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public bool IsMovable { get; set; }
        public string Owner { get; set; }
        public string PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string DepreciationDuration { get; set; }
        public string DisposedDate { get; set; }
        public string ManufactureDate { get; set; }
        public int CategoryCD { get; set; }
        public int StatusCD { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedDate { get; set; }
        public string DeletedBy { get; set; }

    }

    public class AssetViewModel : ViewModel
    {
        public string Code { get; set; }
        public bool IsMovable { get; set; }

        public override string MainCategory
        {
            get { return "0101"; } //asset
        }
        public override string SubCategory
        {
            get
            {
                switch (this.CategoryCD.ToString().Length)
                {
                    case 0:
                        return "0000";
                    case 1:
                        return "000" + this.CategoryCD.ToString();
                    case 2:
                        return "00" + this.CategoryCD.ToString();
                    case 3:
                        return "0" + this.CategoryCD.ToString();
                    case 4:
                        return this.CategoryCD.ToString();
                    default:
                        return this.CategoryCD.ToString();
                }
            } //asset category
        }

    }

    [Table("Location")]
    public class Location : BaseClass
    {
        public Location(LocationViewModel model)
        {
            this.Name = model.Name;
            this.Description = model.Description;
            this.StatusCD = model.StatusCD;
            this.LocationTypeCD = model.LocationTypeCD;
            this.Floor = model.Floor;
            this.StatusCD = model.StatusCD;

            this.CreatedDate = this.getDate();
        }

        public Location()
        {

        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public string Floor { get; set; }
        public int? LocationTypeCD { get; set; }
        public int StatusCD { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }

    public class LocationViewModel : ViewModel
    {
        public string Code { get; set; }

        public override string MainCategory
        {
            get { return "0202"; } //location
        }
        public override string SubCategory
        {
            get
            {
                string category;

                switch (this.LocationTypeCD.ToString().Length)
                {
                    case 1:
                        category = "0" + this.CategoryCD.ToString();
                        break;
                    default:
                        category = this.CategoryCD.ToString();
                        break;
                }

                switch (this.Floor.ToString().Length)
                {
                    case 1:
                        category = "0" + this.Floor.ToString();
                        break;
                    default:
                        category = this.Floor.ToString();
                        break;
                }

                return category;
            } //location category
        }

    }

    public class AssetLocation
    {
        public int ID { get; set; }
        public int AssetID { get; set; }
        public int LocationID { get; set; }
        public string OnTransaction { get; set; }
        public int MovementRequestDetail { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }

}