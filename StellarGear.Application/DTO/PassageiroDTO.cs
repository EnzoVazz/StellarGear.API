namespace StellarGear.Application.DTO;

public record PassageiroRequestDTO(string Nome, string Cpf, int? Idade, string? StatusMedico);

public record PassageiroResponseDTO(int Id, string Nome, string Cpf, int? Idade, string? StatusMedico);