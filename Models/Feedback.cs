using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Feedback
    {
        [Key]
        public int feedback_Id { get; set; }
        public string user_name {  get; set; }
        public string user_message { get; set; }
    }
}
