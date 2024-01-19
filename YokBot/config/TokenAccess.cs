using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YokBot.config
{
    internal class TokenAccess
    {
        public string token { get; set; }
        public string call { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                Structure data = JsonConvert.DeserializeObject<Structure>(json);

                this.token = data.token;
                this.call = data.call;
            }
        }

    }

    internal sealed class Structure{
        public string token {  set; get; }  
        public string call { set; get; }
    
    }

}
