using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class ProEventosPersistence : IPalestrantesPersist
    {
        private readonly ProEventosContext _context;
        
        public ProEventosPersistence(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
       
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(e => e.RedesSociais);

            if(includeEventos){
                query = query.Include( e => e.PalestrantesEventos).ThenInclude( e => e.Evento);
            }

            query = query.OrderBy( e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include( e => e.RedesSociais)
                .Where( x => x.Nome.ToLower().Contains(nome.ToLower()));

            if(includeEventos){
                query = query.Include( e => e.PalestrantesEventos).ThenInclude( pe => pe.Evento);
            }

            return await query.ToArrayAsync();

        }
        public async Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais).Where( p => p.Id == id);

            if(includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(p => p.Evento);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}