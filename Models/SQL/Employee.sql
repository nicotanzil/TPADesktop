--Dropping all table 
DROP TABLE LeavingPermit
DROP TABLE FiringRequest
DROP TABLE SalaryRaiseRequest
DROP TABLE RequestExpense
DROP TABLE ViolationReport
DROP TABLE MaintenanceSchedule
DROP TABLE MaintenanceReport
DROP TABLE Employee
DROP TABLE Candidate
DROP TABLE ATM
DROP TABLE HouseCompanyPartner
DROP TABLE Item
DROP TABLE ItemType 
DROP TABLE Bill 
DROP TABLE Department
DROP TABLE Branch
DROP TABLE Manager



------------MANAGER------------

CREATE TABLE Manager(
	ManagerId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL
)

INSERT INTO Manager VALUES
('MA001', 'Hao Pa')

------------------------------------------------

------------Branch------------

CREATE TABLE Branch(
	BranchId VARCHAR(5) PRIMARY KEY, 
	ManagerId VARCHAR(5) NOT NULL, 
	[Location] VARCHAR(255) NOT NULL, 

	FOREIGN KEY (ManagerId) REFERENCES Manager(ManagerId)
)

INSERT INTO Branch VALUES
('BR001', 'MA001', 'Alam Sutera')

------------------------------------------------

------------DEPARTMENT------------

CREATE TABLE [Department] (
	DepartmentId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL, 
	AvailablePosition INT NOT NULL, 
	BranchId VARCHAR(5) NOT NULL, 

	FOREIGN KEY (BranchId) REFERENCES Branch(BranchId) 
)

INSERT INTO Department VALUES
('DE001', 'Teller', 10, 'BR001'), 
('DE002', 'Customer Service', 10, 'BR001'), 
('DE003', 'Maintenance Team', 15, 'BR001'), 
('DE004', 'HRM', 10, 'BR001'), 
('DE005', 'Finance Team', 10, 'BR001')

------------------------------------------------

------------BILL------------
CREATE TABLE Bill (
	BillId VARCHAR(5) PRIMARY KEY, 
	DepartmentId VARCHAR(5) NOT NULL, 
	[Name] VARCHAR(255) NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	IsPaid BIT NOT NULL, 
	PaidDate DATETIME NOT NULL, 

	FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId)
)

------------------------------------------------

------------ITEM TYPE------------

CREATE TABLE ItemType(
	ItemTypeId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL 
)

INSERT INTO ItemType VALUES
('TY001', 'Stationary'), 
('TY002', 'Accessories'), 
('TY003', 'Electronics')


------------------------------------------------

------------ITEM------------

CREATE TABLE Item(
	ItemId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL, 
	ItemTypeId VARCHAR(5) NOT NULL, 
	DepartmentId VARCHAR(5) NOT NULL, 
	[Value] DECIMAL(20, 2) NOT NULL, 
	CreatedAt DATETIME NOT NULL, 

	FOREIGN KEY (ItemTypeId) REFERENCES ItemType(ItemTypeId), 
	FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId) 
)

INSERT INTO Item VALUES 
('IT001', 'Desk', 'TY001', 'DE001', 750000, GETDATE()), 
('IT002', 'Desk', 'TY001', 'DE002', 750000, GETDATE()), 
('IT003', 'Wastebasket', 'TY001', 'DE001', 150000, GETDATE()), 
('IT004', 'Wastebasket', 'TY001', 'DE003', 150000, GETDATE()), 
('IT005', 'Flower Pot', 'TY002', 'DE001', 75000, GETDATE()), 
('IT006', 'Flower Pot', 'TY002', 'DE002', 75000, GETDATE()),
('IT007', 'Laptop', 'TY003', 'DE001', 6500000, GETDATE()),
('IT008', 'Computer', 'TY003', 'DE002', 6500000, GETDATE()),
('IT009', 'Computer', 'TY003', 'DE003', 6500000, GETDATE())

SELECT * FROM Item

------------------------------------------------



------------HOUSE COMPANY PARTNER------------

CREATE TABLE HouseCompanyPartner(
	CompanyId VARCHAR(5) PRIMARY KEY, 
	CompanyName VARCHAR(255) NOT NULL, 
	BranchId VARCHAR(5) NOT NULL, 

	FOREIGN KEY (BranchId) REFERENCES Branch(BranchId) 
)

------------------------------------------------

------------ATM------------

CREATE TABLE ATM (
	AtmId VARCHAR(5) PRIMARY KEY, 
	BranchId VARCHAR(5) NOT NULL, 
	[Location] VARCHAR(255) NOT NULL, 
	MonthlyBill DECIMAL(20, 2) NOT NULL, 

	FOREIGN KEY (BranchId) REFERENCES Branch(BranchId)
)

------------------------------------------------

------------CANDIDATE------------

CREATE TABLE Candidate (
	CandidateId VARCHAR(5) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL, 
	Dob DATETIME NOT NULL, 
	RegistrationDate DATETIME NOT NULL,
	[Status] VARCHAR(20) NOT NULL
)

INSERT INTO Candidate VALUES
('CA001', 'William', '2002-03-18', GETDATE(), 'Accepted'),
('CA002', 'James', '2000-05-12', GETDATE(), 'Accepted'),
('CA003', 'Tom', '2001-03-20', GETDATE(), 'Accepted'),
('CA004', 'John', '2001-04-23', GETDATE(), 'Accepted')

------------------------------------------------

------------EMPLOYEE------------

CREATE TABLE [Employee] (
	EmployeeId VARCHAR(5) PRIMARY KEY, 
	CandidateId VARCHAR(5) NOT NULL,
	[Name] VARCHAR(255) NOT NULL, 
	Dob DATETIME NOT NULL, 
	DepartmentId VARCHAR(5) NOT NULL, 
	Email VARCHAR(255) NOT NULL, 
	[Password] VARCHAR(255) NOT NULL, 
	PerformanceScore DECIMAL(20, 2) NOT NULL, 
	ViolationScore DECIMAL(20, 2) NOT NULL, 
	Salary DECIMAL(20, 2) NOT NULL, 
	IsActive BIT NOT NULL, 

	FOREIGN KEY(CandidateId) REFERENCES Candidate(CandidateId),
	FOREIGN KEY(DepartmentId) REFERENCES Department(DepartmentId)
)

INSERT INTO Employee VALUES 
('EM001', 'CA001', 'William', '2002-03-18', 'DE001', 'william@mail.com', 'password', 0, 0, 4500000, 1), 
('EM002', 'CA002', 'James', '2000-05-12', 'DE002', 'james@mail.com', 'password', 0, 0, 4500000, 1),
('EM003', 'CA003', 'Tom', '2001-03-20', 'DE003', 'tom@mail.com', 'password', 0, 0, 4500000, 1),
('EM004', 'CA004', 'John', '2001-04-23', 'DE004', 'john@mail.com', 'password', 0, 0, 5000000, 1)

------------------------------------------------

------------MAINTENANCE REPORT------------

CREATE TABLE [MaintenanceReport] (
	ReportId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	ItemId VARCHAR(5) NOT NULL, 
	ReportDate DATETIME NOT NULL, 
	[Status] BIT NOT NULL, 
	[Description] VARCHAR(255), 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId), 
	FOREIGN KEY (ItemId) REFERENCES Item(ItemId)
)

SELECT * FROM MaintenanceReport

------------------------------------------------

------------MAINTENANCE SCHEDULE------------
CREATE TABLE [MaintenanceSchedule] (
	ReportId VARCHAR(5) PRIMARY KEY, 
	[Status] VARCHAR(15) NOT NULL, 
	StartDate DATETIME, 
	EndDate DATETIME, 
	EstimateCost DECIMAL(20, 2), 
	[Description] VARCHAR(255), 

	FOREIGN KEY (ReportId) REFERENCES MaintenanceReport(ReportId) 
)

------------VIOLATION REPORT------------

CREATE TABLE ViolationReport(
	ViolationId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Score DECIMAL(5, 2) NOT NULL, 
	[Description] VARCHAR(255) NOT NULL,
	[Date] DATETIME NOT NULL, 
	IsActive BIT NOT NULL,

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------REQUEST EXPENSE------------

CREATE TABLE RequestExpense(
	RequestId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	[Description] VARCHAR(255) NOT NULL, 
	IsApproved VARCHAR(20) NOT NULL, 
	RequestDate DATETIME NOT NULL, 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------SALARY RAISE REQUEST------------

CREATE TABLE SalaryRaiseRequest(
	RequestId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	IsApproved VARCHAR(20) NOT NULL, 
	RequestDate DATETIME NOT NULL, 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------FIRING REQUEST------------

CREATE TABLE FiringRequest(
	RequestId VARCHAR(5) PRIMARY KEY,
	TargetEmployeeId VARCHAR(5) NOT NULL, 
	[Description] VARCHAR(255) NOT NULL, 
	RequestDate DATETIME NOT NULL, 
	IsActive BIT NOT NULL,

	FOREIGN KEY (TargetEmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------LEAVING PERMIT------------

CREATE TABLE LeavingPermit(
	LeavingPermitId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	LeaveDate DATETIME NOT NULL, 
	Reason VARCHAR(255) NOT NULL,

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------