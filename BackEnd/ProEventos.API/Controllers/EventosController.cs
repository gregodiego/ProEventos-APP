using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventoService;


        public EventosController(IEventosService eventosService)
        {
            _eventoService = eventosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventos(bool includePalestrantes){
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) return NotFound("Evento não encontrado");
                return Ok(eventos);

            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Erro ao tentar recuperar eventos. ERRO: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id, bool includePalestrante){
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id, includePalestrante);

                if(evento == null) return NotFound("Evento não encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(500, $"Erro ao recuperar eventos. ERRO {ex.Message}  ");
            }
            
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema, bool includePalestrante){
            try
            {
                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, includePalestrante);

                if(evento == null) return NotFound("Objeto não encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Erro ao recuperar eventos. ERRO {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(Evento model){
            try
            {
                var evento = await _eventoService.AddEvento(model);
                if(evento == null) return BadRequest("Erro ao tentar adicionar evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Erro ao adicionar evento. ERRO {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model){
            try
            {
                var evento = await _eventoService.UpdateEvento(id, model);

                if(evento == null) return BadRequest("Não foi possível atualizar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Erro ao tentar atualizar evento, ERRO {ex.Message}"); 
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            try
            {
                //esse método poderia ser escrito com operador condicional ternário
                return await _eventoService.RemoveEvento(id) ? BadRequest("Erro ao tentar deletar evento") : Ok("Evento deletado") ; 

                //jeito tradicional de fazer o if
                // if(await _eventoService.RemoveEvento(id)){
                //     return BadRequest("Erro ao tentar deletar evento");
                // }else{
                //     return Ok("Evento deletado");
                // }
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Erro ao deletar evento, ERRO {ex.Message}");
            }
        }
    }
}
