using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class PersonEndpoint
    {
        public static void ConfigurePersonEndpoint(this WebApplication app)
        {
            var people = app.MapGroup("people");
            people.MapGet("/", Get);

            var complaints = app.MapGroup("complaints");
            complaints.MapPost("/", Add);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Get(IRepository<Person> service)
        {
            var people = await service.GetAll();

            var response = new List<Object>();
            people.ToList().ForEach(person => response.Add(new { name=person.Name, address=person.Address, phone=person.PhoneNumber}));            
            return TypedResults.Ok(response);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Add(IRepository<Person> service, FormPost model)
        {
            var entity = await service.Insert(new Person() { Name = model.name, Address=model.address, PhoneNumber=model.tel });
            
            return TypedResults.Ok(new { status="added record", data= new { name=entity.Name, address=entity.Address, phone=entity.PhoneNumber } });
        }
    }
}
