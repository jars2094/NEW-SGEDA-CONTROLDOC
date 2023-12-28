namespace ControlDoc.Models.Models.Administration.Response
{
    public class CompaniesDtoResponse
    {
        public int CompanyId { get; set; }
        public int AddressId { get; set; }
        public string IdentificationType { get; set; }
        public string Identification { get; set; }
        public string BusinessName { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string LegalAgentIdType { get; set; }
        public string LegalAgentId { get; set; }
        public string LegalAgentFullName { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string? Domain { get; set; }
    }
}