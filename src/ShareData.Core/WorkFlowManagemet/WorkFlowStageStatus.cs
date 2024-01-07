using Abp.Domain.Entities.Auditing;

using ShareData.Authorization.Roles;
using ShareData.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    [Table("WorkFlowStageStatus")]
    public class WorkFlowStageStatus : FullAuditedEntity
    {
        public LevelStatus LevelStatus { get; set; }

        [ForeignKey("WorkFlowStage")]
        public int WorkFlowStageId { get; set; }
        public WorkFlowStage WorkFlowStage { get; set; }

        [ForeignKey("WorkFlowStageParent")]
        public int ParentId { get; set; }
        public WorkFlowStage WorkFlowStageParent { get; set; }
    }
}
