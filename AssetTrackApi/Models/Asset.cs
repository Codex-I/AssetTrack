using ZXing;
using SkiaSharp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTrackApi.Models
{
    public class Asset
    {
        public Asset(Category category, string name, AssetCondition condition, DateTime acquisitionDate, DateTime? warrantyExpiry)
        {
            ArgumentNullException.ThrowIfNull(category);
            ArgumentNullException.ThrowIfNull(condition);

            if (acquisitionDate == default)
            {
                throw new ArgumentException("Acquisition date is set to default, please specify.", nameof(acquisitionDate));
            }

            Id = Guid.NewGuid();
            AcquisitionDate = acquisitionDate;
            Category = category;
            CreatedAt = DateTime.UtcNow;

            UpdateName(name);     
            UpdateCondition(condition);
            UpdateWarrantyExpiry(warrantyExpiry);
            AssetTag = GenerateBarcodeAssetTag();
            BarcodeImagePath = SaveBarcodeImageToFileSystem();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string AssetTag { get; private set; }
        public string BarcodeImagePath { get; private set; }
        public long? LocationId { get; private set; }
        public long? SubLocationId { get; private set; }
        public string Owner { get; private set; } // The original owner or manager of the asset
        public string? AssignedUser { get; private set; } // The current user or person the asset is assigned to
        public AssetCondition Condition { get; private set; }
        public DateTime AcquisitionDate { get; private set; }
        public DateTime? WarrantyExpiry { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }

        public Category Category { get; private set; }
        public Location Location { get; private set; }
        public SubLocation SubLocation { get; private set; }

        public virtual ICollection<Maintenance> MaintenanceSchedules { get; private set; } = new List<Maintenance>();

        private static string GenerateBarcodeAssetTag()
        {
            return $"ASSET-{Guid.NewGuid().ToString()[..8]}";
        }

        public SKBitmap GenerateBarcodeImage()
        {
            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 300,
                    Margin = 10
                }
            };

            var pixelData = barcodeWriter.Write(AssetTag);
            var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

            using (var pixmap = bitmap.PeekPixels())
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, pixmap.GetPixels(), pixelData.Pixels.Length);
            }

            return bitmap;
        }

        public string SaveBarcodeImageToFileSystem()
        {
            try
            {
                string barcodeDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "barcodes");
                if (!Directory.Exists(barcodeDirectory))
                {
                    Directory.CreateDirectory(barcodeDirectory);
                }

                var barcodeImage = GenerateBarcodeImage();
                string barcodeFilePath = Path.Combine(barcodeDirectory, $"{AssetTag}.png");

                using (var image = SKImage.FromBitmap(barcodeImage))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(barcodeFilePath))
                {
                    data.SaveTo(stream);
                }

                return $"/barcodes/{AssetTag}.png";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving barcode image: {ex.Message}");
                return string.Empty;
            }
        }

        public void UpdateName(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);
            Name = newName;
        }

        public void UpdateWarrantyExpiry(DateTime? newWarrantyExpiry)
        {
            if (newWarrantyExpiry.HasValue && newWarrantyExpiry < AcquisitionDate)
            {
                throw new ArgumentException("Warranty expiry cannot be earlier than the acquisition date.", nameof(newWarrantyExpiry));
            }

            WarrantyExpiry = newWarrantyExpiry;
        }

        // Assign the current user of the asset
        public void AssignToUser(string assignedUser)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(assignedUser);
            AssignedUser = assignedUser;
        }

        public void AssignLocation(Location location, SubLocation? subLocation)
        {
            ArgumentNullException.ThrowIfNull(location);
            LocationId = location.Id;
            SubLocationId = subLocation?.Id;
        }

        public void MarkForMaintenance()
        {
            UpdateCondition(AssetCondition.INMAINTENANCE);
        }

        public void AddMaintenanceRecord(Maintenance maintenanceRecord)
        {
            ArgumentNullException.ThrowIfNull(maintenanceRecord);
            MaintenanceSchedules.Add(maintenanceRecord);
        }

        public void RemoveMaintenanceRecord(Maintenance maintenanceRecord)
        {
            ArgumentNullException.ThrowIfNull(maintenanceRecord);
            MaintenanceSchedules.Remove(maintenanceRecord);
        }

        public IEnumerable<Maintenance> GetMaintenanceHistory()
        {
            return MaintenanceSchedules.OrderBy(m => m.ScheduledDate);
        }

        public void UpdateCondition(AssetCondition newCondition)
        {
            Condition = newCondition;
        }

        public bool IsWarrantyValid()
        {
            return WarrantyExpiry.HasValue && DateTime.UtcNow <= WarrantyExpiry.Value;
        }

        #pragma warning disable CS8618
        private Asset() { }
    }
}
