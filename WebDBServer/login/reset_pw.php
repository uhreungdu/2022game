<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];
$pw=$_POST['pw'];

// 비밀번호 리셋
$result = mysqli_query($conn,"UPDATE account SET account_pw=$pw WHERE binary(account_id)=$id;");

echo("OK");