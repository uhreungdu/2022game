<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$iname=$_POST['iname'];
$ename=$_POST['ename'];
$nowPnum=$_POST['nowPnum'];
$maxPnum=$_POST['maxPnum'];
$Pname=$_POST['Pname'];

// 이미 생성된 방 체크    
$result = mysqli_query($conn,"SELECT COUNT(*) FROM room WHERE internal_name='$iname';");
$resultval = $result->fetch_array()[0];
if($resultval > 0){
    echo('이미 생성된 방');
    exit();
}

// 방 생성
mysqli_query($conn,"
INSERT IGNORE INTO room (
    internal_name,
    external_name,
    now_playernum,
    max_playernum,
    host_name
    ) 
    VALUE (
        $iname,
        $ename,
        $nowPnum,
        $maxPnum,
        $Pname
        );
        "
    );

// Player join room 
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
echo ('방 생성 성공');

?>