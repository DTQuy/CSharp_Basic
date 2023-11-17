using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.Object
{
    /// <summary>
    /// Class Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Id Product
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }
    }
}