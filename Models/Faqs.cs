using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Faqs
    {
        [Key]
        public int faqs_Id { get; set; }
        public string faqs_Question { get; set; }
        public string faqs_Answer { get; set; }
    }
}
