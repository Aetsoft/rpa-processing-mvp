using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PostBase64DataOcrRequestModel
    {
        [JsonProperty(PropertyName="base64")]
        private string Base64String;
        
        [JsonProperty(PropertyName = "lang")]
        private string LanguagesConfiguration;

        public PostBase64DataOcrRequestModel(String base64String, String langs)
        {
            this.Base64String = base64String;
            this.LanguagesConfiguration = langs;
        }

        public PostBase64DataOcrRequestModel() { }

        public string GetBase64String()
        {
            return Base64String;
        }

        public string GetLanguagesConfiguration()
        {
            return LanguagesConfiguration;
        }
    }
}
