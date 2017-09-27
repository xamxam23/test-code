<?php

$cmd = isset($_POST["cmd"])? $_POST["cmd"] : (isset($_GET["cmd"])? $_GET["cmd"]: "");

function get($key){
    return stripslashes(isset($_POST[$key])? $_POST[$key] : (isset($_GET[$key])? $_GET[$key] : null));
}

$arr=null;
if ($cmd == "GetAnimals"){
    require_once("db.php"); //$con
    $data = array();
    $result = mysqli_query($con, "SELECT * FROM animals") or die(mysqli_error($con));
    while($row=mysqli_fetch_array($result, MYSQL_ASSOC)){
	   $data[] = $row;
    }
    $arr = array("message"=>"get users", "ok"=>true, "Data"=>$data); 
    mysqli_close($con);
} else if ($cmd == "GetAnimalsCount"){
    require_once("db.php"); //$con
    $data = array();
    $result = mysqli_query($con, "SELECT Name, count(*) as Count FROM animals GROUP BY Name") or die(mysqli_error($con));
    while($row=mysqli_fetch_array($result, MYSQL_ASSOC)){
	   $data[] = $row;
    }
    $arr = array("message"=>"get users", "ok"=>true, "Data"=>$data); 
    mysqli_close($con);
}else if ($cmd == "GetAnimalsCountPerGroup"){
    require_once("db.php"); //$con
    $data = array();
    $result = mysqli_query($con, "SELECT Group_ID, count(*) as Count FROM animals GROUP BY Group_ID") or die(mysqli_error($con));
    while($row=mysqli_fetch_array($result, MYSQL_ASSOC)){
	   $data[] = $row;
    }
    $arr = array("message"=>"get users", "ok"=>true, "Data"=>$data);
    mysqli_close($con);
} else if ($cmd == "UpdateLocations"){
    require_once("db.php");
    $json = get('data');
    $obj = json_decode($json);

    $c = 0;
    foreach($obj as $o) {
        $id = $o->Animal_ID;
        $long = $o->Longitude;
        $lat = $o->Latitude;
        
        $sql = "UPDATE animals SET Longitude=$long, Latitude=$lat WHERE Animal_ID=$id";
        $result = mysqli_query($con, $sql) or die(mysqli_error($con));
        
        $sql = "INSERT INTO locations (Animal_ID, Longitude, Latitude) VALUES ($id, $long, $lat)";
        $result = mysqli_query($con, $sql) or die(mysqli_error($con));
        
        $id = mysqli_insert_id($con);
        $c = $id;
    }
   
	mysqli_close($con);
    
    $arr = array("message"=>"insert data", "ok"=>($c>0));
} else if ($cmd == "CreateAnimals"){
    require_once("db.php");
    $json = get('data');
    $obj = json_decode($json);
    
    mysqli_query($con, "DELETE FROM animals") or die(mysqli_error($con));
     
    $c = 0;
    foreach($obj as $o) {
        $name = $o->Name;
        $long = $o->Longitude;
        $lat = $o->Latitude;
        $groupid = $o->Group_ID;
        
        $sql = "INSERT INTO animals (Name, Longitude, Latitude, Group_ID) VALUES ('$name', $long, $lat, $groupid)";
        $result = mysqli_query($con, $sql) or die(mysqli_error($con));
        
        $id = mysqli_insert_id($con);
        $c = $id;
    }
   
	mysqli_close($con);
    
    $arr = array("message"=>"insert data", "ok"=>($c>0));
}  else {
   $arr = array("message"=>"unknown command", "ok"=>false);
}
echo json_encode($arr);
?> 