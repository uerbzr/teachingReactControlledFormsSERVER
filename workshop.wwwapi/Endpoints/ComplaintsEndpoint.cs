using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web.Http.Cors;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class ComplaintsEndpoint
    {
        public static void ConfigureComplaintsEndpoint(this WebApplication app)
        {
            var people = app.MapGroup("complaints");
            people.MapGet("/", Get);

            var complaints = app.MapGroup("complaints");
            complaints.MapPost("/", Add);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Get(IRepository<Complaint> service)
        {
            var people = await service.GetAll();

            var response = new List<Object>();
            people.ToList().ForEach(person => response.Add(new { name=person.Name, address=person.Address, phone=person.PhoneNumber}));            
            return TypedResults.Ok(response);
        }
        [EnableCors(origins: "*",headers: "*", methods: "*")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Add(IRepository<Complaint> service, ComplaintFormPost model)
        {

            var entity = await service.Insert(new Complaint() { Name = model.name, Address=model.address, PhoneNumber=model.phone, Description=model.complaint });
            
            return TypedResults.Ok(new { status="added record", data= new { name=entity.Name, address=entity.Address, phone=entity.PhoneNumber } });
        }
    }
}
