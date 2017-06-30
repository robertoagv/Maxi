using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.classHelper
{
    
    public class MovementsHelper
    {
        private static maxifarmaciabdContext db = new maxifarmaciabdContext();

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

                            var restarp = db.Inventories.Where(i => i.ProductId == detalle.ProductId).FirstOrDefault();

                            #region Para recorrer varias bodegas.
                            //if (restarp.Product.Inventories.Count > 1)
                            //{
                            //    foreach (var item in restarp.Product.Inventories)
                            //    {
                            //        int descs = detalle.Cantidad;

                            //        var inventps = db.Inventories.Where(i => i.inventoryId == item.inventoryId && i.ProductId == item.ProductId).FirstOrDefault();
                            //        var oldexists = int.Parse(inventps.Existencia.ToString());
                            //        if (oldexists > detalle.Cantidad)
                            //        {
                            //             descs = oldexists - detalle.Cantidad;
                            //        }
                            //        else
                            //        {
                            //            detalle.Cantidad -= oldexists;
                            //            var invetarioUpdates = db.Inventories.Find(item.inventoryId);
                            //            invetarioUpdates.Existencia = detalle.Cantidad;
                            //            db.SaveChanges();
                            //        }
                            //    }
                            //} 
                            #endregion

                            var oldExist = int.Parse(restarp.Existencia.ToString());
                            var desc = oldExist - detalle.Cantidad;

                            var inventarioUpdate = db.Inventories.Find(restarp.inventoryId);
                            inventarioUpdate.Existencia = desc;
                            db.SaveChanges();

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