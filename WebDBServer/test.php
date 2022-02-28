<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$result = mysqli_query($conn,"SELECT COUNT(*) FROM room WHERE internal_name='$iname';");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('이미 생성된 방');
    exit();
}

echo('db연결');
?>