using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.swth.entidades.Utils;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/TiposMovimientoInterno")]
    public class TiposMovimientoInternoController : Controller
    {
        private readonly SwTHDbContext db;

        public TiposMovimientoInternoController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarTiposMovimientoInterno")]
        public async Task<List<TipoMovimientoInterno>> GetTipoMovimientoInterno()
        {
            try
            {
                return await db.TipoMovimientoInterno.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<TipoMovimientoInterno>();
            }
        }

        // GET: api/BasesDatos/5
        [HttpGet("{id}")]
        public async Task<Response> GetTipoMovimientoInterno([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var TipoMovimientoInterno = await db.TipoMovimientoInterno.SingleOrDefaultAsync(m => m.IdTipoMovimientoInterno == id);

                if (TipoMovimientoInterno == null)
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
                    Resultado = TipoMovimientoInterno,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // PUT: api/BasesDatos/5
        [HttpPut("{id}")]
        public async Task<Response> PutTipoMovimientoInterno([FromRoute] int id, [FromBody] TipoMovimientoInterno TipoMovimientoInterno)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var existe = Existe(TipoMovimientoInterno);
                if (existe.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }

                var TipoMovimientoInternoActualizar = await db.TipoMovimientoInterno.Where(x => x.IdTipoMovimientoInterno == id).FirstOrDefaultAsync();

                if (TipoMovimientoInternoActualizar != null)
                {
                    try
                    {
                        TipoMovimientoInternoActualizar.Nombre = TipoMovimientoInterno.Nombre;
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = Mensaje.Satisfactorio,
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwTH),
                            ExceptionTrace = ex.Message,
                            Message = Mensaje.Excepcion,
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = Mensaje.Error,
                        };
                    }
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
                    Message = Mensaje.Excepcion
                };
            }
        }

        // POST: api/BasesDatos
        [HttpPost]
        [Route("InsertarTipoMovimientoInterno")]
        public async Task<Response> PostTipoMovimientoInterno([FromBody] TipoMovimientoInterno TipoMovimientoInterno)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var respuesta = Existe(TipoMovimientoInterno);
                if (!respuesta.IsSuccess)
                {
                    db.TipoMovimientoInterno.Add(TipoMovimientoInterno);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
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
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // DELETE: api/BasesDatos/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteTipoMovimientoInterno([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var respuesta = await db.TipoMovimientoInterno.SingleOrDefaultAsync(m => m.IdTipoMovimientoInterno == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.TipoMovimientoInterno.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex.Message,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.BorradoNoSatisfactorio,
                };
            }
        }

        private Response Existe(TipoMovimientoInterno TipoMovimientoInterno)
        {
            var bdd = TipoMovimientoInterno.Nombre;
            var TipoMovimientoInternorespuesta = db.TipoMovimientoInterno.Where(p => p.Nombre == bdd).FirstOrDefault();
            if (TipoMovimientoInternorespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.ExisteRegistro,
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = TipoMovimientoInternorespuesta,
            };
        }

    }
}