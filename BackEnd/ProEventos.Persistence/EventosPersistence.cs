using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contracts;

namespace Eventos.Persistence
{
    public class EventosPersistence : IEventosPersist
    {
        private readonly ProEventosContext _context;
        
        public EventosPersistence(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                                .Include(e => e.RedesSociais)
                                                .Include(e => e.Lotes);

            if(includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id); 

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.RedesSociais);

            if(includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id).Where( e => e.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.RedesSociais);

            if(includePalestrantes){
                query = query.Include(e => e.PalestrantesEventos).ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id).Where(e => e.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}