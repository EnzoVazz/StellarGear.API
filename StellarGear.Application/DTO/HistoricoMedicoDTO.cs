namespace StellarGear.Application.DTO;

public record HistoricoMedicoRequestDTO(int IdPassageiro, string? Diagnostico);

public record HistoricoMedicoResponseDTO(int Id, int IdPassageiro, string? Diagnostico, DateTime? DataRegistro);