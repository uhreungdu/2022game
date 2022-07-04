<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$name=$_GET['pname'];
$win=$_GET['win'];

if($win==1){
    mysqli_query($conn,
    "UPDATE `character` as C
    SET C.win=C.win+1
    WHERE binary(C.character_name)=$name;");
}
else{
    mysqli_query($conn,
    "UPDATE `character` as C
    SET C.lose=C.lose+1
    WHERE binary(C.character_name)=$name;");
}

echo("OK");