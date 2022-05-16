<?php
$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];

mysqli_query($conn,
"UPDATE `character` as C
SET C.online=0
WHERE binary(C.account_id)=$id;");