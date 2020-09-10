using System;

namespace Demo.Dto.PhoneBook
{
    public class GetPhoneBookDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
