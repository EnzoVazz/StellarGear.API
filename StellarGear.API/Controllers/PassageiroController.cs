using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Gerenciamento de Passageiros (Civis e Turistas Espaciais)")]
public class PassageiroController : ControllerBase
{
    private readonly IPassageiroRepository _repository;

    public PassageiroController(IPassageiroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os passageiros", Description = "Retorna uma lista completa com todos os passageiros cadastrados.")]
    public async Task<IActionResult> GetAll()
    {
        var passageiros = await _repository.GetAllAsync();
        var response = passageiros.Select(p => new PassageiroResponseDTO(p.Id, p.Nome, p.Cpf, p.Idade, p.StatusMedico));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um passageiro pelo ID", Description = "Retorna os detalhes de um passageiro específico com base no seu ID único.")]
    public async Task<IActionResult> GetById(int id)
    {
        var passageiro = await _repository.GetByIdAsync(id);
        if (passageiro == null) return NotFound("Passageiro não encontrado.");
        return Ok(new PassageiroResponseDTO(passageiro.Id, passageiro.Nome, passageiro.Cpf, passageiro.Idade, passageiro.StatusMedico));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cadastra um novo passageiro", Description = "Cria um novo registro de passageiro. A idade e o status médico são opcionais.")]
    public async Task<IActionResult> Create([FromBody] PassageiroRequestDTO request)
    {
        try
        {
            var passageiro = new Passageiro(request.Nome, request.Cpf, request.Idade, request.StatusMedico);
            await _repository.AddAsync(passageiro);
            var response = new PassageiroResponseDTO(passageiro.Id, passageiro.Nome, passageiro.Cpf, passageiro.Idade, passageiro.StatusMedico);
            return CreatedAtAction(nameof(GetById), new { id = passageiro.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza os dados de um passageiro", Description = "Modifica as informações de um passageiro existente.")]
    public async Task<IActionResult> Update(int id, [FromBody] PassageiroRequestDTO request)
    {
        try
        {
            var passageiro = await _repository.GetByIdAsync(id);
            if (passageiro == null) return NotFound("Passageiro não encontrado.");

            passageiro.UpdateNome(request.Nome);
            passageiro.UpdateCpf(request.Cpf);
            passageiro.UpdateIdade(request.Idade);
            if (!string.IsNullOrEmpty(request.StatusMedico)) passageiro.UpdateStatusMedico(request.StatusMedico);

            await _repository.UpdateAsync(passageiro);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remove um passageiro", Description = "Deleta o registro de um passageiro e todas as suas dependências (Trajes, Históricos) via Cascade.")]
    public async Task<IActionResult> Delete(int id)
    {
        var passageiro = await _repository.GetByIdAsync(id);
        if (passageiro == null) return NotFound("Passageiro não encontrado.");
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}