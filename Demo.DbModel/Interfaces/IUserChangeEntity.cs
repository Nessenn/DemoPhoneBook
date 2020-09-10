using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DbModel.Interfaces
{
    public interface IUserChangeEntity
    {
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
