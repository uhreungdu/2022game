<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];
$pw=$_POST['pw'];

// 계정 id 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE account_id=$id;");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('ID 중복');
    exit();
}

// 계정 작성
mysqli_query($conn,"INSERT INTO account 
(account_id,account_pw) 
    VALUE ($id, $pw);");