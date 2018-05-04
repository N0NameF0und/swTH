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
    [Route("api/GastoPersonal")]
    public class GastoPersonalController : Controller
    {
        private readonly SwTHDbContext db;

        public GastoPersonalController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarGastoPersonal")]
        public async Task<List<GastoPersonal>> ListarGastoPersonal()
        {
            try
            {
                return await db.GastoPersonal.OrderBy(x => x.Valor).Include(x => x.TipoDeGastoPersonal).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<GastoPersonal>();
            }
        }

        // GET: api/BasesDatos/5
        [HttpGet]
        [HttpPost("ObtenerGastoPersonal")]
        public async Task<Response> ObtenerGastoPersonal([FromBody] GastoPersonal GastoPersonal)
        {
            try
            {
                var gastoPersonal = await db.GastoPersonal.SingleOrDefaultAsync(m => m.IdGastoPersonal == GastoPersonal.IdGastoPersonal);

                if (gastoPersonal == null)
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
                    Resultado = gastoPersonal,
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
        [Route("EditarGastoPersonal")]
        public async Task<Response> EditarGastoPersonal([FromBody] GastoPersonal GastoPersonal)
        {
            try
            {
                if (await Existe(GastoPersonal))
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }

                var GastoPersonalActualizar = await db.GastoPersonal.Where(x => x.IdGastoPersonal == GastoPersonal.IdGastoPersonal).FirstOrDefaultAsync();
                if (GastoPersonalActualizar == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                    };

                }

                GastoPersonalActualizar.Valor = GastoPersonal.Valor;
                GastoPersonalActualizar.IdTipoGasto = GastoPersonal.IdTipoGasto;
                db.GastoPersonal.Update(GastoPersonalActualizar);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Resultado = GastoPersonalActualizar
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
        [Route("InsertarGastoPersonal")]
        public async Task<Response> PostGastoPersonal([FromBody] GastoPersonal GastoPersonal)
        {
            try
            {

                if (!await Existe(GastoPersonal))
                {
                    db.GastoPersonal.Add(GastoPersonal);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio,
                        Resultado = GastoPersonal,
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
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

        // DELETE: api/BasesDatos/5
        [HttpPost]
        [Route("EliminarGastoPersonal")]
        public async Task<Response> EliminarGastoPersonal([FromBody]GastoPersonal GastoPersonal)
        {
            try
            {
                var respuesta = await db.GastoPersonal.Where(m => m.IdGastoPersonal == GastoPersonal.IdGastoPersonal).FirstOrDefaultAsync();
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.GastoPersonal.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.BorradoNoSatisfactorio,
                };

            }
        }

        private async Task<bool> Existe(GastoPersonal GastoPersonal)
        {
            var tipoGasto = GastoPersonal.IdTipoGasto;
            var ano = GastoPersonal.Ano;
            var GastoPersonalrespuesta = await db.GastoPersonal.Where(p => p.IdTipoGasto== tipoGasto && p.Ano==ano).FirstOrDefaultAsync();

            if (GastoPersonalrespuesta == null || GastoPersonalrespuesta.IdGastoPersonal == GastoPersonal.IdGastoPersonal)
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