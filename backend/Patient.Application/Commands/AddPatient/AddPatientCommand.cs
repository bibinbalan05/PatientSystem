using MediatR;
using Patient.Infrastructure.Data;

namespace Patient.Application.Commands
{
    public class AddPatientCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public char Gender { get; set; }
        public string ContactInfo { get; set; }

        public AddPatientCommand(
            string firstName,
            string lastName,
            DateTime dob,
            char gender,
            string contactInfo)
        {
            FirstName = firstName;
            LastName = lastName;
            Dob = dob;
            Gender= gender;
            ContactInfo= contactInfo;
        }

        public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand, int>
        {
            private readonly IDbHelper _dbHelper;

            public AddPatientCommandHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<int> Handle(AddPatientCommand request, CancellationToken cancellationToken)
            {
                await using var conn = _dbHelper.GetConnection();
                await using var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO Patients (first_name, last_name,dob,gender,contact_info) OUTPUT INSERTED.patient_id VALUES (@FirstName, @LastName, @Dob,@Gender,@ContactInfo)";              
                command.Parameters.AddWithValue("@FirstName", request.FirstName);
                command.Parameters.AddWithValue("@LastName", request.LastName);
                command.Parameters.AddWithValue("@Gender", request.Gender);
                command.Parameters.AddWithValue("@Dob", request.Dob);
                command.Parameters.AddWithValue("@ContactInfo", request.ContactInfo);                               
                var newUserId = (int)await command.ExecuteScalarAsync(cancellationToken);
                return newUserId;                
                
            }
        }
    }
}
