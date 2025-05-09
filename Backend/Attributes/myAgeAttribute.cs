using System.ComponentModel.DataAnnotations;

namespace Freelancing.Attributes
{
	public class myAgeAttribute:ValidationAttribute //new
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			return base.IsValid(value, validationContext);
		}
		public override bool IsValid(object? value)
		{


			if (value is not null)
			{
				if (value is DateOnly date )
				{
					DateTime dateTime = date.ToDateTime(TimeOnly.MinValue);
					TimeSpan age = DateTime.Now - dateTime;


					if (age.Days < 100*365 && age.Days>=18*365)
					{
						return true;
					}
					else
					{
						ErrorMessage = "You cant add an age that is less than 18 and more than 100";
						return false;
					}
				}

				else
				{
					ErrorMessage = "You Have to add an Age";
					return false;
				}
			}
			else
			{
				return true;
			}


		}
	}
}
