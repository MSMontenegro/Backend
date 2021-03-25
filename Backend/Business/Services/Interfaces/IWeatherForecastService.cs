using Backend.Business.DTO;
using System.Collections.Generic;

namespace Backend.Business.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        IList<WeatherForecastDTO> List();
    }
}
