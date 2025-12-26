namespace MyEccommerce.Route.Web.HandleErrors
{
    public class ValidationErrorResponse
    {
        public int Status { get;  }=StatusCodes.Status404NotFound;
        public IEnumerable<ValidationError> Errors { get; set; }
        public string ErrorMessage { get; } = "Validation error";


    }
}
