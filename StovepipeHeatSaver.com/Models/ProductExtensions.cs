using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cstieg.Sales.Interfaces;
using Cstieg.Sales.Models;

namespace StovepipeHeatSaver.Models
{
    /// <summary>
    /// Model of product to be sold
    /// </summary>
    public class ProductExtension : ISalesEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public const decimal MinTolerancePct = 0.028M;
        public const decimal MaxTolerancePct = 0.08M;

        public decimal Diameter { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal Circumference
        {
            get
            {
                return Diameter * (decimal) Math.PI;
            }
        }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal MinCircumference
        {
            get
            {
                return Circumference * (1.0M - MinTolerancePct);
            }
        }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal MaxCircumference
        {
            get
            {
                return Circumference * (1.0M + MaxTolerancePct);
            }
        }
    }
}