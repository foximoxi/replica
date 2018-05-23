﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace X.Config
{
    public interface IOperationDefinition
    {
        EndPointUri Uri { get; set; }
        X.Public.Op Operation { get; set; }
        string Database { get; set; }
        Type ExposedType { get; }//type of which definition revolves on        
    }
}