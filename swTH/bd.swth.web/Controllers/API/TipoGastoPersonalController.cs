using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.swth.entidades.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/TipoDeGastoPersonal")]
    public class TipoGastoPersonalController : Controller
    {
        private readonly SwTHDbContext db;

        public TipoGastoPersonalController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarTipoDeGastoPersonal")]
        public async Task<List<TipoDeGastoPersonal>> ListarTipoDeGastoPersonal()
        {
            try
            {
                return await db.TipoDeGastoPersonal.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception )
            {
                return new List<TipoDeGastoPersonal>();
            }
        }

        // GET: api/BasesDatos/5
        [HttpGet]
        [HttpPost("ObtenerTipoDeGastoPersonal")]
        public async Task<Response> ObtenerTipoDeGastoPersonal([FromBody] TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            try
            {
                var tipoDeGastoPersonal = await db.TipoDeGastoPersonal.SingleOrDefaultAsync(m => m.IdTipoGastoPersonal == TipoDeGastoPersonal.IdTipoGastoPersonal);

                if (tipoDeGastoPersonal == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = tipoDeGastoPersonal,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // PUT: api/BasesDatos/5
        [HttpPost]
        [Route("EditarTipoDeGastoPersonal")]
        public async Task<Response> EditarTipoDeGastoPersonal([FromBody] TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            try
            {
                if (await Existe(TipoDeGastoPersonal))
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }

                var TipoDeGastoPersonalActualizar = await db.TipoDeGastoPersonal.Where(x => x.IdTipoGastoPersonal == TipoDeGastoPersonal.IdTipoGastoPersonal).FirstOrDefaultAsync();
                if (TipoDeGastoPersonalActualizar == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                    };

                }

                TipoDeGastoPersonalActualizar.Descripcion = TipoDeGastoPersonal.Descripcion;
                db.TipoDeGastoPersonal.Update(TipoDeGastoPersonalActualizar);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Resultado = TipoDeGastoPersonalActualizar
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion
                };
            }
        }

        // POST: api/BasesDatos
        [HttpPost]
        [Route("InsertarTipoDeGastoPersonal")]
        public async Task<Response> PostTipoDeGastoPersonal([FromBody] TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            try
            {

                if (!await Existe(TipoDeGastoPersonal))
                {
                    db.TipoDeGastoPersonal.Add(TipoDeGastoPersonal);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = TipoDeGastoPersonal,
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // DELETE: api/BasesDatos/5
        [HttpPost]
        [Route("EliminarTipoDeGastoPersonal")]
        public async Task<Response> EliminarTipoDeGastoPersonal([FromBody]TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            try
            {
                var respuesta = await db.TipoDeGastoPersonal.Where(m => m.IdTipoGastoPersonal == TipoDeGastoPersonal.IdTipoGastoPersonal).FirstOrDefaultAsync();
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.TipoDeGastoPersonal.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.BorradoNoSatisfactorio,
                };
            }
        }

        private async Task<bool> Existe(TipoDeGastoPersonal TipoDeGastoPersonal)
        {
            var bdd = TipoDeGastoPersonal.Descripcion.ToUpper().TrimEnd().TrimStart();
            var TipoDeGastoPersonalrespuesta = await db.TipoDeGastoPersonal.Where(p => p.Descripcion.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefaultAsync();

            if (TipoDeGastoPersonalrespuesta == null || TipoDeGastoPersonalrespuesta.IdTipoGastoPersonal == TipoDeGastoPersonal.IdTipoGastoPersonal)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}