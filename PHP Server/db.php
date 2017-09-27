<?php
date_default_timezone_set("UTC"); 
$host="127.0.0.1";
$port="5432";
$dbname="multichoice";
$dbuser="root";
$dbpass="";

$con = mysqli_connect($host, $dbuser, $dbpass); 
$o = mysqli_select_db($con, $dbname);
?>