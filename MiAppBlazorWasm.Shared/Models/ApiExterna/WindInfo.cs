using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiAppBlazorWasm.Model.Models.ApiExterna
{
    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }
}
