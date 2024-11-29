using MediatR;
using Patient.Infrastructure.Data;


namespace Patient.Application.Commands
{
    public class DeletePatientCommand : IRequest<bool>
    {
        public int PatientID { get; set; }

        public DeletePatientCommand(int patientID)
        {
            PatientID = patientID;
        }

        internal class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
        {
            private readonly IDbHelper _dbHelper;

            public DeletePatientCommandHandler(IDbHelper dbHelper)
            {
                _dbHelper = dbHelper;
            }

            public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
            {
                await using var conn = _dbHelper.GetConnection();

                await using var command = conn.CreateCommand();
                command.CommandText = "DELETE FROM Patients WHERE patient_id = @PatientID";
                command.Parameters.AddWithValue("@PatientID", request.PatientID);
                var rowaffected= await command.ExecuteNonQueryAsync();
                return true;
            }
        }
    }
}
