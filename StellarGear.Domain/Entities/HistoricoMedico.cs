using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class HistoricoMedico : BaseEntity
{
    public int IdPassageiro { get; private set; }
    public string? Diagnostico { get; private set; }
    public DateTime? DataRegistro { get; private set; }

    public Passageiro? Passageiro { get; private set; }

    public HistoricoMedico(int idPassageiro, string? diagnostico)
    {
        if (idPassageiro <= 0)
            throw new Exception("ID do Passageiro inválido.");

        IdPassageiro = idPassageiro;
        Diagnostico = diagnostico;
        DataRegistro = DateTime.Now;
    }
    
    
    protected HistoricoMedico() { }
    
    public void UpdateDiagnostico(string? diagnostico)
    {
        Diagnostico = diagnostico;
    }
}