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

namespace BestChoiceQatar.Areas.Backend.Controllers
{
    //[AuthorizeAccess]
    public class UsersController : BackendBaseController
    {
        // GET: Backend/Users
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> UserList(DataTableRequest request)
        {
            var _users = from b in db.Users
                         where b.IsDataActive
                         select new
                         {
                             b.ID,
                             b.Username,
                             b.Email,
                             b.MobileNumber,
                             b.IsSuperAdmin,
                             b.CreatedOn,
                             b.LastUpdatedOn,
                             b.IsDataActive,
                             b.IsActive
                         };

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                request.sSearch = request.sSearch.Trim();
                _users = _users.Where(b => b.CreatedOn.ToString().Contains(request.sSearch)
                || b.Email.ToString().Contains(request.sSearch) || b.Username.Contains(request.sSearch) ||
                b.MobileNumber.Contains(request.sSearch) || (b.IsSuperAdmin ? "Yes" : "No").Contains(request.sSearch));
            }

            switch (request.iSortCol_0)
            {
                case 0:
                    _users = request.sSortDir_0 == "asc" ? _users.OrderBy(b => b.Username) : _users.OrderByDescending(b => b.Username);
                    break;
                case 1:
                    _users = request.sSortDir_0 == "asc" ? _users.OrderBy(b => b.Email) : _users.OrderByDescending(b => b.Email);
                    break;
                case 2:
                    _users = request.sSortDir_0 == "asc" ? _users.OrderBy(b => b.MobileNumber) : _users.OrderByDescending(b => b.MobileNumber);
                    break;
                case 3:
                    _users = request.sSortDir_0 == "asc" ? _users.OrderBy(b => b.IsSuperAdmin) : _users.OrderByDescending(b => b.IsSuperAdmin);
                    break;
                case 4:
                    _users = request.sSortDir_0 == "asc" ? _users.OrderBy(b => b.CreatedOn) : _users.OrderByDescending(b => b.CreatedOn);
                    break;


                default:
                    _users = _users.OrderByDescending(b => b.ID);
                    break;
            }

            var _count = await _users.CountAsync();

            var _data = await _users.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToListAsync();
            List<DataTableRow> _rows = new List<DataTableRow>();

            foreach (var _rowData in _data)
            {
                DataTableRow _row = new DataTableRow();

                _row.Add(@"<a href='/backend/users/details/" + _rowData.ID + "'> " + _rowData.Username + " </a> ");
                _row.Add(_rowData.Email);
                _row.Add(_rowData.MobileNumber);
                _row.Add(_rowData.IsSuperAdmin ? "<span class='label label-success'>Yes</span>" : "<span class='label label-warning'>No</span>");
                _row.Add(_rowData.CreatedOn.ToString());
                _row.Add("<a href='/backend/users/edit/" + _rowData.ID + "' title='Edit' class='mr5'><i class='icon-pencil7'></i></a>");
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
        
        public async Task<ActionResult> Edit(int ID)
        {
            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User _user = await db.Users.FindAsync(ID);

            if (_user == null)
            {
                return HttpNotFound();
            }

            return View(_user);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {

                User _user = await db.Users.FindAsync(model.ID);

                if (_user != null)
                {
                    _user.AuthData = model.Password == null ? _user.AuthData : GetAuthstring(model.Username, model.Password);

                    _user.Email = model.Email;
                    _user.MobileNumber = model.MobileNumber;

                    _user.LastUpdatedOn = Now;

                    await db.SaveChangesAsync();

                    TempData["Notification"] = new Core.Entities.Common.Notification("Success", "Requested User has been saved successfully.");
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
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

            User _user = await db.Users.FindAsync(ID);

            if (_user == null)
            {
                return HttpNotFound();
            }

            return View(_user);
        }
    }
}