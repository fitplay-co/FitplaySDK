namespace SEngineBasic
{
    public struct ApplicationClient : OSApplicationBase
    {
        public string type;
        public string id;
        
        public ApplicationClient(string param = null)
        {
            type = "application_client";
            id = "application_id";
        }
    }
}