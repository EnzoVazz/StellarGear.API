using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class AlertaEmergencia : BaseEntity
{
    public int IdLeitura { get; private set; }
    public int IdMedico { get; private set; }
    public string? Descricao { get; private set; }
    public string? NivelGravidade { get; private set; }
    public char Resolvido { get; private set; }
    public DateTime? DataAlerta { get; private set; }

    public LeituraSensor? Leitura { get; private set; }
    public Medico? Medico { get; private set; }

    public AlertaEmergencia(int idLeitura, int idMedico, string? descricao, string? nivelGravidade)
    {
        if (idLeitura <= 0 || idMedico <= 0)
            throw new Exception("IDs de Leitura ou Médico inválidos.");

        IdLeitura = idLeitura;
        IdMedico = idMedico;
        Descricao = descricao;
        NivelGravidade = nivelGravidade?.ToUpper();
        Resolvido = 'N';
        DataAlerta = DateTime.Now;
    }

    protected AlertaEmergencia() { }

    public void ResolverAlerta()
    {
        Resolvido = 'S';
    }
    
    public void UpdateDetalhes(string? descricao, string? nivelGravidade)
    {
        Descricao = descricao;
        NivelGravidade = nivelGravidade?.ToUpper();
    }
}