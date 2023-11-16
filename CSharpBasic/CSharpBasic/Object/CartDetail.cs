using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.Object
{
    // CartDetail
    public class CartDetail
    {
        /// <summary>
        /// Cart id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// product id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }
    }

}
