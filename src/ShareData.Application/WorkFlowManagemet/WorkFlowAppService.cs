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

        public WorkFlowAppService(IRepository<WorkFlow> workFlowRepository
            , IRepository<WorkFlowStage> workFlowStageRepository
            , IRepository<WorkFlowStageStatus> workFlowStageStatusRepository)
        {
            _WorkFlowRepository = workFlowRepository;
            _WorkFlowStageRepository = workFlowStageRepository;
            _WorkFlowStageStatusRepository = workFlowStageStatusRepository;
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
            return L("SavedSuccessfully");
        }
        public async Task<string> UpdateStage(UpdateWorkFlowStageInput input)
        {
            WorkFlowStage workFlowStage = await _WorkFlowStageRepository.GetAll().Where(b => b.Id == input.Id).FirstOrDefaultAsync();
            workFlowStage.Name = input.Name;
            workFlowStage.Description = input.Description;
            workFlowStage.HasAcceptNextStep = input.HasAcceptNextStep;
            workFlowStage.HasRejectNextStep = input.HasRejectNextStep;
            workFlowStage.HasMediaFiles = input.HasMediaFiles;
            workFlowStage.IsArchived = input.IsArchived;
            workFlowStage.RoleId = input.RoleId;
            await _WorkFlowStageRepository.UpdateAsync(workFlowStage);
            return L("UpdatedSuccessfully");
        }
        public async Task<WorkFlowStage> GetStage(int workFlowStageId)
        {
            var stage = await _WorkFlowStageRepository.GetAll()
                .Where(w => w.Id == workFlowStageId)
                .FirstOrDefaultAsync();
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
