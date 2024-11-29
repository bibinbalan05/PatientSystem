import axiosInstance from './axios';
import {Patient} from "../data-models/patient";

const getAll = async (): Promise<Patient[]> => {
    const res = await axiosInstance.get(`/patient`);
    return res.data
}
const get = async (
    patientId: number
  ): Promise<Patient> => {
    const res = await axiosInstance.get(`/patient/${patientId}`)
    return res.data
}
const create = async (patient: Patient): Promise<Patient> => {
    const res = await axiosInstance.post(`/patient`, patient); 
    return res.data;
  };
const update = async (
    patientId: number,
    patient: Patient
  ): Promise<Patient> => {
    const res = await axiosInstance.put(`/patient/${patientId}`, patient)
    return res.data
}
const remove = async (
    patientId: number
  ) => {
    await axiosInstance.delete(`/patient/${patientId}`)
   
}
const PatientService = {
    getAll,
    get,
    create,
    update,
    remove,
};
export default PatientService;