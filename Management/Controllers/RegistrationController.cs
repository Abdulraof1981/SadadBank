using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/Registration")]
    public class RegistrationController : Controller
    {

        private readonly wallet2Context db;
        private Helper help;

        private MPay _Mpays;
        private settings _Settings;
        static HttpClient client = new HttpClient();

        public RegistrationController(wallet2Context context, IOptions<MPay> Mpays, IOptions<settings> settings)
        {
            _Mpays = Mpays.Value;
            _Settings = settings.Value;
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize, string search, int status)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var bankUser = (from rec in db.BanksysUsers where rec.UserId == userId
                                select  rec).SingleOrDefault();
                if ( bankUser.UserType == 1) // Admin
                {
                    var regsTrns = (from rec in db.BanksysBankActions
                                    where rec.ActionType == 1 && rec.PersonalInfo.Reference == 3
                                    orderby rec.PersonalInfo.LastModifiedOn descending
                                    select new
                                    {
                                        FirstName = rec.PersonalInfo.Name,
                                        rec.PersonalInfo.FatherName,
                                        rec.PersonalInfo.GrandName,
                                        rec.PersonalInfo.SurName,
                                        rec.PersonalInfo.BirthDate,
                                        rec.PersonalInfo.Nid,
                                        rec.PersonalInfo.Phone,
                                        LastModifiedOn = rec.ActionDate.ToString("hh:mm:ss dd'/'MM'/'yyyy"),

                                        AllActions = (from a in db.BanksysBankActions
                                                      where a.PersonalInfoId == rec.PersonalInfo.Id && (a.ActionType == 0 || a.ActionType == 1 || a.ActionType == 2)
                                                      select new
                                                      {
                                                          UserFullName = a.User.FullName,
                                                          BranchName = a.Branch.Name,
                                                          BankName = a.Branch.Bank.Name,
                                                          ActionDate = a.ActionDate.ToString("hh:mm:ss dd'/'MM'/'yyyy"),
                                                          a.ActionType,
                                                          a.UserType,
                                                          a.Description,
                                                      }).ToList(),

                                        rec.PersonalInfo.Status,
                                        rec.Description,
                                        rec.BankActionId
                                    });

                    int type = -1;
                    if (status == 1)
                        type = 2;
                    else if (status == 2)
                        type = 3;
                    else if (status == 3)
                        type = 0;
                    if (type != -1)
                        regsTrns = regsTrns.Where(t => t.Status == type);

                    if (search != null && !search.Equals("") && !search.Equals("undefined"))
                        regsTrns = regsTrns.Where(t => t.Phone.Contains(search));

                    int count = regsTrns.Count();
                    regsTrns = regsTrns.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    return Ok(new { Customers = regsTrns.ToList(), count });
                }
                else if (bankUser.UserType == 2) // Bank Manager
                {
                    long BankId = (from b in db.BanksysBranch where b.BranchId == bankUser.BranchId select b.BankId).SingleOrDefault().Value;

                    var regsTrns = (from rec in db.BanksysBankActions
                                    where rec.ActionType == 1 && rec.PersonalInfo.Reference == 3 && rec.User.Branch.BankId == BankId
                                    orderby rec.PersonalInfo.LastModifiedOn descending
                                    select new
                                    {
                                        FirstName = rec.PersonalInfo.Name,
                                        rec.PersonalInfo.FatherName,
                                        rec.PersonalInfo.GrandName,
                                        rec.PersonalInfo.SurName,
                                        rec.PersonalInfo.BirthDate,
                                        rec.PersonalInfo.Nid,
                                        rec.PersonalInfo.Phone,
                                        LastModifiedOn = rec.ActionDate.ToString("hh:mm:ss dd'/'MM'/'yyyy"),
                                        
                                        AllActions = (from a in db.BanksysBankActions
                                                      where a.PersonalInfoId == rec.PersonalInfo.Id && (a.ActionType == 0 || a.ActionType == 1 || a.ActionType == 2)
                                                      select new
                                                      {
                                                          UserFullName = a.User.FullName,
                                                          BranchName = a.Branch.Name,
                                                          BankName = a.Branch.Bank.Name,
                                                          ActionDate = a.ActionDate.ToString("dd'/'MM'/'yyyy hh:mm:ss"),
                                                          a.ActionType,
                                                          a.UserType,
                                                          a.Description,
                                                      }).ToList(),
                                        rec.PersonalInfo.Status,
                                        rec.Description,
                                        rec.BankActionId
                                    });

                    int type = -1;
                    if (status == 1)
                        type = 2;
                    else if (status == 2)
                        type = 3;
                    else if (status == 3)
                        type = 0;
                    if (type != -1)
                        regsTrns = regsTrns.Where(t => t.Status == type);

                    if (search != null && !search.Equals("") && !search.Equals("undefined"))
                         regsTrns = regsTrns.Where(t => t.Phone.Contains(search));

                    int count = regsTrns.Count();
                    regsTrns = regsTrns.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    return Ok(new { Customers = regsTrns.ToList(), count });
                }
                else if (bankUser.UserType == 3) // Bank Empployee
                {
                    var regsTrns = (from rec in db.BanksysBankActions
                                    where rec.UserId == userId && rec.ActionType == 1 && rec.PersonalInfo.Reference == 3
                                    orderby rec.PersonalInfo.LastModifiedOn descending
                                    select new
                                    {
                                        FirstName = rec.PersonalInfo.Name,
                                        rec.PersonalInfo.FatherName,
                                        rec.PersonalInfo.GrandName,
                                        rec.PersonalInfo.SurName,
                                        rec.PersonalInfo.BirthDate,
                                        rec.PersonalInfo.Nid,
                                        rec.PersonalInfo.Phone,
                                        LastModifiedOn = rec.ActionDate.ToString("hh:mm:ss dd'/'MM'/'yyyy"),

                                        AllActions = (from a in db.BanksysBankActions
                                                      where a.PersonalInfoId == rec.PersonalInfo.Id && (a.ActionType == 0 || a.ActionType == 1 || a.ActionType == 2)
                                                      select new
                                                      {
                                                          UserFullName = a.User.FullName,
                                                          BranchName = a.Branch.Name,
                                                          BankName = a.Branch.Bank.Name,
                                                          ActionDate = a.ActionDate.ToString("dd'/'MM'/'yyyy hh:mm:ss"),
                                                          a.ActionType,
                                                          a.UserType,
                                                          a.Description,
                                                      }).ToList(),

                                        rec.PersonalInfo.Status,
                                        rec.Description,
                                        rec.BankActionId
                                    });
                    int type = -1;
                    if (status == 1)
                        type = 2;
                    else if (status == 2)
                        type = 3;
                    else if (status == 3)
                        type = 0;
                    if (type != -1)
                        regsTrns = regsTrns.Where(t => t.Status == type);

                    if (search != null && !search.Equals("") && !search.Equals("undefined"))
                        regsTrns = regsTrns.Where(t => t.Phone.Contains(search));

                    int count = regsTrns.Count();
                    regsTrns = regsTrns.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    return Ok(new { Customers = regsTrns.ToList(), count });
                }
                return Ok(new { count = 0 });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        public class FormOb
        {
            public string Nid { get; set; }
            public string Name { get; set; }
            public string FatherName { get; set; }
            public string GrandName { get; set; }
            public string SurName { get; set; }
            public string Gender { get; set; }
            public string BirthDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string CustomerName { get; set; }
            public int Cityid { get; set; }
            public string Address { get; set; }
            public string PassportNumber { get; set; }
            public DateTime PassportExportDate { get; set; }
        }


        [HttpPost("Add")]
        public IActionResult Add([FromBody] FormOb ob)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (ob == null)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var persons = (from p in db.PersonalInfo where p.Phone.Contains(ob.Phone) || p.Nid == ob.Nid select new { p }).ToList();
                if( persons != null)
                {
                    foreach (var ps in persons)
                    {
                        if (ps.p.Nid.Equals(ob.Nid))
                            return StatusCode(400, "الرقم الوطني مستخدم من قبل");

                        if (ps.p.Phone.Contains(ob.Phone))
                            return StatusCode(400, "رقم الهاتف مستخدم من قبل");
                    }
                }
                
                PersonalInfo info = new PersonalInfo();
                info.Name = ob.Name;
                info.FatherName = ob.FatherName;
                info.GrandName = ob.GrandName;
                info.SurName = ob.SurName;
                info.Nid = ob.Nid;
                info.Gender = int.Parse(ob.Nid.Substring(0, 1));
                info.BirthDate = DateTime.Parse(ob.BirthDate);
                info.Email = ob.Email;
                info.Phone = ob.Phone;
                info.CrmFullName = ob.CustomerName;
                info.Address = ob.Address;
                info.Reference = 3;
                info.AppointmentDate = DateTime.Now;

                var cob = (from ci in db.City where ci.CityId == ob.Cityid select new { ci.CityMpayId }).SingleOrDefault();
                info.CityMpayId = cob.CityMpayId;
                info.PassportNumber = ob.PassportNumber;
                info.PassportExportDate = ob.PassportExportDate.ToString("dd'/'MM'/'yyyy");
                info.CreatedBy = (int)userId;
                info.LastModifiedBy = (int)userId;
                info.CreatedOn = DateTime.Now;
                info.LastModifiedOn = DateTime.Now;
                info.Status = 2;

                info.GenratedNumber = "SadadBank-" + Guid.NewGuid().ToString();
                info.Nationality = "ليبي";
                db.PersonalInfo.Add(info);

                db.SaveChanges();
                
                BanksysBankActions action = new BanksysBankActions();
                action.ActionType = 1;
                action.PersonalInfoId = info.Id;
                action.UserId = userId;
                action.BranchId = this.help.GetCurrentBranche(HttpContext);
                action.UserType = this.help.GetCurrentUserType(HttpContext);
                action.CashInId = null;
                action.Description = "تسجيل زبون - تأكيد مبدئي";
                action.ActionDate = DateTime.Now;
                db.BanksysBankActions.Add(action);
                db.SaveChanges();
                return Ok("لقد قمت بتسـجيل بيانات بنــجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        
        [HttpGet("getNidInfo")]
        public async Task<IActionResult> getNidInfo(string nid)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (nid != null && nid.Trim().Length == 12)
                {
                    ServiceReferenceNID.IimplementationOfNidClient client = new ServiceReferenceNID.IimplementationOfNidClient();
                    if (client != null)
                    {
                        ServiceReferenceNID.response response = await client.SearchIdentityDataAsync(nid.Trim());
                        if (response != null && response.responseData != null && response.responseCode == 0)
                        {
                            return Json(new { code = 0, name = response.responseData.FistName, fatherName = response.responseData.FatherName, grandName = response.responseData.GrandFatherName, surName = response.responseData.SurName, gender = response.responseData.NationalIdNumber.Substring(0, 1), birthDate = response.responseData.BirthDate.Value.Date });
                        }
                        else
                        {
                            return Json(new { code = -5 });
                        }
                    }
                    else
                    {
                        return Json(new { code = -4 });
                    }
                }
                else
                {
                    return Json(new { code = -3 });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = -1 });
            }
        }
        


        [HttpGet("GetCurrentUserType")]
        public IActionResult GetCurrentUserType()
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                var bankUser = (from rec in db.BanksysUsers
                                where rec.UserId == userId
                                select rec).SingleOrDefault();
               // bankUser.

                return Json(new { userType = bankUser.UserType });

            }
            catch (Exception ex)
            {
                return Json(new { userType = -1 });
            }
        }

        [HttpGet("GetAllCities")]
        public IActionResult GetAllCities()
        {
            var Cities = (from ci in db.City
                          where ci.Status == 1
                          select new { ci.CityId, ci.CityName }).ToList();
            return Json(new { Cities });
        }

        [HttpGet("getPhoneInfo")]
        public async Task<IActionResult> getPhoneInfo(string phone)
        {
            WebResponse webResponse = null;
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                if (phone != null && phone.Trim().Length == 9)
                {
                    Uri url = new Uri(Uri.EscapeUriString(string.Format(_Settings.CRMUrl, phone.Trim())));
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    webResponse = await request.GetResponseAsync();
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    string responseData = reader.ReadToEnd();

                    JObject jObject = JObject.Parse(responseData);

                    string crmName = jObject["Name"].ToString();
                    string crmNid = jObject["NationalNo"].ToString();

                    if (crmName == null || crmName == "No Data from CRM" || crmName == "")
                    {
                        crmName = "";
                    }
                    if (crmNid == null || crmNid == "No Data from CRM" || crmNid == "")
                    {
                        crmNid = "";
                    }

                    return Json(new { code = 0, name = crmName, nid = crmNid });
                }
                else
                {
                    return Json(new { code = 1 });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = -1 });
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
        }
        
        [HttpPost("{BankActionId}/Reject")]
        public IActionResult Reject(long BankActionId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                
                var BankAction = (from p in db.BanksysBankActions
                                where p.BankActionId == BankActionId && p.ActionType == 1
                                select p).SingleOrDefault();

                if (BankAction == null)
                {
                    return NotFound("خــطأ : الحركة غير موجودة");
                }
                
                var persInfo = (from pIn in db.PersonalInfo
                                  where pIn.Id == BankAction.PersonalInfoId
                                  select pIn).SingleOrDefault();
                if (persInfo == null)
                {
                    return NotFound("خــطأ : الحركة غير موجودة");
                }


                BanksysBankActions action = new BanksysBankActions();
                action.ActionType = 0;
                action.PersonalInfoId = persInfo.Id;
                action.UserId = userId;
                action.BranchId = this.help.GetCurrentBranche(HttpContext);
                action.UserType = this.help.GetCurrentUserType(HttpContext);
                action.CashInId = null;
                action.Description = "رفض عملية تسجيل زبون";
                action.ActionDate = DateTime.Now;
                db.BanksysBankActions.Add(action);
                
                persInfo.LastModifiedBy = int.Parse(userId.ToString());
                persInfo.LastModifiedOn = DateTime.Now;
                persInfo.Status = 0;
                db.SaveChanges();
                 
                return Ok("BankAction Rejected");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        
        [HttpPost("{BankActionId}/LastConfirm")]
        public IActionResult LastConfirm(long BankActionId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var bankAction = (from p in db.BanksysBankActions
                                  where p.BankActionId == BankActionId && p.ActionType == 1
                                  select new { p.User, p.PersonalInfo , p , p.User.BranchId , p.User.Branch.BankId } ).SingleOrDefault();
                
                if (bankAction == null || bankAction.PersonalInfo == null)
                {
                    return NotFound("خــطأ : الحركة غير موجودة");
                }

                var bankUser = (from rec in db.BanksysUsers
                                where rec.UserId == userId
                                select rec).SingleOrDefault();

                Guid id = Guid.NewGuid();
                string msgId = "SadadBank-" + id.ToString();
                
                var result = CreatWalletAsync("00218" + bankAction.PersonalInfo.Phone.Substring(bankAction.PersonalInfo.Phone.Length - 9), bankAction.PersonalInfo.Name, bankAction.PersonalInfo.FatherName, bankAction.PersonalInfo.SurName, bankAction.PersonalInfo.Nid, bankUser.LoginName, userId, bankAction.PersonalInfo.CityMpayId.Value, bankAction.PersonalInfo.BirthDate.Value, bankAction.PersonalInfo.Gender.Value, msgId);
                if (result.responseString == "Success")
                {
                    bankAction.PersonalInfo.Status = 3;
                    bankAction.PersonalInfo.LastModifiedOn = DateTime.Now;
                    bankAction.PersonalInfo.LastModifiedBy = (int)userId;
                    
                    BanksysBankActions action = new BanksysBankActions();
                    action.ActionType = 2;
                    action.PersonalInfoId = bankAction.PersonalInfo.Id;
                    action.UserId = userId;
                    action.BranchId = this.help.GetCurrentBranche(HttpContext);
                    action.UserType = this.help.GetCurrentUserType(HttpContext);
                    action.CashInId = null;
                    action.Description = "تسجيل زبون - تأكيد نهائي";
                    action.ActionDate = DateTime.Now;
                    db.BanksysBankActions.Add(action);
                    
                    db.SaveChanges();

                    return Json(new { code = 0 , message = "تم التسجيل بنجاح" });
                }
                else
                { // Error in mPay response:
                    /*
                    bankAction.PersonalInfo.LastModifiedOn = DateTime.Now;
                    bankAction.PersonalInfo.LastModifiedBy = (int)userId;

                    bankAction.p.ActionType = 5;
                    bankAction.p.Description = result.responseString;
                    db.SaveChanges();
                    
                    bankAction.PersonalInfo.Status = 5;
                    bankAction.PersonalInfo.LastModifiedOn = DateTime.Now;
                    bankAction.PersonalInfo.LastModifiedBy = (int)userId;

                    BanksysBankActions action = new BanksysBankActions();
                    action.ActionType = 5;
                    action.PersonalInfoId = bankAction.PersonalInfo.Id;
                    action.UserId = userId;
                    action.BranchId = null;
                    db.BanksysBankActions.Add(action);

                    db.SaveChanges();
                    */

                    return Json(new { code = 1, message = result.responseString });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = -1, message = "حدث خطا الرجاء المحاولة مرة أخرى" });
            }
        }

        public class responseMpay
        {
            public int responseCode { get; set; }
            public string responseString { get; set; }
            public int refernceWallet { get; set; }

        }


        private responseMpay CreatWalletAsync(string phone, string name, string midname, string sirname, string nid, string usernameFirst, long userId, int cityid, DateTime datebirth, int gender, string msgId)
        {
            var Mpay = _Mpays;
            string MpayUrl = _Settings.MPayUrl;

            Mpay.msgId = msgId;
            Mpay.sender = phone.ToString();

            Mpay.extraData[0].value = name;
            Mpay.extraData[1].value = midname;
            Mpay.extraData[2].value = sirname;
            Mpay.extraData[3].value = nid.ToString();
            Mpay.extraData[5].value = datebirth.ToString("dd'/'MM'/'yyyy");
            Mpay.extraData[13].value = gender + "";
            Mpay.extraData[8].value = 1 + "";

            if (cityid != 0)
            {
                Mpay.extraData[8].value = cityid.ToString();
            }

            Mpay.requestedId = userId.ToString();
            // second confrim
            Mpay.channelId = usernameFirst;

            Mpay.shopId = "SadadBank";

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

    }
}