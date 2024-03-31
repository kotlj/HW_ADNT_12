using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_ADNT_12
{
    internal class Product
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Caloricity { get; set; }
        public int ID { get; set; }
        Product(string name, string type, string color, int caloricity, int iD)
        {
            Name = name;
            Type = type;
            Color = color;
            Caloricity = caloricity;
            ID = iD;
        }
    }
}
