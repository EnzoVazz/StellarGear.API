using StellarGear.Domain.Common;

namespace StellarGear.Domain.Entities;

public class LeituraSensor : BaseEntity
{
    public int IdTraje { get; private set; }
    public decimal? Temperatura { get; private set; }
    public decimal? Humidade { get; private set; }
    public decimal? Batimentos { get; private set; }
    public DateTime? DataLeitura { get; private set; }

    public Traje? Traje { get; private set; }
    public List<AlertaEmergencia> Alertas { get; private set; }

    public LeituraSensor(int idTraje, decimal? temperatura, decimal? humidade, decimal? batimentos)
    {
        if (idTraje <= 0)
            throw new Exception("ID do Traje inválido.");

        IdTraje = idTraje;
        Temperatura = temperatura;
        Humidade = humidade;
        Batimentos = batimentos;
        DataLeitura = DateTime.Now;
        Alertas = [];
    }

    protected LeituraSensor() 
    { 
        Alertas = [];
    }
    
    public void UpdateDadosSensores(decimal? temperatura, decimal? humidade, decimal? batimentos)
    {
        Temperatura = temperatura;
        Humidade = humidade;
        Batimentos = batimentos;
    }
}