namespace FirstProject_Praksa.Database
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Succees { get; set; } = true;
        public string Message { get; set; } = null;
    }
}
