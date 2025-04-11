namespace Echo.Business.Operations.Match.Dtos
{
    public class GetMatchDto
    {
        public Guid Id { get; set; }

        public string SpeakerName { get; set; }

        public string ListenerName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}