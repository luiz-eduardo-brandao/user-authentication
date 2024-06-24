using System.Text.Json;
using ApplicationSecretKeys.Model;

namespace ApplicationSecretKeys
{
    public class EmailSenderKeyValues
    {
        public EmailSenderKeyValues() 
        {
            var values = ReadJsonFile();

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            From = values.From;
            Cc = values.Cc;
            SecretKey = values.SecretKey;
        }

        public string From { get; }    
        public string Cc { get; }
        public string SecretKey { get; }

        private EmailSecretValues ReadJsonFile() 
        {
            var dir = @"C:\assets\secret-key-values\email-values.json";

            using (StreamReader r = new StreamReader(dir))
            {
                string json = r.ReadToEnd();
                r.Close();
                return JsonSerializer.Deserialize<EmailSecretValues>(json);
            }
        }
    }
}