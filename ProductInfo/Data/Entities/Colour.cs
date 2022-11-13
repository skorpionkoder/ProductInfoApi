using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Data.Entities
{
    public class Colour
    {
        [Key]
        public Guid Id { get; set; }
        public string ColourCode { get; set; }
        public string ColourName { get; set; }
    }
}
