using Freelancing.DTOs.MilestoneDTOs;

namespace Freelancing.IRepositoryService
{
    public interface IMilestoneService
    {
        Task<List<MilestoneGetAllDTO>> GetAllAsync();

        Task<MilestoneGetAllDTO> GetByIdAsync(int id);

        Task<List<MilestoneGetByIdOrProjectIdDTO>> GetByProjectId(int ProjectId);
        Task<bool> RemoveFilebyName(string name);

		Task<MilestoneCreateDTO> CreateAsync(MilestoneCreateDTO milestone);

        /*Task<Milestone> UpdateStatusAsync(MilestoneGetAllDTO milestone);*/ //for status only => httppatch


        Task<MilestoneGetAllDTO> UpdateStatusAsync(int MilestoneId, int StatusId);


        Task<MilestoneGetAllDTO> UpdateAsync(MilestoneCreateDTO milestone);

        Task<bool> DeleteAsync(int id);

        Task<List<string>> UploadFile(List<IFormFile> files, int MilestoneId);

        Task<bool> RemoveFile(int FileId);

        Task<List<MilestoneFile>> GetFilesByMilestoneId(int MilsestoneId);


    }
}
