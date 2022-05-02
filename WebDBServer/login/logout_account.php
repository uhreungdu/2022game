<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];

mysqli_query($conn,
"UPDATE `character` as C
SET C.online_status='offline'
WHERE binary(C.account_id)=$id;");