using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Classes;
using Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/CashIn")]
    public class CashInController : Controller
    {
        private readonly wallet2Context db;
        private Helper help;
        public CashInController(wallet2Context context)
        {
            this.db = context;
            help = new Helper();
        }



        [HttpGet("SearchBy/{Digits}/{SearchType}")]
        public IActionResult GetAllBranchsByBankId(string Digits, int SearchType)
        {
            try
            {
                if (SearchType == 1)
                {
                    if (string.IsNullOrEmpty(Digits))
                    {
                        return StatusCode(406, "الرجاء ادخال الرقم الوطني");
                    }

                    if (Digits.Length < 12)
                    {
                        return StatusCode(406, "الرجاء ادخال الرقم الوطني بطريقة الصحيحه");
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(Digits))
                    {
                        return StatusCode(406, "الرجاء ادخال رقم الهاتف");
                    }
                    if (Digits.Length < 9)
                    {
                        return StatusCode(406, "الرجاء ادخال رقم الهاتف بطريقة الصحيحه");
                    }

                }
                IQueryable<PersonalInfo> PersonalInfoQuery;
                if (SearchType == 1)
                {
                    PersonalInfoQuery = from p in db.PersonalInfo
                                        where p.Status == 3 && p.Nid == Digits
                                        select p;
                }
                else
                {
                    PersonalInfoQuery = from p in db.PersonalInfo
                                        where p.Status == 3 && p.Phone == Digits.Substring(Digits.Length - 4)
                                        select p;
                }

                if (PersonalInfoQuery.Count() != 1)
                {
                    return StatusCode(404, "لاتوجد بيانات اشتراك لهذا المواطن , الرجاء انشاء حافظه اولا");
                }


                var PersonalInfoList = (from p in PersonalInfoQuery

                                        select new
                                        {
                                            p.Name,
                                            p.FatherName,
                                            p.SurName,
                                            p.GrandName,
                                            p.PassportNumber,
                                            p.CreatedOn,
                                            p.Phone,
                                            p.Nid,
                                            p.Id,
                                            p.DepositType


                                        }).Single();

                var BranchId = this.help.GetCurrentBranche(HttpContext);

                var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;

                var UserType = this.help.GetCurrentUserType(HttpContext);
                // admin see every thing
                dynamic CashIn = null;
                if (UserType==1)
                {
                     CashIn = db.BanksysBankActions.Where(x => x.PersonalInfoId == PersonalInfoList.Id && x.CashIn.DepositType == 3 && x.CashIn.Refrence == 3 && x.ActionType==3).Select(t => new { t.CashIn.Valuedigits,t.CashIn.NumInvoiceDep,t.CashInId,t.CashIn.Description,t.Branch,t.Branch.Bank,t.CashIn.Status,t.ActionDate}).ToList();
                } else if (UserType == 2)
                {
                    CashIn = db.BanksysBankActions.Where(x => x.Branch.BankId == BankId && x.ActionType == 3).Select(t => new { t.CashIn.Valuedigits, t.CashIn.NumInvoiceDep, t.CashInId, t.CashIn.Description, t.Branch, t.Branch.Bank, t.CashIn.Status, t.ActionDate }).ToList();
                }
                else
                {
                     CashIn = db.BanksysBankActions.Where(x=>x.BranchId == BranchId && x.ActionType == 3).Select(t => new { t.CashIn.Valuedigits, t.CashIn.NumInvoiceDep, t.CashInId, t.CashIn.Description, t.Branch, t.Branch.Bank, t.CashIn.Status, t.ActionDate }).ToList();
                }
              
                return Ok(new { PersonalInfo = PersonalInfoList, CashIn = CashIn });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("Add")]
        public IActionResult AddCashIn([FromBody] CashInObj CashInData)
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (CashInData == null)
                    {
                        transaction.Rollback();
                        return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                    }

                    var userId = this.help.GetCurrentUser(HttpContext);

                    if (userId <= 0)
                    {
                        transaction.Rollback();
                        return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                    }

                    var UserType = this.help.GetCurrentUserType(HttpContext);
                    if (UserType == 1)
                    {
                        transaction.Rollback();
                        return StatusCode(401, "عفوا لايمكن اضافة قيمة نقدية الا عن طريق المصرف");
                    }


                    CashIn Cash = new CashIn();
                    Cash.Status = 1;
                    Cash.Refrence = 3;
                    Cash.Valuedigits = CashInData.Valuedigits;
                    Cash.DepositType = 3;
                    Cash.Description = CashInData.description;
                    Cash.NumInvoiceDep = CashInData.NumInvoiceDep;
                    Cash.PersonalId = CashInData.PersonalId;

                    db.CashIn.Add(Cash);
                    db.SaveChanges();

                    BanksysBankActions BA = new BanksysBankActions();
                    BA.ActionType = 3;
                    BA.Description = "إنشاء عملية نقدية - تأكيد مبدئي";
                    BA.UserId = userId;
                    BA.CashInId = Cash.CashInId;
                    BA.BranchId = this.help.GetCurrentBranche(HttpContext);
                    BA.UserType = UserType;
                    BA.ActionDate = DateTime.Now;
                    db.BanksysBankActions.Add(BA);
                    db.SaveChanges();
                    transaction.Commit();

                    return Ok("لقد قمت بتسـجيل بيانات العملية النقدية");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return StatusCode(500, e.Message);
                }
            }
        }




        [HttpPost("{CashInId}/delete")]
        public IActionResult delete(long CashInId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var userId = this.help.GetCurrentUser(HttpContext);
                    if (userId <= 0)
                    {
                        transaction.Rollback();
                        return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                    }

                    var CashInData = (from p in db.CashIn
                                      where p.CashInId == CashInId
                                        && (p.Status == 1)
                                      select p).SingleOrDefault();

                    if (CashInData == null)
                    {
                        transaction.Rollback();
                        return NotFound("خــطأ : العملية النقدية غير موجوده");
                    }

                    CashInData.Status = 0;

                    db.SaveChanges();

                    BanksysBankActions BA = new BanksysBankActions();
                    BA.ActionType = 6;
                    BA.Description = "رفض عملية نقدية";
                    BA.UserId = userId;
                    BA.CashInId = CashInData.CashInId;
                    BA.BranchId = this.help.GetCurrentBranche(HttpContext);
                    BA.UserType = this.help.GetCurrentUserType(HttpContext);
                    BA.ActionDate = DateTime.Now;
                    db.BanksysBankActions.Add(BA);
                    db.SaveChanges();


                    transaction.Commit();
                    return Ok("تم رفض العملية النقدية");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return StatusCode(500, e.Message);
                }
            }
        }




    }
}