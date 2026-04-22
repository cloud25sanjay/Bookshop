namespace  BookShop.Application.DTOs.Books
{
    public class AddReviewRequest
    {
        public int Rating {get; set;}
        public string Comment {get;set;} = string.Empty;
    }
}