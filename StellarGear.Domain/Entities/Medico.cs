using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class Medico : BaseEntity
{
    public string Nome { get; private set; }
    public string Crm { get; private set; }
    public string? Especialidade { get; private set; }
    public List<AlertaEmergencia> Alertas { get; private set; }

    public Medico(string nome, string crm, string? especialidade)
    {
        UpdateNome(nome);
        UpdateCrm(crm);
        Especialidade = especialidade;
        Alertas = [];
    }

    protected Medico() 
    { 
        Alertas = [];
    }

    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome não pode ser vazio.");
        Nome = nome.Trim();
    }

    public void UpdateCrm(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
            throw new Exception("O CRM não pode ser vazio.");
        Crm = crm.Trim();
    }
    
    public void UpdateEspecialidade(string? especialidade)
    {
        Especialidade = especialidade;
    }
}