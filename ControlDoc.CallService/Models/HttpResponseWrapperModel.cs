using ControlDoc.Models.Models.Pagination;

namespace ControlDoc.Services.Models
{
    public class HttpResponseWrapperModel<T>
    {
        public bool Succeeded { get; set; } = false;
        public T? Data { get; set; } = default;
        public List<string>? Errors { get; set; }
        public string? CodeError { get; set; }
        public string? Message { get; set; }
        public Meta Meta { get; set; }
    }
}
