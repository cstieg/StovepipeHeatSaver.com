using System;
using Cstieg.Sales.Models;

namespace StovepipeHeatSaver.Models
{
    public class Product : ProductBase
    {
        public const decimal MinTolerancePct = 0.028M;
        public const decimal MaxTolerancePct = 0.08M;

        public decimal Diameter { get; set; }

        public decimal Circumference
        {
            get
            {
                return Diameter * (decimal) Math.PI;
            }
        }

        public decimal MinCircumference
        {
            get
            {
                return Circumference * (1.0M - MinTolerancePct);
            }
        }

        public decimal MaxCircumference
        {
            get
            {
                return Circumference * (1.0M + MaxTolerancePct);
            }
        }
    }
}