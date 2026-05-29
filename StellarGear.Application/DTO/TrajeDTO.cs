namespace StellarGear.Application.DTO;

public record TrajeRequestDTO(int IdPassageiro, string? CodigoRfid);

public record TrajeResponseDTO(int Id, int IdPassageiro, string? CodigoRfid, DateTime? DataAlocacao);