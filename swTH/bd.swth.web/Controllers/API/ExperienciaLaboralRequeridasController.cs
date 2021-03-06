using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.swth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.swth.entidades.ViewModels;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/ExperienciaLaboralRequeridas")]
    public class ExperienciaLaboralRequeridasController : Controller
    {
        private readonly SwTHDbContext db;

        public ExperienciaLaboralRequeridasController(SwTHDbContext db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ListarExperienciaLaboralRequeridaPorIndiceOcupacional")]
        public async Task<List<ExperienciaLaboralRequeridaViewModel>> ListarExperienciaLaboralRequeridaPorIndiceOcupacional([FromBody]IndiceOcupacional indiceOcupacional)
        {
            var ListaExperienciaLaboralRequeridas = await db.IndiceOcupacionalExperienciaLaboralRequerida
                                                    .Join(db.IndiceOcupacional
                                                    , rta => rta.IdIndiceOcupacional, ind => ind.IdIndiceOcupacional,
                                                    (rta, ind) => new { hm = rta, gh = ind })
                                                    .Join(db.ExperienciaLaboralRequerida
                                                    , ind_1 => ind_1.hm.ExperienciaLaboralRequerida.IdExperienciaLaboralRequerida, valor => valor.IdExperienciaLaboralRequerida,
                                                    (ind_1, valor) => new { ca = ind_1, rt = valor })
                                                    .Where(ds => ds.ca.hm.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                                    .Select(t => new ExperienciaLaboralRequeridaViewModel
                                                    {
                                                        IdExperienciaLaboralRequerida = t.rt.IdExperienciaLaboralRequerida,
                                                        MesExperiencia=t.rt.MesExperiencia,
                                                        AnoExperiencia=t.rt.AnoExperiencia,
                                                        DescripcionEspecificidadExperiencia=t.rt.EspecificidadExperiencia.Descripcion,
                                                        IdEspecificidadExperiencia=t.rt.EspecificidadExperiencia.IdEspecificidadExperiencia,
                                                        IdEstudio=t.rt.Estudio.IdEstudio,
                                                        NombreEstudio=t.rt.Estudio.Nombre,
                                                        IdIndiceOcupacional=t.ca.gh.IdIndiceOcupacional,
                                                    })
                                                    .ToListAsync();

            if (ListaExperienciaLaboralRequeridas.Count == 0)
            {
                ListaExperienciaLaboralRequeridas.Add(new ExperienciaLaboralRequeridaViewModel { IdIndiceOcupacional = indiceOcupacional.IdIndiceOcupacional, IdExperienciaLaboralRequerida = -1 });
            }

            return ListaExperienciaLaboralRequeridas;

        }


        [HttpPost]
        [Route("EliminarIncideOcupacionalExperienciaLaboralRequeridas")]
        public async Task<Response> EliminarIncideOcupacionalExperienciaLaboralRequeridas([FromBody] IndiceOcupacionalExperienciaLaboralRequerida indiceOcupacionalExperienciaLaboralRequerida)
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

                var respuesta = await db.IndiceOcupacionalExperienciaLaboralRequerida.SingleOrDefaultAsync(m => m.IdExperienciaLaboralRequerida == indiceOcupacionalExperienciaLaboralRequerida.IdExperienciaLaboralRequerida
                                      && m.IdIndiceOcupacional == indiceOcupacionalExperienciaLaboralRequerida.IdIndiceOcupacional);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.IndiceOcupacionalExperienciaLaboralRequerida.Remove(respuesta);
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

        [HttpPost]
        [Route("ListarExperienciaLaboralRequeridaNoAsignadasIndiceOcupacional")]
        public async Task<List<ExperienciaLaboralRequeridaViewModel>> ListarExperienciaLaboralRequeridaNoAsignadasIndiceOcupacional([FromBody]IndiceOcupacional indiceOcupacional)
        {

                
                var Lista = await db.ExperienciaLaboralRequerida
                                   .Where(m => !db.IndiceOcupacionalExperienciaLaboralRequerida
                                                   .Where(a => a.IndiceOcupacional.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                                   .Select(iom => iom.IdExperienciaLaboralRequerida)
                                                   .Contains(m.IdExperienciaLaboralRequerida)).Include(x => x.EspecificidadExperiencia).Include(x => x.Estudio)
                                          .ToListAsync();

            var listaSalida = new List<ExperienciaLaboralRequeridaViewModel>();
            if (Lista.Count == 0)
            {
                listaSalida.Add(new ExperienciaLaboralRequeridaViewModel { IdIndiceOcupacional = indiceOcupacional.IdIndiceOcupacional, IdExperienciaLaboralRequerida = -1 });
            }

            else
            {
                foreach (var item in Lista)
                {
                    listaSalida.Add(new ExperienciaLaboralRequeridaViewModel
                    {
                        AnoExperiencia = item.AnoExperiencia,
                        DescripcionEspecificidadExperiencia=item.EspecificidadExperiencia.Descripcion,
                        MesExperiencia=item.MesExperiencia,
                        NombreEstudio=item.Estudio.Nombre,
                        IdExperienciaLaboralRequerida = item.IdExperienciaLaboralRequerida,
                        IdIndiceOcupacional = indiceOcupacional.IdIndiceOcupacional,
                    });
                }
            }

            return listaSalida;


        }


        // GET: api/ExperienciaLaboralRequeridas
        [HttpGet]
        [Route("ListarExperienciaLaboralRequeridas")]
        public async Task<List<ExperienciaLaboralRequerida>> GetExperienciaLaboralRequeridas()
        {
            try
            {
                return await db.ExperienciaLaboralRequerida.Include(x => x.EspecificidadExperiencia).Include(x=>x.Estudio).OrderBy(x => x.IdExperienciaLaboralRequerida).ToListAsync();
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
                return new List<ExperienciaLaboralRequerida>();
            }
        }

        // GET: api/ExperienciaLaboralRequeridas/5
        [HttpGet("{id}")]
        public async Task<Response> GetExperienciaLaboralRequerida([FromRoute] int id)
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

                var ExperienciaLaboralRequerida = await db.ExperienciaLaboralRequerida.SingleOrDefaultAsync(m => m.IdExperienciaLaboralRequerida == id);

                if (ExperienciaLaboralRequerida == null)
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
                    Resultado = ExperienciaLaboralRequerida,
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

        // PUT: api/ExperienciaLaboralRequeridas/5
        [HttpPut("{id}")]
        public async Task<Response> PutExperienciaLaboralRequerida([FromRoute] int id, [FromBody] ExperienciaLaboralRequerida experienciaLaboralRequerida)
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

                var existe = Existe(experienciaLaboralRequerida);
                var ExperienciaLaboralRequeridaActualizar = (ExperienciaLaboralRequerida)existe.Resultado;
                if (existe.IsSuccess)
                {
                    //if (ExperienciaLaboralRequeridaActualizar.IdExperienciaLaboralRequerida == experienciaLaboralRequerida.IdExperienciaLaboralRequerida)
                    //{
                    //    return new Response
                    //    {
                    //        IsSuccess = true,
                    //    };
                    //}
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }
                var ExperienciaLaboralRequerida = db.ExperienciaLaboralRequerida.Find(experienciaLaboralRequerida.IdExperienciaLaboralRequerida);

                ExperienciaLaboralRequerida.IdEspecificidadExperiencia = experienciaLaboralRequerida.IdEspecificidadExperiencia;
                ExperienciaLaboralRequerida.AnoExperiencia = experienciaLaboralRequerida.AnoExperiencia;
                ExperienciaLaboralRequerida.MesExperiencia = experienciaLaboralRequerida.MesExperiencia;
                ExperienciaLaboralRequerida.IdEstudio = experienciaLaboralRequerida.IdEstudio;
                db.ExperienciaLaboralRequerida.Update(ExperienciaLaboralRequerida);
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
                    IsSuccess = true,
                    Message = Mensaje.Excepcion,
                };
            }

        }

        // POST: api/ExperienciaLaboralRequerida
        [HttpPost]
        [Route("InsertarExperienciaLaboralRequerida")]
        public async Task<Response> PostExperienciaLaboralRequerida([FromBody] ExperienciaLaboralRequerida ExperienciaLaboralRequerida)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ""
                    };
                }

                var respuesta = Existe(ExperienciaLaboralRequerida);
                if (!respuesta.IsSuccess)
                {
                    db.ExperienciaLaboralRequerida.Add(ExperienciaLaboralRequerida);
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
                    Message = Mensaje.ExisteRegistro,
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

        // DELETE: api/ExperienciaLaboralRequeridas/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteExperienciaLaboralRequerida([FromRoute] int id)
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

                var respuesta = await db.ExperienciaLaboralRequerida.SingleOrDefaultAsync(m => m.IdExperienciaLaboralRequerida == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.ExperienciaLaboralRequerida.Remove(respuesta);
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

        private Response Existe(ExperienciaLaboralRequerida ExperienciaLaboralRequerida)
        {
           
            var ExperienciaLaboralRequeridarespuesta = db.ExperienciaLaboralRequerida.Where(p => p.IdEspecificidadExperiencia == ExperienciaLaboralRequerida.IdEspecificidadExperiencia  && p.IdEstudio == ExperienciaLaboralRequerida.IdEstudio && p.AnoExperiencia==ExperienciaLaboralRequerida.AnoExperiencia && p.MesExperiencia == ExperienciaLaboralRequerida.MesExperiencia).FirstOrDefault();
            if (ExperienciaLaboralRequeridarespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.ExisteRegistro,
                    Resultado = ExperienciaLaboralRequeridarespuesta,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = ExperienciaLaboralRequeridarespuesta,
            };
        }

    }
}