<?php include('../_connect.php');?>
<?php include('../../includes/helpers/short.php');?>
<?php include('../../functions.php');?>
<?php

$api_key = isset($_POST['api_key']) ? mysqli_real_escape_string($mysqli, $_POST['api_key']) : null;	
$list_name = isset($_POST['list_name']) ? mysqli_real_escape_string($mysqli, $_POST['list_name']) : null;
$app = isset($_POST['brand_id']) ? mysqli_real_escape_string($mysqli, $_POST['brand_id']) : null;
$fields = isset($_POST['custom_fields']) ? mysqli_real_escape_string($mysqli, $_POST['custom_fields']) : null;
$fieldtypes = isset($_POST['field_types']) ? mysqli_real_escape_string($mysqli, $_POST['field_types']) : null;

function verify_appid($app) {
	global $mysqli;
		
	$q = 'SELECT id FROM apps WHERE id = '.$app;
	$r = mysqli_query($mysqli, $q);
	if(mysqli_num_rows($r) > 0)
		return true;
	else
		return false;
}

function validCustomFields($custom_fields, $field_types) {
    try {
        $customfieldsArray = explode(',', $custom_fields);
        $fieldtypesArray = explode(',', $field_types);

        if(count($customfieldsArray) != count($fieldtypesArray))
        {
            echo 'Number of custom fields should match the number of custom field types';
            return false;
        }
        else
        {
            foreach($customfieldsArray as $custom_field) {
                if(strtolower($custom_field)=='name' || strtolower($custom_field)=='email')
                {
                    echo 'Custom field name "name" or "email" is not allowed';
                    return false;
                }
            }
            foreach($fieldtypesArray as $field_type) {
                if(!(strtolower($field_type)=='text' || strtolower($field_type)=='date'))
                {
                    echo 'Only data types "text" and "date" are supported';
                    return false;
                }
            }
        }
    } catch (Exception $e) {
        echo 'Caught exception: ',  $e->getMessage(), "\n";
        return false;
    }    
    return true;
}

function validFields($api_key, $list_name, $app, $custom_fields, $field_types) {
    $isValid = true;

    if($api_key==null)
    {
	    echo 'API key not passed';
        $isValid = false;
    }
    else if(!verify_api_key($api_key))
    {
	    echo 'Invalid API key';
        $isValid = false;
    }
    else if(!verify_appid($app))
    {
	    echo 'Invalid app/ brand id '.$app;
        $isValid = false;
    } 
    else if($list_name==null)
    {
        echo 'List name is required';
        $isValid = false;
    }
    else if($custom_fields != null)
    {
        $isValid = validCustomFields($custom_fields, $field_types);
    }
    return $isValid;        
}

function getUserId()
{
	global $mysqli;
    //weird way to retrieve a user. We only have an api_key, so take the first one (copied from create campaign...)
    $q = 'SELECT id FROM login ORDER BY id ASC LIMIT 1';
    $r = mysqli_query($mysqli, $q);
    if ($r) while($row = mysqli_fetch_array($r)) $userID = $row['id'];
    return $userID;
}

function convert_custom_fields($fields, $types) {
    //copied from includes/list/add-custom-field.php	
    $custom_fields = explode(',', $fields);
    $field_types = explode(',', $types);

    $custom_fields_sql_value = '';
    foreach($custom_fields as $i => $custom_field) {
        if($custom_fields_sql_value != '') //more than 1 custom field
            $custom_fields_sql_value = $custom_fields_sql_value.'%s%';

        $type = (strtolower($field_types[$i]) == 'text' ? 'Text' : 'Date');
        //we assume both arrays are of the same size
        $custom_fields_sql_value = $custom_fields_sql_value.$custom_field.':'.$type;
    }
    return $custom_fields_sql_value;    
}

if(!validFields($api_key, $list_name, $app, $fields, $fieldtypes))
{
    exit;
}

$userID = getUserId();
$custom_fields = convert_custom_fields($fields, $fieldtypes);

//add new list
$q = 'INSERT INTO lists (app, userID, name, custom_fields) VALUES ('.$app.', '.$userID.', "'.$list_name.'", "'.$custom_fields.'")';

$r = mysqli_query($mysqli, $q);
if ($r)
{
    $listID = mysqli_insert_id($mysqli);
    echo $listID;
}
else {
    echo 'Unable to create list';
	exit;
}
?>
