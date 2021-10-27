using System;
using PlayFab;

[Serializable]
public class LoginFailedException : Exception
{
    #region MyRegion

    /// <summary>
    /// Código del error producido
    /// </summary>
    public PlayFabErrorCode ErrorCode;

    #endregion

    #region Constructors

    public LoginFailedException()
    {
    }

    public LoginFailedException(string message)
        : base(message)
    {
    }

    public LoginFailedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    #endregion
}
