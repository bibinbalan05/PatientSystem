import { Login } from "../data-models/login";
import axiosInstance from "./axios";



const login = async (credentials: Login): Promise<Login> => {
    const res = await axiosInstance.post(`auth/login`, credentials);

    if (res.data?.token) {
      localStorage.setItem("authToken", res.data.token);
    }
  
    return res.data;
  };
const LoginService = {
    login
};
export default LoginService;