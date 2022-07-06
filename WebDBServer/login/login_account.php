<?php
$conn = mysqli_connect("localhost","root","2022project","havocfes");

$id=$_POST['id'];
$pw=$_POST['pw'];

// 계정 id 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE binary(account_id)=$id;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('Msg:ID나 비밀번호가 잘못되었습니다.;');
    exit();
}

// 계정 pw 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM account WHERE binary(account_id)=$id and binary(account_pw)=$pw;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('Msg:ID나 비밀번호가 잘못되었습니다.;');
    exit();
}

// 계정에 생성된 캐릭터 체크
$result = mysqli_query($conn,"SELECT COUNT(*) FROM `character` WHERE binary(account_id)=$id;");
$resultval = $result->fetch_array()[0];
if($resultval == 0){
    echo('Msg:Need Character;');
    exit();
}

// 중복접속 체크
$result = mysqli_query($conn,"SELECT online_status FROM `character` WHERE binary(account_id)=$id;");
$resultval = $result->fetch_array()[0];
if($resultval == "online"){
    echo('Msg:Already Online;');
    exit();
}

$result = mysqli_query($conn,
"SELECT character_name ,account_id
FROM `character` 
WHERE binary(account_id)=$id;");

$charname;
while($row = mysqli_fetch_array($result)){
    $charname = $row['character_name'];
}

// 마지막 로그인 시간 기록
mysqli_query($conn,
"UPDATE account as A, `character` as C
SET A.last_login = current_timestamp(), C.online_status='online'
WHERE binary(A.account_id)=$id and binary(C.account_id)=$id;");

// 플레이중인지 체크
$result = mysqli_query($conn,
"SELECT COUNT(*), room_internal_name FROM `playingchar` 
WHERE binary(character_name)='$charname';");
$data = mysqli_fetch_array($result);
if( $data[0] != 0){
    echo('Msg:INGAME|'."character_name:".$charname."|account_id:".$id.
    "|room_name:". $data[1].";");
    exit();
}

echo "Msg:OK|"."character_name:".$charname."|account_id:".$id.";";