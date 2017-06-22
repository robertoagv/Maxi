using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.classHelper
{
    
    public class MovementsHelper : IDisposable
    {
        private static maxifarmaciabdContext db = new maxifarmaciabdContext();



        public void Dispose()
        {
            db.Dispose();
        }

        public static Response newSale(NewSaleView newSaleView, string nombreUsuario)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    var usuario = db.Users.Where(u => u.NombreUser == nombreUsuario).FirstOrDefault();

                    var sale = new Sale
                    {
                        Nombrecte = newSaleView.Nombrecte,
                        Fechavta = newSaleView.Fechavta,
                        UserId = usuario.UserId,
                        CompanyId = usuario.CompanyId
                    };
                    db.Sales.Add(sale);
                    db.SaveChanges();

                    var detalles = db.SaleDetailTmps.Where(sdt => sdt.NombreUsuario == nombreUsuario).ToList();

                    if (detalles.Count != 0)
                    {
                        foreach (var detalle in detalles)
                        {
                            var ventaDetalle = new SaleDetail
                            {

                                SaleId = sale.SaleID,
                                ProductID = detalle.ProductId,
                                Descriptionpro = detalle.Descriptionpro,
                                Price = detalle.Precio,
                                Cantidad = detalle.Cantidad
                            };

                            db.SaleDetails.Add(ventaDetalle);
                            db.SaleDetailTmps.Remove(detalle);
                        }

                        db.SaveChanges();
                        transaccion.Commit();
                        return new Response { Succeeded = true };
                    }
                    else
                    {
                        transaccion.Rollback();
                        return new Response
                        {
                            Message = "Debe agregar minimo un producto.",
                            Succeeded = false
                        };
                    }
                    
                }
                catch (Exception ex)
                {

                    transaccion.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false
                    };
                }
            }
        }



    }
}