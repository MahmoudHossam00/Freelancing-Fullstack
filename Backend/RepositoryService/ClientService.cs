
using AutoMapper;
using Freelancing.DTOs.AuthDTOs;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
	public class ClientService(ApplicationDbContext _context,IMapper _mapper) : IClientService
	{


		public async Task<List<ViewClientDTO>> GetAllFiltered(ClientFilterationDTO dto)
		{
			var Clients = _context.clients.Include(c=>c.City).ThenInclude(C=>C.Country).Include(f => f.Reviewed).Include(f => f.subscriptionPlan).Where(f => !f.isDeleted);
			if (dto.CountryIDs is { Count: >0})
			{
				Clients = Clients
					.Where(f => dto.CountryIDs.Contains(f.City.CountryId));
			}
			if (dto.AccountCreationDate != default)
			{
				Clients = Clients.Where(f => f.AccountCreationDate <= dto.AccountCreationDate);
			}
			if (!string.IsNullOrEmpty(dto.name))
			{
				Clients = Clients.Where(f =>
				f.firstname.ToLower().Contains(dto.name.ToLower()) ||
				f.lastname.ToLower().Contains(dto.name.ToLower()) ||
				f.UserName.ToLower().Contains(dto.name.ToLower())
				);
			}
			if (dto.IsVerified == true)
			{
				Clients = Clients.Where(f => f.IsVerified); // id verification
			}
			if (dto.Paymentverified==true)
			{
				Clients = Clients.Where(f => f.PaymentVerified); 
			}

			var Clientsslist = await Clients.ToListAsync();

			if (dto.ranks != null && dto.ranks.Count > 0)
			{
				Clientsslist = Clientsslist.AsEnumerable().Where(f => dto.ranks.Contains(f.Rank)).ToList();
			}
			dto.numofpages = dto.pagesize > 0
								? (int)Math.Ceiling((double)Clientsslist.Count() / dto.pagesize)
								: 0;
			List<ViewClientDTO> clientsdtos = _mapper.Map<List<ViewClientDTO>>((Clientsslist.Skip((dto.pageNum-1) * dto.pagesize).Take(dto.pagesize)));

			return clientsdtos;
		}
		public async Task<bool> AddClient(Client Client)
		{


			await _context.clients.AddAsync(Client);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> ClientExists(string id)
		{
			return await _context.clients.AnyAsync(c => c.Id == id);
		}

		public async Task<bool> DeleteClient(string id)
		{
			var client= await _context.clients.FirstOrDefaultAsync(c => c.Id == id);
			if (client == null)
			{
				return false;
			}
			client.isDeleted = true;
			_context.clients.Update(client);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<ViewClientDTO>> GetAllClients()
		{
			
			var clients = await _context.clients.Include(c => c.City).ThenInclude(c => c.Country).Where(f => !f.isDeleted).ToListAsync();
			List<ViewClientDTO> clientsdtos = new();
			foreach (var client in clients)
			{
				clientsdtos.Add(_mapper.Map<ViewClientDTO>(client));
			}
			return clientsdtos;

		}

		public async Task<ViewClientDTO> GetClientById(string id)
		{
			var CLIENT = await _context.clients.Include(c => c.City).ThenInclude(c => c.Country).Where(f => !f.isDeleted).FirstOrDefaultAsync(F => F.Id == id);

			return _mapper.Map<ViewClientDTO>(CLIENT);
		}

		public async Task<bool> UpdateClient(Client Client)
		{
			var client = _mapper.Map<Client>(await GetClientById(Client.Id));
			if (client == null)
			{
				return false;
			}
			else
			{
				_context.clients.Update(client);
				return await _context.SaveChangesAsync() > 0;
			}
			
		}
	}
}
