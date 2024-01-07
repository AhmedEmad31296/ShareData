using Abp.Application.Services;
using ShareData.MultiTenancy.Dto;

namespace ShareData.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

