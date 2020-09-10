using System.ComponentModel.DataAnnotations;

namespace Demo.Dto.PhoneBook
{
    public class SavePhoneBookDto
    {
        public int? Id { get; set; }

        [RegularExpression(@"^[0-9]*$"), Required, StringLength(15)]
        public string PhoneNumber { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(200)]
        public string Address { get; set; }
    }
}
