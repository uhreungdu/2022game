<?php
$conn = mysqli_connect("localhost","root","password","test");

$iname=$_GET['iname'];
$ename=$_GET['ename'];
$nowPnum=$_GET['nowPnum'];
$maxPnum=$_GET['maxPnum'];
$Pname=$_GET['Pname'];

mysqli_query($conn,"
INSERT INTO room (
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
        ");