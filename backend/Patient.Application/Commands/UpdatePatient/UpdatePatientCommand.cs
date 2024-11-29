using MediatR;
using System.Data;
using Patient.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Patient.Application.Commands
{
    public class UpdatePatientCommand : IRequest<bool>
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public char Gender { get; set; }
        public string ContactInfo { get; set; }

        public UpdatePatientCommand(
            int patientID,
            string firstName,
            string lastName,
            DateTime dob,
            char gender,
            string contactInfo)
        {
            PatientID = patientID;
            FirstName = firstName;
            LastName = lastName;
            Dob = dob;
            Gender= gender;
            ContactInfo= contactInfo;
        }

        internal class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
        {
            private readonly IDbHelper _dbHelper;

            public UpdatePatientCommandHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
            {
                await using var conn = _dbHelper.GetConnection();
                await using var command = conn.CreateCommand();                
                command.CommandText = "UPDATE Patients SET first_name = @FirstName, last_name = @LastName, dob= @Dob, gender= @Gender, contact_info=@ContactInfo WHERE patient_id= @PatientId";
                command.Parameters.AddRange(new[] {
                    new SqlParameter("@PatientId", SqlDbType.Int) { Value = request.PatientID },
                    new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = request.FirstName },
                    new SqlParameter("@LastName", SqlDbType.VarChar) { Value = request.LastName },
                    new SqlParameter("@Gender", SqlDbType.Char) { Value = request.Gender },
                    new SqlParameter("@Dob", SqlDbType.DateTime) { Value = request.Dob },
                    new SqlParameter("@ContactInfo", SqlDbType.VarChar) { Value = request.ContactInfo }

                });
                var rowaffected = (int)await command.ExecuteNonQueryAsync(cancellationToken);
                if(rowaffected >0)
                    return true;
                else
                    return false;
            }
        }
    }
}
