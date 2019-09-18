using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Management.Classes;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/CashIn")]
    public class CashInController : Controller
    {
        private readonly wallet2Context db;
        private Helper help;
        private Models.MpayCashIn _Mpays;
        private Models.settings _Settings;
        private HttpClient client = new HttpClient();
        private readonly IHostingEnvironment _hostingEnvironment;
        public CashInController(wallet2Context context, IOptions<Models.MpayCashIn> MpaysCashIn, IOptions<Models.settings> settings, IHostingEnvironment hostingEnvironment)
        {
            _Mpays = MpaysCashIn.Value;
            _Settings = settings.Value;
            this.db = context;
            help = new Helper();
            _hostingEnvironment = hostingEnvironment;
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
                string CorrectPhone = Digits.Substring(Digits.Length - 9);
                if (SearchType == 1)
                {
                    PersonalInfoQuery = from p in db.PersonalInfo
                                        where p.Status == 3 && p.Nid == Digits
                                        select p;
                }
                else
                {
                    PersonalInfoQuery = from p in db.PersonalInfo
                                        where p.Status == 3 && p.Phone == CorrectPhone
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
         
                var UserType = this.help.GetCurrentUserType(HttpContext);
                // admin see every thing
                dynamic CashIn = null;
                if (UserType==1)
                {
                     CashIn = db.BanksysBankActions.Where(x => x.CashIn.Personal.Id == PersonalInfoList.Id && x.CashIn.DepositType == 3 && x.CashIn.Refrence == 3 && x.ActionType==3).Select(t => new { t.CashIn.Valuedigits,t.CashIn.NumInvoiceDep,t.CashInId,t.CashIn.Description,t.Branch,t.Branch.Bank,t.CashIn.Status,t.ActionDate,
                         FName = t.CashIn.Personal.Name,
                         t.CashIn.Personal.FatherName,
                         t.CashIn.Personal.SurName,
                         t.CashIn.Personal.GrandName,
                         t.CashIn.Personal.Nid,
                         t.CashIn.Personal.Phone
                     }).OrderByDescending(x => x.ActionDate).ToList();
                } else if (UserType == 2)
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);

                    var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                    CashIn = db.BanksysBankActions.Where(x => x.CashIn.Personal.Id == PersonalInfoList.Id &&  x.CashIn.DepositType == 3 && x.CashIn.Refrence == 3 && x.Branch.BankId == BankId && x.ActionType == 3).Select(t => new { t.CashIn.Valuedigits, t.CashIn.NumInvoiceDep, t.CashInId, t.CashIn.Description, t.Branch, t.Branch.Bank, t.CashIn.Status, t.ActionDate,
                        FName = t.CashIn.Personal.Name,
                        t.CashIn.Personal.FatherName,
                        t.CashIn.Personal.SurName,
                        t.CashIn.Personal.GrandName,
                        t.CashIn.Personal.Nid,
                        t.CashIn.Personal.Phone
                    }).OrderByDescending(x => x.ActionDate).ToList();
                }
                else
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);

                    var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                    CashIn = db.BanksysBankActions.Where(x=> x.CashIn.Personal.Id == PersonalInfoList.Id && x.CashIn.DepositType == 3 && x.CashIn.Refrence == 3 && x.BranchId == BranchId && x.ActionType == 3).Select(t => new { t.CashIn.Valuedigits, t.CashIn.NumInvoiceDep, t.CashInId, t.CashIn.Description, t.Branch, t.Branch.Bank, t.CashIn.Status, t.ActionDate,
                         FName = t.CashIn.Personal.Name,
                         t.CashIn.Personal.FatherName,
                         t.CashIn.Personal.SurName,
                         t.CashIn.Personal.GrandName,
                         t.CashIn.Personal.Nid,
                         t.CashIn.Personal.Phone
                     }).OrderByDescending(x=>x.ActionDate).ToList();
                }
              
                return Ok(new { PersonalInfo = PersonalInfoList, CashIn = CashIn });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {   
                IQueryable<BanksysBankActions> BankActionsQuery;
                var UserType = this.help.GetCurrentUserType(HttpContext);
                // admin see every thing

                if (UserType == 1)
                {
                    BankActionsQuery = from t in db.BanksysBankActions
                                     where  t.ActionType == 3
                                       select t;
                }
                else if (UserType == 2)
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);

                    var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                    BankActionsQuery = from t in db.BanksysBankActions
                                       where t.Branch.BankId == BankId && t.ActionType == 3
                                       select t;
                }   
                 else
                {
                    var BranchId = this.help.GetCurrentBranche(HttpContext);

                    var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                    BankActionsQuery = from t in db.BanksysBankActions
                                       where  t.BranchId == BranchId && t.ActionType == 3
                                       select t;
                }
                  

                var CashInCount = (from t in BankActionsQuery
                                 select t).Count();

                var CashInList = (from t in BankActionsQuery
                                orderby t.ActionDate descending
                                select new
                                {
                                    t.CashIn.Valuedigits,
                                    t.CashIn.NumInvoiceDep,
                                    t.CashInId,
                                    t.CashIn.Description,
                                    t.Branch,
                                    t.Branch.Bank,
                                    t.CashIn.Status,
                                    t.ActionDate,
                                    FName = t.CashIn.Personal.Name,
                                    t.CashIn.Personal.FatherName,
                                    t.CashIn.Personal.SurName,
                                    t.CashIn.Personal.GrandName,
                                    t.CashIn.Personal.Nid,
                                    t.CashIn.Personal.Phone
                                }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { CashIn = CashInList, count = CashInCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
                    Cash.BankId =(int) db.BanksysBranch.Where(x => x.BranchId == this.help.GetCurrentBranche(HttpContext)).SingleOrDefault().BankId;
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



        [HttpPost("{CashInId}/Confirm")]
        public IActionResult Confirm(long CashInId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var userId = this.help.GetCurrentUser(HttpContext);

                    if (userId <= 0)
                    {
                        trans.Rollback();
                        return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                    }

                    //var UserInfo = db.BanksysUsers.Where(x => x.UserId == userId).SingleOrDefault();


                    var UserInfov1 = from x in db.BanksysBankActions
                                     where  x.CashInId == (int)CashInId
                                     select x;
                



              

                var UserInfo = (from t in UserInfov1
                                orderby t.ActionDate descending
                                  select new
                                  {
                                      t.User.FullName,
                                      t.CashIn.Valuedigits,
                                      t.CashIn.NumInvoiceDep,
                                      t.CashInId,
                                      t.CashIn.Description,
                                      t.Branch,
                                      t.Branch.Bank,
                                      t.CashIn.Status,
                                      t.ActionDate,
                                      FName = t.CashIn.Personal.Name,
                                      t.CashIn.Personal.FatherName,
                                      t.CashIn.Personal.SurName,
                                      t.CashIn.Personal.GrandName,
                                      t.CashIn.Personal.Nid,
                                      t.CashIn.Personal.Phone,
                                      PerId=t.CashIn.PersonalId,
                                      t.CashIn
                                  }).SingleOrDefault();




                // check if he confirmed before 
                //var cashIn = db.BanksysBankActions.Where(b => b.CashInId == CashInId && b.CashIn.RefrenceNumber == null)
                //      .Single();
                    if (UserInfo == null)
                    {
                        return StatusCode(402, "خطأ لقد قمت  بتعبئة مرتين لهذاالرقم");
                    }

                    if (UserInfo.PerId < 0)
                    {
                        return StatusCode(403, "خطأ لايمكن الايداع الرجاء الاتصال بالمشغل");
                    }
                    int personalId = (int)UserInfo.PerId;
                    var PersonalInfo = (from p in db.PersonalInfo
                                        where p.Id == personalId
                                        select p).SingleOrDefault();

                    if (PersonalInfo == null)
                    {
                        return StatusCode(403, "خطأ هذا المستخدم غير موجود");
                    }

                    string MSISDN = PersonalInfo.Phone.Substring(PersonalInfo.Phone.Length - 9);
                    if (MSISDN == null || MSISDN.Length < 9)
                    {
                        return StatusCode(405, "الرجاء تعبئة رقم الهاتف");
                    }
                    string UserNameUpdateBy = UserInfo.FullName;
                    if (UserNameUpdateBy == null)
                    {
                        return StatusCode(406, "الرجاء تسجيل الدخول اولا");
                    }
                    var nid = PersonalInfo.Nid;

                    if (nid == null || nid.Length < 12)
                    {
                        return StatusCode(405, "الرجاء تعبئة الرقم الوطني");
                    }
                    // Created by
               

                    String ShopeId = "";
                    var LRM = "/";
                    var RM = ((char)0x200E).ToString();

                    ShopeId = String.Format("{0}" + LRM + "" + RM + "{1}" + LRM + "{2}" + LRM + "" + RM + "{3}" + LRM + "" + RM + "{4}" + LRM + "" + RM + "{5}" + LRM + "" + RM + "{6}", UserInfo.CashIn.DepositType.ToString(), ((UserInfo.CashIn.CheckNum.ToString() == "") ? "Empty" : UserInfo.CashIn.CheckNum.ToString()), ((UserInfo.CashIn.NumInvoiceDep.ToString() == "") ? "Empty" : UserInfo.CashIn.NumInvoiceDep.ToString()), ((UserInfo.CashIn.CardNumber.ToString() == "") ? "Empty" : UserInfo.CashIn.CardNumber.ToString()), UserInfo.Branch.BankId, ((UserInfo.CashIn.Item == "") ? "Empty" : UserInfo.CashIn.Item), ((UserInfo.CashIn.BanckAccountNumber == "") ? "Empty" : UserInfo.CashIn.BanckAccountNumber));

                    var result = CashInWalletAsync("00218" + MSISDN, (double)UserInfo.CashIn.Valuedigits, ShopeId, UserInfo.FullName);
                    if (result.responseString != "Success")
                    {
                        return StatusCode(405, result.responseString);
                    }

                    var CashInUpdate = (from p in db.CashIn
                                        where p.CashInId == CashInId
                                        && (p.Status == 1)
                                        select p).SingleOrDefault();

                    if (CashInUpdate == null)
                    {
                        trans.Rollback();
                        return BadRequest("خطأ بيانات التعبئة غير موجودة");
                    }


                    CashInUpdate.Status = 2;
                    CashInUpdate.LastModifiedOn = DateTime.Now;
                    CashInUpdate.LastModifiedBy =(int) userId;
                    CashInUpdate.RefrenceNumber = result.refernceWallet;
                    db.SaveChanges();


                    BanksysBankActions BA = new BanksysBankActions();
                    BA.ActionType = 4;
                    BA.Description = "إنشاء عملية نقدية - تأكيد نهائي";
                    BA.UserId = userId;
                    BA.CashInId = CashInId;
                    BA.BranchId = this.help.GetCurrentBranche(HttpContext);
                    BA.UserType = this.help.GetCurrentUserType(HttpContext); 
                    BA.ActionDate = DateTime.Now;
                    db.BanksysBankActions.Add(BA);
                    db.SaveChanges();
                    trans.Commit();

                    return Ok("تم تعديل بينات المشترك بنجاح");
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return StatusCode(500, e.Message);
                }
            }
        }

        public class responseMpay
        {
            public int responseCode { get; set; }
            public string responseString { get; set; }
            public int refernceWallet { get; set; }

        }

        public responseMpay CashInWalletAsync(string phone, Double amount, string shopid, string createdby)
        {
            var Mpay = _Mpays;
            string MpayUrl = _Settings.MPayUrl;
            Guid id = Guid.NewGuid();
            Mpay.msgId = id.ToString();


            Mpay.extraData[0].value = amount.ToString();
            Mpay.extraData[2].value = phone;

            // create guid first
            // last confirm
            string username = createdby;
            Mpay.requestedId = createdby;
            Mpay.channelId = username;
            Mpay.shopId = shopid;
            string jsonRequest = JsonConvert.SerializeObject(Mpay);
            StringContent json = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            object retObject = null;
            var obj = new
            {
                responseCode = 0,
                responseData = new
                {
                    response = new
                    {
                        errorCd = "",
                        desc = "",
                        reF = 0,
                        statusCode = "",
                    },
                    token = ""
                }
            };

            var responesObject = new responseMpay();
            var task = client.PostAsync(MpayUrl, json).ContinueWith((result) =>
            {
                var retObject2 = result.Result.Content.ReadAsStringAsync();
                retObject2.Wait();
                try
                {

                    var mPayResp = JsonConvert.DeserializeAnonymousType(retObject2.Result, obj);
                    //errorCd = 0;
                    retObject = new { mPayResp = mPayResp };
                    responesObject.responseCode = mPayResp.responseCode;
                    responesObject.refernceWallet = mPayResp.responseData.response.reF;

                    responesObject.responseString = mPayResp.responseData.response.desc;

                }
                catch (Exception e)
                {
                    //  errorCd = 500;
                    // "couldn't deserialize wallet response Object.";
                }


            });
            task.Wait();
            return responesObject;

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
                        return NotFound("!خــطأ : العملية النقدية غير موجوده او مرفوضه مسبقا");
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



        [HttpGet("GetCashInCSV/{StartDate}/{EndDate}")]
        public IActionResult GetCashInCSV(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string sFileName = @"CashInExport.xlsx";
                string URL = string.Format("{0}://{1}/Excel/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder+"\\Excel\\", sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder + "\\Excel\\", sFileName));
                }
             
                IQueryable<BanksysBankActions> BankActionsQuery;
                var UserType = this.help.GetCurrentUserType(HttpContext);
                // admin see every thing


                // if date null 
                if (StartDate == null || EndDate == null)
                {
                    if (UserType == 1)
                    {
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.ActionType == 3
                                           select t;
                    }
                    else if (UserType == 2)
                    {
                        var BranchId = this.help.GetCurrentBranche(HttpContext);

                        var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.Branch.BankId == BankId && t.ActionType == 3
                                           select t;
                    }
                    else
                    {
                        var BranchId = this.help.GetCurrentBranche(HttpContext);

                        var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.BranchId == BranchId && t.ActionType == 3
                                           select t;
                    }

                }
                else
                {
                    // if date not null 
                    if (UserType == 1)
                    {
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.ActionType == 3 && t.ActionDate >= StartDate && t.ActionDate <= EndDate
                                           select t;
                    }
                    else if (UserType == 2)
                    {
                        var BranchId = this.help.GetCurrentBranche(HttpContext);

                        var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.Branch.BankId == BankId && t.ActionType == 3 && t.ActionDate >= StartDate && t.ActionDate <= EndDate
                                           select t;
                    }
                    else
                    {
                        var BranchId = this.help.GetCurrentBranche(HttpContext);

                        var BankId = db.BanksysBranch.Where(u => u.BranchId == BranchId).Single().BankId;
                        BankActionsQuery = from t in db.BanksysBankActions
                                           where t.BranchId == BranchId && t.ActionType == 3 && t.ActionDate >= StartDate && t.ActionDate <= EndDate
                                           select t;
                    }
                }




                var CashInCount = (from t in BankActionsQuery
                                   select t).Count();

                var CashInList = (from t in BankActionsQuery
                                  orderby t.ActionDate descending
                                  select new
                                  {
                                      t.CashIn.Valuedigits,
                                      t.CashIn.NumInvoiceDep,
                                      t.CashInId,
                                      t.CashIn.Description,
                                      t.Branch,
                                      BranchName = t.Branch.Name,
                                      t.Branch.Bank,
                                      BankName = t.Branch.Bank.Name,
                                      t.CashIn.Status,
                                      t.ActionDate,
                                      FName = t.CashIn.Personal.Name,
                                      t.CashIn.Personal.FatherName,
                                      t.CashIn.Personal.SurName,
                                      t.CashIn.Personal.GrandName,
                                      t.CashIn.Personal.Nid,
                                      t.CashIn.Personal.Phone
                                  }).ToList();

                if (CashInList.Count <= 0)
                {
                    return StatusCode(404, "لا توجد بيانات لهذا التاريخ");
                }


                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
                    //First add the headers
                    worksheet.Cells[1, 1].Value = "القيمة";
                    worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 15;
                    worksheet.Cells[1, 1].AutoFitColumns();
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Orange);

                    worksheet.Cells[1, 2].Value = "رقم الايصال";
                    worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Style.Font.Size = 15;
                    worksheet.Cells[1, 2].AutoFitColumns();
                    worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.Orange);


                    worksheet.Cells[1, 3].Value = "الهاتف";
                    worksheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Style.Font.Size = 15;
                    worksheet.Cells[1, 3].AutoFitColumns();
                    worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.Orange);


                    worksheet.Cells[1, 4].Value = "الاسم";
                    worksheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Style.Font.Size = 15;
                    worksheet.Cells[1, 4].AutoFitColumns();
                    worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.Orange);


                    worksheet.Cells[1, 5].Value = "الرقم الوطني";
                    worksheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 5].Style.Font.Bold = true;
                    worksheet.Cells[1, 5].Style.Font.Size = 15;
                    worksheet.Cells[1, 5].AutoFitColumns();
                    worksheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.Orange);



                    worksheet.Cells[1, 6].Value = "اسم المصرف";
                    worksheet.Cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 6].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    worksheet.Cells[1, 6].Style.Font.Bold = true;
                    worksheet.Cells[1, 6].Style.Font.Size = 15;
                    worksheet.Cells[1, 6].AutoFitColumns();

                    worksheet.Cells[1, 7].Value = "اسم الفرع";
                    worksheet.Cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    worksheet.Cells[1, 7].Style.Font.Bold = true;
                    worksheet.Cells[1, 7].Style.Font.Size = 15;
                    worksheet.Cells[1, 7].AutoFitColumns();


                    worksheet.Cells[1, 8].Value = "معلومات اخري";
                    worksheet.Cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 8].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    worksheet.Cells[1, 8].Style.Font.Bold = true;
                    worksheet.Cells[1, 8].Style.Font.Size = 15;
                    worksheet.Cells[1, 8].AutoFitColumns();

                    worksheet.Cells[1, 9].Value = "تاريخ العملية";
                    worksheet.Cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 9].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    worksheet.Cells[1, 9].Style.Font.Bold = true;
                    worksheet.Cells[1, 9].Style.Font.Size = 15;
                    worksheet.Cells[1, 9].AutoFitColumns();


                    worksheet.Cells[1, 10].Value = "الحالة";
                    worksheet.Cells[1, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 10].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    worksheet.Cells[1, 10].Style.Font.Bold = true;
                    worksheet.Cells[1, 10].Style.Font.Size = 15;
                    worksheet.Cells[1, 10].AutoFitColumns();

                    int i = 2;
                    foreach (var x in CashInList)
                    {
                        worksheet.Cells["A"+i].Value = x.Valuedigits;
                        worksheet.Cells["A" + i].Style.Font.Size = 12;
                        worksheet.Cells["A" + i].AutoFitColumns();
                        worksheet.Cells["A" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["B" + i].Value = x.NumInvoiceDep;
                        worksheet.Cells["B" + i].Style.Font.Size = 12;
                        worksheet.Cells["B" + i].AutoFitColumns();
                        worksheet.Cells["B" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["B" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);


                        worksheet.Cells["C" + i].Value = x.Phone;
                        worksheet.Cells["C" + i].Style.Font.Size = 12;
                        worksheet.Cells["C" + i].AutoFitColumns();
                        worksheet.Cells["C" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["C" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["D" + i].Value = x.FName +" "+x.FatherName+" "+x.SurName;
                        worksheet.Cells["D" + i].Style.Font.Size = 12;
                        worksheet.Cells["D" + i].AutoFitColumns();
                        worksheet.Cells["D" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["D" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["E" + i].Value = x.Nid;
                        worksheet.Cells["E" + i].Style.Font.Size = 12;
                        worksheet.Cells["E" + i].AutoFitColumns();
                        worksheet.Cells["E" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["E" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["F" + i].Value = x.BankName;
                        worksheet.Cells["F" + i].Style.Font.Size = 12;
                        worksheet.Cells["F" + i].AutoFitColumns();
                        worksheet.Cells["F" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["F" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["G" + i].Value = x.BranchName;
                        worksheet.Cells["G" + i].Style.Font.Size = 12;
                        worksheet.Cells["G" + i].AutoFitColumns();
                        worksheet.Cells["G" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["G" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["H" + i].Value = x.Description;
                        worksheet.Cells["H" + i].Style.Font.Size = 12;
                        worksheet.Cells["H" + i].AutoFitColumns();
                        worksheet.Cells["H" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["H" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["I" + i].Value = x.ActionDate.Date.ToString();
                        worksheet.Cells["I" + i].Style.Font.Size = 12;
                        worksheet.Cells["I" + i].AutoFitColumns();
                        worksheet.Cells["I" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["I" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        worksheet.Cells["J" + i].Value = (x.Status==1?"تأكيد مبدئي": (x.Status == 2 ? "تأكيد نهائي" : "مرفوض"));
                        worksheet.Cells["J" + i].Style.Font.Size = 12;
                        worksheet.Cells["J" + i].AutoFitColumns();
                        worksheet.Cells["J" + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["J" + i].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);

                        i++;
                    }
                        //Add values
                        
                    //worksheet.Cells["B2"].Value = "Jon";
                    //worksheet.Cells["C2"].Value = "M";
                    //worksheet.Cells["D2"].Value = 5000;

                    //worksheet.Cells["A3"].Value = 1001;
                    //worksheet.Cells["B3"].Value = "Graham";
                    //worksheet.Cells["C3"].Value = "M";
                    //worksheet.Cells["D3"].Value = 10000;

                    //worksheet.Cells["A4"].Value = 1002;
                    //worksheet.Cells["B4"].Value = "Jenny";
                    //worksheet.Cells["C4"].Value = "F";
                    //worksheet.Cells["D4"].Value = 5000;

                    package.Save(); //Save the workbook.
                }
                return Ok(URL);






                var myExport = new Jitbit.Utils.CsvExport();
                myExport.AddRow();
                myExport["Valuedigits"] = "القيمة";
                myExport["NumInvoiceDep"] = "رقم الايصال";
                myExport["Description"] = "معلومات اخري";
                myExport["BankName"] = "اسم المصرف";
                myExport["BranchName"] = "اسم الفرع";
                myExport["ActionDate"] = "تاريخ العملية";
                myExport["Nid"] = "Nid";
                myExport["Phone"] = "الهاتف";
                myExport["FName"] = "الاسم";
                myExport["FatherName"] = "اسم الأب";
                myExport["GrandName"] = "اسم الجد";
                myExport["SurName"] = "اللقب";
                myExport["Status"] = "الحالة";
                foreach (var x in CashInList)
                {
                    myExport.AddRow();
                    myExport["Valuedigits"] = x.Valuedigits;
                    myExport["NumInvoiceDep"] = x.NumInvoiceDep;
                    myExport["Description"] = x.Description;
                    myExport["BankName"] = x.BankName;
                    myExport["BranchName"] = x.BranchName;
                    myExport["ActionDate"] = x.ActionDate;
                    myExport["Nid"] = x.Nid;
                    myExport["Phone"] = x.Phone;
                    myExport["FName"] = x.FName;
                    myExport["FatherName"] = x.FatherName;
                    myExport["GrandName"] = x.GrandName;
                    myExport["SurName"] = x.SurName;
                    myExport["Status"] = (x.Status == 1 ? "تأكيد مبدئي" : (x.Status == 2 ? "تأكيد نهائي" : "مرفوض"));
                }

                byte[] bytes;
                bytes = myExport.ExportToBytes();
                var result = new FileContentResult(bytes, "application/octet-stream");
                result.FileDownloadName = "CashIn.csv";
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}