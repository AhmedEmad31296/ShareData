using Abp.Application.Services;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

using ShareData.DataForm.Dto;
using ShareData.Helpers;
using ShareData.Common;
using ShareData.WorkFlowManagemet.Dto;

namespace ShareData.DataForm
{
    public interface IDataFormAppService : IApplicationService
    {
        Task<DatatableFilterdDto<DataFormPagedDto>> GetPaged(FilterDataFormPagedInput input);
        Task<string> Insert(InsertDataFormInput input);
        Task<string> Update(UpdateDataFormInput input);
        GetFullInfoDataFormDto Get(int id);
        Task<string> Delete(int id);
        Task<string> InsertNewFormStage([FromForm] InsertNewFormStageInput input);
        Task<NextWorkFlowStageShortInfoDto> GetNextWorkFlowStageDetailsForDataForm(int formId, LevelStatus stageType);
    }
}
