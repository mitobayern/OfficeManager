namespace OfficeManager.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TemperatureMeter
    {
        public TemperatureMeter()
        {
            this.TemperatureMeasurements = new HashSet<TemperatureMeasurement>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }


        public int? OfficeId { get; set; }

        public virtual Office Office { get; set; }

        public virtual ICollection<TemperatureMeasurement> TemperatureMeasurements { get; set; }
    }
}
