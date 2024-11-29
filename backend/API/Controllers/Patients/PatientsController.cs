using API.Controllers.Patients.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patient.Application.Commands;
using Patient.Domain.Entities.Models;
using Patient.Application.Queries;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Patients
{
    [Route("api/patient")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;  
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _mediator.Send(new GetAllPatientsQuery());
                return Ok(patients);               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _mediator.Send(new GetPatientByIdQuery(id));
            if (patient == null)
            {
                return NotFound(new { Message = $"Patient with ID {id} not found." });
            }

            return Ok(patient);             
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatient createPatient)
        {
            try
            {
                var patientId = await _mediator.Send(new AddPatientCommand(
                    createPatient.FirstName,
                    createPatient.LastName,
                    createPatient.Dob,
                    createPatient.Gender,
                    createPatient.ContactInfo));
                return Ok(new { PatientId = patientId });
            }
            catch (Exception ex) 
            {
                throw;
            }            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatient updatePatient)
        {
            try
            {
                if (id != updatePatient.PatientId)
                {
                    return BadRequest(new { Message = "ID in the URL does not match ID in the body." });
                }

                var result = await _mediator.Send(new UpdatePatientCommand(
                        id,
                        updatePatient.FirstName,
                        updatePatient.LastName,
                        updatePatient.Dob,
                        updatePatient.Gender,
                        updatePatient.ContactInfo));

                if (!result)
                {
                    return NotFound(new { Message = $"Item with ID {id} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {                
                var success = await _mediator.Send(new DeletePatientCommand(id));

                if (!success)
                {
                    return NotFound(new { Message = $"Item with ID {id} not found." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }      

    }
}
