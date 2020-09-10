using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo.DbModel;
using Demo.SqlServer;
using Demo.Infrastructure.Interafaces;
using Demo.Common.Extensions.Grid;
using Demo.Common;
using Demo.Dto.PhoneBook;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient.Server;

namespace Demo.APIControllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneBooksController : ControllerBase
    {
        #region Properties

        private readonly IPhoneBookService _phoneBookService;
        private readonly ILogger<PhoneBooksController> _logger;

        #endregion

        #region Constractor

        public PhoneBooksController(IPhoneBookService phoneBookService, ILogger<PhoneBooksController> logger)
        {
            _phoneBookService = phoneBookService;
            _logger = logger;
        }

        #endregion

        #region Get Data

        [HttpPost]
        [Route("getPhoneBooks")]
        public async Task<IActionResult> GetPhoneBooks([FromBody]DataSourceRequest requestMessage)
        {
            try
            {
                return Ok(await _phoneBookService.GetPhoneBooksAsync(requestMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Message: {0} , StackTrace: {1}", ex.Message, ex.StackTrace));
                return BadRequest(Constants.SomethingWentWrongMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoneBook(int id)
        {
            try
            {
                return Ok(await _phoneBookService.GetPhoneBookAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Message: {0} , StackTrace: {1}", ex.Message, ex.StackTrace));
                return BadRequest(Constants.SomethingWentWrongMessage);
            }
        }

        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> PutPhoneBook([FromBody]SavePhoneBookDto dto)
        {
            try
            {
                return Ok(await _phoneBookService.UpdatePhoneBooksAsync(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Message: {0} , StackTrace: {1}", ex.Message, ex.StackTrace));
                return BadRequest(Constants.SomethingWentWrongMessage);
            }
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<IActionResult> PostPhoneBook([FromBody]SavePhoneBookDto dto)
        {
            try
            {
                return Ok(await _phoneBookService.CreatePhoneBooksAsync(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Message: {0} , StackTrace: {1}", ex.Message, ex.StackTrace));
                return BadRequest(Constants.SomethingWentWrongMessage);
            }
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoneBook(int id)
        {
            try
            {
                return Ok(await _phoneBookService.DeletePhoneBooksAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Message: {0} , StackTrace: {1}", ex.Message, ex.StackTrace));
                return BadRequest(Constants.SomethingWentWrongMessage);
            }
        }

        #endregion
    }
}
