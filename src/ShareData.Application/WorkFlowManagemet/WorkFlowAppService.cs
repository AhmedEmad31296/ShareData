using Abp.Domain.Repositories;
using Abp.UI;

using Microsoft.EntityFrameworkCore;

using ShareData.Common;
using ShareData.WorkFlowManagemet.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    public class WorkFlowAppService : ShareDataAppServiceBase, IWorkFlowAppService
    {
        private readonly IRepository<WorkFlow> _WorkFlowRepository;
        private readonly IRepository<WorkFlowStage> _WorkFlowStageRepository;
        private readonly IRepository<WorkFlowStageStatus> _WorkFlowStageStatusRepository;
        private readonly IRepository<WorkFlowStageUser> _WorkFlowStageUserRepository;

        public WorkFlowAppService(IRepository<WorkFlow> workFlowRepository
            , IRepository<WorkFlowStage> workFlowStageRepository
            , IRepository<WorkFlowStageStatus> workFlowStageStatusRepository
            , IRepository<WorkFlowStageUser> workFlowStageUserRepository)
        {
            _WorkFlowRepository = workFlowRepository;
            _WorkFlowStageRepository = workFlowStageRepository;
            _WorkFlowStageStatusRepository = workFlowStageStatusRepository;
            _WorkFlowStageUserRepository = workFlowStageUserRepository;
        }
        public async Task<string> Create(CreateWorkFlowInput input)
        {
            bool workFlowIsExisting = await _WorkFlowRepository.GetAll().Where(b => b.Name.Equals(input.Name)).AnyAsync();
            if (workFlowIsExisting)
                throw new UserFriendlyException(L("WorkFlow.IsAlreadyExisting"));

            WorkFlow workFlow = new()
            {
                Name = input.Name,
                Description = input.Description,
            };
            _WorkFlowRepository.Insert(workFlow);
            return L("SavedSuccessfully");
        }
        public async Task<string> Update(UpdateWorkFlowInput input)
        {
            bool workFlowIsExisting = await _WorkFlowRepository.GetAll().Where(b => b.Id != input.Id && b.Name.Equals(input.Name)).AnyAsync();
            if (workFlowIsExisting)
                throw new UserFriendlyException(L("WorkFlow.IsAlreadyExisting"));

            WorkFlow workFlow = await _WorkFlowRepository.GetAll().Where(b => b.Id == input.Id).FirstOrDefaultAsync();
            workFlow.Name = input.Name;
            workFlow.Description = input.Description;
            await _WorkFlowRepository.UpdateAsync(workFlow);
            return L("UpdatedSuccessfully");
        }
        public WorkFlow Get()
        {
            var workFlow = _WorkFlowRepository.GetAll()
                .Include(x => x.WorkFlowStages)
                .FirstOrDefault();
            return workFlow;
        }
        public async Task<string> CreateStage(CreateWorkFlowStageInput input)
        {
            WorkFlowStage workFlowStage = new()
            {
                Name = input.Name,
                Description = input.Description,
                HasMediaFiles = input.HasMediaFiles,
                HasAcceptNextStep = input.HasAcceptNextStep,
                HasRejectNextStep = input.HasRejectNextStep,
                IsArchived = input.IsArchived,
                WorkFlowId = input.WorkFlowId,
                RoleId = input.RoleId,
            };
            int workFlowStageId = await _WorkFlowStageRepository.InsertAndGetIdAsync(workFlowStage);
            if (input.ParentId.HasValue)
            {
                WorkFlowStageStatus workFlowStageStatus = new()
                {
                    LevelStatus = input.LevelStatus.Value,
                    ParentId = input.ParentId.Value,
                    WorkFlowStageId = workFlowStageId
                };
                await _WorkFlowStageStatusRepository.InsertAsync(workFlowStageStatus);
            }
            if (input.UserId.HasValue)
            {
                WorkFlowStageUser user = new()
                {
                    UserId = input.UserId.Value,
                    WorkFlowStageId = workFlowStageId
                };
                await _WorkFlowStageUserRepository.InsertAsync(user);
            }
            return L("SavedSuccessfully");
        }
        public async Task<string> UpdateStage(UpdateWorkFlowStageInput input)
        {
            WorkFlowStage workFlowStage = await _WorkFlowStageRepository.GetAll()
                                                                        .Where(b => b.Id == input.Id)
                                                                        .FirstOrDefaultAsync();
            workFlowStage.Name = input.Name;
            workFlowStage.Description = input.Description;
            workFlowStage.HasAcceptNextStep = input.HasAcceptNextStep;
            workFlowStage.HasRejectNextStep = input.HasRejectNextStep;
            workFlowStage.HasMediaFiles = input.HasMediaFiles;
            workFlowStage.IsArchived = input.IsArchived;
            workFlowStage.RoleId = input.RoleId;

            WorkFlowStageUser workFlowStageUser = await _WorkFlowStageUserRepository.GetAll()
                                                                      .Where(b => b.WorkFlowStageId == workFlowStage.Id)
                                                                      .FirstOrDefaultAsync();
            if (input.UserId.HasValue)
            {
                if (workFlowStageUser != null)
                {
                    if (workFlowStageUser.UserId != input.UserId.Value)
                    {
                        workFlowStageUser.UserId = input.UserId.Value;
                        await _WorkFlowStageUserRepository.UpdateAsync(workFlowStageUser);
                    }
                }
                else
                {
                    await _WorkFlowStageUserRepository.InsertAsync(new WorkFlowStageUser
                    {
                        UserId = input.UserId.Value,
                        WorkFlowStageId = workFlowStage.Id,
                    });
                }
            }
            else
            {
                if (workFlowStageUser != null)
                {
                    await _WorkFlowStageUserRepository.DeleteAsync(workFlowStageUser);
                }
            }

            await _WorkFlowStageRepository.UpdateAsync(workFlowStage);
            return L("UpdatedSuccessfully");
        }
        public async Task<WorkFlowStageInfoDto> GetStage(int workFlowStageId)
        {
            var stage = await _WorkFlowStageRepository.GetAll()
                .Where(w => w.Id == workFlowStageId)
                .Select(s => new WorkFlowStageInfoDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    HasMediaFiles = s.HasMediaFiles,
                    HasAcceptNextStep = s.HasAcceptNextStep,
                    HasRejectNextStep = s.HasRejectNextStep,
                    IsArchived = s.IsArchived,
                    RoleId = s.RoleId,
                    UserIds = s.WorkFlowStageUsers.Where(x => !x.IsDeleted).Select(x => x.UserId).ToList(),
                }).FirstOrDefaultAsync();
            return stage;
        }
        public List<WorkFlowStageShortInfoDto> GetChildren(int parentId)
        {
            var children = _WorkFlowStageStatusRepository.GetAll()
                .Where(x => x.ParentId == parentId)
                .Select(w => new WorkFlowStageShortInfoDto
                {
                    WorkFlowId = w.Id,
                    WorkFlowStageId = w.WorkFlowStageId,
                    WorkFlowStageName = w.WorkFlowStage.Name,
                    LevelStatus = w.LevelStatus,
                    HasAcceptNextStep = w.WorkFlowStage.HasAcceptNextStep,
                    HasRejectNextStep = w.WorkFlowStage.HasRejectNextStep,
                })
                .ToList();
            return children;
        }
        public async Task<int> GetRootId()
        {
            return await _WorkFlowStageRepository.GetAll().Where(x => !x.WorkFlowStageStatus.Any()).Select(st => st.Id).FirstOrDefaultAsync();
        }
        public async Task<NextWorkFlowStageShortInfoDto> GetNextWorkFlowStage(int workFlowStageId, LevelStatus stageType)
        {
            var nextWorkFlowStage = await _WorkFlowStageStatusRepository.GetAll()
                .Include(s => s.WorkFlowStage)
                .Where(x => x.ParentId == workFlowStageId && x.LevelStatus == stageType)
                .FirstOrDefaultAsync();
            NextWorkFlowStageShortInfoDto nextStage = new();
            if (nextWorkFlowStage != null)
            {
                nextStage.NextWorkFlowStageId = nextWorkFlowStage.WorkFlowStageId;
                nextStage.HasMediaFiles = nextWorkFlowStage.WorkFlowStage.HasMediaFiles;
            }
            return nextStage;
        }

    }
}
