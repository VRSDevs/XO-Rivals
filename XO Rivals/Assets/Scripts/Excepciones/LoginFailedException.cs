using PlayFab;


public class LoginFailedException : System.Exception
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

    public LoginFailedException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    #endregion
}
