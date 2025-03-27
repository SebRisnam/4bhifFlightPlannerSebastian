-- Set the database name
SET @databaseName = 'FlightPlanner';
SET @dbExists = 0;


-- Check if the database exists and create it if it doesn't
-- MySQL does not have DB_ID, so we use information_schema
SELECT COUNT(*) INTO @dbExists
FROM information_schema.schemata
WHERE schema_name = @databaseName;

-- Create the database if it does not exist
IF @dbExists = 0 THEN
    SET @createDBSQL = CONCAT('CREATE DATABASE ', @databaseName);
    PREPARE stmt FROM @createDBSQL;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
ELSE
    SELECT 'Database already exists';
END IF;

-- Switch to the created database
USE FlightPlanner;

-- Remove foreign key constraints before dropping tables
-- MySQL syntax differs from MSSQL

-- Drop foreign keys if they exist
SET @sql = 'SELECT CONCAT("ALTER TABLE ", table_name, " DROP FOREIGN KEY ", constraint_name) 
            FROM information_schema.key_column_usage
            WHERE table_schema = "FlightPlanner" AND referenced_table_name IS NOT NULL';

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Drop tables if they exist
DROP TABLE IF EXISTS Airline;
DROP TABLE IF EXISTS PlaneType;
DROP TABLE IF EXISTS Plane;
DROP TABLE IF EXISTS Pilot;
DROP TABLE IF EXISTS PilotTraining;
DROP TABLE IF EXISTS Training;
DROP TABLE IF EXISTS Flight;
DROP TABLE IF EXISTS Customer;
DROP TABLE IF EXISTS PilotRoster;
DROP TABLE IF EXISTS Booking;

-- Create the tables

CREATE TABLE Airline (
    Id INT PRIMARY KEY,
    RegisteredCompanyName VARCHAR(40),
    Country VARCHAR(40) NOT NULL,
    HeadQuarters VARCHAR(40) NOT NULL
);

CREATE TABLE PlaneType (
    Id VARCHAR(40) PRIMARY KEY,
    Seats INT NOT NULL,
    Velocity INT NOT NULL
);

CREATE TABLE Plane (
    Id INT PRIMARY KEY,
    OwnershipDate DATE NOT NULL,
    LastMaintenance DATE NOT NULL,
    PlaneTypeId VARCHAR(40),
    AirlineId INT,
    CONSTRAINT FK_Plane_PlaneType FOREIGN KEY (PlaneTypeId) REFERENCES PlaneType(Id) ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT FK_Plane_Airline FOREIGN KEY (AirlineId) REFERENCES Airline(Id) ON DELETE SET NULL ON UPDATE NO ACTION
);

CREATE TABLE Pilot (
    Id INT PRIMARY KEY,
    LastName VARCHAR(40) NOT NULL,
    Birthday DATE NOT NULL,
    Qualification VARCHAR(40) CHECK (Qualification IN('Captain', 'Copilot')),
    FlightHours INT NOT NULL CHECK (FlightHours >= 0),
    FirstDate DATE NOT NULL,
    AirlineId INT NOT NULL DEFAULT -1,
    CONSTRAINT FK_Pilot_Airline FOREIGN KEY (AirlineId) REFERENCES Airline(Id) ON DELETE SET DEFAULT
);

CREATE TABLE Training (
    Id INT PRIMARY KEY,
    Description VARCHAR(40) NOT NULL,
    Level INT NOT NULL,
    CHECK (Level > 0 AND Level <= 5)
);

CREATE TABLE PilotTraining (
    PilotId INT NOT NULL,
    TrainingId INT NOT NULL,
    Date DATE NOT NULL,
    PRIMARY KEY (PilotId, TrainingId),
    CONSTRAINT FK_PilotTraining_Pilot FOREIGN KEY (PilotId) REFERENCES Pilot(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_PilotTraining_Training FOREIGN KEY (TrainingId) REFERENCES Training(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Flight (
    Id INT PRIMARY KEY,
    Departure VARCHAR(40) NOT NULL,
    Destination VARCHAR(40) NOT NULL,
    Duration INT NOT NULL, -- duration in minutes
    DepartureDate DATE NOT NULL,
    PlaneId INT,
    CONSTRAINT FK_Flight_Plane FOREIGN KEY (PlaneId) REFERENCES Plane(Id)
);

CREATE TABLE Customer (
    Id INT PRIMARY KEY,
    LastName VARCHAR(40) NOT NULL,
    Birthday DATE NOT NULL,
    City VARCHAR(40) NOT NULL
);

CREATE TABLE PilotRoster (
    PilotId INT,
    FlightId INT,
    PRIMARY KEY (PilotId, FlightId),
    CONSTRAINT FK_PilotRoster_Pilot FOREIGN KEY (PilotId) REFERENCES Pilot(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_PilotRoster_Flight FOREIGN KEY (FlightId) REFERENCES Flight(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Booking (
    FlightId INT DEFAULT -1,
    CustomerId INT,
    Seats INT NOT NULL,
    TravelClass INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (FlightId, CustomerId),
    CONSTRAINT FK_Booking_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Booking_Flight FOREIGN KEY (FlightId) REFERENCES Flight(Id) ON DELETE NO ACTION ON UPDATE CASCADE
);

-- Insert initial data

INSERT INTO PlaneType VALUES ('Boeing 747', 660, 920);
INSERT INTO PlaneType VALUES ('Airbus A380', 853, 1185);
INSERT INTO PlaneType VALUES ('Concorde', 128, 2179);

INSERT INTO Airline VALUES (-1, 'Default', 'Default', 'Default');
INSERT INTO Airline VALUES (1, 'Austrian Airlines', 'Austria', 'Vienna');
INSERT INTO Airline VALUES (2, 'Air France', 'France', 'Paris');
INSERT INTO Airline VALUES (3, 'Iberia', 'Spain', 'Madrid');

INSERT INTO Plane VALUES (10, '1981-12-03', '2000-12-30', 'Concorde', 2);
INSERT INTO Plane VALUES (11, '1990-06-02', '2005-10-23', 'Boeing 747', 2);
INSERT INTO Plane VALUES (21, '1993-12-27', '2015-09-28', 'Airbus A380', 1);
INSERT INTO Plane VALUES (22, '1999-07-22', '2017-03-16', 'Boeing 747', 1);
INSERT INTO Plane VALUES (33, '2000-01-17', '2019-04-18', 'Concorde', 3);

INSERT INTO Flight VALUES (203, 'Berlin', 'Paris', 120, '2018-02-02', 21);
INSERT INTO Flight VALUES (204, 'Frankfurt', 'Atlanta', 900, '2018-04-10', 11);
INSERT INTO Flight VALUES (205, 'Berlin', 'Paris', 360, '2016-05-22', 10);
INSERT INTO Flight VALUES (206, 'Hawaii', 'Moskau', 480, '2017-06-12', 22);
INSERT INTO Flight VALUES (207, 'Paris', 'London', 120, '2019-02-02', 33);
INSERT INTO Flight VALUES (208, 'Hawaii', 'Atlanta', 150, '2014-05-03', 11);
INSERT INTO Flight VALUES (209, 'Moskau', 'Paris', 210, '2013-02-13', 10);
INSERT INTO Flight VALUES (210, 'Berlin', 'Moskau', 120, '2012-01-12', 33);

INSERT INTO Customer VALUES (1000, 'Scott', '2002-01-10', 'Vienna');
INSERT INTO Customer VALUES (1001, 'Meier', '2001-12-31', 'Prague');
INSERT INTO Customer VALUES (1002, 'Huber', '2002-05-15', 'Rome');
INSERT INTO Customer VALUES (1003, 'King', '2000-09-23', 'Rome');
INSERT INTO Customer VALUES (1004, 'Puyol', '2000-09-17', 'Paris');
INSERT INTO Customer VALUES (1005, 'Becaud', '2000-09-16', 'London');

INSERT INTO Pilot VALUES (111, 'Nemec', '2002-06-03', 'Captain', 80, '2015-01-01', 2);
INSERT INTO Pilot VALUES (222, 'Nordmann', '1999-04-10', 'Captain', 150, '2013-01-01', 2);
INSERT INTO Pilot VALUES (333, 'Nagel', '2000-02-27', 'Copilot', 100, '2016-01-01', 1);
INSERT INTO Pilot VALUES (444, 'Nero', '2000-08-13', 'Copilot', 60, '2015-01-01', 1);
INSERT INTO Pilot VALUES (555, 'Nano', '1992-04-10', 'Captain', 120, '2014-01-01', 2);
INSERT INTO Pilot VALUES (666, 'Nano1', '1992-04-10', 'Captain', 120, '2014-01-01', 3);
INSERT INTO Pilot VALUES (777, 'Nano2', '1992-04-10', 'Captain', 100, '2014-01-01', 2);
INSERT INTO Pilot VALUES (888, 'Nano3', '1992-04-10', 'Captain', 800, '2014-01-01', 3);
INSERT INTO Pilot VALUES (999, 'Nano4', '1992-04-10', 'Copilot', 700, '2014-01-01', 1);
INSERT INTO Pilot VALUES (112, 'Nano5', '1992-04-10', 'Captain', 600, '2014-01-01', 3);
INSERT INTO Pilot VALUES (113, 'Nano6', '1992-04-10', 'Captain', 500, '2014-01-01', 1);
INSERT INTO Pilot VALUES (114, 'Nano7', '1992-04-10', 'Copilot', 400, '2014-01-01', 3);

INSERT INTO Training VALUES (1, 'Flight Training', 1);
INSERT INTO Training VALUES (2, 'Emergency Training', 1);
INSERT INTO Training VALUES (3, 'Flight Training', 2);
INSERT INTO Training VALUES (4, 'Airbus A380 Training', 5);
INSERT INTO Training VALUES (5, 'Airbus A380 Training', 4);
INSERT INTO Training VALUES (6, 'Airbus A380 Training', 3);
INSERT INTO Training VALUES (7, 'Airbus A380 Training', 2);
INSERT INTO Training VALUES (8, 'Airbus A380 Training', 1);

INSERT INTO PilotTraining VALUES (111, 1, '2019-02-23');
INSERT INTO PilotTraining VALUES (222, 1, '2016-02-04');
INSERT INTO PilotTraining VALUES (111, 2, '2014-03-12');
INSERT INTO PilotTraining VALUES (444, 4, '2019-02-02');
INSERT INTO PilotTraining VALUES (555, 2, '2013-09-12');

INSERT INTO PilotRoster VALUES (111, 204);
INSERT INTO PilotRoster VALUES (222, 208);
INSERT INTO PilotRoster VALUES (111, 205);
INSERT INTO PilotRoster VALUES (444, 206);
INSERT INTO PilotRoster VALUES (555, 207);
INSERT INTO PilotRoster VALUES (333, 203);
INSERT INTO PilotRoster VALUES (444, 203);
INSERT INTO PilotRoster VALUES (111, 209);
INSERT INTO PilotRoster VALUES (555, 210);

INSERT INTO Booking VALUES (209, 1000, 1, 2, 240);
INSERT INTO Booking VALUES (206, 1002, 2, 1, 300);
INSERT INTO Booking VALUES (205, 1003, 3, 2, 180);
INSERT INTO Booking VALUES (204, 1004, 1, 2, 150);
INSERT INTO Booking VALUES (207, 1005, 2, 1, 250);
INSERT INTO Booking VALUES (209, 1002, 3, 2, 340);
INSERT INTO Booking VALUES (206, 1003, 1, 1, 250);
INSERT INTO Booking VALUES (205, 1004, 1, 2, 150);
INSERT INTO Booking VALUES (204, 1003, 2, 2, 220);
INSERT INTO Booking VALUES (207, 1001, 3, 1, 450);

-- Select statements to view the inserted data
SELECT * FROM Airline;
SELECT * FROM Plane;
SELECT * FROM PlaneType;
SELECT * FROM Pilot;
SELECT * FROM PilotTraining;
SELECT * FROM Training;
SELECT * FROM PilotRoster;
SELECT * FROM Customer;
SELECT * FROM Booking;
SELECT * FROM Flight;
