using Microsoft.EntityFrameworkCore;
using StellarGear.Domain.Entities;

namespace StellarGear.Infrastructure.Persistence;

public class StellarGearContext : DbContext
{
    public StellarGearContext(DbContextOptions<StellarGearContext> options) : base(options) { }

    public DbSet<Passageiro> Passageiros { get; set; }
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Traje> Trajes { get; set; }
    public DbSet<LeituraSensor> LeiturasSensores { get; set; }
    public DbSet<AlertaEmergencia> AlertasEmergencia { get; set; }
    public DbSet<HistoricoMedico> HistoricosMedicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================
        // MAPEAMENTO: PASSAGEIRO
        // =========================================
        modelBuilder.Entity<Passageiro>(entity =>
        {
            entity.ToTable("passageiro");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id_passageiro").ValueGeneratedOnAdd();
            entity.Property(p => p.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
            entity.Property(p => p.Cpf).HasColumnName("cpf").IsRequired().HasMaxLength(14);
            entity.Property(p => p.Idade).HasColumnName("idade");
            entity.Property(p => p.StatusMedico).HasColumnName("status_medico").HasMaxLength(50);
        });

        // =========================================
        // MAPEAMENTO: MEDICO
        // =========================================
        modelBuilder.Entity<Medico>(entity =>
        {
            entity.ToTable("medico");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("id_medico").ValueGeneratedOnAdd();
            entity.Property(m => m.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
            entity.Property(m => m.Crm).HasColumnName("crm").IsRequired().HasMaxLength(20);
            entity.Property(m => m.Especialidade).HasColumnName("especialidade").HasMaxLength(50);
        });

        // =========================================
        // MAPEAMENTO: TRAJE
        // =========================================
        modelBuilder.Entity<Traje>(entity =>
        {
            entity.ToTable("traje");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("id_traje").ValueGeneratedOnAdd();
            entity.Property(t => t.IdPassageiro).HasColumnName("id_passageiro").IsRequired();
            entity.Property(t => t.CodigoRfid).HasColumnName("codigo_rfid").HasMaxLength(50);
            entity.Property(t => t.DataAlocacao).HasColumnName("dt_alocacao");

            // Relacionamento 1:N
            entity.HasOne(t => t.Passageiro)
                  .WithMany(p => p.Trajes)
                  .HasForeignKey(t => t.IdPassageiro)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================================
        // MAPEAMENTO: LEITURA SENSOR
        // =========================================
        modelBuilder.Entity<LeituraSensor>(entity =>
        {
            entity.ToTable("leitura_sensor");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id).HasColumnName("id_leitura").ValueGeneratedOnAdd();
            entity.Property(l => l.IdTraje).HasColumnName("id_traje").IsRequired();
            entity.Property(l => l.Temperatura).HasColumnName("temperatura").HasColumnType("decimal(5,2)");
            entity.Property(l => l.Humidade).HasColumnName("humidade").HasColumnType("decimal(5,2)");
            entity.Property(l => l.Batimentos).HasColumnName("batimentos").HasColumnType("decimal(5,2)");
            entity.Property(l => l.DataLeitura).HasColumnName("dt_leitura");

            // Relacionamento 1:N
            entity.HasOne(l => l.Traje)
                  .WithMany(t => t.Leituras)
                  .HasForeignKey(l => l.IdTraje)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================================
        // MAPEAMENTO: ALERTA EMERGENCIA
        // =========================================
        modelBuilder.Entity<AlertaEmergencia>(entity =>
        {
            entity.ToTable("alerta_emergencia");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("id_alerta").ValueGeneratedOnAdd();
            entity.Property(a => a.IdLeitura).HasColumnName("id_leitura").IsRequired();
            entity.Property(a => a.IdMedico).HasColumnName("id_medico").IsRequired();
            entity.Property(a => a.Descricao).HasColumnName("descricao").HasMaxLength(200);
            entity.Property(a => a.NivelGravidade).HasColumnName("nivel_gravidade").HasMaxLength(20);
            entity.Property(a => a.Resolvido).HasColumnName("resolvido");
            entity.Property(a => a.DataAlerta).HasColumnName("dt_alerta");

            // Relacionamentos 1:N
            entity.HasOne(a => a.Leitura)
                  .WithMany(l => l.Alertas)
                  .HasForeignKey(a => a.IdLeitura)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.Medico)
                  .WithMany(m => m.Alertas)
                  .HasForeignKey(a => a.IdMedico)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================
        // MAPEAMENTO: HISTORICO MEDICO
        // =========================================
        modelBuilder.Entity<HistoricoMedico>(entity =>
        {
            entity.ToTable("historico_medico");
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Id).HasColumnName("id_historico").ValueGeneratedOnAdd();
            entity.Property(h => h.IdPassageiro).HasColumnName("id_passageiro").IsRequired();
            entity.Property(h => h.Diagnostico).HasColumnName("diagnostico").HasMaxLength(500);
            entity.Property(h => h.DataRegistro).HasColumnName("dt_registro");

            // Relacionamento 1:N
            entity.HasOne(h => h.Passageiro)
                  .WithMany(p => p.Historicos)
                  .HasForeignKey(h => h.IdPassageiro)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}