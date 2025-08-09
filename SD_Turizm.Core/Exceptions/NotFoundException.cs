namespace SD_Turizm.Core.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message, string errorCode = "NOT_FOUND")
            : base(message, errorCode, 404)
        {
        }

        public NotFoundException(string entityName, object id)
            : base($"{entityName} with id {id} was not found.", "NOT_FOUND", 404)
        {
        }
    }
}
