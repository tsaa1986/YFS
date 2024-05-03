using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YFS.Core.Models
{
    public class Bank
    {
        /*[Column("BankId")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required(ErrorMessage = "Acount's name is a required field")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string Name { get; set; }*/
        [Key]
        [Column("GLMFO")]
        public int GLMFO { get; set; }

        [Column("NAME_E")]
        [MaxLength(255)]
        public string NameEnglish { get; set; }

        [Column("KOD_EDRPOU")]
        [MaxLength(50)]
        public string CodeEDRPOU { get; set; }

        [Column("SHORTNAME")]
        [MaxLength(255)]
        public string ShortName { get; set; }

        [Column("FULLNAME")]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Column("NKB")]
        [MaxLength(50)]
        public string NKB { get; set; }

        [Column("TYP")]
        public int Type { get; set; }

        [Column("KU")]
        public int KU { get; set; }

        [Column("N_OBL")]
        [MaxLength(100)]
        public string NOBL { get; set; }

        [Column("OBL_UR")]
        public int OBLUR { get; set; }

        [Column("N_OBL_UR")]
        [MaxLength(100)]
        public string NOBLUR { get; set; }

        [Column("P_IND")]
        [MaxLength(20)]
        public string PostalIndex { get; set; }

        [Column("TNP")]
        [MaxLength(50)]
        public string TNP { get; set; }

        [Column("NP")]
        [MaxLength(100)]
        public string NP { get; set; }

        [Column("ADRESS")]
        [MaxLength(255)]
        public string Address { get; set; }

        [Column("TELEFON")]
        public string? Telephone { get; set; }

        [Column("KSTAN")]
        public int Status { get; set; }

        [Column("N_STAN")]
        [MaxLength(100)]
        public string StatusName { get; set; }

        [Column("D_STAN")]
        public DateTime? DateStatus { get; set; }

        [Column("D_OPEN")]
        public DateTime OpenDate { get; set; }

        [Column("D_CLOSE")]
        public DateTime? CloseDate { get; set; }

        [Column("IDNBU")]
        [MaxLength(50)]
        public string IDNBU { get; set; }

        [Column("NUM_LIC")]
        public int LicenseNumber { get; set; }

        [Column("DT_GRAND_LIC")]
        public DateTime GrantLicenseDate { get; set; }

        [Column("PR_LIC")]
        public int LicenseStatus { get; set; }

        [Column("N_PR_LIC")]
        [MaxLength(255)]
        public string LicenseStatusDescription { get; set; }

        [Column("DT_LIC")]
        public DateTime? LicenseDate { get; set; }

        [Column("SHORTNAME_EN")]
        [MaxLength(255)]
        public string ShortNameEnglish { get; set; }

        [Column("GR_SP")]
        [MaxLength(50)]
        public string GroupSpecial { get; set; }

        [Column("D_GR_SP")]
        public DateTime? GroupSpecialDate { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
