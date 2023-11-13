using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.Object
{
    /// <summary>
    /// Class Customer
    /// </summary>
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        /// <summary>
        /// Id Customer
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FirstName Customer
        /// </summary>
        public string? FirstName { get; set; } = string.Empty;

        /// <summary>
        /// LastName Customer
        /// </summary>
        public string? LastName { get; set; } = string.Empty;

        /// <summary>
        /// GetFullName
        /// </summary>
        public string GetFullName()
        {
            return $"{LastName} {FirstName}";
        }


        /// <summary>
        /// Email User
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// PhoneNumber User
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string? Address { get; set; }
    }
}
