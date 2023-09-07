-- creating database
DROP DATABASE Zoo;
CREATE DATABASE Zoo;
USE Zoo;

-- -----------------------------------------------------------------------------------------------------------
-- creating tables

CREATE TABLE Konto
(
    Id         INT PRIMARY KEY AUTO_INCREMENT,
    Kontostand DOUBLE
);

CREATE TABLE Kassa
(
    Id         INT PRIMARY KEY AUTO_INCREMENT,
    Kassastand DOUBLE,
    KontoId    INT,
    FOREIGN KEY (KontoId) REFERENCES Konto (Id)
);

CREATE TABLE Shop
(
    Id      INT PRIMARY KEY AUTO_INCREMENT,
    Fläche  INT,
    KontoId INT,
    FOREIGN KEY (KontoId) REFERENCES Konto (Id)
);

CREATE TABLE Produkt
(
    Id        INT PRIMARY KEY AUTO_INCREMENT,
    Preis     DOUBLE,
    Name      VARCHAR(255),
    Zeitpunkt TIMESTAMP NOT NULL,
    Bestand   INT,
    KassaId   INT,
    FOREIGN KEY (kassaId) REFERENCES Kassa (Id),
    ShopId    INT,
    FOREIGN KEY (ShopId) REFERENCES Shop (Id)
);

CREATE TABLE Mitarbeiter
(
    Id               INT PRIMARY KEY AUTO_INCREMENT,
    Position         VARCHAR(255),
    MitarbeiterAlter INT,
    Name             VARCHAR(255),
    ShopId           INT,
    FOREIGN KEY (ShopId) REFERENCES Shop (Id),
    KassaId          INT,
    FOREIGN KEY (KassaId) REFERENCES Kassa (Id)
);

CREATE TABLE Gehege
(
    Id            INT PRIMARY KEY AUTO_INCREMENT,
    Volumen       INT,
    Standort      VARCHAR(255),
    Gehegeart     VARCHAR(255),
    MitarbeiterId INT,
    FOREIGN KEY (MitarbeiterId) REFERENCES Mitarbeiter (Id)
);

CREATE TABLE Tiere
(
    Id       INT PRIMARY KEY AUTO_INCREMENT,
    Gattung  VARCHAR(255),
    Nahrung  VARCHAR(255),
    GehegeId INT,
    FOREIGN KEY (GehegeId) REFERENCES Gehege (Id)
);

CREATE TABLE Tickets
(
    Id        INT PRIMARY KEY AUTO_INCREMENT,
    Preis     DOUBLE,
    Zeitpunkt TIMESTAMP NOT NULL
);

-- -----------------------------------------------------------------------------------------------------------
-- creating user

DROP USER 'zooUser'@'Zoo';
CREATE USER 'mariadb'@'Zoo' IDENTIFIED BY 'password';

GRANT INSERT, SELECT ON Zoo.* TO 'mariadb'@'Zoo';

SHOW GRANTS FOR 'mariadb'@'Zoo';

-- -----------------------------------------------------------------------------------------------------------
-- inserting values into tables
    
INSERT INTO Konto(Kontostand) VALUE (100);
INSERT INTO Kassa(Kassastand, KontoId) VALUE (10,1);
INSERT INTO Kassa(Kassastand, KontoId) VALUE (12,1);
INSERT INTO Shop(Fläche, KontoId) VALUE (30,1);

INSERT INTO Produkt(Preis, Name, Bestand, KassaId) VALUE (8.50,'Ticket',1000,1);

INSERT INTO Mitarbeiter(Position, MitarbeiterAlter, Name, ShopId, KassaId) VALUE ('Tierpfleger','26','Georg',1,NULL);
INSERT INTO Mitarbeiter(Position, MitarbeiterAlter, Name, ShopId, KassaId) VALUE ('Tierpfleger','42','Paul',NULL, NULL);
INSERT INTO Mitarbeiter(Position, MitarbeiterAlter, Name, ShopId, KassaId) VALUE ('Tierpfleger','33','Herbert',NULL, NULL);
INSERT INTO Mitarbeiter(Position, MitarbeiterAlter, Name, ShopId, KassaId) VALUE ('Kassierer','21','Martin',NULL,1);

INSERT INTO Gehege(Volumen, Standort, Gehegeart, Mitarbeiterid) VALUE (200,'Wald','Käfig',1);
INSERT INTO Gehege(Volumen, Standort, Gehegeart, Mitarbeiterid) VALUE (500,'Wald','Käfig',1);
INSERT INTO Gehege(Volumen, Standort, Gehegeart, Mitarbeiterid) VALUE (500,'Wald','Käfig',3);

INSERT INTO Tiere(Gattung, Nahrung, GehegeId) VALUE ('Tiger','Fleisch',2);
INSERT INTO Tiere(Gattung, Nahrung, GehegeId) VALUE ('Tigermücke','Nektar',2);
INSERT INTO Tiere(Gattung, Nahrung, GehegeId) VALUE ('Affen ','Blätter',2);
INSERT INTO Tiere(Gattung, Nahrung, GehegeId) VALUE ('Warzenschwein ','Wurzeln ',5);

INSERT INTO Tickets(Preis) VALUE (8.50);

-- -----------------------------------------------------------------------------------------------------------
-- select querys

SELECT *
FROM Tiere;
SELECT *
FROM Mitarbeiter;
SELECT *
FROM Gehege;

SELECT *
FROM Tickets
WHERE DATE (Zeitpunkt) = '2023-08-03';

SELECT *
FROM Tiere
WHERE Gattung = 'Tiger';

SELECT Tiere.Id, Tiere.Gattung, Tiere.Nahrung, Tiere.GehegeId
FROM Tiere
         JOIN Gehege G on G.Id = Tiere.GehegeId
         JOIN Mitarbeiter M on M.Id = G.MitarbeiterId
WHERE MitarbeiterId = '5';

-- update querys
UPDATE Tiere
SET Nahrung = 'Fleisch'
WHERE Id = 6;

UPDATE Gehege
SET MitarbeiterId = 5
WHERE Id = 5;
