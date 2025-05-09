using System.ComponentModel.DataAnnotations;

namespace Freelancing.DTOs
{
	public class CardPaymentDTO
	{
		public int amount { set; get; }


		[CreditCard]
		public string Cardnumber { set; get; }


		public int cvv { set; get; }
	}
}
