using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public class SD
    {
        public static string APIBaseUrl = "https://localhost:44346/"; 
        public static string NationalParkAPIPath = APIBaseUrl +"api/v1/nationalpark";
        public static string TrailAPIPath = APIBaseUrl+ "api/v1/trails";
    }
}
