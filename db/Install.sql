DROP TABLE Tickets;
DROP TABLE Sessions;
DROP TABLE Cinema;
DROP TABLE Movies;
DROP TABLE Genres;
DROP TABLE Users;

GO

CREATE TABLE Genres
(
	id 	 INTEGER PRIMARY KEY NOT NULL IDENTITY(1, 1),
	name VARCHAR(64) UNIQUE
);

INSERT INTO Genres (name) VALUES ('Боевик');
INSERT INTO Genres (name) VALUES ('Драма');
INSERT INTO Genres (name) VALUES ('Ужасы');
INSERT INTO Genres (name) VALUES ('Комедия');
INSERT INTO Genres (name) VALUES ('Трилер');
INSERT INTO Genres (name) VALUES ('Мультфильм');

GO

CREATE TABLE Movies
(
	id 			INTEGER PRIMARY KEY NOT NULL IDENTITY(1, 1),
	genre_id	INTEGER NOT NULL,
	name 		VARCHAR(128) NOT NULL,
	duration	INTEGER NOT NULL,
	year		INTEGER NOT NULL,
	image		VARCHAR(256),
	FOREIGN KEY(genre_id) REFERENCES Genres(id)
);

GO

INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(1, 'Трансформеры 7', 124, 2014, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(2, 'Побег из Шоушенка', 168, 1994, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(6, 'Король лев', 105, 1994, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(6, 'Wall-E', 95, 2010, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(2, 'Терминатор 2', 132, 1991, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(2, 'Титаник', 143, 1999, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(4, 'Один дома', 86 ,1995, NULL);
INSERT INTO Movies(genre_id, name, duration, year, image) VALUES(3, 'Звонок', 99, 2008, NULL);

GO

CREATE TABLE Cinema
(
	id 		INTEGER PRIMARY KEY NOT NULL IDENTITY(1, 1),
	name	VARCHAR(64) NOT NULL,
	address	VARCHAR(256) NOT NULL,
	rows	INTEGER NOT NULL,
	places	INTEGER NOT NULL,
	image	VARCHAR(256)
);

GO

INSERT INTO Cinema(name, address, rows, places, image) VALUES('IMAX-Z', 'Украина г. Запорожье пр.Ленина 98', 10, 12, NULL);
INSERT INTO Cinema(name, address, rows, places, image) VALUES('Маяковского', 'Украина г. Запорожье пр.Ленина 110', 10, 10, NULL);
INSERT INTO Cinema(name, address, rows, places, image) VALUES('Довженко', 'Украина г. Запорожье пр.Ленина 198', 12, 15, NULL);
INSERT INTO Cinema(name, address, rows, places, image) VALUES('Байда', 'Украина г. Запорожье пр.Ленина 50', 10, 10, NULL);
INSERT INTO Cinema(name, address, rows, places, image) VALUES('Мультиплекс', 'Украина г. Запорожье пр.Ленина 10', 8, 10, NULL);

GO

CREATE TABLE Sessions
(
	id 			INTEGER PRIMARY KEY NOT NULL IDENTITY(1, 1),
	movie_id	INTEGER NOT NULL,
	cinema_id	INTEGER NOT NULL,
	beginning	INTEGER NOT NULL,
	FOREIGN KEY(movie_id) REFERENCES Movies(id),
	FOREIGN KEY(cinema_id) REFERENCES Cinema(id)
);

GO

INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(1, 1, 155037600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(2, 1, 155055600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(3, 1, 155087600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(5, 2, 155047600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(3, 2, 155067600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(7, 2, 155097600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(5, 3, 155027600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(6, 3, 155067600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(8, 3, 155097600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(3, 4, 155045600);
INSERT INTO Sessions(movie_id, cinema_id, beginning) VALUES(6, 5, 155053600);

GO

CREATE TABLE Tickets
(
	id 			INTEGER PRIMARY KEY  NOT NULL IDENTITY(1, 1),
	session_id	INTEGER NOT NULL,
	row 		INTEGER NOT NULL,
	place 		INTEGER NOT NULL,
	FOREIGN KEY(session_id) REFERENCES Sessions(id),
);

GO

CREATE TABLE Users
(
	id 			INTEGER PRIMARY KEY  NOT NULL IDENTITY(1, 1),
	login 		VARCHAR(16) NOT NULL,
	password	VARCHAR(16) NOT NULL,
	type 		VARCHAR(8) NOT NULL DEFAULT 'admin'
);

INSERT INTO Users(login, password, type) VALUES('admin', 'admin', 'admin');