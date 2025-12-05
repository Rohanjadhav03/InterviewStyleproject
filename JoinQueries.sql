CREATE TABLE Departments
(
    DeptId INT IDENTITY(1,1) PRIMARY KEY,
    DeptName VARCHAR(50)
);


INSERT INTO Departments (DeptName)
VALUES ('IT')


CREATE TABLE Employees
(
    EmpId INT IDENTITY(1,1) PRIMARY KEY,
    EmpName VARCHAR(50),
    DeptId INT 
);

INSERT INTO Employees (EmpName, DeptId)
VALUES 
('Rohan', 1),
('Amit', 1),
('Sneha', 2),
('Vikas', 3),
('Rutuja', NULL);  -- Employee with no department


select * from Departments 
select * from Employees


select d.DeptName , d.DeptId from Departments d join Employees e  on d.DeptId = e.DeptId  where e.EmpName = 'amit' -- inner join

select * from Departments d FULL JOIN Employees e on d.DeptId = e.DeptId

select * from Departments d right JOIN Employees e on d.DeptId = e.DeptId

select * from Departments d left JOIN Employees e on d.DeptId = e.DeptId


select top 5* from Departments order by DeptId desc

select DeptName ,COUNT(*) from Departments group by DeptName

select DeptName ,COUNT(*) from Departments group by DeptName HAVING COUNT(DeptName)>1

select * from Departments where DeptName like '%ket%'


select  max(d.DeptId) ,DeptName from Departments d group by DeptName 