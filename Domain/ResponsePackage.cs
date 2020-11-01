namespace Domain
{
    using System.Collections.Generic;
    public class ResponsePackage 
    {
        public List<string> Errors { get; set; }
        public object Data { get; set; }
    }
}
