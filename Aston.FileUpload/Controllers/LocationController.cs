using Aston.FileUpload.DataContexts;
using Aston.FileUpload.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LINQtoCSV;

namespace Aston.FileUpload.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class LocationController : Controller
    {

        // GET: FlatFile/UploadAndSave
        /// <summary>
        /// Show UploadAndSave Page.
        /// </summary>
        /// <returns>UploadAndSave View</returns>
        public ActionResult UploadAndSave()
        {
            if (TempData["prevUploadStatus"] != null)
            {
                ViewBag.MessageColor = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Green" : "Red";
                ViewBag.Message = Convert.ToBoolean(TempData["prevUploadStatus"]) ? "Success" : "Failed";
                ViewBag.Status = true;
            }
            else
            {
                ViewBag.Status = false;
            }
            TempData.Remove("prevUploadStatus");

            return View();
        }

        // POST: FlatFile/UploadAndSave
        /// <summary>
        /// Upload file and save data to db.
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <returns>UploadAndSave View with previous upload status</returns>
        [HttpPost]
        //public ActionResult UploadAndSave(ViewModel obj, HttpPostedFileBase file)
        public ActionResult UploadAndSave(HttpPostedFileBase file)

        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    bool useExtension = ConfigurationManager.AppSettings["useExtension"].Equals("1");
                    bool useRealFileName = ConfigurationManager.AppSettings["useRealFileName"].Equals("1");
                    string _path = ConfigurationManager.AppSettings["rootPath"];
                    string _fileExtenstion = useExtension ? Path.GetExtension(file.FileName) : "";
                    string _fileName = useRealFileName ? Path.GetFileName(file.FileName) : string.Format("{0}{1}", Guid.NewGuid().ToString(), _fileExtenstion);
                    string _savePath = Path.Combine(Server.MapPath(_path), _fileName);
                    //string _absolutePath = HttpContext.Server.MapPath(_savePath);

                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        // Save to path                   
                        if (System.IO.File.Exists(_savePath))
                        {
                            System.IO.File.Delete(_savePath);
                        }
                        file.SaveAs(_savePath);
                        // END Save to path
                    }

                    char separator = ConfigurationManager.AppSettings["separatorLocation"].ToCharArray()[0];
                    bool haveHeader = ConfigurationManager.AppSettings["haveHeaderLocation"].Equals("1");

                    if (haveHeader)
                    {
                        // replace first line with header from web.config
                        string fileHeader = ConfigurationManager.AppSettings["headerLocation"];
                        string[] lines = System.IO.File.ReadAllLines(_savePath);
                        string[] newLines = new string[lines.Length + 1];
                        newLines[0] = fileHeader;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            newLines[i+1] = lines[i];
                        }
                        System.IO.File.WriteAllLines(_savePath, newLines); 
                    }

                    // Convert file to Linq    
                    CsvContext cc = new CsvContext();
                    List<LocationViewModel> list = cc.Read<LocationViewModel>(_savePath, new CsvFileDescription
                    {
                        SeparatorChar = separator,
                        FirstLineHasColumnNames = haveHeader,
                        FileCultureName = "en-US", // default is the current culture
                        EnforceCsvColumnAttribute = !haveHeader
                    }).ToList();

                    // Insert to DB
                    using (var context = new DBContext())
                    {
                        List<int> listNo = context.Locations.ToList().Select(o => Convert.ToInt32(o.No)).ToList();

                        int lastNumber = listNo.Count > 0 ? listNo.Max() : 0;

                        foreach (LocationViewModel item in list)
                        {
                            if (item != null)
                            {
                                List<Location> listLocation = new List<Location>();

                                for (int i = 0; i < 1; i++)
                                {
                                    lastNumber++;
                                    item.No = lastNumber.ToString();

                                    Location newObj = new Location(item);
                                    newObj.No = item.Number;
                                    newObj.Code = item.GenerateCode();
                                    newObj.CreatedBy = "System";
                                    listLocation.Add(newObj);
                                }

                                context.Locations.AddRange(listLocation); 
                            }
                        }

                        context.SaveChanges();
                    }

                    TempData["prevUploadStatus"] = true;
                    return RedirectToAction("UploadAndSave");
                }
                else
                {
                    // Do something if no file uploaded
                    return RedirectToAction("UploadAndSave");
                }

            }
            catch (Exception ex)
            {
                // set to false if doesn't pass validation
                TempData["prevUploadStatus"] = false;
                return RedirectToAction("UploadAndSave");
            }
        }

        // GET: FlatFile/Index
        /// <summary>
        /// Show Index page that contain list of uploaded file.
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult Index()
        {
            List<Location> list = new List<Location>();

            using (var context = new DBContext())
            {
                list = context.Locations.ToList();
            }

            return View(list);
        }

    }
}
