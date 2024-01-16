using Abp.Domain.Entities.Auditing;
using ShareData.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
