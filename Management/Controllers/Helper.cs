using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class Helper
    {


        public long GetCurrentUser(HttpContext HttpUser)
        {
            try
            {
                var user = HttpUser.User;
                if (user == null || user.Claims == null)
                {
                    return 0;
                }

                var claims = user.Claims.ToList();

                if (claims.Count == 0)
                {
                    //return 0;
                    return 1;
                }
                string userIdClaim = "";
                if (claims.Count > 1)
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id").ToList()[0].Value;
                }
                else
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id").SingleOrDefault().Value;
                }


                long userId = Convert.ToInt64(userIdClaim);


                return Convert.ToInt64(userId);
            }
            catch (Exception)
            {
                return -999;
            }
        }


        public int GetCurrentUserType(HttpContext HttpUser)
        {
            try
            {
                var user = HttpUser.User;
                if (user == null || user.Claims == null)
                {
                    return 0;
                }

                var claims = user.Claims.ToList();

                if (claims.Count == 0)
                {
                    //return 0;
                    return 1;
                }
                string userIdClaim = "";
                if (claims.Count > 1)
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/UserType").ToList()[0].Value;
                }
                else
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/UserType").SingleOrDefault().Value;
                }


                long userId = Convert.ToInt64(userIdClaim);


                return Convert.ToInt32(userId);
            }
            catch (Exception)
            {
                return -999;
            }
        }


        public long GetCurrentBranche(HttpContext HttpUser)
        {
            try
            {
                var user = HttpUser.User;
                if (user == null || user.Claims == null)
                {
                    return 0;
                }

                var claims = user.Claims.ToList();

                if (claims.Count == 0)
                {
                    //return 0;
                    return 1;
                }
                string userIdClaim = "";
                if (claims.Count > 1)
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/BranchId").ToList()[0].Value;
                }
                else
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/BranchId").SingleOrDefault().Value;
                }


                long userId = Convert.ToInt64(userIdClaim);


                return Convert.ToInt64(userId);
            }
            catch (Exception)
            {
                return -999;
            }
        }


    }
}
