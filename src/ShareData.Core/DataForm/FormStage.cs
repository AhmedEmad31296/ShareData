using Abp.Domain.Entities.Auditing;

using ShareData.Authorization.Roles;
using ShareData.WorkFlowManagemet;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.DataForm
{
    [Table("FormStages")]
    public class FormStage : FullAuditedEntity
    {
        [ForeignKey("Form")]
        public int FormId { get; set; }
        public Form Form { get; set; }
        public string Note { get; set; }

        [ForeignKey("WorkFlowStage")]
        public int WorkFlowStageId { get; set; }
        public WorkFlowStage WorkFlowStage { get; set; }
    }
}
