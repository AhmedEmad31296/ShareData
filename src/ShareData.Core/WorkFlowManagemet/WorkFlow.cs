using Abp.Domain.Entities.Auditing;

using ShareData.Authorization.Roles;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.WorkFlowManagemet
{
    [Table("WorkFlows")]
    public class WorkFlow : FullAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkFlowStage> WorkFlowStages { get; set; }
    }
}
