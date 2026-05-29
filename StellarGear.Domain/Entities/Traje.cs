using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class Traje : BaseEntity
{
    public int IdPassageiro { get; private set; }
    public string? CodigoRfid { get; private set; }
    public DateTime? DataAlocacao { get; private set; }
    
    public Passageiro? Passageiro { get; private set; }
    public List<LeituraSensor> Leituras { get; private set; }

    public Traje(int idPassageiro, string? codigoRfid)
    {
        if (idPassageiro <= 0)
            throw new Exception("ID do Passageiro inválido.");

        IdPassageiro = idPassageiro;
        CodigoRfid = codigoRfid;
        DataAlocacao = DateTime.Now;
        Leituras = [];
    }

    protected Traje() 
    { 
        Leituras = [];
    }
    
    public void UpdateCodigoRfid(string? codigoRfid)
    {
        CodigoRfid = codigoRfid;
    }
}