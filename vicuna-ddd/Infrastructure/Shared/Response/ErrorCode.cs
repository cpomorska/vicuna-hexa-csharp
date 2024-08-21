namespace vicuna_ddd.Shared.Response;

public enum ErrorCode : ushort
{
    NoError = 0,
    Unknown = 1,
    DbError = 200,
    UserNotFound = 300,
    UserNotCreated = 304,
    UserNotUpdated = 305,
    UserNotEnabled = 399,
    UserNotRemoved = 400
}