namespace MyProject.Global
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyProject.Global.Dtos;
    using MyProject.Users.Dto;

    public interface ILookupTableAppService
    {
        Task<List<LookupTableDto<string>>> GetAllStringLookupTableAsync();

        Task<List<LookupTableDto<long>>> GetAllLongLookupTableAsync();

        Task<List<LookupTableDto>> GetAllTrangThaiHieuLucAsync();

        Task<List<LookupTableDto>> GetAllTrangThaiDuyetAsync();

        Task<List<LookupTableDto>> GetAllDemoAsync();

        Task<bool> ChangePassword(ChangePasswordDto input);

        Task<List<TreeviewItemDto>> GetAllDropDownTreeViewAsync();

        Task<List<FlatTreeSelectDto>> GetAllMultipleCheckboxTreeViewAsync();
    }
}
