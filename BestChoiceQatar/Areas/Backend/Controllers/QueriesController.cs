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

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    [AuthorizeAccess]
    public class QueriesController : BackendBaseController
    {
        //: Backend/Queries
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> List(DataTableRequest request)
        {
            var _queries = from b in db.Enquiries
                           where b.IsDataActive
                           select new
                           {
                               b.ID,
                               b.FullName,
                               b.Email,
                               b.ContactNumber,
                               b.Subject,
                               b.Message,
                               b.CreatedOn,
                           };

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                request.sSearch = request.sSearch.Trim();
                _queries = _queries.Where(b => b.CreatedOn.ToString().Contains(request.sSearch)
                || b.FullName.Contains(request.sSearch) || b.Email.Contains(request.sSearch)  || b.Subject.Contains(request.sSearch) || b.Message.Contains(request.sSearch));
            }

            switch (request.iSortCol_0)
            {
                case 0:
                    _queries = request.sSortDir_0 == "asc" ? _queries.OrderBy(b => b.FullName) : _queries.OrderByDescending(b => b.FullName);
                    break;
                case 1:
                    _queries = request.sSortDir_0 == "asc" ? _queries.OrderBy(b => b.Email) : _queries.OrderByDescending(b => b.Email);
                    break;                
                case 2:
                    _queries = request.sSortDir_0 == "asc" ? _queries.OrderBy(b => b.Subject) : _queries.OrderByDescending(b => b.Subject);
                    break;
                case 3:
                    _queries = request.sSortDir_0 == "asc" ? _queries.OrderBy(b => b.ContactNumber) : _queries.OrderByDescending(b => b.ContactNumber);
                    break;
                case 4:
                    _queries = request.sSortDir_0 == "asc" ? _queries.OrderBy(b => b.CreatedOn) : _queries.OrderByDescending(b => b.CreatedOn);
                    break;

                default:
                    _queries = _queries.OrderByDescending(b => b.ID);
                    break;
            }

            var _count = await _queries.CountAsync();

            var _data = await _queries.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToListAsync();
            List<DataTableRow> _rows = new List<DataTableRow>();

            foreach (var _rowData in _data)
            {
                DataTableRow _row = new DataTableRow();

                _row.Add(@"<a href='/backend/queries/details/" + _rowData.ID + "'> " + _rowData.FullName + " </a> ");
                _row.Add(_rowData.Email);
                if (_rowData.Subject !=null)
                {
                    _row.Add(_rowData.Subject);

                }
                else
                {
                    _row.Add("NA");
                }
                if (_rowData.Message!=null && _rowData.Message.ToString().Length > 100)
                {
                    _row.Add(_rowData.Message.ToString().Substring(0, 100) + "...");
                }
                else
                {
                    _row.Add(_rowData.Message.ToString());
                }
               
                _row.Add(_rowData.CreatedOn.ToString());
                _row.Add("<a class='confirm mr5' data-confirm='Are you sure you want to delete this Enquiry?' href='/backend/queries/delete/" + _rowData.ID + "' title='Delete'><i class='icon-trash'></i></a>");

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


        public async Task<ActionResult> Details(int ID)
        {
            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enquiry _query = await db.Enquiries.FindAsync(ID);

            if (_query == null)
            {
                return HttpNotFound();
            }

            return View(_query);
        }
        public async Task<ActionResult> Delete(int ID)
        {
            try
            {
                Enquiry _video = await db.Enquiries.FindAsync(ID);

                if (_video != null)
                {
                    _video.DeletedOn = Now;
                    _video.IsDataActive = false;

                    TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Enquiry has been deleted successfully.");
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
    }
}