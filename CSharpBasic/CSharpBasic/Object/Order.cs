using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.Object
{
    public class Order
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// customerId
        /// </summary> 
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Order Day
        /// </summary>
        public DateTime OrderDay { get; set; }


        /// <summary>
        /// Total
        /// </summary>
        public decimal TotalAmount { get; set; }
    }

}
