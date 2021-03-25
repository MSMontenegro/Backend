using Backend.Business.DTO.Mappers.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Business.DTO.Mappers
{
    public class WeatherForecastMapper : IWeatherForecastMapper
    {
        public WeatherForecastDTO CreateFrom(WeatherForecast entity)
        {
            return new WeatherForecastDTO
            {
                Date = entity.Date,
                TemperatureC = entity.TemperatureC,
                TemperatureF = entity.TemperatureF,
                Summary = entity.Summary
            };
        }
    }
}
