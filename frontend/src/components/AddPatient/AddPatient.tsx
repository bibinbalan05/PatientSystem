import React, { useState } from "react";
import PatientService from "../../services/patientService";
import { Patient } from "../../data-models/patient";

const AddPatient = () => {
  const initialState:Patient = {
    patientId:0,
    firstName: "",
    lastName:"",
    dob: new Date(),
    gender: "",
    contactInfo:""
  };

  const [formData, setFormData] = useState(initialState);
  const [showSuccess, setShowSuccess] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value, 
    });
  };

  const handleSubmit = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    console.log("Form submitted");
    await PatientService.create(formData);
    setShowSuccess(true);
  };
  const reset=()=>{
    setShowSuccess(false);
    setFormData(initialState);
  }
  return (
    <div className="submit-form">
      {showSuccess ? (
         <div className="alert alert-success alert-dismissible fade show mt-3" role="alert">
         Success! Your operation was completed successfully.
         <button type="button" className="close" data-dismiss="alert" aria-label="Close" onClick={() => reset()}>
             <span aria-hidden="true">&times;</span>
         </button>
         </div>
      ) : (
        <div className="form-group col-sm-12 col-md-6">
          <div className="form-group" >
            <label htmlFor="firstName">First Name</label>
            <input
              type="text"
              className="form-control"
              id="firstName"
              required
              value={formData.firstName}
              onChange={handleChange}
              name="firstName"
            />
          </div>

          <div className="form-group">
            <label htmlFor="title">Last Name</label>
            <input
              type="text"
              className="form-control"
              id="lastName"
              required
              value={formData.lastName}
              onChange={handleChange}
              name="lastName"
            />
          </div>

          <div className="form-group">
            <label htmlFor="description">DOB</label>
            <input
              type="date"
              className="form-control"
              id="dob"
              required
              
              onChange={handleChange}             
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
              value={formData.gender}
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
            <label htmlFor="ContactInfo">Phone</label>
            <input
              type="text"
              className="form-control"
              id="contactInfo"
              required
              value={formData.contactInfo}
              onChange={handleChange}
              name="contactInfo"
            />
          </div>
          
          <div className="form-group">
            <button className="btn btn-success" style={{  marginTop: '20px' }} onClick={handleSubmit}>
              Submit
            </button>
        </div>
        </div>
      )}
    </div>
  );
};

export default AddPatient;