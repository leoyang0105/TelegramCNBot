using Newtonsoft.Json;

namespace CNBot.Core.Dtos
{
    public class TGResponseDTO<T>
    {
        [JsonProperty("ok")]
        public bool IsOK { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
