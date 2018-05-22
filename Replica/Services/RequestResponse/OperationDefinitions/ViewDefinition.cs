using System;
using System.Collections.Generic;

namespace X.Config
{
    public class ViewDefinition: SubViewDefinition,IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public string Database { get; set; }
        public IOperationDefinition OperationDefinition
        {
            get { return this;  }
        }
        public X.Public.Op Operation { get; set; }
    }

    public class SubViewDefinition
    {
        public ReturnType ReturnType { get; set; }
        public string Name { get; set; }//name of subfield, not required in main Viewdefinition
        public string Query { get; set; }
        public List<SubViewDefinition> SubFields { get; set; } = new List<SubViewDefinition>();
        public string Type { get; set; }//type of what result should be mapped
        //for typed
        public Type TypeForQuery { get; set; }
        public Type ExposedType
        {
            get { return TypeForQuery; }
        }
    }

    public enum ReturnType
    {
        Default,//lista elementow lub obiekt
        Single//wymuszaj zwrot pojedynczego obiektu/wartosci a nie listy
    }

    //abc/def
    //zakodowane query
    //dodatkowe rekordy (podrekordy)
    //wymiana pol na pola z innych np. kluczy obcych
    //zliczenia/grupowanie etc.
    //linq
    //zdefiniowane kroki lub elementy przetworzenia
    //kawalek kodu pokazujacy cos

    //public List<QueryParameter> QueryParameter { get; set; } = new List<QueryParameter>();
    //public string[] SelectedColumns { get; set; }
    //public int Depth { get; set; }
    //public bool Filtering { get; set; }
    //public string OrderColumn { get; set; }
    //public bool OrderDescending{ get; set; }
    //public bool RecordLimit { get; set; }
    //public bool StartIndx { get; set; }
}
