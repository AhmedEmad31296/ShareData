using Abp.Domain.Entities;

using ShareData.Authorization.Roles;
using ShareData.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet.Dto
{
    public class CreateWorkFlowInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class UpdateWorkFlowInput : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CreateWorkFlowStageInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasMediaFiles { get; set; }
        public bool HasAcceptNextStep { get; set; }
        public bool HasRejectNextStep { get; set; }
        public bool IsArchived { get; set; }
        public int RoleId { get; set; }
        public long? UserId { get; set; }
        public int WorkFlowId { get; set; }
        public LevelStatus? LevelStatus { get; set; }
        public int? ParentId { get; set; }
    }
    public class UpdateWorkFlowStageInput : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasAcceptNextStep { get; set; }
        public bool HasRejectNextStep { get; set; }
        public bool HasMediaFiles { get; set; }
        public bool IsArchived { get; set; }
        public int RoleId { get; set; }
        public long? UserId { get; set; }

    }
    public class WorkFlowStageShortInfoDto
    {
        public int WorkFlowId { get; set; }
        public int WorkFlowStageId { get; set; }
        public string WorkFlowStageName { get; set; }
        public bool HasAcceptNextStep { get; set; }
        public bool HasRejectNextStep { get; set; }
        public LevelStatus LevelStatus { get; set; }

    }
    public class WorkFlowStageInfoDto : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasMediaFiles { get; set; }
        public bool HasAcceptNextStep { get; set; }
        public bool HasRejectNextStep { get; set; }
        public bool IsArchived { get; set; }
        public int RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
    public class NextWorkFlowStageShortInfoDto
    {
        public int NextWorkFlowStageId { get; set; }
        public bool HasMediaFiles { get; set; }
    }
}
