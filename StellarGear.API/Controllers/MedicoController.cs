using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Gerenciamento do Corpo Médico e Especialistas")]
public class MedicoController : ControllerBase
{
    private readonly IMedicoRepository _repository;

    public MedicoController(IMedicoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os médicos", Description = "Obtém o cadastro de todos os médicos disponíveis na tripulação.")]
    public async Task<IActionResult> GetAll()
    {
        var medicos = await _repository.GetAllAsync();
        var response = medicos.Select(m => new MedicoResponseDTO(m.Id, m.Nome, m.Crm, m.Especialidade));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um médico pelo ID", Description = "Retorna os detalhes e o CRM de um médico específico.")]
    public async Task<IActionResult> GetById(int id)
    {
        var medico = await _repository.GetByIdAsync(id);
        if (medico == null) return NotFound("Médico não encontrado.");
        return Ok(new MedicoResponseDTO(medico.Id, medico.Nome, medico.Crm, medico.Especialidade));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cadastra um novo médico", Description = "Adiciona um novo profissional de saúde ao sistema.")]
    public async Task<IActionResult> Create([FromBody] MedicoRequestDTO request)
    {
        try
        {
            var medico = new Medico(request.Nome, request.Crm, request.Especialidade);
            await _repository.AddAsync(medico);
            var response = new MedicoResponseDTO(medico.Id, medico.Nome, medico.Crm, medico.Especialidade);
            return CreatedAtAction(nameof(GetById), new { id = medico.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza o cadastro médico", Description = "Permite corrigir informações como Nome, CRM ou Especialidade.")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicoRequestDTO request)
    {
        try
        {
            var medico = await _repository.GetByIdAsync(id);
            if (medico == null) return NotFound("Médico não encontrado.");

            medico.UpdateNome(request.Nome);
            medico.UpdateCrm(request.Crm);
            medico.UpdateEspecialidade(request.Especialidade);
            
            await _repository.UpdateAsync(medico);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Desliga um médico do sistema", Description = "Remove o registro de um médico. Retorna erro se ele possuir alertas pendentes vinculados.")]
    public async Task<IActionResult> Delete(int id)
    {
        var medico = await _repository.GetByIdAsync(id);
        if (medico == null) return NotFound("Médico não encontrado.");
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}