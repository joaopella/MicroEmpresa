namespace MicroEmpresa.Entity
{
    public class ResponseMessage
    {
        public string Message { get; set; } = "";
        public string Data { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
