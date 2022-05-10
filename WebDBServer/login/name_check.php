<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$name=$_POST['name'];

// 닉네임 중복 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM `character` WHERE binary(character_name)=$name;");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('닉네임 중복');
    exit();
}

echo("OK");