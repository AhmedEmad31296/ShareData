using Abp.Domain.Entities.Auditing;

using ShareData.Authorization.Roles;
using ShareData.Common;
using ShareData.WorkFlowManagemet;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    [Table("WorkFlowStages")]
    public class WorkFlowStage : FullAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasMediaFiles { get; set; }
        public bool HasAcceptNextStep { get; set; }
        public bool HasRejectNextStep { get; set; }
        public bool IsArchived { get; set; }


        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }


        [ForeignKey("WorkFlow")]
        public int WorkFlowId { get; set; }
        public WorkFlow WorkFlow { get; set; }
        public ICollection<WorkFlowStageStatus> WorkFlowStageStatus { get; set; }
    }
}
