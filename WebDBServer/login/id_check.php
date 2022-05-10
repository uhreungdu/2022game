<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];

// 계정 id 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE binary(account_id)=$id;");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('ID 중복');
    exit();
}

echo("OK");