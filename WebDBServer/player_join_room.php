<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$iname=$_POST['iname'];
$Pname=$_POST['Pname'];

// 방 존재하는지 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM room WHERE internal_name=$iname;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('방이 존재하지 않습니다.');
    exit();
}
// 인원수 체크
$result = mysqli_query($conn,"
SELECT now_playernum, max_playernum FROM room 
WHERE internal_name = $iname;
");
$resultval = $result->fetch_array();
if($resultval[0] >= $resultval[1]){
    echo('인원이 꽉 찼습니다.');
    exit();
}

// 플레이중인 유저 목록에 추가
mysqli_query($conn,"
INSERT IGNORE INTO playingchar (
    character_name,
    room_internal_name
    ) 
    VALUE (
        $Pname,
        $iname
        );
        "
    );

// 방 현재 인원수 증가
mysqli_query($conn,"
UPDATE room
SET now_playernum = now_playernum + 1
WHERE internal_name = $iname;
");