namespace CourseDemo.Domain.Common
{
    public class JsonResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public JsonResponse(bool success, T data, string message)
        {
            this.Success = success;
            this.Data = data;
            this.Message = message;
        }
    }
}
