<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$iname=$_POST['iname'];
$Pname=$_POST['Pname'];

// 플레이중인 유저 목록에서 제거
mysqli_query($conn,"
DELETE FROM playingchar
WHERE character_name = $Pname;
");

// 방 현재 인원수 감소
mysqli_query($conn,"
UPDATE room
SET now_playernum = now_playernum - 1
WHERE internal_name = $iname;
");

// 방 인원수가 0명 이하이면 방 제거
$result = mysqli_query($conn,"
SELECT now_playernum FROM room 
WHERE internal_name = $iname;
");
$resultval = $result->fetch_array()[0];

if($resultval <= 0){
    mysqli_query($conn,"
    DELETE FROM room
    WHERE internal_name = $iname;
    ");
}

fclose($myfile);