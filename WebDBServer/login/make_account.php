<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];
$pw=$_POST['pw'];
$name=$_POST['name'];

// 계정 작성
mysqli_query($conn,"INSERT INTO account 
(account_id,account_pw) 
    VALUE ($id, $pw);");

mysqli_query($conn,"INSERT INTO `character`
(account_id,character_name) 
    VALUE ($id, $name);");
    
echo("OK");