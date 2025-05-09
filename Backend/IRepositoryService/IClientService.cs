using Freelancing.DTOs.AuthDTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
	public interface IClientService
	{
		Task<List<ViewClientDTO>> GetAllFiltered(ClientFilterationDTO dto);
		Task<bool> AddClient(Client Client);
		Task<bool> UpdateClient(Client Client);
		Task<bool> DeleteClient(string id);
		Task<ViewClientDTO> GetClientById(string id);

		//Task<IEnumerable<Client>> GetAllClients();
		Task<IEnumerable<ViewClientDTO>> GetAllClients();

		Task<bool> ClientExists(string id);

	}
}
