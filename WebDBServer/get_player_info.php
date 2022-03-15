<?php
$conn = mysqli_connect("localhost","root","kpu2022project","2022project");

$id=$_POST['id'];

$result = mysqli_query($conn,"
SELECT character_name, character_level, customize_1, customize_2
FROM `character` 
WHERE account_id = $id;
");

while($row = mysqli_fetch_array($result)){
    echo "character_name:".$row['character_name']."|character_level:".$row['character_level'].
    "|customize_1:".$row['customize_1']."|customize_2:".$row['customize_2'].";";
}
