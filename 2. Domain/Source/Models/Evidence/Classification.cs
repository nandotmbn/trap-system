using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Classification : Mandatory
	{
		[Key]
		[Required]
		public Guid Id { get; set; } = Guid.NewGuid();
    public string Prediction { get; set; } = string.Empty;
    public double Confidence { get; set; } = 0.0;

		[Required]
		public Guid DetectionId { get; set; }
		[Required]
		public Detection? Detection { get; set; }
	}
}