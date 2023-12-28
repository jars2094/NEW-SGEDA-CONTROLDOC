namespace ControlDoc.Models.Models.Administration.Response
{
    public class DocumentaryTypologiesDtoResponse
    {
        public int DocumentaryTypologyId { get; set; }

        public int DocumentaryTypologyBagId { get; set; }

        public int SeriesId { get; set; }

        public int? SubSeriesId { get; set; }

        public string ClassCode { get; set; }

        public string CorrespondenceType { get; set; }

        public int? LeadManagerId { get; set; }

        public string LeadManagerName { get; set; }

        public string ManagerInstructionCode { get; set; }
    }
}