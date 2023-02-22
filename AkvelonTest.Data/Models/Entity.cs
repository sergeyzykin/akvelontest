using Newtonsoft.Json;

namespace AkvelonTest
{
    public class Entity<T>
    {
        [JsonProperty(PropertyName = "id")]
        public virtual T Id { get; set; }

        [JsonProperty(PropertyName = "partitionId")]
        public virtual long PartitionId { get; internal set; } = 1;
    }
}
