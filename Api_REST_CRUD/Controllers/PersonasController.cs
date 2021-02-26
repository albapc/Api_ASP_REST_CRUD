using ConectarDatos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

namespace Api_REST_CRUD.Controllers
{
    public class PersonasController : ApiController
    {
        private AdventureWorksLT2017Entities dbContext = new AdventureWorksLT2017Entities();

        // Visualiza todos los registros (api/personas)
        [HttpGet]
        public IEnumerable<Persona> Get()
        {
            using (AdventureWorksLT2017Entities personasEntities = new AdventureWorksLT2017Entities())
            {
                return personasEntities.Personas.ToList();
            }
        }

        // Visualiza solo un registro (api/personas/1)
        [HttpGet]
        public Persona Get(int id)
        {
            using(AdventureWorksLT2017Entities personasEntities = new AdventureWorksLT2017Entities())
            {
                return personasEntities.Personas.FirstOrDefault(e => e.ID == id);
            }
        }

        // Graba nuevos registros en la base de datos personas
        [HttpPost]
        public IHttpActionResult AgregarPersona([FromBody] Persona per)
        {
            if (ModelState.IsValid)
            {
                dbContext.Personas.Add(per);
                dbContext.SaveChanges();
                return Ok(per);
            }
            else
            {
                return BadRequest();
            }
        }

        // Modifica los registros existentes en la base de datos personas
        [HttpPut]
        public IHttpActionResult ActualizarPersona(int id, [FromBody] Persona per)
        {
            if (ModelState.IsValid)
            {
                var PersonaExiste = dbContext.Personas.Count(c => c.ID == id) > 0;

                if (PersonaExiste)
                {
                    dbContext.Entry(per).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        // Elimina un registro en la base de datos personas (api/personas/1)
        [HttpDelete]
        public IHttpActionResult EliminarPersona(int id)
        {
            var per = dbContext.Personas.Find(id);

            if(per != null)
            {
                dbContext.Personas.Remove(per);
                dbContext.SaveChanges();

                return Ok(per);
            } 
            else
            {
                return NotFound();
            }
        }
    }
}
