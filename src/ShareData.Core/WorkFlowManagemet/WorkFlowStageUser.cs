using Abp.Domain.Entities.Auditing;

using ShareData.Authorization.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    [Table("WorkFlowStageUsers")]
    public class WorkFlowStageUser : FullAuditedEntity
    {
        [ForeignKey("WorkFlowStage")]
        public int WorkFlowStageId { get; set; }
        public WorkFlowStage WorkFlowStage { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
