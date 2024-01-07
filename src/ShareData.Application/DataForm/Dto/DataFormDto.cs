using Abp.Domain.Entities;

using Microsoft.AspNetCore.Http;

using ShareData.Common;
using ShareData.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShareData.DataForm.Dto
{
    public class FilterDataFormPagedInput : DatatableFilterInput
    {

    }
    public class DataFormPagedDto : Entity
    {
        public string EntityName { get; set; }
        public string EntityRepresentativeName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DataSetName { get; set; }
        public string RequiredData { get; set; }
        public string ParticipationPurpose { get; set; }
        public DateTime UseDate { get; set; }
        public short UsePeriod { get; set; }
        public DataSharingPeriodicity DataSharingPeriodicity { get; set; }
    }
    public class InsertDataFormInput
    {
        public string EntityName { get; set; }
        public string EntityRepresentativeName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DataSetName { get; set; }
        public string RequiredData { get; set; }
        public string ParticipationPurpose { get; set; }
        public DateTime UseDate { get; set; }
        public short UsePeriod { get; set; }
        public DataSharingPeriodicity DataSharingPeriodicity { get; set; }
    }
    public class UpdateDataFormInput : Entity
    {
        public string EntityName { get; set; }
        public string EntityRepresentativeName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DataSetName { get; set; }
        public string RequiredData { get; set; }
        public string ParticipationPurpose { get; set; }
        public DateTime UseDate { get; set; }
        public short UsePeriod { get; set; }
        public DataSharingPeriodicity DataSharingPeriodicity { get; set; }
    }
    public class GetFullInfoDataFormDto : Entity
    {
        public string EntityName { get; set; }
        public string EntityRepresentativeName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DataSetName { get; set; }
        public string RequiredData { get; set; }
        public string ParticipationPurpose { get; set; }
        public DateTime UseDate { get; set; }
        public short UsePeriod { get; set; }
        public DataSharingPeriodicity DataSharingPeriodicity { get; set; }
        public bool IsArchived { get; set; } = false;
        public bool HasPermission { get; set; }
    }
    public class GetCurruntFormStageInfoDto
    {
        public bool IsArchived { get; set; }
        public int RoleId { get; set; }
    }
    public class InsertNewFormStageInput
    {
        public int FormId { get; set; }
        public int NextWorkFlowStageId { get; set; }
        public string Note { get; set; }
        public List<IFormFile> Attachments { get; set; }

    }
}
