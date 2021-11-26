using System.Collections.Generic;
using System.Linq;
using Toast.Internal;

namespace Toast.Iap
{
    public class ProductDetailsResult
    {
        public List<IapProduct> Products { get; set; }
        public List<IapProduct> InvalidProducts { get; set; }

        public override string ToString()
        {
            return string.Format("Product : {0}\n" +
                                 "InvalidProducts : {1}",
                ToString(Products), ToString(InvalidProducts));
        }

        private string ToString(List<IapProduct> products)
        {
            if (products == null || products.Count <= 0)
            {
                return "(empty)";
            }

            return products.Select(p => p.ToString())
                       .Aggregate("(", (p1, p2) => p1 + "," + p2) + ")";
        }
    }
}