using Neptuo.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebUI.Models
{
    public class Folder : IBaseObject<int>
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey("Parent")]
        public int? Parent_FolderID { get; set; }
        public virtual Folder Parent { get; set; }
    }
}