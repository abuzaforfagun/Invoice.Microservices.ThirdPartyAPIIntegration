namespace InvoiceProcessor.Domain.ValueObjects
{
    public record Debtor(string FirstName, 
        string LastName, 
        string Email, 
        string Phone, 
        int DebtorType,
        string Address,
        string ZipCode, 
        string City);
}
