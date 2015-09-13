use master
go
ALTER DATABASE SaleManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE
go
	--do you stuff here 
--ALTER DATABASE SaleManagement SET MULTI_USER
--go
DROP DATABASE SaleManagement
go