using AutoMapper;
using Demo.Dto.PhoneBook;
using Demo.DbModel;

namespace Demo.AutoMapper
{
    public class PhoneBookProfile : Profile
    {
        public PhoneBookProfile()
        {
            CreateMap<PhoneBook, GetPhoneBookDto>();
            CreateMap<SavePhoneBookDto, PhoneBook>();
        }
    }
}
