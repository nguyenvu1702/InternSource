namespace DbEntities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities;
    using Abp.Domain.Entities.Auditing;

    [Table("TreeView")]
    public class TreeView : FullAuditedEntity, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual int? TreeViewParentId { get; set; }

        public virtual string Ma { get; set; }

        public virtual string Ten { get; set; }

        public virtual string GhiChu { get; set; }
    }
}
