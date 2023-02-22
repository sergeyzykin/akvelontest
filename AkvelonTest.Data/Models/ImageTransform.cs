using Newtonsoft.Json;

namespace AkvelonTest
{
    public class ImageTransform : Entity<string>
    {
        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "originalFilePath")]
        public string OriginalFilePath { get; set; }

        [JsonProperty(PropertyName = "modifiedFilePath")]
        public string ModifiedFilePath { get; set; }

        [JsonProperty(PropertyName = "state")]
        public State State { get; set; }
    }

    public enum State { Created = 0, InProgress , Done, Error } 
}
