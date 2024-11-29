import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Routes, Route,Link } from 'react-router-dom';
import AddPatient from './components/AddPatient/AddPatient';
import PatientList from './components/PatientList/PatientList';
import ViewPatient from './components/ViewPatient/ViewPatient';
import { Login } from "./data-models/login";
import LoginService from './services/authService';
import { username,password } from "./services/config";

function App() {
  const [credentials] = useState<Login>({ userName: username, password: password });
  
  useEffect(() => {
    const response =  LoginService.login(credentials);
  }, []);
  return (
    <div>
      <Router>
    <nav className="navbar navbar-expand navbar-dark bg-dark">
      <a href="/tutorials" className="navbar-brand">
        Patient System
      </a>
      <div className="navbar-nav mr-auto">
        <li className="nav-item">
          <Link to="/add" className="nav-link">
            Create
          </Link>
        </li>
        <li className="nav-item">
          <Link to="/list" className="nav-link">
            View All
          </Link>
        </li>
      </div>
    </nav>

    <div className="container mt-3">
      <Routes>
        <Route path="/add" element={<AddPatient />} />
        <Route path="/list" element={<PatientList />} />
        <Route path="/ViewPatient/:id" element={<ViewPatient />} />
      </Routes>
    </div>
    </Router>
  </div>
  );
}

export default App;
