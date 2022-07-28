namespace ProductManagementAPI.Data.ViewModel.ResponseVM
{
    public class ApiBaseResponseVM
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; } = 200;
    }

    public class ApiBaseDataResponseVM<T> : ApiBaseResponseVM
    {
        public ApiBaseDataResponseVM(T data, bool status = true)
        {
            this.Data = data;
            this.Status = status;
            this.StatusCode = status ? 200 : 400;
            this.Message = status ? "Success" : "Failed";
        }

        public ApiBaseDataResponseVM()
        {
        }

        public T Data { get; set; }
    }

    public class ApiBaseErrorResponseVM : ApiBaseResponseVM
    { 
        public ApiBaseErrorResponseVM()
        {
            Status = false;
        } 
    }
}
