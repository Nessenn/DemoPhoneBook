using Demo.Common.Extensions.Grid;
using Demo.Dto.PhoneBook;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Interafaces
{
    public interface IPhoneBookService
    {
        Task<bool> CreatePhoneBooksAsync(SavePhoneBookDto dto);
        Task<bool> DeletePhoneBooksAsync(int id);
        Task<GetPhoneBookDto> GetPhoneBookAsync(int id);
        Task<DataSourceResult> GetPhoneBooksAsync(DataSourceRequest requestMessage);
        Task<bool> UpdatePhoneBooksAsync(SavePhoneBookDto dto);
    }
}
