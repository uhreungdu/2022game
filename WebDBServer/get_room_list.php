<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$result = mysqli_query($conn,"
SELECT internal_name ,external_name ,now_playernum ,max_playernum , ingame 
FROM room 
order by created_time asc;
");

while($row = mysqli_fetch_array($result)){
    echo "internal_name:".$row['internal_name']."|external_name:".$row['external_name'].
    "|now_playernum:".$row['now_playernum']."|max_playernum:".$row['max_playernum'].
    "|ingame:".$row['ingame'].";";
}
