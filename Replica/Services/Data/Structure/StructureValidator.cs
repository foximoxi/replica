using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using R.Public;

namespace R.Config
{
    public class StructureValidator:Validator
    {
        public void ValidateStructure(R.Config.IStructure structure)
        {
            if (String.IsNullOrEmpty(structure.Name))
                this.validations.Add(new ValidationInfo() { Name = "Name", Message = "Structure name cannot be empty" });
        }
    }
}