using Neptuo.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebUI.Models
{
    public class File : IBaseObject<int>
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        [ForeignKey("Folder")]
        public int FolderID { get; set; }
        public virtual Folder Folder { get; set; }
    }
}