using BestChoiceQatar.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BestChoiceQatar.Models;
using System.IO;
using System.Net;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    public class GalleryController : BackendBaseController
    {
        // GET: Backend/Gallery
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> List(DataTableRequest request)
        {
            var _news = from b in db.Galleries
                        where b.IsDataActive && (b.Category == "Portable Cabin Fabrication"|| b.Category == "Building Painting Contractor" || b.Category == "Steel & Aluminum Fabrication" || b.Category == "General Contracting" || b.Category == "Optical Fibre Networks")
                        select new
                        {
                            b.ID,
                            b.Title,
                            b.Description,
                            b.Image,
                            b.CreatedOn,
                            b.IsDataActive,
                        };

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                request.sSearch = request.sSearch.Trim();
                _news = _news.Where(b => b.CreatedOn.ToString().Contains(request.sSearch)
                || b.Title.ToString().Contains(request.sSearch) || b.Description.ToString().Contains(request.sSearch));

            }

            switch (request.iSortCol_0)
            {

                case 0:
                    _news = request.sSortDir_0 == "asc" ? _news.OrderBy(b => b.Title) : _news.OrderByDescending(b => b.Title);
                    break;
                case 1:
                    _news = request.sSortDir_0 == "asc" ? _news.OrderBy(b => b.Description) : _news.OrderByDescending(b => b.Description);
                    break;
                case 2:
                    _news = request.sSortDir_0 == "asc" ? _news.OrderBy(b => b.CreatedOn) : _news.OrderByDescending(b => b.CreatedOn);
                    break;

                default:
                    _news = _news.OrderByDescending(b => b.ID);
                    break;
            }

            var _count = await _news.CountAsync();

            var _data = await _news.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToListAsync();
            List<DataTableRow> _rows = new List<DataTableRow>();

            foreach (var _rowData in _data)
            {
                DataTableRow _row = new DataTableRow();
                _row.Add(_rowData.Title.ToString());
                //if (_rowData.Description.ToString().Length > 100)
                //{
                //    _row.Add(_rowData.Description.ToString().Substring(0, 100) + "...");
                //}
                //else
                //{
                //    _row.Add(_rowData.Description.ToString());
                //}
                _row.Add("<img src='/Uploads/Products/" + _rowData.Image + "' style='width: 100px; height: 100px; ' />");
                _row.Add(_rowData.CreatedOn.ToString());
                _row.Add("<a href='/backend/gallery/edit/" + _rowData.ID + "' title='Edit' class='mr5'><i class='icon-pencil7'></i></a> <a class='confirm mr5' data-confirm='Are you sure you want to delete this gallery?' href='/backend/gallery/delete/" + _rowData.ID + "' title='Delete'><i class='icon-trash'></i></a>");
                _rows.Add(_row);
            }

            return Json(new DataTableResponse
            {
                sEcho = request.sEcho,
                iDisplayLength = request.iDisplayLength,
                iTotalRecords = _count,
                iDisplayStart = request.iDisplayStart,
                iTotalDisplayRecords = _count,
                aaData = _rows
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Gallery model, string IPBillFile)
        {
            try
            {
                TryValidateModel(model);
                ModelState.Clear();
                if (ModelState.IsValid)
                {
                    string _image = string.Empty;
                    if (model.Photo != null)
                    {
                        _image = Path.GetFileName(model.Photo.FileName);

                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/products"), _image)))
                        {
                            _image = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                        }

                        var _path = Path.Combine(Server.MapPath("~/uploads/products"), _image);
                        model.Photo.SaveAs(_path);
                    }
                    if (!string.IsNullOrEmpty(_image))
                    {
                        model.Image = _image;
                    }
                    model.Image = model.Image;

                    string[] _IPFile = IPBillFile.Split('|');
                    string _IPFileName = null;
                    string IPBillName = null;
                    if (model.FileUpload != null)
                    {
                        foreach (HttpPostedFileBase File in model.FileUpload)
                        {
                            if (File != null)
                            {
                                IPBillName = Path.GetFileName(File.FileName);
                                if (_IPFile.Contains(IPBillName))
                                {

                                    if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/products"), IPBillName)))
                                    {
                                        IPBillName = "ClientFile_" + Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                                    }

                                    var _path = Path.Combine(Server.MapPath("~/uploads/products"), IPBillName);
                                    File.SaveAs(_path);

                                    if (_IPFileName != null)
                                    {
                                        _IPFileName = _IPFileName + "|" + IPBillName;
                                    }
                                    else
                                    {
                                        _IPFileName = IPBillName;
                                    }
                                }
                            }
                        }
                        model.ChildImages = _IPFileName;
                    }


                    model.CreatedOn = Now;
                    model.LastUpdatedOn = Now;
                    model.IsDataActive = true;
                    model.Title = model.Title;
                    model.Description = model.Description;
                    model.Category = model.Category;

                    db.Galleries.Add(model);
                    await db.SaveChangesAsync();

                    TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Services has been created successfully.");
                    return RedirectToAction("Index");

                }
            }
            catch (Exception exception)
            {
                //save exception log in db
                //Log logger = new Log();
                //logger.ExceptionMessage = exception.Message.ToString();
                //logger.ExceptionType = exception.GetType().Name.ToString();
                //logger.ExceptionSource = exception.StackTrace.ToString();
                //logger.ExceptionUrl = context.Current.Request.Url.ToString();
                //logger.Logdate = DateTime.Now.Date;
                //db.Logs.Add(logger);
                //await db.SaveChangesAsync();

                ////send exception log email
                //Appmanager.SendExceptionLogEmail(exception);
            }
            TempData["Notification"] = new Core.Entities.Common.Notification("Error", "One or more fields are missing or contains invalid value. Please try again later.");
            return View(model);
        }
        public async Task<ActionResult> Edit(int ID)
        {

            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Gallery _brochure = await db.Galleries.FindAsync(ID);

            if (_brochure == null)
            {
                return HttpNotFound();
            }
            return View(_brochure);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Gallery model, string IPBillFile)
        {
            try
            {
                TryValidateModel(model);
                ModelState.Clear();
                if (ModelState.IsValid)
                {

                    Gallery _video = await db.Galleries.FindAsync(model.ID);

                    if (_video != null)
                    {
                        string _image = string.Empty;
                        if (model.Photo != null)
                        {
                            _image = Path.GetFileName(model.Photo.FileName);

                            if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/products"), _image)))
                            {
                                _image = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                            }

                            var _path = Path.Combine(Server.MapPath("~/uploads/products"), _image);
                            model.Photo.SaveAs(_path);
                        }
                        if (!string.IsNullOrEmpty(_image))
                        {
                            _video.Image = _image;
                        }

                        string[] _IPFile = IPBillFile.Split('|');

                        string IPBillName = null;
                        string _IPFileName = null;
                        if (model.FileUpload != null && model.FileUpload.Count() > 0)
                        {
                            foreach (HttpPostedFileBase File in model.FileUpload)
                            {
                                if (File != null)
                                {
                                    IPBillName = Path.GetFileName(File.FileName);
                                    if (_IPFile.Contains(IPBillName))
                                    {

                                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/uploads/products"), IPBillName)))
                                        {
                                            IPBillName = "ClientFile_" + Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                                        }

                                        var _path = Path.Combine(Server.MapPath("~/uploads/products"), IPBillName);
                                        File.SaveAs(_path);

                                        if (_IPFileName != null)
                                        {
                                            _IPFileName = _IPFileName + "|" + IPBillName;
                                        }
                                        else
                                        {
                                            _IPFileName = IPBillName;
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(_IPFileName))
                            {
                                if (!string.IsNullOrEmpty(_video.ChildImages))
                                {
                                    _video.ChildImages = _IPFileName + "|" + _video.ChildImages;
                                }
                                else
                                {
                                    _video.ChildImages = _IPFileName;
                                }

                            }
                        }
                        _video.LastUpdatedOn = Now;
                        _video.Title = model.Title;
                        _video.Description = model.Description;

                        await db.SaveChangesAsync();

                        TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Requested gallery has been saved successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception exception)
            {
                //save exception log in db
                //Log logger = new Log();
                //logger.ExceptionMessage = exception.Message.ToString();
                //logger.ExceptionType = exception.GetType().Name.ToString();
                //logger.ExceptionSource = exception.StackTrace.ToString();
                //logger.ExceptionUrl = context.Current.Request.Url.ToString();
                //logger.Logdate = DateTime.Now.Date;
                //db.Logs.Add(logger);
                //await db.SaveChangesAsync();

                ////send exception log email
                //Appmanager.SendExceptionLogEmail(exception);
            }
            TempData["Notification"] = new Core.Entities.Common.Notification("Error", "One or more fields are missing or contains invalid value. Please try again later.");
            return View(model);
        }

        public async Task<ActionResult> Details(int ID)
        {
            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Gallery _brochure = await db.Galleries.FindAsync(ID);

            if (_brochure == null)
            {
                return HttpNotFound();
            }

            return View(_brochure);
        }
        public async Task<ActionResult> Delete(int ID)
        {
            try
            {
                Gallery _video = await db.Galleries.FindAsync(ID);

                if (_video != null)
                {
                    _video.DeletedOn = Now;
                    _video.IsDataActive = false;

                    TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Requested gallery has been deleted successfully.");
                    await db.SaveChangesAsync();
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception exception)
            {
                //save exception log in db
                //Log logger = new Log();
                //logger.ExceptionMessage = exception.Message.ToString();
                //logger.ExceptionType = exception.GetType().Name.ToString();
                //logger.ExceptionSource = exception.StackTrace.ToString();
                //logger.ExceptionUrl = context.Current.Request.Url.ToString();
                //logger.Logdate = DateTime.Now.Date;
                //db.Logs.Add(logger);
                //await db.SaveChangesAsync();

                ////send exception log email
                //Appmanager.SendExceptionLogEmail(exception);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteDocumentFile(int ID, string File)
        {
            Gallery _insurancePayment = await db.Galleries.FirstOrDefaultAsync(f => f.ID == ID && f.IsDataActive && f.ChildImages.Contains(File));

            string _fileName = null;
            string _newFile = null;

            if (_insurancePayment != null)
            {
                _fileName = _insurancePayment.ChildImages.Replace(File, "");


                _fileName = string.Join("|", _fileName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                if (_fileName != null)
                {
                    _insurancePayment.ChildImages = _fileName;

                    await db.SaveChangesAsync();

                    string path = Server.MapPath(@"~/uploads/products" + File);

                    FileInfo file = new FileInfo(path);

                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    return Json(new { file = _fileName, status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }
    }
}