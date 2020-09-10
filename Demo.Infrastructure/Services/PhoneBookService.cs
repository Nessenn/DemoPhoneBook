using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.Common.Extensions.Grid;
using Demo.DbModel;
using Demo.Dto.PhoneBook;
using Demo.Infrastructure.Interafaces;
using Demo.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    public class PhoneBookService : IPhoneBookService
    {
        #region Properties

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMapper _mapper;

        #endregion

        #region Constractor

        public PhoneBookService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapper = mapper;
        }

        #endregion

        #region Public methods

        public async Task<DataSourceResult> GetPhoneBooksAsync(DataSourceRequest requestMessage)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                return await uow.Repository.Query<PhoneBook>().OrderBy(v => v.Id).
                                ProjectTo<GetPhoneBookDto>(_mapper.ConfigurationProvider).
                                ToDataSourceResult(requestMessage.Take,
                                    requestMessage.Skip,
                                    requestMessage.Sort,
                                    requestMessage.Filter);
            }
        }

        public async Task<GetPhoneBookDto> GetPhoneBookAsync(int id)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var data = await uow.Repository.Query<PhoneBook>().FirstOrDefaultAsync(v => v.Id == id);
                return _mapper.Map<GetPhoneBookDto>(data);
            }
        }

        public async Task<bool> UpdatePhoneBooksAsync(SavePhoneBookDto dto)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var entity = await uow.Repository.Query<PhoneBook>().FirstOrDefaultAsync(v => v.Id == dto.Id);
                if (entity == null)
                {
                    return false;
                }

                _mapper.Map(dto, entity);
                await uow.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CreatePhoneBooksAsync(SavePhoneBookDto dto)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                dto.Id = null;
                var entity = new PhoneBook();
                _mapper.Map(dto, entity);
                uow.Repository.Add(entity);
                await uow.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeletePhoneBooksAsync(int id)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var phoneBook = await uow.Repository.Query<PhoneBook>().FirstOrDefaultAsync(v => v.Id == id);
                if (phoneBook == null)
                {
                    return false;
                }

                uow.Repository.Remove<PhoneBook>(id);
                await uow.SaveChangesAsync();

                return true;
            }
        }

        #endregion
    }
}
