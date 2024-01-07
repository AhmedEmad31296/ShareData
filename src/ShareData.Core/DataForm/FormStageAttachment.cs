using Abp.Domain.Entities.Auditing;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.DataForm
{
    [Table("FormStageAttachments")]
    public class FormStageAttachment : FullAuditedEntity
    {
        public string MediaFileName { get; set; }
        public string OriginalMediaFileName { get; set; }

        [ForeignKey("FormStage")]
        public int FormStageId { get; set; }
        public FormStage FormStage { get; set; }

    }
}
