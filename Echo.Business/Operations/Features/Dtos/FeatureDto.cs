namespace Echo.Business.Operations.Feature.Dtos
{
public class FeatureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? Source { get; set; } = string.Empty;
        public string? Tag { get; set; } = string.Empty;
    }
}