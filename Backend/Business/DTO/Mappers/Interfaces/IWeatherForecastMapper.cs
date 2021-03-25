using Backend.Domain.Entities;

namespace Backend.Business.DTO.Mappers.Interfaces
{
    public interface IWeatherForecastMapper
    {
        WeatherForecastDTO CreateFrom(WeatherForecast entity);
    }
}
