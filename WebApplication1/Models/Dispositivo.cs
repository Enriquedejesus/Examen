using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DecimalPrecisionAttribute : Attribute
{
    public DecimalPrecisionAttribute(byte precision, byte scale)
    {
        Precision = precision;
        Scale = scale;
    }

    public byte Precision { get; set; }
    public byte Scale { get; set; }
}

namespace WebApplication1.Models
{
    public enum EstatusDispositivos
    {
        Apagado = 0,
        Encendido = 1
    }

    public class Dispositivo
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string? Nombre { get; set; }

        public DateTime FechaCreacion{get;set;}=DateTime.Now;
        
        [DecimalPrecision(18, 10)]
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Latitud { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
         [DecimalPrecision(18, 10)]
        public decimal Longitud { get; set; }
        public string? Responsable { get; set; }
        public string? Zona { get; set; }
        public string? Descripcion { get; set; }
        public int Prioridad { get; set; }
        public EstatusDispositivos Estatus { get; set; }
        public decimal LitrosRegistrados { get; set; }
    }
}
