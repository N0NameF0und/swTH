using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.Enumeradores;
using Microsoft.EntityFrameworkCore;
using bd.log.guardar.ObjectTranfer;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Utiles;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Provincia")]
    public class ProvinciaController : Controller
    {
        private readonly SwTHDbContext db;

        public ProvinciaController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/Provincia
        [HttpGet]
        [Route("ListarProvincia")]
        public async Task<List<Provincia>> GetProvincia()
        {
            try
            {
                return await db.Provincia.OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<Provincia>();
            }
        }

        // GET: api/Provincia/5
        [HttpGet("{id}")]
        public async Task<Response> GetProvincia([FromRoute] int id)
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

                var provincia = await db.Provincia.SingleOrDefaultAsync(m => m.IdProvincia == id);

                if (provincia == null)
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
                    Resultado = provincia,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
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

        // PUT: api/Provincia/5
        [HttpPut("{id}")]
        public async Task<Response> PutProvincia([FromRoute] int id, [FromBody] Provincia provincia)
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

                var provinciaActualizar = await db.Provincia.Where(x => x.IdProvincia == id).FirstOrDefaultAsync();
                if (provinciaActualizar != null)
                {
                    try
                    {
                        provinciaActualizar.Nombre = provincia.Nombre;
                        db.Provincia.Update(provinciaActualizar);
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
                            Message = "Se ha producido una exepci�n",
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

        // POST: api/Provincia
        [HttpPost]
        [Route("InsertarProvincia")]
        public async Task<Response> PostProvincia([FromBody] Provincia provincia)
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

                var respuesta = Existe(provincia);
                if (!respuesta.IsSuccess)
                {
                    db.Provincia.Add(provincia);
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
                    Message = "OK"
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepci�n",
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

        // DELETE: api/Provincia/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteProvincia([FromRoute] int id)
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

                var respuesta = await db.Provincia.SingleOrDefaultAsync(m => m.IdProvincia == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.Provincia.Remove(respuesta);
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
                    Message = "Se ha producido una exepci�n",
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

        private bool ProvinciaExists(string nombre)
        {
            return db.Provincia.Any(e => e.Nombre == nombre);
        }

        public Response Existe(Provincia provincia)
        {
            var bdd = provincia.Nombre.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.Provincia.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe una provincia de igual nombre",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = loglevelrespuesta,
            };
        }

    }
}