using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Gestão de Alertas e Intervenções de Emergência")]
public class AlertaEmergenciaController : ControllerBase
{
    private readonly IAlertaEmergenciaRepository _repository;

    public AlertaEmergenciaController(IAlertaEmergenciaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os alertas", Description = "Obtém todos os alertas gerados por anomalias nas leituras dos sensores.")]
    public async Task<IActionResult> GetAll()
    {
        var alertas = await _repository.GetAllAsync();
        var response = alertas.Select(a => new AlertaEmergenciaResponseDTO(a.Id, a.IdLeitura, a.IdMedico, a.Descricao, a.NivelGravidade, a.Resolvido, a.DataAlerta));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca os detalhes de um alerta", Description = "Verifica se um alerta foi atendido e as suas características de gravidade.")]
    public async Task<IActionResult> GetById(int id)
    {
        var alerta = await _repository.GetByIdAsync(id);
        if (alerta == null) return NotFound();
        return Ok(new AlertaEmergenciaResponseDTO(alerta.Id, alerta.IdLeitura, alerta.IdMedico, alerta.Descricao, alerta.NivelGravidade, alerta.Resolvido, alerta.DataAlerta));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Dispara um novo alerta", Description = "Cria um alerta vinculando uma leitura fora do padrão a um médico responsável.")]
    public async Task<IActionResult> Create([FromBody] AlertaEmergenciaRequestDTO request)
    {
        try
        {
            var alerta = new AlertaEmergencia(request.IdLeitura, request.IdMedico, request.Descricao, request.NivelGravidade);
            await _repository.AddAsync(alerta);
            var response = new AlertaEmergenciaResponseDTO(alerta.Id, alerta.IdLeitura, alerta.IdMedico, alerta.Descricao, alerta.NivelGravidade, alerta.Resolvido, alerta.DataAlerta);
            return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza detalhes do alerta", Description = "Permite editar a descrição e o nível de gravidade de um alerta pendente.")]
    public async Task<IActionResult> Update(int id, [FromBody] AlertaEmergenciaRequestDTO request)
    {
        try
        {
            var alerta = await _repository.GetByIdAsync(id);
            if (alerta == null) return NotFound();

            alerta.UpdateDetalhes(request.Descricao, request.NivelGravidade);
            await _repository.UpdateAsync(alerta);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPatch("{id}/resolver")]
    [SwaggerOperation(Summary = "Marca o alerta como Resolvido", Description = "Endpoint exclusivo para o médico informar que a intervenção foi concluída (muda o status para 'S').")]
    public async Task<IActionResult> Resolver(int id)
    {
        var alerta = await _repository.GetByIdAsync(id);
        if (alerta == null) return NotFound();

        alerta.ResolverAlerta();
        await _repository.UpdateAsync(alerta);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Exclui um alerta do sistema", Description = "Remove um alerta de emergência do banco de dados.")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}