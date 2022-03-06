<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];
$pw=$_POST['pw'];

// 계정 id 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE binary(account_id)=$id;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('ID나 비밀번호가 잘못되었습니다.');
    exit();
}

// 계정 pw 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE binary(account_id)=$id and binary(account_pw)=$pw;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('ID나 비밀번호가 잘못되었습니다.');
    exit();
}

echo("OK");