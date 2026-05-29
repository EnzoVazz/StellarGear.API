namespace StellarGear.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime DataCriacao { get; protected set; } 
}