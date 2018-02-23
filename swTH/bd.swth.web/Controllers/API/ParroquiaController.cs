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
using bd.swth.entidades.Utils;

namespace bd.swrm.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Parroquia")]
    public class ParroquiaController : Controller
    {
        private readonly SwTHDbContext db;

        public ParroquiaController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/Parroquia
        [HttpGet]
        [Route("ListarParroquia")]
        public async Task<List<Parroquia>> GetParroquia()
        {
            try
            {
                return await db.Parroquia.Include(x=>x.Ciudad).ThenInclude(x=>x.Provincia).ThenInclude(x => x.Pais).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<Parroquia>();
            }
        }


        [HttpPost]
        [Route("ListarParroquiaPorCiudad")]
        public async Task<List<Parroquia>> GetParroquia([FromBody] Ciudad ciudad)
        {
            try
            {
                if (ciudad==null)
                {
                    return new List<Parroquia>();
                }
                return await db.Parroquia.Where(x => x.IdCiudad == ciudad.IdCiudad).OrderBy(x => x.Nombre).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<Parroquia>();
            }
        }

        // GET: api/Parroquia/5
        [HttpGet("{id}")]
        public async Task<Response> GetParroquia([FromRoute] int id)
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

                var pParroquia = await db.Parroquia.Include(x=>x.Ciudad).ThenInclude(X=>X.Provincia).ThenInclude(x=>x.Pais).Where(x=>x.IdParroquia==id).FirstOrDefaultAsync();

                if (pParroquia == null)
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
                    Resultado = pParroquia,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
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

        // PUT: api/Parroquia/5
        [HttpPut("{id}")]
        public async Task<Response> PutParroquia([FromRoute] int id, [FromBody] Parroquia pParroquia)
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




                var respuesta = Existe(pParroquia);
                
                if (respuesta.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.ExisteRegistro
                    };
                }




                var pParroquiaActualizar = await db.Parroquia.Where(x => x.IdParroquia == id).FirstOrDefaultAsync();
                if (pParroquiaActualizar != null)
                {
                    try
                    {
                        pParroquiaActualizar.Nombre = pParroquia.Nombre;
                        pParroquiaActualizar.IdCiudad = pParroquia.IdCiudad;

                        db.Parroquia.Update(pParroquiaActualizar);
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
                            ExceptionTrace = ex,
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

        // POST: api/Parroquia
        [HttpPost]
        [Route("InsertarParroquia")]
        public async Task<Response> PostParroquia([FromBody] Parroquia pParroquia)
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

               
                var existe = Existe(pParroquia);
                if (existe.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Existe una parroquia con ese nombre asignada a esa ciudad",
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Satisfactorio
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
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

        // DELETE: api/Parroquia/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteParroquia([FromRoute] int id)
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

                var respuesta = await db.Parroquia.SingleOrDefaultAsync(m => m.IdParroquia == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }



                db.Parroquia.Remove(respuesta);
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
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro,
                };
            }
        }

        private bool ParroquiaExists(string nombre)
        {
            return db.Parroquia.Any(e => e.Nombre == nombre);
        }

        public Response Existe(Parroquia pParroquia)
        {
            var bdd = pParroquia.Nombre.ToUpper().TrimEnd().TrimStart();
            var ppc = pParroquia.IdCiudad;

            var loglevelrespuesta = db.Parroquia.Where(
                p => 
                p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd &&
                p.IdCiudad == ppc
            
            ).FirstOrDefault();
            if (loglevelrespuesta != null)
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
                Resultado = loglevelrespuesta,
            };
        }

    }
}