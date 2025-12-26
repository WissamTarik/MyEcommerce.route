namespace MyEccommerce.Route.Web.HandleErrors
{
    public class ValidationError
    {
        public string Field { get; set; }
        public IEnumerable<string> Message { get; set; }
    }
}
