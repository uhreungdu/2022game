<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];
$pw=$_POST['pw'];

// 계정 작성
mysqli_query($conn,"INSERT INTO account 
(account_id,account_pw) 
    VALUE ($id, $pw);");
    
echo("OK");