using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCommsException : System.Exception
{
    #region Vars

    #endregion

    #region Constructors

    public CloudCommsException()
    {
        
    }

    public CloudCommsException(string message) : base(message)
    {
        
    }

    public CloudCommsException(string message, System.Exception inner) : base(message, inner)
    {
        
    }

    #endregion
}
