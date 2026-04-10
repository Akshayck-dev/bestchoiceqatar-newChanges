using BestChoiceQatar.Core;
using BestChoiceQatar.Core.Entities.Common;
using BestChoiceQatar.Core.Security;
using BestChoiceQatar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using context = System.Web.HttpContext;

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    [AuthorizeAccess]
    public class AppointmentsController : BackendBaseController
    {
        // GET: Backend/Registrations
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public async Task<JsonResult> List(DataTableRequest request)
        //{
        //    var _appointments = from b in db.Appointments
        //                        where b.IsDataActive
        //                        select new
        //                        {
        //                            b.ID,
        //                            name = b.FullName,
        //                            b.ContactNumber,
        //                            b.Email,
        //                            b.Appointment_Date,
        //                            b.CreatedOn,
        //                            b.IsDataActive,
        //                        };

        //    if (!string.IsNullOrEmpty(request.sSearch))
        //    {
        //        request.sSearch = request.sSearch.Trim();
        //        _appointments = _appointments.Where(b => b.CreatedOn.ToString().Contains(request.sSearch)
        //        || b.name.Contains(request.sSearch) || b.ContactNumber.Contains(request.sSearch) || b.Email.Contains(request.sSearch));
        //    }

        //    switch (request.iSortCol_0)
        //    {
        //        case 0:
        //            _appointments = request.sSortDir_0 == "asc" ? _appointments.OrderBy(b => b.name) : _appointments.OrderByDescending(b => b.name);
        //            break;
        //        case 1:
        //            _appointments = request.sSortDir_0 == "asc" ? _appointments.OrderBy(b => b.ContactNumber) : _appointments.OrderByDescending(b => b.ContactNumber);
        //            break;
        //        case 2:
        //            _appointments = request.sSortDir_0 == "asc" ? _appointments.OrderBy(b => b.Email) : _appointments.OrderByDescending(b => b.Email);
        //            break;
        //        case 3:
        //            _appointments = request.sSortDir_0 == "asc" ? _appointments.OrderBy(b => b.Appointment_Date) : _appointments.OrderByDescending(b => b.Appointment_Date);
        //            break;
        //        case 4:
        //            _appointments = request.sSortDir_0 == "asc" ? _appointments.OrderBy(b => b.CreatedOn) : _appointments.OrderByDescending(b => b.CreatedOn);
        //            break;
        //        default:
        //            _appointments = _appointments.OrderByDescending(b => b.ID);
        //            break;
        //    }

        //    var _count = await _appointments.CountAsync();

        //    var _data = await _appointments.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToListAsync();
        //    List<DataTableRow> _rows = new List<DataTableRow>();

        //    foreach (var _rowData in _data)
        //    {
        //        DataTableRow _row = new DataTableRow();

        //        _row.Add(@"<a href='/backend/appointments/details/" + _rowData.ID + "'> " + _rowData.name + " </a>");

        //        _row.Add(_rowData.ContactNumber);
        //        _row.Add(_rowData.Email);
        //        _row.Add(_rowData.Appointment_Date.ToString());
        //        _row.Add(_rowData.CreatedOn.ToString());
        //        _row.Add("<a class='confirm mr5' data-confirm='Are you sure you want to delete this Appointment?' href='/backend/appointments/delete/" + _rowData.ID + "' title='Delete'><i class='icon-trash'></i></a>");

        //        _rows.Add(_row);
        //    }
        //    return Json(new DataTableResponse
        //    {
        //        sEcho = request.sEcho,
        //        iDisplayLength = request.iDisplayLength,
        //        iTotalRecords = _count,
        //        iDisplayStart = request.iDisplayStart,
        //        iTotalDisplayRecords = _count,
        //        aaData = _rows
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public async Task<ActionResult> Create(Appointment model)
        //{
        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            model.CreatedOn = Now;
        //            model.IsDataActive = true;

        //            db.Appointments.Add(model);
        //            await db.SaveChangesAsync();

        //            TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Appointment has been created successfully.");
        //            return RedirectToAction("Index");

        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        //save exception log in db
        //        Log logger = new Log();
        //        logger.ExceptionMessage = exception.Message.ToString();
        //        logger.ExceptionType = exception.GetType().Name.ToString();
        //        logger.ExceptionSource = exception.StackTrace.ToString();
        //        logger.ExceptionUrl = context.Current.Request.Url.ToString();
        //        logger.Logdate = DateTime.Now.Date;
        //        db.Logs.Add(logger);
        //        await db.SaveChangesAsync();

        //        //send exception log email
        //        Appmanager.SendExceptionLogEmail(exception);

        //    }
        //    TempData["Notification"] = new Core.Entities.Common.Notification("Error", "One or more fields are missing or contains invalid value. Please try again later.");
        //    return View(model);
        //}
        //public async Task<ActionResult> Edit(int ID)
        //{
        //    if (ID == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Appointment _registration = await db.Appointments.FindAsync(ID);

        //    if (_registration == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(_registration);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public async Task<ActionResult> Edit(Appointment model)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        Appointment _registration = await db.Appointments.FindAsync(model.ID);

        //        if (_registration != null)
        //        {

        //            _registration.FullName = model.FullName;
        //            _registration.Appointment_Date = model.Appointment_Date;
        //            _registration.ContactNumber = model.ContactNumber;
        //            _registration.Email = model.Email;

        //            await db.SaveChangesAsync();

        //            TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Requested Appointment has been saved successfully.");
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //    }

        //    TempData["Notification"] = new Core.Entities.Common.Notification("Error", "One or more fields are missing or contains invalid value. Please try again later.");
        //    return View(model);
        //}

        //public async Task<ActionResult> Details(int ID)
        //{
        //    if (ID == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Appointment _registration = await db.Appointments.FindAsync(ID);

        //    if (_registration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(_registration);
        //}


        //public async Task<ActionResult> Delete(int ID)
        //{
        //    Appointment _registration = await db.Appointments.FindAsync(ID);

        //    if (_registration != null)
        //    {
        //        _registration.DeletedOn = Now;
        //        _registration.IsDataActive = false;

        //        TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Requested Appointment has been deleted successfully.");
        //        await db.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> GetEmails(string q, int i, bool all = false)
        //{
        //    int _skip = i == 1 ? 0 : (i - 1) * 20;

        //    var _query = from r in db.Appointments
        //                 where r.IsDataActive && r.Email.Contains(q)
        //                 orderby r.Email
        //                 select new
        //                 {
        //                     r.ID,
        //                     r.Email,
        //                 };

        //    Select2PagedResult _result = new Select2PagedResult
        //    {
        //        Total = await _query.OrderBy(t => t.Email).CountAsync(),
        //        Results = (await _query.OrderBy(t => t.Email).Skip(_skip).GroupBy(t => t.Email).Select(t => t.FirstOrDefault()).Take(all ? int.MaxValue : 20).ToListAsync()).Select(r => new Select2Item
        //        {
        //            id = r.ID,
        //            text = r.Email
        //        }).ToList()
        //    };

        //    return Json(_result, JsonRequestBehavior.AllowGet);
        //}
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}
    }
}