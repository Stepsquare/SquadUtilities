namespace SQUtilitiesNetFramework.Webservice
{
    public class GenericOutput<T> : IOutput<T>
    {
        public T Data { get; set; }
        public string Messages { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        OK = 200,
        FAIL = 500
    }
}
