namespace StellarGear.Application.DTO;

public record AlertaEmergenciaRequestDTO(int IdLeitura, int IdMedico, string? Descricao, string? NivelGravidade);

public record AlertaEmergenciaResponseDTO(int Id, int IdLeitura, int IdMedico, string? Descricao, string? NivelGravidade, char Resolvido, DateTime? DataAlerta);