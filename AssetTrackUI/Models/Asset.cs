namespace AssetTrackUI.Models
{
    public class Asset
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? Identifier { get; set; } // Assuming this is a string for barcodes, QR codes, etc.
        public long LocationId { get; set; }
        public string? Status { get; set; }
        public string? AssignedTo { get; set; }
        public long SubCategoryId { get; set; }
        public long SubLocationId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpiry { get; set; }
    }
}
