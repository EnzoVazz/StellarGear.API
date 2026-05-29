namespace StellarGear.Application.DTO;

public record LeituraSensorRequestDTO(int IdTraje, decimal? Temperatura, decimal? Humidade, decimal? Batimentos);

public record LeituraSensorResponseDTO(int Id, int IdTraje, decimal? Temperatura, decimal? Humidade, decimal? Batimentos, DateTime? DataLeitura);