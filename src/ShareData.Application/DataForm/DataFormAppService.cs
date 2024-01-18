using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using ShareData.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Abp.Extensions;
using ShareData.Authorization.Users;
using Abp.Runtime.Session;
using ShareData.DataForm.Dto;
using ShareData.Common;
using ShareData.WorkFlowManagemet;
using ShareData.WorkFlowManagemet.Dto;
using ShareData.Heplers;
using ShareData.Authorization.Roles;

namespace ShareData.DataForm
{
    public class DataFormAppService : ShareDataAppServiceBase, IDataFormAppService
    {
        private readonly IRepository<Form> _FormRepository;
        private readonly IRepository<FormStage> _FormStageRepository;
        private readonly IRepository<FormStageAttachment> _FormStageAttachmentRepository;
        private readonly RoleManager _RoleManager;
        private readonly IWorkFlowAppService _WorkFlowAppService;
        private readonly IHostEnvironment _HostEnvironment;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        readonly string BaseUrl = "";
        public DataFormAppService(IRepository<Form> FormRepository
            , IRepository<FormStage> FormStageRepository
            , IRepository<FormStageAttachment> FormStageAttachmentRepository
            , RoleManager roleManager
            , IWorkFlowAppService workFlowAppService
            , IHostEnvironment hostEnvironment
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _FormRepository = FormRepository;
            _FormStageRepository = FormStageRepository;
            _FormStageAttachmentRepository = FormStageAttachmentRepository;
            _RoleManager = roleManager;
            _WorkFlowAppService = workFlowAppService;
            _HostEnvironment = hostEnvironment;
            _HttpContextAccessor = httpContextAccessor;
            BaseUrl = $"{this._HttpContextAccessor.HttpContext.Request.Scheme}://{this._HttpContextAccessor.HttpContext.Request.Host}{this._HttpContextAccessor.HttpContext.Request.PathBase}";
        }
        [AbpAuthorize]
        public async Task<DatatableFilterdDto<DataFormPagedDto>> GetPaged(FilterDataFormPagedInput input)
        {
            IQueryable<Form> query = _FormRepository.GetAll().Where(e => !e.IsDeleted)
                ;

            int totalCount = await query.CountAsync();

            query = query
                .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), b => b.EntityName.ToLower().Contains(input.SearchTerm))
                ;


            int recordsFiltered = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrEmpty(input.SortColumn) && !string.IsNullOrEmpty(input.SortDirection))
            {
                query = query.OrderBy(string.Concat(input.SortColumn, " ", input.SortDirection));
            }

            // Pagination
            List<DataFormPagedDto> forms = await query
                .Select(b => new DataFormPagedDto
                {
                    Id = b.Id,
                    EntityName = b.EntityName,
                    DataSetName = b.DataSetName,
                    DataSharingPeriodicity = b.DataSharingPeriodicity,
                    EmailAddress = b.EmailAddress,
                    EntityRepresentativeName = b.EntityRepresentativeName,
                    ParticipationPurpose = b.ParticipationPurpose,
                    PhoneNumber = b.PhoneNumber,
                    RequiredData = b.RequiredData,
                    UseDate = b.UseDate,
                    UsePeriod = b.UsePeriod
                })
                //.Skip((input.Page - 1) * input.PageSize)
                //.Take(input.PageSize)
                .Page(input.Page, input.PageSize)
                .ToListAsync();

            return new DatatableFilterdDto<DataFormPagedDto>
            {
                RecordsFiltered = recordsFiltered,
                RecordsTotal = totalCount,
                AaData = forms,
                Draw = input.Draw
            };

        }
        public async Task<string> Insert(InsertDataFormInput input)
        {
            Form form = new()
            {
                EntityName = input.EntityName,
                DataSetName = input.DataSetName,
                DataSharingPeriodicity = input.DataSharingPeriodicity,
                EmailAddress = input.EmailAddress,
                PhoneNumber = input.PhoneNumber,
                RequiredData = input.RequiredData,
                UseDate = input.UseDate,
                UsePeriod = input.UsePeriod,
                EntityRepresentativeName = input.EntityRepresentativeName,
                ParticipationPurpose = input.ParticipationPurpose,
            };

            int formId = await _FormRepository.InsertAndGetIdAsync(form);
            int workFlowStageId = await _WorkFlowAppService.GetRootId();
            FormStage stage = new()
            {
                FormId = formId,
                WorkFlowStageId = workFlowStageId,
            };
            await _FormStageRepository.InsertAsync(stage);
            return L("SavedSuccessfully");

        }
        [AbpAuthorize]
        public async Task<string> Update(UpdateDataFormInput input)
        {
            bool dailyTaskIsExisting = await _FormRepository.GetAll().Where(b => b.Id != input.Id && b.EntityName.Equals(input.EntityName)).AnyAsync();
            if (dailyTaskIsExisting)
                throw new UserFriendlyException(L("Form.DataForm.IsAlreadyExisting"));

            Form form = await _FormRepository.GetAll().Where(b => b.Id == input.Id).FirstOrDefaultAsync();

            form.EntityName = input.EntityName;
            form.PhoneNumber = input.PhoneNumber;
            form.EmailAddress = input.EmailAddress;
            form.DataSetName = input.DataSetName;
            form.UsePeriod = input.UsePeriod;
            form.UseDate = input.UseDate;
            form.EntityRepresentativeName = input.EntityRepresentativeName;
            form.DataSharingPeriodicity = input.DataSharingPeriodicity;
            form.ParticipationPurpose = input.ParticipationPurpose;
            form.RequiredData = input.RequiredData;

            await _FormRepository.UpdateAsync(form);

            return L("UpdatedSuccessfully");
        }
        [AbpAuthorize]
        public GetFullInfoDataFormDto Get(int id)
        {
            Form form = _FormRepository.GetAll()
               .Where(x => x.Id == id)
               .FirstOrDefault() ?? throw new UserFriendlyException(L("Form.DataForm.IsNotExisting"));

            GetFullInfoDataFormDto entity = _FormRepository.GetAll()
                                                                .Where(e => e.Id == id)
                                                                .Select(e => new GetFullInfoDataFormDto
                                                                {
                                                                    Id = e.Id,
                                                                    EntityName = e.EntityName,
                                                                    DataSetName = e.DataSetName,
                                                                    EntityRepresentativeName = e.EntityRepresentativeName,
                                                                    EmailAddress = e.EmailAddress,
                                                                    PhoneNumber = e.PhoneNumber,
                                                                    UseDate = e.UseDate,
                                                                    UsePeriod = e.UsePeriod,
                                                                    DataSharingPeriodicity = e.DataSharingPeriodicity,
                                                                    RequiredData = e.RequiredData,
                                                                    ParticipationPurpose = e.ParticipationPurpose,
                                                                }).FirstOrDefault();
            if (entity != null)
            {
                var formStage = _FormStageRepository.GetAll()
                      .Where(s => s.FormId == id)
                      .Select(st => new GetCurruntFormStageInfoDto
                      {
                          IsArchived = st.WorkFlowStage.IsArchived,
                          RoleId = st.WorkFlowStage.RoleId,
                      }).FirstOrDefault();
                entity.IsArchived = formStage.IsArchived;
                entity.HasPermission = IsCurrentUserHasPermission(formStage.RoleId).GetAwaiter().GetResult();
            }
            return entity;
        }
        [AbpAuthorize]
        public async Task<string> Delete(int id)
        {
            Form form = await _FormRepository.GetAll()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? throw new UserFriendlyException(L("Form.DataForm.IsNotExisting"));

            await _FormRepository.HardDeleteAsync(form);

            return L("DeletedSuccessfully");
        }
        [AbpAuthorize]
        public async Task<string> InsertNewFormStage([FromForm] InsertNewFormStageInput input)
        {
            FormStage formStage = new()
            {
                FormId = input.FormId,
                WorkFlowStageId = input.NextWorkFlowStageId,
                Note = input.Note,
            };
            int formStageId = _FormStageRepository.InsertAndGetId(formStage);

            if (input.Attachments != null)
            {
                List<FormStageAttachment> formStageAttachments = new();
                var rootPath = _HostEnvironment.ContentRootPath;
                foreach (IFormFile attachment in input.Attachments)
                {
                    var uploadedMediaFile = await MediaFileService.UploadMediaFileAsync(attachment, rootPath + ShareDataConsts.FormStageAttachmentPath.UploadPath);
                    if (uploadedMediaFile != null)
                    {
                        FormStageAttachment formStageAttachment = new()
                        {
                            MediaFileName = uploadedMediaFile.FileName,
                            OriginalMediaFileName = uploadedMediaFile.OriginalFileName,
                            FormStageId = formStageId
                        };
                        formStageAttachments.Add(formStageAttachment);
                        _FormStageAttachmentRepository.Insert(formStageAttachment);
                    }
                }
            }
            return L("Form.DataForm.MovedToNextStageSuccessfully");

        }
        [AbpAuthorize]
        public async Task<NextWorkFlowStageShortInfoDto> GetNextWorkFlowStageDetailsForDataForm(int formId, LevelStatus stageType)
        {
            var currentStage = await _FormStageRepository.GetAll().Where(s => s.FormId == formId).OrderByDescending(s => s.CreationTime).FirstOrDefaultAsync();
            var nextWorkFlowStage = await _WorkFlowAppService.GetNextWorkFlowStage(currentStage.WorkFlowStageId, stageType);
            return nextWorkFlowStage;
        }
        async Task<bool> IsCurrentUserHasPermission(int roleId)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user != null)
            {
                IList<string> currentUserRoles = await UserManager.GetRolesAsync(user);
                Role role = await _RoleManager.GetRoleByIdAsync(roleId);
                if (role != null)
                {
                    return currentUserRoles.Any(r => r.Equals(role.Name));
                }
            }
            return false;
        }
    }
}
