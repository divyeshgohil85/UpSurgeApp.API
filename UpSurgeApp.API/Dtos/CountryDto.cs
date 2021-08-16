using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Dtos
{
    public class CountryDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Imgpath { get; set; }
    }
}
