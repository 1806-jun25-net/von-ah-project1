
--create tables
CREATE TABLE Orders
( OrderID int Identity(1,1), LocationID int NOT NULL, UserID int NOT NULL, OrderTime datetime NOT NULL, TotalPrice decimal(13,2) NOT NULL);

CREATE TABLE Users
( UserID int Identity(1,1), FirstName nvarchar(128) NOT NULL, LastName nvarchar(128) NOT NULL, defaultAddress nvarchar(128) NOT NULL);

CREATE TABLE Locations
( LocationID int Identity(1,1), Address nvarchar(128) NOT NULL, ToppingInventoryPepperoni int NOT NULL, ToppingInventoryCheese int NOT NULL);

CREATE TABLE OrderPizza
( ID int Identity(1,1), OrderID int NOT NULL, PizzaID int NOT NULL);

CREATE TABLE Pizzas
( PizzaID int Identity(1,1), ToppingPepperoni bit NOT NULL, ToppingCheese bit NOT NULL, Price decimal(13,2) NOT NULL);

-- add primary keys
ALTER TABLE Orders
ADD PRIMARY KEY (OrderID);

ALTER TABLE Users
ADD PRIMARY KEY (UserID);

ALTER TABLE Locations
ADD PRIMARY KEY (LocationID);

ALTER TABLE OrderPizza
ADD PRIMARY KEY (ID)

ALTER TABLE Pizzas
ADD PRIMARY KEY (PizzaID)

-- add foreign keys
ALTER TABLE Orders
ADD FOREIGN KEY (LocationID) REFERENCES Locations(LocationID);

ALTER TABLE Orders
ADD FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE OrderPizza
ADD FOREIGN KEY (OrderID) REFERENCES Orders(OrderID);

ALTER TABLE OrderPizza
ADD FOREIGN KEY (PizzaID) REFERENCES Pizzas(PizzaID);


Alter Table Users
Add ManagerFlag bit Default 0
go 
update Users
Set ManagerFlag = 0

insert Users
Values ('Papa', 'John', '123 Grove St.', 1)

SELECT * from Orders
SELECT * from Users
SELECT * from Locations
SELECT * from OrderPizza
SELECT * from Pizzas

--initalize some values

INSERT Users
VALUES ('Lance', 'Von Ah', '123 Grove St.') 

Insert Users
Values ('Rolando', 'Toledo', '21 Jump St.')

INSERT Locations
VALUES ('123 Grove St.', 50, 50), ('21 Jump St.', 50, 50),('221B Baker St.', 50, 50)


INSERT Pizzas
VALUES (0,0,5.00), (0,1,6.00), (1,0,6.00), (1,1,7.00)

Insert Orders
Values (1, 1, '20120618 10:34:09 AM', 12)

Insert Orders
Values (2, 2, '20130618 10:34:09 AM', 7)

Insert OrderPizza
Values (1, 1), (1, 4)

Insert OrderPizza
Values (2,4)

Update Users
Set defaultAddress = '123 Grove St.'
WHERE UserID = 1

DELETE FROM OrderPizza
Where ID = 15

DELETE FROM Orders
WHERE OrderID = 10

DELETE FROM Users
WHERE UserID = 4

DELETE FROM Locations
WHERE LocationID = 5
--go

DROP TABLE Orders
DROP TABLE Users
DROP TABLE Locations
DROP TABLE OrderPizza
DROP TABLE Pizzas