namespace TwitterStreamApi.Pipeline
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }

        public T? Data { get; set; }

        public string? Message { get; set; }


        public static Response<T> Success(T Data)
        {
            return new Response<T> { Succeeded = true, Data = Data };
        }

        public static Response<T> Fail(string errorMessage)
        {
            return new Response<T> { Succeeded = false, Message = errorMessage };
        }
    }
}
