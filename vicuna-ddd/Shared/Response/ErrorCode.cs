namespace vicuna_ddd.Shared.Response
{
    public enum ErrorCode : ushort
    {
        NoError = 0,
        Unknown = 1,
        DbError = 200,
        UserNotFound = 300,
        UserNotEnabled = 399
    }
}