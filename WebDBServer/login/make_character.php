<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];
$name=$_POST['name'];

// 계정 id 유효 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE account_id=$id;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('존재하지 않는 ID');
    exit();
}

// 닉네임 중복 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM `character` WHERE character_name=$name;");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('닉네임 중복');
    exit();
}

// 계정 id 중복 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM `character` WHERE account_id=$id;");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('ID 중복');
    exit();
}

// 계정 작성
mysqli_query($conn,"INSERT INTO `character`
(account_id,character_name) 
    VALUE ($id, $name);");

echo('OK');