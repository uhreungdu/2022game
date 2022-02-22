<?php
$conn = mysqli_connect("localhost","root","password","test");

$iname=$_GET['iname'];

mysqli_query($conn,"DELETE FROM room WHERE internal_name=$iname");