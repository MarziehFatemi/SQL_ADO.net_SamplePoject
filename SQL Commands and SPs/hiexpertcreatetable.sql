DROP TABLE IF EXISTS Physician;
CREATE TABLE Physician (
  UserName VARCHAR(30) PRIMARY KEY NOT NULL,
  Name VARCHAR(30) NOT NULL,
  KartNumber VARCHAR(16)  NULL,
  AccountNumber VARCHAR(16)  NULL,
  CommisionPercent INTEGER NOT NULL Default 50,
  TotalSale INTEGER  NULL,
  TotalIncome INTEGER  NULL,
  TotalPayments INTEGER  NULL,
  RemindingPayment INTEGER  NULL,
  --CONSTRAINT pk_physician PRIMARY KEY(EmployeeID)

); 

DROP TABLE IF EXISTS Patient;
CREATE TABLE Patient (
  UserName VARCHAR(30) PRIMARY KEY NOT NULL,
  Name VARCHAR(30) NOT NULL,
  Email VARCHAR(30) NOT NULL,
  Phone VARCHAR(30) NOT NULL,
  InsuranceID INTEGER NOT NULL,
  TotalPayments INTEGER NOT NULL,
  --PCP INTEGER NOT NULL,
 -- CONSTRAINT fk_Patient_Physician_EmployeeID FOREIGN KEY(PCP) REFERENCES Physician(EmployeeID)
);

DROP TABLE IF EXISTS Appointment;
CREATE TABLE Appointment (
  AppointmentID INTEGER PRIMARY KEY NOT NULL,
  Patient VARCHAR(30) NOT NULL FOREIGN KEY(Patient) REFERENCES Patient(UserName),    
  ---PrepNurse INTEGER,
  Physician VARCHAR(30) NOT NULL FOREIGN KEY(Physician) REFERENCES Physician(UserName),
  DateOfVisit DATETIME NOT NULL,
 
  --CONSTRAINT fk_Appointment_Patient_SSN FOREIGN KEY(Patient) REFERENCES Patient(UserName),
  --CONSTRAINT fk_Appointment_Nurse_EmployeeID FOREIGN KEY(PrepNurse) REFERENCES Nurse(EmployeeID),
  --CONSTRAINT fk_Appointment_Physician_EmployeeID FOREIGN KEY(Physician) REFERENCES Physician(EmployeeID)
);

