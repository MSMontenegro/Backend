using Backend.Business.DTO;
using Backend.Business.DTO.Mappers.Interfaces;
using Backend.Business.Services.Interfaces;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business.Services
{
    public class WeatherForecastService: IWeatherForecastService
    {
        private IWeatherForecastMapper _mapper;

        public WeatherForecastService(IWeatherForecastMapper mapper)
        {
            _mapper = mapper;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IList<WeatherForecastDTO> List()
        {
            var rng = new Random();
            var entities = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();

            return entities.Select(x => _mapper.CreateFrom(x)).ToList();
        }
    }
}
