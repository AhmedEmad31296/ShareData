using System.Threading.Tasks;
using Abp.Application.Services;
using ShareData.Sessions.Dto;

namespace ShareData.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
