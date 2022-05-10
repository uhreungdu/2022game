<?php

$conn = mysqli_connect("localhost","root","2022project","havocfes");

$iname=$_GET['iname'];

// 게임 시작표시로 변경
$result = 
mysqli_query($conn,"UPDATE room
SET ingame = 1
WHERE internal_name = $iname;
");
