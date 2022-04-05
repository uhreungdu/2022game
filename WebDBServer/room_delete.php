<?php

$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$iname=$_GET['iname'];

// 방 목록에서 방 제거
mysqli_query($conn,"
DELETE FROM room
WHERE internal_name = $iname;
");

// 그 방에 있던 플레이어들이 게임중 목록에 있다면 제거
mysqli_query($conn,"
DELETE FROM playingchar
WHERE room_internal_name = $iname;
");