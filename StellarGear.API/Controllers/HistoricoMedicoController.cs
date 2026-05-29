using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Prontuários e Histórico Médico dos Passageiros")]
public class HistoricoMedicoController : ControllerBase
{
    private readonly IHistoricoMedicoRepository _repository;

    public HistoricoMedicoController(IHistoricoMedicoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista os prontuários globais", Description = "Exibe todos os registros de saúde cadastrados para a tripulação.")]
    public async Task<IActionResult> GetAll()
    {
        var historicos = await _repository.GetAllAsync();
        var response = historicos.Select(h => new HistoricoMedicoResponseDTO(h.Id, h.IdPassageiro, h.Diagnostico, h.DataRegistro));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um histórico pelo ID", Description = "Retorna um diagnóstico ou evolução clínica específica.")]
    public async Task<IActionResult> GetById(int id)
    {
        var historico = await _repository.GetByIdAsync(id);
        if (historico == null) return NotFound();
        return Ok(new HistoricoMedicoResponseDTO(historico.Id, historico.IdPassageiro, historico.Diagnostico, historico.DataRegistro));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo registro clínico", Description = "Adiciona um diagnóstico ao prontuário do passageiro.")]
    public async Task<IActionResult> Create([FromBody] HistoricoMedicoRequestDTO request)
    {
        try
        {
            var historico = new HistoricoMedico(request.IdPassageiro, request.Diagnostico);
            await _repository.AddAsync(historico);
            var response = new HistoricoMedicoResponseDTO(historico.Id, historico.IdPassageiro, historico.Diagnostico, historico.DataRegistro);
            return CreatedAtAction(nameof(GetById), new { id = historico.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza o diagnóstico", Description = "Permite corrigir a descrição de um quadro clínico ou diagnóstico médico.")]
    public async Task<IActionResult> Update(int id, [FromBody] HistoricoMedicoRequestDTO request)
    {
        try
        {
            var historico = await _repository.GetByIdAsync(id);
            if (historico == null) return NotFound();

            historico.UpdateDiagnostico(request.Diagnostico);
            await _repository.UpdateAsync(historico);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remove um registro médico", Description = "Exclui a anotação do prontuário do banco de dados.")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}