using System;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventosService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        public readonly IEventosPersist _eventosPersist;

        public EventosService(IGeralPersist geralPersist, IEventosPersist eventosPersist)
        {
            _eventosPersist = eventosPersist;
            _geralPersist = geralPersist;

        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _geralPersist.Add(model);
                if(await _geralPersist.SaveChangesAsync()){
                    return await _eventosPersist.GetEventoByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveEvento(int eventoId)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(eventoId);
                if(evento == null) new Exception("Evento não foi encontrado");

                _geralPersist.Delete(evento);   

                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(eventoId);
                if(evento == null) return null;

                model.Id = evento.Id;

                _geralPersist.Update(model);   

                if(await _geralPersist.SaveChangesAsync()){
                    return await _eventosPersist.GetEventoByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersist.GetAllEventosAsync(includePalestrantes);

                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);

                if(eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventosPersist.GetEventoByIdAsync(id, includePalestrantes);

            if(evento == null) return null;

            return evento; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        
    }
}