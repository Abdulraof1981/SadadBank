using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/User")]
    public class UserController : Controller
    {

        private readonly wallet2Context db;
        private Helper help;
        public UserController(wallet2Context context)
        {
            this.db = context;
            help = new Helper();
        }
        
        [HttpGet("GetData")]
        public IActionResult GetData(int userId)
        {
            try
            {
                var usrId = this.help.GetCurrentUser(HttpContext);
                if (usrId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var user = (from u in db.BanksysUsers
                            where u.UserId == userId
                            select new
                            {
                                u.UserId,
                                u.LoginName,
                                u.FullName,
                                u.UserType,
                                u.Branch,
                                u.Branch.Bank,
                                u.Email,
                                u.Gender,
                                u.DateOfBirth,
                                u.RegisterChecker,
                                u.RegisterMaker,
                                u.CashInChecker,
                                u.CashInMaker,
                                u.Status,
                                BanksysUserBranchs = (from ub in db.BanksysUserBranchs
                                                      where ub.UserId == u.UserId && ub.Status == 1
                                                      select new
                                                      {
                                                          ub.BranchId,
                                                          ub.Branch,
                                                          ub.RegisterChecker,
                                                          ub.RegisterMaker,
                                                          ub.CashInChecker,
                                                          ub.CashInMaker,
                                                          ub.Status
                                                      }).ToList(),
                                savedPermissions = (from ub in u.BanksysUserBranchs
                                                    select new
                                                    {
                                                        ub.UserBranchId,
                                                        ub.BranchId,
                                                        ub.Branch,
                                                        ub.Branch.Bank,
                                                        ub.RegisterChecker,
                                                        ub.RegisterMaker,
                                                        ub.CashInChecker,
                                                        ub.CashInMaker,
                                                    }).ToList()
                            }).SingleOrDefault();
                return Json(new { user });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize,short userType, long BranchId)
        {
            try
            {
                
                IQueryable<BanksysUsers> UserQuery;

                if (userType == 0)
                {
                    UserQuery = from p in db.BanksysUsers
                                where p.Status != 9
                                select p;
                } else
                {

                    if (userType == 1)
                    {
                        UserQuery = from p in db.BanksysUsers
                                    where p.UserType == userType && p.Status != 9
                                    select p;
                    }
                    else
                    {
                        // 2,3
                        if (BranchId != 0)
                        {
                            UserQuery = from p in db.BanksysUsers
                                        where p.UserType == userType && p.BranchId == BranchId && p.Status != 9
                                        select p;
                        } else
                        {
                            UserQuery = from p in db.BanksysUsers
                                        where p.UserType == userType && p.Status != 9
                                        select p;
                        }
                    }

                }

                var UserCount = (from p in UserQuery
                                 select p).Count();

                var UserList = (from p in UserQuery
                                orderby p.CreatedOn descending
                                select new
                                {
                                    p.UserId,
                                    LoginName = p.LoginName,
                                    Email = p.Email,
                                    p.CreatedOn,
                                    p.FullName,
                                    p.Gender,
                                    p.UserType,
                                    p.RegisterChecker,
                                    p.RegisterMaker,
                                    p.CashInChecker,
                                    p.CashInMaker,
                                    p.Status,
                                    UserBranch=p.BanksysUserBranchs.Select(x=>new { x.CashInChecker ,x.CashInMaker,x.RegisterMaker,x.RegisterChecker,x.Branch.Name,x.UserBranchId}),
                                    BankName=p.Branch.Bank.Name,
                                    BranchName=p.Branch.Name,
                                    p.DateOfBirth,
                                }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Users = UserList, count = UserCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] BanksysUsers user)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var userId = this.help.GetCurrentUser(HttpContext);

                    if (string.IsNullOrWhiteSpace(user.LoginName))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادحال اسم المسنخدم بطريقة صحيحة");
                    }

                    if (string.IsNullOrWhiteSpace(user.FullName))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء إدخال الاسم الرباعي");
                    }

                    if (!Validation.IsValidEmail(user.Email))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادخال الايميل بالطريقة الصحيحة");
                    }

                    if (user.Gender != 1 && user.Gender != 2)
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادخال الجنس (ذكر - انثي)");
                    }
                    if (string.IsNullOrWhiteSpace(user.DateOfBirth.ToString()))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء دخال تاريخ الميلاد المستخدم");
                    }
                    if ((DateTime.Now.Year - user.DateOfBirth.Year) < 18)
                    {
                        transaction.Rollback();
                        return BadRequest("يجب ان يكون عمر المستخدم اكبر من 18");
                    }

                    var cLoginName = (from u in db.BanksysUsers
                                      where u.LoginName == user.LoginName
                                      select u).SingleOrDefault();
                    if (cLoginName != null)
                    {
                        transaction.Rollback();
                        return BadRequest(" اسم الدخول موجود مسبقا");
                    }


                    if (user.UserType != 1)
                    {
                        if (user.Permissions == null)
                        {
                            transaction.Rollback();
                            return BadRequest("الرجاء اختيار الصلاحية المطلوبة");

                        }

                    }


                    var cPhone = (from u in db.BanksysUsers
                                  where u.Email == user.Email
                                  select u).SingleOrDefault();
                    if (cPhone != null)
                    {
                        transaction.Rollback();
                        return BadRequest(" البريد الإلكتروني موجود مسبقا");
                    }

                    var cUser = (from u in db.BanksysUsers
                                 where u.Email == user.Email && u.Status != 9
                                 select u).SingleOrDefault();

                    if (cUser != null)
                    {
                        if (cUser.Status == 0)
                        {
                            transaction.Rollback();
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي تقعيل الحساب فقط");
                        }
                        if (cUser.Status == 1 || cUser.Status == 2)
                        {
                            transaction.Rollback();
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي دخول فقط");
                        }
                    }



                    cUser = new BanksysUsers();
                    foreach (var x in user.Permissions)
                    {
                        if (x == 1)
                        {
                            cUser.RegisterMaker = 1;
                        }
                        else if (x == 2)
                        {
                            cUser.RegisterChecker = 1;
                        }
                        else if (x == 3)
                        {
                            cUser.CashInMaker = 1;
                        }
                        else
                        {
                            cUser.CashInChecker = 1;
                        }
                    }


                    cUser.LoginName = user.LoginName;
                    cUser.FullName = user.FullName;
                    cUser.UserType = user.UserType;
                    cUser.Email = user.Email;
                    cUser.DateOfBirth = user.DateOfBirth;
                    cUser.Gender = (short)user.Gender;
                    cUser.LoginTryAttempts = 0;
                    cUser.CreatedBy = userId;
                    cUser.CreatedOn = DateTime.Now;
                    cUser.Password = Security.ComputeHash(user.Password, HashAlgorithms.SHA512, null);
                    cUser.BranchId = user.BranchId;

                    cUser.Photo = Convert.
                        FromBase64String("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////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAADhCSU0EAAAAAAAAAgAAOEJJTQQCAAAAAAACAAA4QklNBDAAAAAAAAEBADhCSU0ELQAAAAAABgABAAAAAjhCSU0ECAAAAAAAEAAAAAEAAAJAAAACQAAAAAA4QklNBB4AAAAAAAQAAAAAOEJJTQQaAAAAAANJAAAABgAAAAAAAAAAAAAAjAAAAIwAAAAKAFUAbgB0AGkAdABsAGUAZAAtADEAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAIwAAACMAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAEAAAAAAABudWxsAAAAAgAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAACMAAAAAFJnaHRsb25nAAAAjAAAAAZzbGljZXNWbExzAAAAAU9iamMAAAABAAAAAAAFc2xpY2UAAAASAAAAB3NsaWNlSURsb25nAAAAAAAAAAdncm91cElEbG9uZwAAAAAAAAAGb3JpZ2luZW51bQAAAAxFU2xpY2VPcmlnaW4AAAANYXV0b0dlbmVyYXRlZAAAAABUeXBlZW51bQAAAApFU2xpY2VUeXBlAAAAAEltZyAAAAAGYm91bmRzT2JqYwAAAAEAAAAAAABSY3QxAAAABAAAAABUb3AgbG9uZwAAAAAAAAAATGVmdGxvbmcAAAAAAAAAAEJ0b21sb25nAAAAjAAAAABSZ2h0bG9uZwAAAIwAAAADdXJsVEVYVAAAAAEAAAAAAABudWxsVEVYVAAAAAEAAAAAAABNc2dlVEVYVAAAAAEAAAAAAAZhbHRUYWdURVhUAAAAAQAAAAAADmNlbGxUZXh0SXNIVE1MYm9vbAEAAAAIY2VsbFRleHRURVhUAAAAAQAAAAAACWhvcnpBbGlnbmVudW0AAAAPRVNsaWNlSG9yekFsaWduAAAAB2RlZmF1bHQAAAAJdmVydEFsaWduZW51bQAAAA9FU2xpY2VWZXJ0QWxpZ24AAAAHZGVmYXVsdAAAAAtiZ0NvbG9yVHlwZWVudW0AAAARRVNsaWNlQkdDb2xvclR5cGUAAAAATm9uZQAAAAl0b3BPdXRzZXRsb25nAAAAAAAAAApsZWZ0T3V0c2V0bG9uZwAAAAAAAAAMYm90dG9tT3V0c2V0bG9uZwAAAAAAAAALcmlnaHRPdXRzZXRsb25nAAAAAAA4QklNBCgAAAAAAAwAAAACP/AAAAAAAAA4QklNBBQAAAAAAAQAAAADOEJJTQQMAAAAAAUrAAAAAQAAAIwAAACMAAABpAAA5bAAAAUPABgAAf/Y/+0ADEFkb2JlX0NNAAH/7gAOQWRvYmUAZIAAAAAB/9sAhAAMCAgICQgMCQkMEQsKCxEVDwwMDxUYExMVExMYEQwMDAwMDBEMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMAQ0LCw0ODRAODhAUDg4OFBQODg4OFBEMDAwMDBERDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCACMAIwDASIAAhEBAxEB/90ABAAJ/8QBPwAAAQUBAQEBAQEAAAAAAAAAAwABAgQFBgcICQoLAQABBQEBAQEBAQAAAAAAAAABAAIDBAUGBwgJCgsQAAEEAQMCBAIFBwYIBQMMMwEAAhEDBCESMQVBUWETInGBMgYUkaGxQiMkFVLBYjM0coLRQwclklPw4fFjczUWorKDJkSTVGRFwqN0NhfSVeJl8rOEw9N14/NGJ5SkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2N0dXZ3eHl6e3x9fn9xEAAgIBAgQEAwQFBgcHBgU1AQACEQMhMRIEQVFhcSITBTKBkRShsUIjwVLR8DMkYuFygpJDUxVjczTxJQYWorKDByY1wtJEk1SjF2RFVTZ0ZeLys4TD03Xj80aUpIW0lcTU5PSltcXV5fVWZnaGlqa2xtbm9ic3R1dnd4eXp7fH/9oADAMBAAIRAxEAPwD1VJJJJSkkkklKSSSSUpJRfYysS8gBVn9QYPoNLvM6JKbaSoHqFnZo/FOOou/OYD8DCSm8kq9ebS/Q+w+fH3o/PCSl0kkklKSSSSUpJJJJT//Q9VSSSSUpJJJJSkHJyG0t01eeB/FFc4NaXHgCSsm2x1jy93JSUs97nu3PMkqKSSSVJJJJKUj4+S+kwdWdx/cgJJKdhrmvaHNMg8FSVDAtIcajw7VvxV9JCkkkklKSSSSU/wD/0fVUkkklKSSSSUgzDGO7zgfisxaeYJx3eUflWYkpSSSSSVJJJJKUkkkkpJQdtzD/ACgtZZFIm5g/lD8q10kKSSSSUpJJJJT/AP/S9VSSSSUpJJJJTC5u6p7fEGFkLUypGO+OY/istJSkkkkkqSSSSUpJJJJSfDbuyG+UlaaysYn12R4rVSQpJJJJSkkkklP/0/VUkkklKSSSSUwuE0vH8k/kWQtlZeRQ6l8fmn6JSUiSSSSSpJJJJSkkkklJ8ITkN8pP4LTVXCxzWPUdy4aDyVpJCkkkklKSSSSU/wD/1PVUkkklKSSSSUpV86vdTu7sM/JWEzgHAtPBEFJTjJKT2lri08tJH3KKSVJJJJKUiUV+pa1nYnX4BDV3p9f0rP7I/KUlN1JJJJCkkkklKSSSSU//1fVUkkklKSSSSUpRssZW3c8wE1trKmFzjp+VZl1z7nbncdh4JKY2ODrHOHDiSPmVFJJJKkkkklKVzBvra01uMEmQeyppJKdpJUsTLiK7Dp+a4/kKupIUkkkkpSSSSSn/1vVUkK3Jqq0Jl37o1Kp251r9GeweXP3pKb1ltdYl7gPyqtZ1Bo0rbPmVSJJMkyT3TJKSW3WXOl544A4Q0kkkqSSSSUpJJJJSkkkklKVirNtrAaQHAePP3qukkp0q82l+jvYfPj70cEESDIPcLGU67bKzLHEfkSQ66Sp1dQHFoj+UP7lY9enbv3jb4pKf/9fu0kkkkqSSSSUpJJJJSkkkklKSSSSUpJJJJSkkkklKSSSSUpJJJJT/AP/ZADhCSU0EIQAAAAAAXQAAAAEBAAAADwBBAGQAbwBiAGUAIABQAGgAbwB0AG8AcwBoAG8AcAAAABcAQQBkAG8AYgBlACAAUABoAG8AdABvAHMAaABvAHAAIABDAEMAIAAyADAAMQA1AAAAAQA4QklNBAYAAAAAAAcABAEBAAEBAP/hDgRodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTExIDc5LjE1ODMyNSwgMjAxNS8wOS8xMC0wMToxMDoyMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1vZGlmeURhdGU9IjIwMTctMTItMDFUMTk6MzQ6MTcrMDI6MDAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MmE0MzNlNTUtNzk5ZC00NTRlLWI1ZTUtYWIwNjFmOTUwNThhIiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZDM3ZThiZTYtZDZiZC0xMWU3LWIxNWEtOTViY2JlMzViMTFhIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSI+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNyZWF0ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBzdEV2dDp3aGVuPSIyMDE3LTEyLTAxVDE5OjM0OjE3KzAyOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cykiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjJhNDMzZTU1LTc5OWQtNDU0ZS1iNWU1LWFiMDYxZjk1MDU4YSIgc3RFdnQ6d2hlbj0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDwvcmRmOlNlcT4gPC94bXBNTTpIaXN0b3J5PiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8P3hwYWNrZXQgZW5kPSJ3Ij8+/+IMWElDQ19QUk9GSUxFAAEBAAAMSExpbm8CEAAAbW50clJHQiBYWVogB84AAgAJAAYAMQAAYWNzcE1TRlQAAAAASUVDIHNSR0IAAAAAAAAAAAAAAAEAAPbWAAEAAAAA0y1IUCAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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");

                    cUser.CreatedOn = DateTime.Now;

                    //1- Active
                    //2- locked
                    //9- deleted not exist
                    cUser.Status = 0;
                    db.BanksysUsers.Add(cUser);
                    db.SaveChanges();

                    if (user.SavedPermissions.Length >0)
                    {
                        foreach(var x in user.SavedPermissions)
                        {
                            BanksysUserBranchs UB = new BanksysUserBranchs();
                            UB.BranchId = x.BranchId;                          
                            if (x.ExtrPer.Contains(1))
                            {
                                UB.RegisterMaker = 1;
                            }
                            if (x.ExtrPer.Contains(2))
                            {
                                UB.RegisterChecker = 1;
                            }
                            if (x.ExtrPer.Contains(3))
                            {
                                UB.CashInMaker = 1;
                            }
                            if (x.ExtrPer.Contains(4))
                            {
                                UB.CashInChecker = 1;
                            }
                            UB.CreatedBy = userId;
                            UB.CreatedOn = DateTime.Now;
                            UB.UserId = cUser.UserId;
                            UB.Status = 1;
                            db.BanksysUserBranchs.Add(UB);
                            db.SaveChanges();
                        }
                    }

                    transaction.Commit();
                    return Ok("تم تسجيل المستخدم بنجاح , الحساب غير مفعل الأن");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return StatusCode(500, e.Message);
                }

            }
        }


        [HttpPost("EditUser")]
        public IActionResult EditUser([FromBody] BanksysUsers user)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var userId = this.help.GetCurrentUser(HttpContext);

                    /*if (string.IsNullOrWhiteSpace(user.LoginName))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادحال اسم المسنخدم بطريقة صحيحة");
                    }*/

                    if (string.IsNullOrWhiteSpace(user.FullName))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء إدخال الاسم الرباعي");
                    }

                    /*if (!Validation.IsValidEmail(user.Email))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادخال الايميل بالطريقة الصحيحة");
                    }*/

                    if (user.Gender != 1 && user.Gender != 2)
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء ادخال الجنس (ذكر - انثي)");
                    }
                    if (string.IsNullOrWhiteSpace(user.DateOfBirth.ToString()))
                    {
                        transaction.Rollback();
                        return BadRequest("الرجاء دخال تاريخ الميلاد المستخدم");
                    }
                    if ((DateTime.Now.Year - user.DateOfBirth.Year) < 18)
                    {
                        transaction.Rollback();
                        return BadRequest("يجب ان يكون عمر المستخدم اكبر من 18");
                    }

                    var cLoginName = (from u in db.BanksysUsers
                                      where u.LoginName == user.LoginName 
                                      select u).SingleOrDefault();
                    if (cLoginName == null || cLoginName.Status == 9)
                    {
                        transaction.Rollback();
                        return BadRequest("هذا المستخدم غير موجود الرجاء المحاولة مرة أخرى");
                    }

                    if (cLoginName.Status == 0)
                    {
                        transaction.Rollback();
                        return BadRequest("يجب تفعيل المستخدم قبل التعديل");
                    }
                    
                    if (user.UserType != 1)
                    {
                        if (user.Permissions == null)
                        {
                            transaction.Rollback();
                            return BadRequest("الرجاء اختيار الصلاحية المطلوبة");
                        }
                    }

                    /*
                    var cPhone = (from u in db.BanksysUsers
                                  where u.Email == user.Email
                                  select u).SingleOrDefault();
                    if (cPhone != null)
                    {
                        transaction.Rollback();
                        return BadRequest(" البريد الإلكتروني موجود مسبقا");
                    }*/

                    /*
                    var cUser = (from u in db.BanksysUsers
                                 where u.Email == user.Email && u.Status != 9
                                 select u).SingleOrDefault();

                    if (cUser != null)
                    {
                        if (cUser.Status == 0)
                        {
                            transaction.Rollback();
                            return BadRequest("هذا المستخدم موجود من قبل يحتاج إلى تقعيل الحساب فقط");
                        }
                        if (cUser.Status == 1 || cUser.Status == 2)
                        {
                            transaction.Rollback();
                            return BadRequest("هذا المستخدم موجود من قبل يحتاج الي دخول فقط");
                        }
                    }*/
                    
                    //cUser = new BanksysUsers();
                    foreach (var x in user.Permissions)
                    {
                        if (x == 1)
                        {
                            cLoginName.RegisterMaker = 1;
                        }
                        else if (x == 2)
                        {
                            cLoginName.RegisterChecker = 1;
                        }
                        else if (x == 3)
                        {
                            cLoginName.CashInMaker = 1;
                        }
                        else
                        {
                            cLoginName.CashInChecker = 1;
                        }
                    }
                    
                    //cLoginName.LoginName = user.LoginName;
                    cLoginName.FullName = user.FullName;
                    //cLoginName.UserType = user.UserType;
                    //cLoginName.Email = user.Email;
                    cLoginName.DateOfBirth = user.DateOfBirth;
                    cLoginName.Gender = (short)user.Gender;
                    //cLoginName.LoginTryAttempts = 0;
                    cLoginName.ModifiedBy = userId;
                    cLoginName.ModifiedOn = DateTime.Now;
                    //cLoginName.Password = Security.ComputeHash(user.Password, HashAlgorithms.SHA512, null);
                    //cLoginName.BranchId = user.BranchId;

                    //cUser.Photo = Convert.FromBase64String("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////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAADhCSU0EAAAAAAAAAgAAOEJJTQQCAAAAAAACAAA4QklNBDAAAAAAAAEBADhCSU0ELQAAAAAABgABAAAAAjhCSU0ECAAAAAAAEAAAAAEAAAJAAAACQAAAAAA4QklNBB4AAAAAAAQAAAAAOEJJTQQaAAAAAANJAAAABgAAAAAAAAAAAAAAjAAAAIwAAAAKAFUAbgB0AGkAdABsAGUAZAAtADEAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAIwAAACMAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAEAAAAAAABudWxsAAAAAgAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAACMAAAAAFJnaHRsb25nAAAAjAAAAAZzbGljZXNWbExzAAAAAU9iamMAAAABAAAAAAAFc2xpY2UAAAASAAAAB3NsaWNlSURsb25nAAAAAAAAAAdncm91cElEbG9uZwAAAAAAAAAGb3JpZ2luZW51bQAAAAxFU2xpY2VPcmlnaW4AAAANYXV0b0dlbmVyYXRlZAAAAABUeXBlZW51bQAAAApFU2xpY2VUeXBlAAAAAEltZyAAAAAGYm91bmRzT2JqYwAAAAEAAAAAAABSY3QxAAAABAAAAABUb3AgbG9uZwAAAAAAAAAATGVmdGxvbmcAAAAAAAAAAEJ0b21sb25nAAAAjAAAAABSZ2h0bG9uZwAAAIwAAAADdXJsVEVYVAAAAAEAAAAAAABudWxsVEVYVAAAAAEAAAAAAABNc2dlVEVYVAAAAAEAAAAAAAZhbHRUYWdURVhUAAAAAQAAAAAADmNlbGxUZXh0SXNIVE1MYm9vbAEAAAAIY2VsbFRleHRURVhUAAAAAQAAAAAACWhvcnpBbGlnbmVudW0AAAAPRVNsaWNlSG9yekFsaWduAAAAB2RlZmF1bHQAAAAJdmVydEFsaWduZW51bQAA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IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTExIDc5LjE1ODMyNSwgMjAxNS8wOS8xMC0wMToxMDoyMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgeG1wOk1vZGlmeURhdGU9IjIwMTctMTItMDFUMTk6MzQ6MTcrMDI6MDAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MmE0MzNlNTUtNzk5ZC00NTRlLWI1ZTUtYWIwNjFmOTUwNThhIiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZDM3ZThiZTYtZDZiZC0xMWU3LWIxNWEtOTViY2JlMzViMTFhIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSI+IDx4bXBNTTpIaXN0b3J5PiA8cmRmOlNlcT4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNyZWF0ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6ZjMzZTk0OWItYWNkYi04MjQxLWIxNTctNDgwNDEyMDdkMzNmIiBzdEV2dDp3aGVuPSIyMDE3LTEyLTAxVDE5OjM0OjE3KzAyOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cykiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjJhNDMzZTU1LTc5OWQtNDU0ZS1iNWU1LWFiMDYxZjk1MDU4YSIgc3RFdnQ6d2hlbj0iMjAxNy0xMi0wMVQxOTozNDoxNyswMjowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDwvcmRmOlNlcT4gPC94bXBNTTpIaXN0b3J5PiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8P3hwYWNrZXQgZW5kPSJ3Ij8+/+IMWElDQ19QUk9GSUxFAAEBAAAMSExpbm8CEAAAbW50clJHQiBYWVogB84AAgAJAAYAMQAAYWNzcE1TRlQAAAAASUVDIHNSR0IAAAAAAAAAAAAAAAEAAPbWAAEAAAAA0y1IUCAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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");

                    //cLoginName.CreatedOn = DateTime.Now;

                    //1- Active
                    //2- locked
                    //9- deleted not exist
                    //cUser.Status = 0;
                    db.BanksysUsers.Update(cLoginName);
                    db.SaveChanges();

                    var userBranchs = (from ub in db.BanksysUserBranchs where ub.UserId == cLoginName.UserId select ub).ToList();
                    if (userBranchs != null && userBranchs.Count > 0)
                    {
                        db.BanksysUserBranchs.RemoveRange(userBranchs);
                        db.SaveChanges();
                    }

                    foreach (var x in user.SavedPermissions)
                    {
                        BanksysUserBranchs UB = new BanksysUserBranchs();
                        UB.BranchId = x.BranchId;
                        if (x.ExtrPer.Contains(1))
                        {
                            UB.RegisterMaker = 1;
                        }
                        if (x.ExtrPer.Contains(2))
                        {
                            UB.RegisterChecker = 1;
                        }
                        if (x.ExtrPer.Contains(3))
                        {
                            UB.CashInMaker = 1;
                        }
                        if (x.ExtrPer.Contains(4))
                        {
                            UB.CashInChecker = 1;
                        }
                        UB.CreatedBy = userId;
                        UB.CreatedOn = DateTime.Now;
                        UB.UserId = cLoginName.UserId;
                        UB.Status = 1;
                        db.BanksysUserBranchs.Add(UB);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                    return Ok("تم تعديل بيانات المستخدم بنجاح");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return StatusCode(500, e.Message);
                }
            }
        }
        
        [HttpGet("{UserId}/image")]
        public IActionResult GetUserImage(long UserId)
        {
            try
            {
                var UserImage = (from p in db.BanksysUsers
                                 where p.UserId == UserId
                                 select p.Photo).SingleOrDefault();

                if (UserImage == null)
                {
                    return NotFound("المستخدم غير موجــود");
                }

                return File(UserImage, "image/jpeg");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost("{UserId}/delete")]
        public IActionResult DeleteUser(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Branch = (from p in db.BanksysUsers
                              where p.UserId == UserId
                              && (p.Status != 1)
                              select p).SingleOrDefault();


                if (Branch == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                Branch.Status = 9;
                Branch.ModifiedBy = userId;
                Branch.ModifiedOn = DateTime.Now;
                db.SaveChanges();
                return Ok("User Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("{CashInId}/UserActions")]
        public IActionResult UserActions(long CashInId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Users = (from p in db.BanksysBankActions
                              where p.CashInId == CashInId
                              select p).Select(x=> new { x.UserType,x.ActionDate,x.User.FullName,x.User.LoginName,x.CashIn.Valuedigits,x.Description,BranchName=x.Branch.Name,BankName=x.Branch.Bank.Name}).ToList();

                db.SaveChanges();
                return Ok(new { Logs = Users });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost("{Status}/{UserId}/Activate")]
        public IActionResult ActivateUser(short Status, long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Users = (from p in db.BanksysUsers
                             where p.UserId == UserId
                             && (p.Status != 9)
                             select p).SingleOrDefault();

                if (Users == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                Users.Status = Status;
                Users.ModifiedBy = userId;
                Users.ModifiedOn = DateTime.Now;
                db.SaveChanges();
                return Ok("User Activted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }





        [HttpGet("GetStatistic")]
        public IActionResult GetStatistic()
        {
            try
            {
                int RM, RC, CC, MC = 0;
                var UserType = this.help.GetCurrentUserType(HttpContext);
                if (UserType == 1) {
                     RM = db.BanksysBankActions.Where(x => x.ActionType == 1).Count();
                     RC = db.BanksysBankActions.Where(x => x.ActionType == 2).Count();
                     CC = db.BanksysBankActions.Where(x => x.ActionType == 4).Count();
                     MC = db.BanksysBankActions.Where(x => x.ActionType == 3).Count();

                } else if (UserType==2)
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);
                    var BankId = db.BanksysBranch.Where(x => x.BranchId == BranchId).SingleOrDefault().BankId;
                     RM = db.BanksysBankActions.Where(x => x.ActionType == 1 && x.Branch.BankId== BankId).Count();
                     RC = db.BanksysBankActions.Where(x => x.ActionType == 2 && x.Branch.BankId == BankId).Count();
                     CC = db.BanksysBankActions.Where(x => x.ActionType == 4 && x.Branch.BankId == BankId).Count();
                     MC = db.BanksysBankActions.Where(x => x.ActionType == 3 && x.Branch.BankId == BankId).Count();
                }
                else
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);
                     RM = db.BanksysBankActions.Where(x => x.ActionType == 1 && x.BranchId == BranchId).Count();
                     RC = db.BanksysBankActions.Where(x => x.ActionType == 2 && x.BranchId == BranchId).Count();
                     CC = db.BanksysBankActions.Where(x => x.ActionType == 4 && x.BranchId == BranchId).Count();
                     MC = db.BanksysBankActions.Where(x => x.ActionType == 3 && x.BranchId == BranchId).Count();

                }

                return Ok(new { RM = RM , RC=RC,CC=CC,MC=MC });
                //    IQueryable<BanksysUsers> UserQuery;

                //    if (userType == 0)
                //    {
                //        UserQuery = from p in db.BanksysUsers
                //                    where p.Status != 9
                //                    select p;
                //    }
                //    else
                //    {

                //        if (userType == 1)
                //        {
                //            UserQuery = from p in db.BanksysUsers
                //                        where p.UserType == userType && p.Status != 9
                //                        select p;
                //        }
                //        else
                //        {
                //            // 2,3
                //            if (BranchId != 0)
                //            {
                //                UserQuery = from p in db.BanksysUsers
                //                            where p.UserType == userType && p.BranchId == BranchId && p.Status != 9
                //                            select p;
                //            }
                //            else
                //            {
                //                UserQuery = from p in db.BanksysUsers
                //                            where p.UserType == userType && p.Status != 9
                //                            select p;
                //            }
                //        }

                //    }

                //    var UserCount = (from p in UserQuery
                //                     select p).Count();

                //    var UserList = (from p in UserQuery
                //                    orderby p.CreatedOn descending
                //                    select new
                //                    {
                //                        p.UserId,
                //                        LoginName = p.LoginName,
                //                        Email = p.Email,
                //                        p.CreatedOn,
                //                        p.FullName,
                //                        p.Gender,
                //                        p.UserType,
                //                        p.RegisterChecker,
                //                        p.RegisterMaker,
                //                        p.CashInChecker,
                //                        p.CashInMaker,
                //                        p.Status,
                //                        UserBranch = p.BanksysUserBranchs.Select(x => new { x.CashInChecker, x.CashInMaker, x.RegisterMaker, x.RegisterChecker, x.Branch.Name, x.UserBranchId }),
                //                        BankName = p.Branch.Bank.Name,
                //                        BranchName = p.Branch.Name,
                //                        p.DateOfBirth,
                //                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                //    return Ok(new { Users = UserList, count = UserCount });
                //}]
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}