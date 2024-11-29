

using MediatR;
using Patient.Infrastructure.Data;
using Patient.Domain.Entities.Models;
using Microsoft.Data.SqlClient;

namespace Patient.Application.Queries
{
    public class GetPatientByIdQuery : IRequest<PatientModel>
    {
        public int Id { get; set; }
        public GetPatientByIdQuery(int id)
        {
            Id = id;
        }

        internal class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientModel>
        {
            private readonly IDbHelper _dbHelper;

            public GetPatientByIdQueryHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<PatientModel> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
            {
                
                await using var conn = _dbHelper.GetConnection();
               
                await using var command = conn.CreateCommand();
                command.CommandText = "SELECT *  FROM Patients where patient_id=@PatientId";
                command.Parameters.AddWithValue("@PatientId", request.Id);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new PatientModel
                    {
                        PatientId = (int)reader["patient_id"],
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        Dob = (DateTime)reader["dob"],
                        Gender = reader["gender"].ToString()[0],
                        ContactInfo = reader["contact_info"].ToString()
                    };
                }
                return null;
            }
        }
    }
}
