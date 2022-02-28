CREATE TABLE PLAYINGCHAR(
character_name VARCHAR(255) NOT NULL,
room_internal_name VARCHAR(255) NOT NULL,
primary key(character_name),
foreign key(room_internal_name) REFERENCES ROOM (internal_name) 
on delete cascade on update cascade
);