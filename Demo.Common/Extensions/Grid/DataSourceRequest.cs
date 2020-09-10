using System.Collections.Generic;

namespace Demo.Common.Extensions.Grid
{
    public class DataSourceRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public List<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
    }
}
