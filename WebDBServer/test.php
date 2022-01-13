<?php
$DB = new SQLite3('DBÃÖÁ¾.db');

if($DB->lastErrorCode() == 1){
	echo "Database connection failed";
	echo $DB->lastErrorMsg();
}

$result = $DB->query("SELECT * FROM 'USER_ACOUNT';");

while($row = $result->fetchArray(SQLITE3_ASSOC)){
	echo $row["ID"]."<br>";
}
?>