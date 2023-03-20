using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBalls.DataBase
{
    public partial class Agent
    {
        public int SellsCount { get => ProductSales.Sum(x => x.ProductCount); }
        public int Discount
        {
            get
            {
                var sum = ProductSales.Sum(x => x.ProductCount * x.Product.MinCostForAgent);
                return sum < 10000 ? 0 : sum < 50000 ? 5 : sum < 150000 ? 10 : sum < 50000 ? 20 : 25;
            }
        }
        public string Background => Discount >= 25 ? "LightGreen" : "#FFE9F9";
    }
}
