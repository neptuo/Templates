using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeObjects
{
    /// <summary>
    /// Code object that describes comment is source template.
    /// </summary>
    public class CommentCodeObject : ICodeObject
    {
        /// <summary>
        /// Value of comment.
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Creates new intance with <paramref name="commentText"/> as value of comment.
        /// </summary>
        /// <param name="commentText">Value of comment.</param>
        public CommentCodeObject(string commentText)
        {
            CommentText = commentText;
        }
    }
}
