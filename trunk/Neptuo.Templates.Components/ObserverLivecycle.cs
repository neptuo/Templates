using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    public enum ObserverLivecycle
    {
        /// <summary>
        /// Observer instance per attribute.
        /// </summary>
        PerAttribute, 
        
        /// <summary>
        /// Observer instance per control.
        /// </summary>
        PerControl, 
        
        /// <summary>
        /// Singleton observer instance per view page.
        /// </summary>
        PerPage
    }
}
