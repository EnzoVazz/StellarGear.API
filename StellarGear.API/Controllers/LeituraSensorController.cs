using Microsoft.AspNetCore.Mvc;
using StellarGear.Application.DTO;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarGear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Monitoramento de Sinais Vitais em Tempo Real")]
public class LeituraSensorController : ControllerBase
{
    private readonly ILeituraSensorRepository _repository;
    private readonly ITrajeRepository _trajeRepository;

    public LeituraSensorController(ILeituraSensorRepository repository, ITrajeRepository trajeRepository)
    {
        _repository = repository;
        _trajeRepository = trajeRepository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Histórico geral de leituras", Description = "Retorna todos os dados captados pelos sensores dos trajes (Temp, Umi, BPM).")]
    public async Task<IActionResult> GetAll()
    {
        var leituras = await _repository.GetAllAsync();
        var response = leituras.Select(l => new LeituraSensorResponseDTO(l.Id, l.IdTraje, l.Temperatura, l.Humidade, l.Batimentos, l.DataLeitura));
        return Ok(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Busca uma leitura específica", Description = "Exibe os detalhes de uma captação de dados específica pelo seu ID.")]
    public async Task<IActionResult> GetById(int id)
    {
        var leitura = await _repository.GetByIdAsync(id);
        if (leitura == null) return NotFound("Leitura não encontrada.");
        return Ok(new LeituraSensorResponseDTO(leitura.Id, leitura.IdTraje, leitura.Temperatura, leitura.Humidade, leitura.Batimentos, leitura.DataLeitura));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Registra novos sinais vitais", Description = "Recebe os dados vitais do traje e armazena a leitura no banco de dados.")]
    public async Task<IActionResult> Create([FromBody] LeituraSensorRequestDTO request)
    {
        try
        {
            var trajeExiste = await _trajeRepository.GetByIdAsync(request.IdTraje);
            if (trajeExiste == null) return BadRequest("O ID do Traje informado não existe.");

            var leitura = new LeituraSensor(request.IdTraje, request.Temperatura, request.Humidade, request.Batimentos);
            await _repository.AddAsync(leitura);
            var response = new LeituraSensorResponseDTO(leitura.Id, leitura.IdTraje, leitura.Temperatura, leitura.Humidade, leitura.Batimentos, leitura.DataLeitura);
            return CreatedAtAction(nameof(GetById), new { id = leitura.Id }, response);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Corrige uma leitura de sensor", Description = "Ajusta os dados de temperatura, humidade ou batimentos em caso de erro no hardware.")]
    public async Task<IActionResult> Update(int id, [FromBody] LeituraSensorRequestDTO request)
    {
        try
        {
            var leitura = await _repository.GetByIdAsync(id);
            if (leitura == null) return NotFound("Leitura não encontrada.");

            leitura.UpdateDadosSensores(request.Temperatura, request.Humidade, request.Batimentos);
            await _repository.UpdateAsync(leitura);
            return NoContent();
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remove um registro de leitura", Description = "Deleta fisicamente uma leitura de sensor específica do banco.")]
    public async Task<IActionResult> Delete(int id)
    {
        var leitura = await _repository.GetByIdAsync(id);
        if (leitura == null) return NotFound("Leitura não encontrada.");
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}