CREATE TABLE ROOM(
internal_name VARCHAR(255) NOT NULL,
external_name VARCHAR(255) NOT NULL,
now_playernum INT,
max_playernum INT,
host_name VARCHAR(255) NOT NULL,
primary key(internal_name) 
);
ALTER TABLE room ADD COLUMN created_time DATETIME DEFAULT now();