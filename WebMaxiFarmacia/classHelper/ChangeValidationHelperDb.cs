using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.classHelper
{
    public class ChangeValidationHelperDb
    {
        public static Response ChangeDb(maxifarmaciabdContext db)
        {
            try
            {
                db.SaveChanges();
                return new Response { Succeeded = true };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false };

                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("_index"))
                {
                    response.Message = "Este registro ya Existe";
                }
                else if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "El registro no puede ser Eliminado porque tiene registros relacionados.";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }
    }
}