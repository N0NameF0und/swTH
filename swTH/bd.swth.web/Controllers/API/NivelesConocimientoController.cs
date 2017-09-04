using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using bd.swth.entidades.Utils;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/NivelesConocimiento")]
    public class NivelesConocimientoController : Controller
    {
        private readonly SwTHDbContext db;

        public NivelesConocimientoController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/NivelConocimientoes
        [HttpGet]
        [Route("ListarNivelesConocimiento")]
        public async Task<List<NivelConocimiento>> GetNivelConocimiento()
        {
            try
            {
                return await db.NivelConocimiento.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<NivelConocimiento>();
            }
        }

        // GET: api/NivelConocimientoes/5
        [HttpGet("{id}")]
        public async Task<Response> GetNivelConocimiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido",
                    };
                }

                var NivelConocimiento = await db.NivelConocimiento.SingleOrDefaultAsync(m => m.IdNivelConocimiento == id);

                if (NivelConocimiento == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No encontrado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Resultado = NivelConocimiento,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // PUT: api/NivelConocimientoes/5
        [HttpPut("{id}")]
        public async Task<Response> PutNivelConocimiento([FromRoute] int id, [FromBody] NivelConocimiento NivelConocimiento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var existe = Existe(NivelConocimiento);
                if (existe.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Existe un registro de igual Nombre",
                    };
                }

                var NivelConocimientoActualizar = await db.NivelConocimiento.Where(x => x.IdNivelConocimiento == id).FirstOrDefaultAsync();

                if (NivelConocimientoActualizar != null)
                {
                    try
                    {
                        NivelConocimientoActualizar.Nombre = NivelConocimiento.Nombre;
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwTH),
                            ExceptionTrace = ex,
                            Message = "Se ha producido una excepci�n",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Error ",
                        };
                    }
                }




                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe"
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepci�n"
                };
            }
        }

        // POST: api/NivelConocimientoes
        [HttpPost]
        [Route("InsertarNivelesConocimiento")]
        public async Task<Response> PostNivelConocimiento([FromBody] NivelConocimiento NivelConocimiento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var respuesta = Existe(NivelConocimiento);
                if (!respuesta.IsSuccess)
                {
                    db.NivelConocimiento.Add(NivelConocimiento);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "OK"
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe un registro de igual Nombre..."
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // DELETE: api/NivelConocimientoes/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteNivelConocimiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido ",
                    };
                }

                var respuesta = await db.NivelConocimiento.SingleOrDefaultAsync(m => m.IdNivelConocimiento == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.NivelConocimiento.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = "Eliminado ",
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        private Response Existe(NivelConocimiento NivelConocimiento)
        {
            var bdd = NivelConocimiento.Nombre;
            var NivelConocimientorespuesta = db.NivelConocimiento.Where(p => p.Nombre == bdd).FirstOrDefault();
            if (NivelConocimientorespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe un Nombre de igual nombre",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = NivelConocimientorespuesta,
            };
        }

    }
}