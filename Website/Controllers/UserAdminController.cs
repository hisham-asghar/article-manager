using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;
using Website.Models;
using LayerBao;
// ReSharper disable Mvc.ViewNotResolved

namespace RentalsProject.Controllers.Users
{
    //[UserAuthorize(Roles = "Admin")]
    // ReSharper disable once InconsistentNaming
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
            Context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Context));
        }

        public UsersAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ApplicationDbContext Context { get; private set; }

        //
        // GET: /Users/
        public ActionResult Index(string id)
        {
            var users = UserBao.GetUsersByTagName(id);
            var applicationUsers = users.Select(Website.Models.MyStores.MyUserStore.ToApplicationUser).ToList();
            return View(applicationUsers);
        }

        ////
        //// GET: /Users/Details/5
        //public async Task<ActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);
            
        //    return View(user);
        //}

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create(string returnUrl)
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult>  Create(RegisterViewModel userViewModel, string roleId,string phoneNumber, List<string> userRoles, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = userViewModel.Email,
                    UserName = userViewModel.Email,
                    
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {

                    var ur = userRoles.Where(u => u != null && u.Trim() != "").ToList();
                    var result = UserBao.UpdateRoles(user.Id, user.Email, ur);
                    
                    return returnUrl == null
                        ? RedirectToAction("Details", "UsersAdmin", new { id = user.Id })
                        : RedirectToRoute(returnUrl);
                    //if (!string.IsNullOrEmpty(roleId))
                    //{
                    //    //Find Role Admin
                    //    var role = await RoleManager.FindByIdAsync(roleId);
                    //    var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                    //    //UserDAO.Update(new AspNetUser() {PersonName = user.PersonName,PhoneNumber = user.PhoneNumber,user}, user.Id);

                    //    if (result.Succeeded)
                    //        return RedirectToAction("Index");

                    //    ModelState.AddModelError("", result.Errors.First());
                    //    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
                    //    return View();
                    //}
                }

                ModelState.AddModelError("", adminresult.Errors.First());
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            ViewBag.Roles = RoleManager.Roles.ToList();

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserName,Id,HomeTown")] ApplicationUser formuser, string id,List<string> userRoles,string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            var user = await UserManager.FindByIdAsync(id);

            var email = Request.Form["Email"];
            if (!string.IsNullOrWhiteSpace(email))
                user.UserName = email;
            
            var name = Request.Form["Name"];
            if (!string.IsNullOrWhiteSpace(name))
                user.Name = name;

            if (ModelState.IsValid)
            {
                //Update the user details
                await UserManager.UpdateAsync(user);
                var aspuser = UserBao.GetUserById(id);
                aspuser.Name = user.Name;
                UserBao.Update(aspuser, id);
                //If user has existing Role then remove the user from the role
                // This also accounts for the case when the Admin selected Empty from the drop-down and
                // this means that all roles for the user must be removed

                var ur = userRoles.Where(u => u != null && u.Trim() != "").ToList();
                var result = UserBao.UpdateRoles(id, user.Email, ur);
                
                if (!result)
                {
                    //ModelState.AddModelError("", result.Errors.First());
                    //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                    return View();
                }
                //var rolesForUser = await UserManager.GetRolesAsync(id);
                //if (rolesForUser.Any())
                //{   
                //    foreach (var item in rolesForUser)
                //    {
                //        var result = await UserManager.RemoveFromRoleAsync(id,item);
                //    }
                //}


                //if (!string.IsNullOrEmpty(RoleId))
                //{

                //    //Find Role
                //    var role = await RoleManager.FindByIdAsync(RoleId);
                //    //Add user to new role
                //    var result = await UserManager.AddToRoleAsync(id,role.Name);
                //    if (!result.Succeeded)
                //    {
                //        ModelState.AddModelError("", result.Errors.First());
                //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                //        return View();
                //    }
                //}
                return returnUrl == null
                    ? RedirectToAction("Index","UsersAdmin")
                    : RedirectToRoute(returnUrl);
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View(user);
            }
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                bool res = UserBao.RemoveUserById(id);
                //var user = await UserManager.FindByIdAsync(id);
                //var logins = user.Logins;
                //foreach (var login in logins)
                //{
                //    UserManager.RemoveLogin(id,login.);
                //}
                //var rolesForUser = await IdentityManager.Roles.GetRolesForUserAsync(id, CancellationToken.None);
                //if (rolesForUser.Count() > 0)
                //{

                //    foreach (var item in rolesForUser)
                //    {
                //        var result = await IdentityManager.Roles.RemoveUserFromRoleAsync(user.Id, item.Id, CancellationToken.None);
                //    }
                //}
                //context.Users.Remove(user);
                //await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
