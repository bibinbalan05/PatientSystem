import React, { useState, useEffect, useMemo, useRef } from "react";
import { useTable, Column } from 'react-table';
import { Patient } from "../../data-models/patient";
import PatientService from "../../services/patientService";
import { useNavigate } from 'react-router-dom';

interface PatientDTO {
  id: number
  name: string;
  dob: string;
  gender: string;
  contactinfo:string;
}

const PatientList = ()  => {
  const [patients, setPatients] = useState<PatientDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const patientsRef = useRef<PatientDTO[]>([]);
  const navigate = useNavigate(); 

  patientsRef.current = patients;

  const openPatient = (id: any) => {
    navigate(`/ViewPatient/${id}`);
  };
  
  useEffect(() => {
    const fetchPatientsData = async () => {
      setLoading(true);
      setError(null); 
      
      try {
        const patientsList = await PatientService.getAll(); 
        const transformedPatients: PatientDTO[] = patientsList.map((patient: Patient) => ({
          id: patient.patientId,
          name: `${patient.firstName} ${patient.lastName}`,
          dob: new Date(patient.dob).toISOString().split('T')[0],
          contactinfo: patient.contactInfo,
          gender:patient.gender
        }));
        
        setPatients(transformedPatients);  
      } catch (error) {
        setError(error instanceof Error ? error.message : 'Failed to fetch patients.');
      } finally {
        setLoading(false); 
      }
    };
    fetchPatientsData();
  }, []);

  const columns: Column<PatientDTO>[] = useMemo(
    () => [
      {
        Header: "Name",
        accessor: "name",
      },
      {
        Header: "DOB",
        accessor: "dob",
      },
      {
        Header: "Gender",
        accessor: "gender"
      },
      {
        Header: "Phone",
        accessor: "contactinfo"
      },
      {
        Header: "Actions",
        accessor: "id",
        Cell: ({ value }) => {
          return (
              <button  onClick={() => openPatient(value)}>
                Open
              </button>
          );
        },
      },
    ],
    []
  );
  const data: PatientDTO[] =patients;

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    useTable({ columns, data });

  return (
    <table {...getTableProps()} style={{ border: '1px solid black', width: '100%' }}>
      <thead>
        {headerGroups.map((headerGroup:any) => (
          <tr {...headerGroup.getHeaderGroupProps()} style={{ background: '#ddd' }}>
            {headerGroup.headers.map((column:any) => (
              <th {...column.getHeaderProps()} style={{ padding: '10px', textAlign: 'left' }}>
                {column.render('Header')}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody {...getTableBodyProps()}>
        {rows.map((row:any) => {
          prepareRow(row);
          return (
            <tr {...row.getRowProps()} style={{ borderBottom: '1px solid black' }}>
              {row.cells.map((cell:any) => (
                <td {...cell.getCellProps()} style={{ padding: '10px' }}>
                  {cell.render('Cell')}
                </td>
              ))}
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};

export default PatientList;