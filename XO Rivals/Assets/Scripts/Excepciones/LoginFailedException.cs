using System;
using PlayFab;

[Serializable]
public class LoginFailedException : Exception
{
    public PlayFabErrorCode ErrorCode;
    
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
}
