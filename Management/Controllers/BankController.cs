using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/Bank")]
    public class BankController : Controller
    {

        private readonly wallet2Context db;
        private Helper help;
        public BankController(wallet2Context context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {
                var userType = this.help.GetCurrentUserType(HttpContext);
                if (userType!=1)
                {
                    return StatusCode(401,"لا تملك الصلاحية لعرض المصارف");
                }
                IQueryable<BanksysBank> BankQuery;
                BankQuery = from p in db.BanksysBank
                            where p.Status == 1 
                               select p;

                var BankCount = (from p in BankQuery
                                    select p).Count();

                var BankList = (from p in BankQuery
                                   orderby p.CreatedOn descending
                                   select new
                                   {
                                       p.BankId,
                                       Name = p.Name,
                                       Description = p.Description,
                                       p.CreatedOn
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { Banks = BankList, count = BankCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("GetAllBank")]
        public IActionResult GetAllBank()
        {
            try
            {
                var Banks = db.BanksysBank.Where(y=>y.Status==1).Select(
                    x => new
                    {
                        x.BankId,
                        x.Name
                    }).ToList();

                return Ok(new { Banks = Banks});
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add")]
        public IActionResult AddBank([FromBody] BanksysBank Bank)
        {
            try
            {
                var userType = this.help.GetCurrentUserType(HttpContext);
                if (userType != 1)
                {
                    return StatusCode(401, "لا تملك الصلاحية لإضافة المصرف");
                }

                if (Bank == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                Bank.CreatedBy = userId;
                Bank.CreatedOn = DateTime.Now;
                Bank.Status = 1;
                db.BanksysBank.Add(Bank);
                db.SaveChanges();

                return Ok("لقد قمت بتسـجيل بيانات المصرف بنــجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("{BankId}/delete")]
        public IActionResult DeleteBank(long BankId)
        {
            try
            {
                var userType = this.help.GetCurrentUserType(HttpContext);
                if (userType != 1)
                {
                    return StatusCode(401, "لا تملك الصلاحية لمسح المصارف");
                }
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Bank = (from p in db.BanksysBank
                            where p.BankId == BankId
                              && (p.Status == 1)
                              select p).SingleOrDefault();

                if (Bank == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                Bank.Status = 9;
                Bank.UpdatedBy = userId;
                Bank.UpdatedOn = DateTime.Now;
                db.SaveChanges();
                return Ok("Bank Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



    }
}