<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id = $_POST['id'];
$costume=$_POST['costume'];

// 코스튬 변경
$result = mysqli_query($conn,"UPDATE `character` SET costume=$costume WHERE binary(account_id)=$id;");

echo("OK");