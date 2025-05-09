namespace Freelancing.Helpers
{
	public static class RoleSeeder
	{
		public static readonly string client= "Client";
		public static readonly string freelancer = "Freelancer";
		public static readonly string admin= "Admin";

		public static string[] roles = { admin, client, freelancer };
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{

			foreach (var role in roles)
			{

				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}


			var admin = await userManager.FindByNameAsync("admin");
			if (admin != null)
			{
				// Assign the Admin role to the user
				await userManager.AddToRoleAsync(admin, "Admin");
			}
		}
	}
}
