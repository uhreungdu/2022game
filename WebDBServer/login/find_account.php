<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];
$name=$_POST['name'];

// 계정 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM `character` 
WHERE binary(account_id)=$id AND binary(character_name)=$name;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('No Account');
    exit();
}

echo("OK");