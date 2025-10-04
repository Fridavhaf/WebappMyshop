using System.ComponentModel.DataAnnotations;

namespace Myshop.Models
// Definerer et "namespace" (navnerom). Her legger du Item-klassen inn under 
// Myshop.Models slik at den er organisert og lett å bruke i resten av prosjektet.
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        // "string.Empty" betyr at verdien starter som en tom streng i stedet for null.
        public decimal Price { get; set; }
        public string? Description { get; set; }
        // "string?" betyr at den kan være null (valgfri informasjon)
        public string? ImageUrl { get; set; }
        // navigation property
        public virtual List<OrderItem>? OrderItems { get; set; }

    }
}
