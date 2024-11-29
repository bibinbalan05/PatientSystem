CREATE DATABASE hospitalDB;
CREATE TABLE Patients (
    patient_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    dob DATE NOT NULL,
    gender CHAR(1) CHECK (gender IN ('M', 'F', 'O')),
    contact_info VARCHAR(100) 
);
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(50) UNIQUE NOT NULL,
	PasswordHash NVARCHAR(50) NOT NULL,
);

INSERT INTO Users (Email, PasswordHash)
VALUES ('bibin.balan@example.com', '$2a$10$0azVqhHuoqfwLSUMdiGd3exGPGXTYdpm2LHZzgRbWTM.mw1CzGDT2');

