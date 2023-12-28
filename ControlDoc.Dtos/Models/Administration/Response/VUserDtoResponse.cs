using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Models.Models.Administration.Response
{
    public class VUserDtoResponse
    {
        public int? CompanyId { get; set; }

        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string FullName { get; set; } = null!;

        public string? IdentificationTypeCode { get; set; }

        public string? IdentificationType { get; set; }

        public string? Identification { get; set; }

        public int AdministrativeUnitId { get; set; }

        public string AdministrativeUnitCode { get; set; } = null!;

        public string? AdministrativeUnitName { get; set; }

        public string AdministrativeUnit { get; set; } = null!;

        public int? ProductionOfficeId { get; set; }

        public string? ProductionOfficeCode { get; set; }

        public string? ProductionOfficeName { get; set; }

        public string ProductionOffice { get; set; } = null!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CellPhoneNumber { get; set; }

        public string? ChargeCode { get; set; }

        public string? Charge { get; set; }

        public string? ContractTypeCode { get; set; }

        public string? ContractType { get; set; }

        public string? ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractFinishDate { get; set; }

        public bool Selected { get; set; } = false;
        public bool Copy { get; set; } = false;
    }
}