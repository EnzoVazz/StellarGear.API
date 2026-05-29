using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class Passageiro : BaseEntity
{
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public int? Idade { get; private set; }
    public string? StatusMedico { get; private set; }
    public List<Traje> Trajes { get; private set; }
    public List<HistoricoMedico> Historicos { get; private set; }

    public Passageiro(string nome, string cpf, int? idade, string? statusMedico = "EM AVALIACAO")
    {
        UpdateNome(nome);
        UpdateCpf(cpf);
        UpdateIdade(idade);
        StatusMedico = statusMedico;
        
        Trajes = [];
        Historicos = [];
    }

    protected Passageiro() 
    { 
        Trajes = [];
        Historicos = [];
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome não pode ser vazio.");

        Nome = nome.Trim();
    }

    public void UpdateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length > 14)
            throw new Exception("O CPF informado é inválido.");

        Cpf = cpf.Trim();
    }

    public void UpdateIdade(int? idade)
    {
        if (idade.HasValue && (idade < 0 || idade > 120))
            throw new Exception("Idade fora do limite permitido.");
            
        Idade = idade;
    }

    public void UpdateStatusMedico(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new Exception("O status médico não pode ser vazio.");
            
        StatusMedico = status.ToUpper();
    }
}