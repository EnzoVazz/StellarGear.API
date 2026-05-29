using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Alocação e Controle de Trajes Espaciais (Equipamento)")]
public class TrajeController : ControllerBase
{
    private readonly ITrajeRepository _repository;
    private readonly IPassageiroRepository _passageiroRepository;

    public TrajeController(ITrajeRepository repository, IPassageiroRepository passageiroRepository)
    {
        _repository = repository;
        _passageiroRepository = passageiroRepository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os trajes", Description = "Retorna o inventário completo de trajes alocados aos passageiros.")]
    public async Task<IActionResult> GetAll()
    {
        var trajes = await _repository.GetAllAsync();
        var response = trajes.Select(t => new TrajeResponseDTO(t.Id, t.IdPassageiro, t.CodigoRfid, t.DataAlocacao));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca um traje pelo ID", Description = "Exibe as informações do traje e o seu código RFID único.")]
    public async Task<IActionResult> GetById(int id)
    {
        var traje = await _repository.GetByIdAsync(id);
        if (traje == null) return NotFound("Traje não encontrado.");
        return Ok(new TrajeResponseDTO(traje.Id, traje.IdPassageiro, traje.CodigoRfid, traje.DataAlocacao));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Aloca um traje para um passageiro", Description = "Vincula um novo traje a um passageiro existente validando o ID do passageiro.")]
    public async Task<IActionResult> Create([FromBody] TrajeRequestDTO request)
    {
        try
        {
            var passageiroExiste = await _passageiroRepository.GetByIdAsync(request.IdPassageiro);
            if (passageiroExiste == null) return BadRequest("O ID do Passageiro informado não existe.");

            var traje = new Traje(request.IdPassageiro, request.CodigoRfid);
            await _repository.AddAsync(traje);
            var response = new TrajeResponseDTO(traje.Id, traje.IdPassageiro, traje.CodigoRfid, traje.DataAlocacao);
            return CreatedAtAction(nameof(GetById), new { id = traje.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza o RFID do traje", Description = "Modifica o identificador físico (RFID) do traje em caso de manutenção.")]
    public async Task<IActionResult> Update(int id, [FromBody] TrajeRequestDTO request)
    {
        try
        {
            var traje = await _repository.GetByIdAsync(id);
            if (traje == null) return NotFound("Traje não encontrado.");

            traje.UpdateCodigoRfid(request.CodigoRfid);
            await _repository.UpdateAsync(traje);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Desaloca e remove um traje", Description = "Exclui o registro de um traje. Leituras vinculadas a ele serão apagadas.")]
    public async Task<IActionResult> Delete(int id)
    {
        var traje = await _repository.GetByIdAsync(id);
        if (traje == null) return NotFound("Traje não encontrado.");
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}