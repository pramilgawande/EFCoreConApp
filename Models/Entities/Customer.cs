namespace EFCoreConApp.Models.Entities
{
  public class Customer
  {
    public string CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    
    public override string ToString()
    {
      return $"CustomerID: {CustomerID}, CompanyName: {CompanyName}, ContactName: {ContactName}, City: {City}, Country: {Country}";
    }
  }

}