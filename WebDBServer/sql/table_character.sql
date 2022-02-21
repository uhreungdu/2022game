CREATE TABLE `CHARACTER`(
character_name VARCHAR(255) NOT NULL,
character_level INT DEFAULT 1,
online_status TEXT DEFAULT `offline`,
account_id VARCHAR(255) NOT NULL,
cutomize_parts1 INT DEFAULT 0,
cutomize_parts2 INT DEFAULT 0,
primary key(character_name) 
);