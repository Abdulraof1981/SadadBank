using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/Branch")]
    public class BranchController : Controller
    {

        private readonly wallet2Context db;
        private Helper help;
        public BranchController(wallet2Context context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize,long BankId)
        {
            try
            {
                IQueryable<BanksysBranch> BranchQuery;
                if (BankId == 0)
                {
                    BranchQuery = from p in db.BanksysBranch
                                  where p.Status == 1
                                  select p;
                } else
                {
                    BranchQuery = from p in db.BanksysBranch
                                  where p.Status == 1 && p.BankId ==BankId
                                  select p;

                }
                        
                var BranchCount = (from p in BranchQuery
                                 select p).Count();

                var BranchList = (from p in BranchQuery
                                orderby p.CreatedOn descending
                                select new
                                {
                                    p.BranchId,
                                    BankName=p.Bank.Name,
                                    p.BankId,
                                    Name = p.Name,
                                    Description = p.Description,
                                    p.CreatedOn
                                }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { branchs = BranchList, count = BranchCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddBank([FromBody] BanksysBranch Branch)
        {
            try
            {
                if (Branch == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                Branch.CreatedBy = userId;
                Branch.CreatedOn = DateTime.Now;
                Branch.Status = 1;
                db.BanksysBranch.Add(Branch);
                db.SaveChanges();

                return Ok("لقد قمت بتسـجيل بيانات الفرع بنــجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("{BranchId}/delete")]
        public IActionResult DeleteBranch(long BranchId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Branch = (from p in db.BanksysBranch
                              where p.BranchId == BranchId
                            && (p.Status == 1)
                            select p).SingleOrDefault();

                if (Branch == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                Branch.Status = 9;
                Branch.UpdatedBy = userId;
                Branch.UpdatedOn = DateTime.Now;
                db.SaveChanges();
                return Ok("Branch Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

      
        [HttpGet("GetAllBranchsByBankId/{BankId}")]
        public IActionResult GetAllBranchsByBankId(long BankId)
        {
            try
            {
                var Branchs = db.BanksysBranch.Where(y => y.Status == 1 && y.BankId == BankId).Select(
                    x => new
                    {
                        x.BranchId,
                        x.Name
                    }).ToList();

                return Ok(new { Branchs = Branchs });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetAllBranch")]
        public IActionResult GetAllBranch()
        {
            try
            {
                var Branchs = db.BanksysBranch.Where(y => y.Status == 1).Select(
                    x => new
                    {
                        x.BranchId,
                        x.Name
                    }).ToList();

                return Ok(new { Branchs = Branchs });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}