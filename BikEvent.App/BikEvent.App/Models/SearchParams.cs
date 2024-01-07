using System;
using System.Collections.Generic;
using System.Text;

namespace BikEvent.App.Models
{
    public class SearchParams
    {
        public string Word { get; set; }
        public string CityState { get; set; }
        public int PageNumber { get; set; }
    }
}
