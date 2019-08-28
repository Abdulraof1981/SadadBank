using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Principal;
using Common;
using Management.Models;
using Web.Controllers;

namespace Management.Controllers
{
    public class SecurityController : Controller
    {
        [TempData]
        public string ErrorMessage { get; set; }
        private Helper help;
        private readonly wallet2Context db;

        public SecurityController(wallet2Context context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

 



        public class user
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class userPassword
        {
            public string NewPassword { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> loginUser([FromBody] user loginUser)
        {
            try
            {
                if (loginUser == null)
                {
                    return NotFound("There is an error in the email or password");
                }

                //if (!Validation.IsValidEmail(loginUser.Email))
                //{
                //    return BadRequest("Please enter correct email address");
                //}
                if (string.IsNullOrWhiteSpace(loginUser.Email))
                {
                    return BadRequest("Please enter your Login name");
                }

                if (string.IsNullOrWhiteSpace(loginUser.Password))
                {
                    return BadRequest("Please enter your Password");
                }

                var cUser = (from p in db.BanksysUsers
                             where (p.Email ==loginUser.Email || p.LoginName == loginUser.Email) && p.Status != 9
                             select p).SingleOrDefault();

                if (cUser == null)
                {
                    return NotFound("There is an error in the email or password");

                }

                if (cUser.UserType != 1 && cUser.UserType != 2 && cUser.UserType != 3)
                {
                    return BadRequest("You are not authorized to access here");
                }

                if (cUser.Status == 0)
                {
                    return BadRequest("Please activate your account first");
                }
                if (cUser.Status == 2)
                {
                    return BadRequest("Your are account is suspended");
                }

                if (!Security.VerifyHash(loginUser.Password, cUser.Password, HashAlgorithms.SHA512))
                {
                    cUser.LoginTryAttempts++;
                    if (cUser.LoginTryAttempts >= 5 && cUser.Status == 1)
                    {
                        cUser.Status = 2;
                    }
                    db.SaveChanges();
                    return NotFound("There is an error in the email or password");
                }


                cUser.LoginTryAttempts = 0;
                db.SaveChanges();


                // if he has one permission
               
                //var branchName = db.Branch.Where(h => h.BranchId == cUser.BranchId).SingleOrDefault();
                //var BankName = db.Bank.Where(h => h.BankId == cUser.).SingleOrDefault().Name;
                //   if (UserBranch.Count()>0)
                //   {

            

                //}
                //else
                //{
                //    var userInfo = new
                //    {
                //        userId = cUser.UserId,
                //        fullName = cUser.FullName,
                //        userType = cUser.UserType,
                //        LoginName = cUser.LoginName,
                //        DateOfBirth = cUser.DateOfBirth,
                //        Email = cUser.Email,
                //        Gender = cUser.Gender,
                //        Status = cUser.Status,

                //    };
                //}
                // if he has many permission
                const string Issuer = "http://www.sadad.ly";
                var claims = new List<Claim>();
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id", cUser.UserId.ToString(), ClaimValueTypes.Integer64, Issuer));
                claims.Add(new Claim(ClaimTypes.Name, cUser.FullName, ClaimValueTypes.String, Issuer));
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/UserType", cUser.UserType.ToString(), ClaimValueTypes.Integer64, Issuer));
                if (cUser.UserType != 1)
                {

                    claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/BranchId", cUser.BranchId.ToString(), ClaimValueTypes.Integer64, Issuer));
                }
               

                claims.Add(new Claim("userType", cUser.UserType.ToString(), ClaimValueTypes.Integer32, Issuer));                  
                var userIdentity = new ClaimsIdentity("thisisasecreteforauth");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = true,
                        AllowRefresh = true
                    });


                var userInfo = (dynamic) null;
                if (cUser.UserType != 1)
                {
                    var UserBranch = db.BanksysUserBranchs.Where(x => x.UserId == cUser.UserId && x.Status==1);
                    var branch = db.BanksysBranch.Where(h => h.BranchId == cUser.BranchId && cUser.Status==1).SingleOrDefault();
                    var BankName = db.BanksysBank.Where(h => h.BankId == branch.BankId && cUser.Status==1).SingleOrDefault();
                     userInfo = new
                    {
                        userId = cUser.UserId,
                        fullName = cUser.FullName,
                        userType = cUser.UserType,
                        LoginName = cUser.LoginName,
                        DateOfBirth = cUser.DateOfBirth,
                        Email = cUser.Email,
                        Gender = cUser.Gender,
                        Status = cUser.Status,
                        RM = cUser.RegisterMaker,
                        RC = cUser.RegisterChecker,
                        CM = cUser.CashInMaker,
                        CC = cUser.CashInChecker,
                        BranchId = cUser.BranchId,
                        BranchName = cUser.Branch.Name,
                        BankName = cUser.Branch.Bank.Name,
                        BankId = cUser.Branch.BankId,
                        UserBranch = UserBranch.Select(t=> new { t.Branch.Name,t.BranchId,t.RegisterMaker,t.RegisterChecker,t.CashInMaker,t.CashInChecker,t.UserBranchId}).ToList()
                    };
                }
                else
                {
                    //var UserBranch = db.UserBranchs.Where(x => x.UserId == cUser.UserId);
                    //var branch = db.Branch.Where(h => h.BranchId == cUser.BranchId).SingleOrDefault();
                    //var BankName = db.Bank.Where(h => h.BankId == branch.BankId).SingleOrDefault();
                     userInfo = new
                    {
                        userId = cUser.UserId,
                        fullName = cUser.FullName,
                        userType = cUser.UserType,
                        LoginName = cUser.LoginName,
                        DateOfBirth = cUser.DateOfBirth,
                        Email = cUser.Email,
                        Gender = cUser.Gender,
                        Status = cUser.Status,
                        RM = cUser.RegisterMaker,
                        RC = cUser.RegisterChecker,
                        CM = cUser.CashInMaker,
                        CC = cUser.CashInChecker,
                        BranchId = -1,
                        BranchName =-1,
                        BankName = -1,
                        BankId = -1,
                        UserBranch = -1
                    };
                }
             

                return Ok(userInfo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> setClaims(long BranchId)
        {
            try
            {
                const string Issuer = "http://www.sadad.ly";
                var claims = new List<Claim>();


                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/BranchId", BranchId.ToString(), ClaimValueTypes.Integer64, Issuer));

                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id", this.help.GetCurrentUser(HttpContext).ToString(), ClaimValueTypes.Integer64, Issuer));
               
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/UserType", this.help.GetCurrentUserType(HttpContext).ToString(), ClaimValueTypes.Integer64, Issuer));


                var userIdentity = new ClaimsIdentity("thisisasecreteforauth");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = true,
                        AllowRefresh = true
                    });


                return Ok("Done");
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "error while logout");
            }

        }
        [HttpPost]
      
        public IActionResult ChangePassword([FromBody] userPassword loginUser)
        {
            try
            {

                var userId = this.help.GetCurrentUser(HttpContext);
                var User = (from p in db.BanksysUsers
                            where p.UserId == userId && p.Status != 9
                            select p).SingleOrDefault();

                if (Security.VerifyHash(loginUser.Password, User.Password, HashAlgorithms.SHA512))
                {

                    User.Password = Security.ComputeHash(loginUser.NewPassword, HashAlgorithms.SHA512, null);
                    User.ModifiedBy = userId;
                    User.ModifiedOn= DateTime.Now;
                    db.SaveChanges();


                }
                else
                {

                    return BadRequest("«·—Ã«¡ «· «ﬂœ „‰ ﬂ·„… «·„—Ê—");


                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "error while logout");
            }

        }
        [HttpGet]
        public IActionResult GetUserImage(long userId)
        {
            var userimage = (from p in db.BanksysUsers
                             where p.UserId == userId
                             select p.Photo).SingleOrDefault();

            return File(userimage, "image/jpg");
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IActionResult Unsupported()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
