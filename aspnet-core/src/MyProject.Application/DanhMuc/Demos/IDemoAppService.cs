using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MyProject.DanhMuc.Demos.Dtos;
using MyProject.Data;

namespace MyProject.DanhMuc.Demos
{
    public interface IDemoAppService
    {
        Task<PagedResultDto<DemoForView>> GetAllAsync(DemoGetAllInputDto input);

        Task<int> CreateOrEdit(DemoCreateInput input);

        Task<DemoCreateInput> GetForEditAsync(EntityDto input);

        Task Delete(EntityDto input);

        Task<FileDto> ExportToExcel(DemoGetAllInputDto input);

        Task<string> ImportFileExcel(string filePath);

        Task<FileDto> DownloadFileMau();
    }
}
