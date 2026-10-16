using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Common.User;

namespace Test.Core.Entities
{

    public class BaseEntity<T>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        public int? EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [ForeignKey("EntryBy")]
        public virtual User Maker { get; set; }

        [ForeignKey("UpdateBy")]
        public virtual User Modifier { get; set; }

        [ForeignKey("ApprovedBy")]
        public virtual User Checker { get; set; }

    }
}
