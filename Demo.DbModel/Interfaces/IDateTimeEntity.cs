using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DbModel.Interfaces
{
    public interface IDateTimeEntity
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
