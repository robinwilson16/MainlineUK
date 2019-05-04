using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Models
{
    public class StocklistImport
    {
        public int StocklistImportID { get; set; }

        //Needed for JSON _unionBy as both JSONs must have the same key field
        [JsonProperty(NamingStrategyType = typeof(DefaultNamingStrategy))]
        public string Id
        {
            get
            {
                return StocklistImportID.ToString();
            }
        }

        public int DealerID { get; set; }

        [StringLength(100)]
        public string StockItemID { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [StringLength(20)]
        public string Category { get; set; }

        [Required]
        [StringLength(7)]
        public string Registration { get; set; }

        public int RegCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        [StringLength(100)]
        public string Derivative { get; set; }

        [StringLength(100)]
        public string AttentionGrabber { get; set; }

        public int EngineSize { get; set; }

        [StringLength(10)]
        public string EngineSizeUnit { get; set; }

        [StringLength(20)]
        public string FuelType { get; set; }

        public int ManufacturedYear { get; set; }
        public int Mileage { get; set; }

        [StringLength(10)]
        public string MileageUnit { get; set; }

        [StringLength(20)]
        public string BodyType { get; set; }

        [StringLength(20)]
        public string Colour { get; set; }

        [StringLength(20)]
        public string Transmission { get; set; }
        public int Doors { get; set; }
        public int? PreviousOwners { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(50)]
        [Display(Name = "Price Extra")]
        public string PriceExtra { get; set; }

        [StringLength(50)]
        public string WheelBaseType { get; set; }

        [StringLength(50)]
        public string CabType { get; set; }

        [StringLength(1)]
        public string FourWheelDrive { get; set; }

        [StringLength(1)]
        public string FranchiseApproved { get; set; }

        [StringLength(50)]
        public string Style { get; set; }

        [StringLength(50)]
        public string SubStyle { get; set; }
        public int? Hours { get; set; }
        public int? NumberOfBerths { get; set; }

        [StringLength(50)]
        public string Axle { get; set; }

        [StringLength(20)]
        public string DealerReference { get; set; }

        [JsonIgnore]
        public string Images { get; set; }

        public string VideoURL { get; set; }

        [Display(Name = "DateOfRegistration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfRegistration { get; set; }

        [StringLength(100)]
        public string ServiceHistory { get; set; }

        public string KeyInformation { get; set; }

        [StringLength(500)]
        public string OtherVehicleText { get; set; }

        [StringLength(500)]
        public string Closer { get; set; }
        public string Feature { get; set; }

        [StringLength(20)]
        public string VatStatus { get; set; }

        [Required]
        public DateTime DateImported { get; set; }

        public ICollection<Photo> Photo { get; set; }
    }
}
