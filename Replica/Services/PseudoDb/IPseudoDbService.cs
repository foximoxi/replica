using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using R.Config;
using R.Component;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace R.Services
{
    public interface IPseudoDbService
    {
        Dictionary<string, Collection> ObjectCollections { get; set; }
    }
}