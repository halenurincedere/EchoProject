namespace Echo.Business.Operations.Feature.Dtos
{
    public class AddFeatureDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public string? Source { get; set; }
        public string? Tag { get; set; }
    }
}