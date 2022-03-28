namespace MyProject.Global
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Auditing;
    using Abp.Authorization;
    using Abp.Domain.Repositories;
    using Abp.UI;
    using DbEntities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MyProject.Authorization;
    using MyProject.Authorization.Users;
    using MyProject.Global.Dtos;
    using MyProject.Users.Dto;

    [AbpAuthorize]
    [DisableAuditing]
    public class LookupTableAppService : MyProjectAppServiceBase, ILookupTableAppService
    {
        private readonly LogInManager logInManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IRepository<Demo> demoRepository;
        private readonly IRepository<TreeView> treeViewRepository;

        public LookupTableAppService(
            LogInManager logInManager,
            IPasswordHasher<User> passwordHasher,
            IRepository<Demo> demoRepository,
            IRepository<TreeView> treeViewRepository)
        {
            this.logInManager = logInManager;
            this.passwordHasher = passwordHasher;
            this.demoRepository = demoRepository;
            this.treeViewRepository = treeViewRepository;
        }

        public async Task<List<LookupTableDto<string>>> GetAllStringLookupTableAsync()
        {
            return await Task.FromResult(new List<LookupTableDto<string>>());
        }

        public async Task<List<LookupTableDto<long>>> GetAllLongLookupTableAsync()
        {
            return await Task.FromResult(new List<LookupTableDto<long>>());
        }

        public async Task<List<LookupTableDto>> GetAllTrangThaiHieuLucAsync()
        {
            List<LookupTableDto> result = new List<LookupTableDto>();
            foreach (var item in GlobalModel.TrangThaiHieuLucSorted)
            {
                result.Add(new LookupTableDto { Id = item.Key, DisplayName = item.Value });
            }

            return await Task.FromResult(result);
        }

        public async Task<List<LookupTableDto>> GetAllTrangThaiDuyetAsync()
        {
            List<LookupTableDto> result = new List<LookupTableDto>();
            foreach (var item in GlobalModel.TrangThaiDuyetSorted)
            {
                result.Add(new LookupTableDto { Id = item.Key, DisplayName = item.Value });
            }

            return await Task.FromResult(result);
        }

        public async Task<List<LookupTableDto>> GetAllDemoAsync()
        {
            var result = this.demoRepository.GetAll().Select(e => new LookupTableDto() { Id = e.Id, DisplayName = e.Ma }).ToList();
            return await Task.FromResult(result);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            #region Check null
            if (input == null)
            {
                throw new UserFriendlyException(StringResources.NullParameter);
            }
            #endregion

            if (this.AbpSession.UserId == null)
            {
                throw new UserFriendlyException(StringResources.UserNotLogin);
            }

            long userId = this.AbpSession.UserId.Value;
            var user = await this.UserManager.GetUserByIdAsync(userId);
            var loginAsync = await this.logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException(StringResources.PasswordIncorect);
            }

            user.Password = this.passwordHasher.HashPassword(user, input.NewPassword);
            this.CurrentUnitOfWork.SaveChanges();
            return true;
        }

        public async Task<List<TreeviewItemDto>> GetAllDropDownTreeViewAsync()
        {
            return await GlobalFunction.GetAllDropDownTreeViewAsync(this.treeViewRepository);
        }

        public async Task<List<FlatTreeSelectDto>> GetAllMultipleCheckboxTreeViewAsync()
        {
            var query = await this.treeViewRepository.GetAll().Select(e => new FlatTreeSelectDto
            {
                Id = e.Id,
                DisplayName = e.Ten,
                ParentId = (int)e.TreeViewParentId,
            }).ToListAsync();
            return query;
        }
    }
}