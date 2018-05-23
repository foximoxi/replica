using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R.Public;


namespace R.Config
{
    /// <summary>
    /// Interfejs requestu, ktory krazy miedzy warstwami
    /// </summary>
    public interface IRequestContext
    {
        RequestStatus Status { get; set; }
        object HttpContext { get; set; }
        HttpMethod HttpMethod { get; set; }
        int ThreadId { get; set; }
        Dictionary<string, string> InputParameters { get; set; }
        R.Security.UserProfile User { get; set; }
        object Response { get; set; }
        ResponseType ResponseType { get; set; }
        void Respond();
        String BodyString { get; set; }//body of request
    }
}
