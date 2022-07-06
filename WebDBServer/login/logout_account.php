<?php
$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];

mysqli_query($conn,
"UPDATE `character` as C
SET C.online_status='offline'
WHERE binary(C.account_id)=$id;");