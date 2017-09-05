using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Aston.FileUpload.Models
{
    [Table("UploadedData")]
    public class UploadedData
    {
        [CsvColumn(Name = "ID", FieldIndex = 1)]
        public int ID { get; set; }
        [CsvColumn(Name = "Name", FieldIndex = 2)]
        public string Name { get; set; }
        [CsvColumn(Name = "Description", FieldIndex = 3)]
        public string Description { get; set; }
    }
}