using Freelancing.DTOs.ProposalDTOS;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IproposalService
    {

		Task<List<ProposalViewDTO>> GetAllProposalsAsync();

		Task<ProposalViewDTO> GetProposalByIdAsync(int id);

		Task<List<ProposalViewDTO>> GetProposalsByProjectIdAsync(int projectId);
        Task<List<ProposalViewDTO>> GetProposalsByFreelancerIdAsync(string freelancerId);


        Task<ProposalViewDTO> UpdateProposalAsync(int id, EditProposalDTO proposaldto);

		Task<ProposalViewDTO> CreateProposalAsync(CreateProposalDTO proposaldto, string freelancerId);

		Task<bool> DeleteProposalAsync(int id);


    }
}
