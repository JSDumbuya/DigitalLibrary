namespace DigitalLibrary.API.Common;

public enum ErrorType
{
    None = 0,
    LibraryNotFound,
    UserNotFound,
    LibraryAlreadyExists,
    BookNotFound,
    InvalidCredentials,
    UserAlreadyExists

}