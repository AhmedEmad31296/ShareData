using Abp.Domain.Entities.Auditing;

using ShareData.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareData.DataForm
{
    [Table("Forms")]
    public class Form : FullAuditedEntity
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
}
