using Demo.DbModel.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.DbModel
{
    public class PhoneBook: IDateTimeEntity
    {
        public int Id { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        //public int UserId { get; set; }
        //public virtual User User { get; set; }
    }
}
