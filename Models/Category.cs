using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int Category_Id {  get; set; }
        public string Category_Name { get; set; }
    }
}
