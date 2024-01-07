using Abp.Application.Services;

using ShareData.Common;
using ShareData.WorkFlowManagemet.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    public interface IWorkFlowAppService : IApplicationService
    {
        Task<string> Create(CreateWorkFlowInput input);
        Task<string> Update(UpdateWorkFlowInput input);
        Task<WorkFlow> Get();
        Task<string> CreateStage(CreateWorkFlowStageInput input);
        Task<string> UpdateStage(UpdateWorkFlowStageInput input);
        Task<WorkFlowStage> GetStage(int workFlowStageId);
        List<WorkFlowStageShortInfoDto> GetChildren(int parentId);
        Task<int> GetRootId();
        Task<NextWorkFlowStageShortInfoDto> GetNextWorkFlowStage(int workFlowStageId, LevelStatus stageType);
    }
}
