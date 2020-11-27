--Dropping all table 

DROP TABLE SalaryRaiseRequest
DROP TABLE RequestExpense
DROP TABLE ViolationReport
DROP TABLE Employee
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
	BranchId VARCHAR(5) NOT NULL, 

	FOREIGN KEY (BranchId) REFERENCES Branch(BranchId) 
)

INSERT INTO Department VALUES
('DE001', 'Teller', 'BR001'), 
('DE002', 'Customer Service', 'BR001'), 
('DE003', 'Maintenance Team', 'BR001'), 
('DE004', 'Human Resource Management Team', 'BR001'), 
('DE005', 'Finance Team', 'BR001')

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

------------EMPLOYEE------------

CREATE TABLE [Employee] (
	EmployeeId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL, 
	Dob DATETIME NOT NULL, 
	DepartmentId VARCHAR(5) NOT NULL, 
	Email VARCHAR(255) NOT NULL, 
	[Password] VARCHAR(255) NOT NULL, 
	IsActive BIT NOT NULL, 

	FOREIGN KEY(DepartmentId) REFERENCES Department(DepartmentId)
)

INSERT INTO Employee VALUES 
('EM001', 'William', '2002-03-18', 'DE001', 'william@mail.com', 'password', 1), 
('EM002', 'James', '2000-05-12', 'DE002', 'james@mail.com', 'password', 1)

------------------------------------------------

------------VIOLATION REPORT------------

CREATE TABLE ViolationReport(
	ViolationId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Score DECIMAL(5, 2) NOT NULL, 
	[Date] DATETIME NOT NULL, 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------REQUEST EXPENSE------------

CREATE TABLE RequestExpense(
	RequestId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	[Description] VARCHAR(255) NOT NULL, 
	IsApproved BIT NOT NULL, 
	RequestDate DATETIME NOT NULL, 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------

------------SALARY RAISE REQUEST------------

CREATE TABLE SalaryRaiseRequest(
	RequestId VARCHAR(5) PRIMARY KEY, 
	EmployeeId VARCHAR(5) NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	IsApproved BIT NOT NULL, 
	RequestDate DATETIME NOT NULL, 

	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId)
)

------------------------------------------------