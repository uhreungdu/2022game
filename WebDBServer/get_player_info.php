<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_GET['id'];

$result = mysqli_query($conn,
"SELECT character_name, character_level, costume, win, lose
FROM `character` 
WHERE account_id = $id;
");

while($row = mysqli_fetch_array($result)){
    echo "name:".$row['character_name']."|level:".$row['character_level'].
    "|costume:".$row['costume']."|win:".$row['win']."|lose:".$row['lose'];
}
