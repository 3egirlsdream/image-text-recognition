using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate
{
    public class Dict
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Webdict
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string tSpeakUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> translation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dict dict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Webdict webdict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string speakUrl { get; set; }
    }
}
