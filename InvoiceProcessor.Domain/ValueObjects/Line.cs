namespace InvoiceProcessor.Domain.ValueObjects
{
    public record Line (
        int UnitNetPrice, 
        string Description, 
        int VatRate, 
        int DiscountType, 
        int DiscountValue);
}
