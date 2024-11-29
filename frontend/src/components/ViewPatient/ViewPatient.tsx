import React, { useState, useEffect } from "react";
import PatientService from "../../services/patientService";
import { Patient } from "../../data-models/patient";
import { useNavigate, useParams } from "react-router-dom";

const ViewPatient = () => {
    const initialState:Patient = {
        patientId:0,
        firstName: "",
        lastName:"",
        dob: new Date(),
        gender: "",
        contactInfo:""
    };
  const { id } = useParams<{ id: string }>();
  const [patient, setPatient] = useState(initialState);
  const [showSuccess, setShowSuccess] = useState(false);
  
  const navigate = useNavigate(); 

  const getPatient = async (id:number) => {
    const patient = await PatientService.get(id); 
    setPatient(patient);
  };
  const deletePatient = async (e: React.MouseEvent<HTMLButtonElement>) => {  
    e.preventDefault();   
    await PatientService.remove(patient.patientId);
    setPatient(initialState);
    navigate("/list", { replace: true });
  };
  const updatePatient = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    var res= await PatientService.update(patient.patientId,patient);
    setShowSuccess(true);
  };
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setPatient({
      ...patient,
      [name]: value, 
    });
  };

 
  useEffect(() => {
    if (id) {
        const numericId = Number(id); 
      if (!isNaN(numericId)) {
        getPatient(numericId); 
      } else {
        console.error("Invalid ID:", id);
      }      
    }
  }, [id]);
  return (
    <div className="form-group col-sm-12 col-md-6">
      {patient ? (
        <div className="edit-form">
          <h4>Patient</h4>
          <form>
            <div className="form-group">
              <label htmlFor="firstName">First Name</label>
              <input
                type="text"
                className="form-control"
                id="firstName"
                required
                value={patient.firstName}
                onChange={handleChange}
                name="firstName"
                />
            </div>
            <div className="form-group">
              <label htmlFor="lastName">Last Name</label>
              <input
                type="text"
                className="form-control"
                id="lastName"
                required
                value={patient.lastName}
                onChange={handleChange}
                name="lastName"
                />
            </div>
            <div className="form-group">
              <label htmlFor="DOB">DOB</label>
               <input
                type="date"
                className="form-control"
                id="dob"
                required              
                onChange={handleChange} 
                value={new Date(patient.dob).toISOString().split('T')[0]}            
                name="dob"
                />
            </div>
            <div className="form-group">
              <label htmlFor="Gender">Gender</label>
              <select
                id="gender"
                name="gender"
                className="form-control"
                required
                value={patient.gender}
                onChange={handleChange}              
                >
                <option value="" disabled>
                    Select Gender
                </option>
                <option value="M">Male</option>
                <option value="F">Female</option>
               
                </select>
            </div>
            <div className="form-group">
              <label htmlFor="description">ContactInfo</label>
              <input
                type="text"
                className="form-control"
                id="contactInfo"
                required
                value={patient.contactInfo}
                onChange={handleChange}
                name="contactInfo"
                />
            </div>
            <div className="form-group">
                <button className="btn btn-success" style={{  marginTop: '20px' }} onClick={deletePatient}>
                    Delete
                </button>
                <button className="btn btn-success" style={{  marginTop: '20px',marginLeft: '20px' }} onClick={updatePatient}>
                    Update
                </button>
            </div>
          </form>
          {showSuccess && (
                <div className="alert alert-success alert-dismissible fade show mt-3" role="alert">
                Success! Your operation was completed successfully.
                <button type="button" className="close" data-dismiss="alert" aria-label="Close" onClick={() => setShowSuccess(false)}>
                    <span aria-hidden="true">&times;</span>
                </button>
                </div>
            )}
          
        </div>
      ) : (
        <div>
          <br />
          <p>No Data</p>
        </div>
      )}
    </div>
  );
};

export default ViewPatient;