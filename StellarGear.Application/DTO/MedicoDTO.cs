namespace StellarGear.Application.DTO;

public record MedicoRequestDTO(string Nome, string Crm, string? Especialidade);

public record MedicoResponseDTO(int Id, string Nome, string Crm, string? Especialidade);