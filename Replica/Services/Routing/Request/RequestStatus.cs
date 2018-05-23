using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R.Config
{
    /// <summary>
    /// Interfejs profilu uzytkownika zwracanego z bazy
    /// </summary>
    public enum RequestStatus
    {
        ReadyToProcess=0,
        Requested=1,
        Deserialization=2,
        Validation=3,
        ResourceAvailablility=4,
        BeforeOperation=5,
        Operation=6,
        AfterOperation=7,
        ResponsePreparation=8,
        ResponsePrepared =9,
        ResponseSend=10,

        DelayedResponseInPreparation =20,
        DelayedResponded=21,

        AccessDenied =30,
    }
}
