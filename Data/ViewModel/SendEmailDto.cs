using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Data.ViewModel
{
    public class AddProduct : BaseEntityDto
    {
        [Required]
        public string ProjectId { get; set; }


        [Required]
        public string OrganizationId { get; set; }
    }

    public class Email
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Recepient { get; set; }
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
    }

    public class SendEmailDto
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        //reciepeintAddress
        public string ReciepeintAddress { get; set; }
    }

    public class GetEmailDto
    {
        public string Emailid { get; set; }
        public string EmailAddress { get; set; }
    }

    public class EmailResult
    {

        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Data { get; set; }

        public List<ErrorItemModel> Errors { get; set; }


        public EmailResult()
        {
            Errors = new List<ErrorItemModel>();
        }
    }
}
