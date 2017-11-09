using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cstieg.ShoppingCart;

namespace StovepipeHeatSaver.Models
{
    public class Product : ProductBase
    {
        public static decimal MinTolerance = (decimal)0.5;
        public static decimal MaxTolerance = (decimal)1.0;

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
                return Circumference - MinTolerance;
            }
        }

        public decimal MaxCircumference
        {
            get
            {
                return Circumference + MaxTolerance;
            }
        }
    }
}