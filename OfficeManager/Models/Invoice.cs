namespace OfficeManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice
    {
        public Invoice()
        {
            this.InvoiceDescriptions = new HashSet<InvoiceDescription>();
        }
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime IssuedOn { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; }
        public virtual Tenant Customer { get; set; }

        public int LandlordId { get; set; }
        public virtual Landlord Landlord { get; set; }

        public virtual ICollection<InvoiceDescription> InvoiceDescriptions { get; set; }
    }
}
