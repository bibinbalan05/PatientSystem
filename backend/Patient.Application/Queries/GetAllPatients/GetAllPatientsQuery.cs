using MediatR;
using Patient.Infrastructure.Data;
using Patient.Domain.Entities.Models;

namespace Patient.Application.Queries
{
    public class GetAllPatientsQuery : IRequest<IEnumerable<PatientModel>>
    {
        public GetAllPatientsQuery()
        {
           
        }

        internal class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, IEnumerable<PatientModel>>
        {
            private readonly IDbHelper _dbHelper;

            public GetAllPatientsQueryHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<IEnumerable<PatientModel>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
            {
                var patients = new List<PatientModel>();
                await using var conn = _dbHelper.GetConnection();

                await using var command = conn.CreateCommand();
                command.CommandText = "SELECT *  FROM Patients"; 

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (reader.Read())
                    {
                        patients.Add(new PatientModel
                        {
                            PatientId = (int)reader["patient_id"],
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Dob = (DateTime)reader["dob"],
                            Gender = reader["gender"].ToString()[0],
                            ContactInfo = reader["contact_info"].ToString()
                        });
                    }
                }
                return patients;
            }
        }
    }
}
