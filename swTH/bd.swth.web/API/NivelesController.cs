using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.swth.entidades.Utils;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Niveles")]
    public class NivelesController : Controller
    {
        private readonly SwTHDbContext db;

        public NivelesController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/Nivels
        [HttpGet]
        [Route("ListarNiveles")]
        public async Task<List<Nivel>> GetNivel()
        {
            try
            {
                return await db.Nivel.OrderBy(x => x.Nombre).ToListAsync();
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
                return new List<Nivel>();
            }
        }

        // GET: api/Nivels/5
        [HttpGet("{id}")]
        public async Task<Response> GetNivel([FromRoute] int id)
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

                var adscbdd = await db.Nivel.SingleOrDefaultAsync(m => m.IdNivel == id);

                if (adscbdd == null)
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
                    Resultado = adscbdd,
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

        // PUT: api/Nivels/5
        [HttpPut("{id}")]
        public async Task<Response> PutNivel([FromRoute] int id, [FromBody] Nivel Nivel)
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


                try
                {
                    var entidad = await db.Nivel.Where(x => x.IdNivel == id).FirstOrDefaultAsync();

                    if (entidad == null)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "No existe informaci�n acerca del Nivel ",
                        };

                    }
                    else
                    {

                        entidad.Nombre = Nivel.Nombre;
                        db.Nivel.Update(entidad);
                        await db.SaveChangesAsync();
                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };
                    }


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
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepci�n"
                };
            }
        }

        // POST: api/Nivels
        [HttpPost]
        [Route("InsertarNivel")]
        public async Task<Response> PostNivel([FromBody] Nivel Nivel)
        {
            try
            {

                var respuesta = Existe(Nivel.Nombre);
                if (!respuesta.IsSuccess)
                {
                    db.Nivel.Add(Nivel);
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

        // DELETE: api/Nivels/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteNivel([FromRoute] int id)
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

                var respuesta = await db.Nivel.SingleOrDefaultAsync(m => m.IdNivel == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.Nivel.Remove(respuesta);
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

        private bool NivelExists(int id)
        {
            return db.Nivel.Any(e => e.IdNivel == id);
        }


        public Response Existe(string nombreNivel)
        {

            var loglevelrespuesta = db.Nivel.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == nombreNivel).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe un sistema de igual nombre",
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